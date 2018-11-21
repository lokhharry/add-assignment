using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace add_assignment.Models.StoredProcedures.Response
{
    public class sp_getTransactionByBranch
    {
        public string Ref { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int? SoldPrice { get; set; }
        public int? RentalPrice { get; set; }
        public decimal Commission { get; set; }
        public string DistrictName { get; set; }
        public string EstateName { get; set; }
        public string Block { get; set; }
        public string Floor { get; set; }
        public string Flat { get; set; }
        public string AgentName { get; set; }
    }
}