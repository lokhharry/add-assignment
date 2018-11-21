using add_assignment.Manager;
using add_assignment.Models.Api.Response;
using add_assignment.Models.StoredProcedures.Response;
using add_assignment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace add_assignment.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            List<sp_getDistrict> spGetDistrict = await StoredProceduresManager.spGetDistrict();
            return View("~/Views/Home/Index.cshtml", new ViewModel_Index()
            {
                listDistrict = spGetDistrict
            });
        }

        public ActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml");
        }

        [System.Web.Http.HttpGet]
        public async Task<JsonResult> getEstate(int districtID)
        {
            List<sp_getEstateByDistrict> spGetEstateByDistrict = await StoredProceduresManager.spGetEstateByDistrict(districtID);
            return Json(spGetEstateByDistrict.Select(e => new Response_HomeGetEstate()
            {
                ID = e.ID,
                Name = e.Name
            }), JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpGet]
        public async Task<JsonResult> getProperty(int estateID)
        {
            List<sp_getPropertyByEstate> spGetPropertyByEstate = await StoredProceduresManager.spGetPropertyByEstate(estateID);
            return Json(spGetPropertyByEstate.Select(e => new Response_HomeGetProperty()
            {
                ID = e.ID,
                DistrictName = e.DistrictName,
                EstateName = e.EstateName,
                Block = e.Block,
                Floor = e.Floor,
                Flat = e.Flat,
                GrossFloorArea = e.GrossFloorArea,
                NumberOfBedroom = e.NumberOfBedroom,
                CarParkProvided = e.CarParkProvided,
                SellingPrice = e.SellingPrice,
                RentalPrice = e.RentalPrice
            }), JsonRequestBehavior.AllowGet);
        }
    }
};