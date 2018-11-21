using add_assignment.Models.StoredProcedures.Response;
using add_assignment.Processor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace add_assignment.Manager
{
    public static class StoredProceduresManager
    {
        public static async Task<List<sp_getDistrict>> spGetDistrict()
        {
            StoredProceduresProcessor spProcessor = new StoredProceduresProcessor("spGetDistrict", ConfigurationManager.AppSettings["connName"]);
            return await spProcessor.getResult<sp_getDistrict>();
        }

        public static async Task<List<sp_getEstateByDistrict>> spGetEstateByDistrict(int districtID)
        {
            StoredProceduresProcessor spProcessor = new StoredProceduresProcessor("spGetEstateByDistrict", ConfigurationManager.AppSettings["connName"]);
            spProcessor.setInputValue(new Dictionary<string, object>()
            {
                { "districtID", districtID },
            });
            return await spProcessor.getResult<sp_getEstateByDistrict>();
        }

        public static async Task<List<sp_getPropertyByEstate>> spGetPropertyByEstate(int estateID)
        {
            StoredProceduresProcessor spProcessor = new StoredProceduresProcessor("spGetPropertyByEstate", ConfigurationManager.AppSettings["connName"]);
            spProcessor.setInputValue(new Dictionary<string, object>()
            {
                { "estateID", estateID },
            });
            return await spProcessor.getResult<sp_getPropertyByEstate>();
        }

        public static async Task<List<sp_getPropertyByID>> spGetPropertyByID(int propertyID)
        {
            StoredProceduresProcessor spProcessor = new StoredProceduresProcessor("spGetPropertyByID", ConfigurationManager.AppSettings["connName"]);
            spProcessor.setInputValue(new Dictionary<string, object>()
            {
                { "propertyID", propertyID },
            });
            return await spProcessor.getResult<sp_getPropertyByID>();
        }

        public static async Task<List<sp_getRecommendPropertyByCustomer>> spGetRecommendPropertyByCustomer(int customerID)
        {
            StoredProceduresProcessor spProcessor = new StoredProceduresProcessor("spGetRecommendPropertyByCustomer", ConfigurationManager.AppSettings["connName"]);
            spProcessor.setInputValue(new Dictionary<string, object>()
            {
                { "customerID", customerID },   
            });
            return await spProcessor.getResult<sp_getRecommendPropertyByCustomer>();
        }

        public static async Task<List<sp_getTransactionByBranch>> spGetTransactionByBranch(int branchID)
        {
            StoredProceduresProcessor spProcessor = new StoredProceduresProcessor("spGetTransactionByBranch", ConfigurationManager.AppSettings["connName"]);
            spProcessor.setInputValue(new Dictionary<string, object>()
            {
                { "branchID", branchID },
            });
            return await spProcessor.getResult<sp_getTransactionByBranch>();
        }
    }
}