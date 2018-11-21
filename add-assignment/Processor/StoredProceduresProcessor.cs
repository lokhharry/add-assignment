using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace add_assignment.Processor
{
    public class StoredProceduresProcessor
    {
        private SqlConnection _conn;

        private string _spName;
        private Dictionary<string, object> _spInputValue;
        private Dictionary<string, object> _spOutputValue;
        private object _result;

        public StoredProceduresProcessor(string spName, string connName)
        {
            this._spName = spName;
            this._conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connName].ConnectionString);
        }

        public void setInputValue(Dictionary<string, object> spInputValue)
        {
            this._spInputValue = spInputValue;
        }

        public void setOutputValue(Dictionary<string, object> spOutputValue)
        {
            this._spOutputValue = spOutputValue;
        }

        public async Task<object> getResult(object outputObject)
        {
            this._result = outputObject;
            using (this._conn)
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(this._spName, this._conn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (this._spInputValue != null)
                    {
                        foreach (KeyValuePair<string, object> input in this._spInputValue)
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@" + input.Key, input.Value));
                        }
                    }

                    //if(this._spOutputValue != null)
                    //{
                    //    foreach(KeyValuePair<string, object> output in this._spOutputValue)
                    //    {
                    //        sqlCommand.Parameters.Add(new SqlParameter("@" + output.Key, null))
                    //    }
                    //}

                    this._conn.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (this._result is IList && this._result.GetType() == typeof(List<List<object>>) && this._result.GetType().GetGenericArguments()[0].GetGenericArguments()[0] == typeof(object))
                    {
                        IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(this._result.GetType().GetGenericArguments()[0]));
                        foreach (var listRowData in this._result as List<List<object>>)
                        {
                            while (await sqlDataReader.ReadAsync())
                            {
                                IList objectList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(this._result.GetType().GetGenericArguments()[0].GetGenericArguments()[0]));
                                foreach (var listTableObject in listRowData)
                                {
                                    object instance = Activator.CreateInstance(listTableObject.GetType());
                                    objectList.Add(this.setRowData(sqlDataReader, instance));
                                }
                                list.Add(objectList);
                            }
                        }
                        this._result = list;
                    }
                    else if (this._result is IList && this._result.GetType() == typeof(List<object>) && this._result.GetType().GetGenericArguments()[0] == typeof(object))
                    {
                        IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(this._result.GetType().GetGenericArguments()[0]));
                        while (await sqlDataReader.ReadAsync())
                        {
                            foreach (var listTableObject in this._result as List<object>)
                            {
                                object instance = Activator.CreateInstance(listTableObject.GetType());
                                list.Add(this.setRowData(sqlDataReader, instance));
                            }
                        }
                        this._result = list;
                    }
                    else if (this._result is IList && this._result.GetType().IsGenericType)
                    {
                        IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(this._result.GetType().GetGenericArguments()[0]));
                        while (await sqlDataReader.ReadAsync())
                        {
                            object instance = Activator.CreateInstance(this._result.GetType().GetGenericArguments()[0]);
                            list.Add(this.setRowData(sqlDataReader, instance));
                        }
                        this._result = list;
                    }
                    else
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            this._result = this.setRowData(sqlDataReader, this._result);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    this._conn.Close();
                }
                return this._result;
            }
        }

        public async Task<List<T>> getResult<T>()
        {
            using (this._conn)
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(this._spName, this._conn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (this._spInputValue != null)
                    {
                        foreach (KeyValuePair<string, object> input in this._spInputValue)
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@" + input.Key, input.Value));
                        }
                    }

                    //if(this._spOutputValue != null)
                    //{
                    //    foreach(KeyValuePair<string, object> output in this._spOutputValue)
                    //    {
                    //        sqlCommand.Parameters.Add(new SqlParameter("@" + output.Key, null))
                    //    }
                    //}

                    this._conn.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    List<T> list = (List<T>)Activator.CreateInstance(typeof(List<T>));
                    while (await sqlDataReader.ReadAsync())
                    {
                        object instance = Activator.CreateInstance(typeof(T));
                        list.Add((T)this.setRowData(sqlDataReader, instance));
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    this._conn.Close();
                }
            }
        }

        public DataTable getDataTableResult()
        {
            DataTable result = new DataTable();
            using (this._conn)
            {
                try
                {
                    using (var sqlCommand = new SqlCommand(this._spName, this._conn))
                    using (var dataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        if (this._spInputValue != null)
                        {
                            foreach (KeyValuePair<string, object> input in this._spInputValue)
                            {
                                sqlCommand.Parameters.Add(new SqlParameter("@" + input.Key, input.Value));
                            }
                        }
                        dataAdapter.Fill(result);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return result;
        }

        private bool hasColumn(IDataRecord sqlDataReader, string columnName)
        {
            for (int i = 0; i < sqlDataReader.FieldCount; i++)
            {
                if (sqlDataReader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        private object setRowData(IDataReader sqlDataReader, object instance)
        {
            foreach (PropertyInfo prop in instance.GetType().GetProperties())
            {
                if (this.hasColumn(sqlDataReader, prop.Name))
                {
                    object value;
                    if (sqlDataReader[prop.Name] == DBNull.Value)
                    {
                        value = null;
                    }
                    else
                    {
                        Type type;
                        switch (prop.PropertyType.Name)
                        {
                            case "Nullable`1":
                                type = prop.PropertyType.GetGenericArguments()[0];
                                break;
                            default:
                                type = prop.PropertyType;
                                //type = sqlDataReader[prop.Name] == DBNull.Value ? typeof(Nullable<>).MakeGenericType(prop.PropertyType) : prop.PropertyType;
                                break;
                        }
                        value = Convert.ChangeType(sqlDataReader[prop.Name], type);
                    }
                    instance.GetType().GetProperty(prop.Name).SetValue(instance, value);
                }
            }
            return instance;
        }
    }
}