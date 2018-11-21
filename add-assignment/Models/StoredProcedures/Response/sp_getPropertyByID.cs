using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace add_assignment.Models.StoredProcedures.Response
{
    public class sp_getPropertyByID
    {
        public string DistrictName { get; set; }
        public string EstateName { get; set; }
        public string Block { get; set; }
        public string Floor { get; set; }
        public string Flat { get; set; }
        public int GrossFloorArea { get; set; }
        public int NumberOfBedrooms { get; set; }
        public bool CarParkProvided { get; set; }
        public int? SellingPrice { get; set; }
        public int? RentalPrice { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhoneNumber { get; set; }
    }
}