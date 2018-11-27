using AddressBookMVC.DAL;
using AddressBookMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AddressBookMVC.Controllers
{
    public class AddressController : Controller
    {
        //AddressBookEntities addressDb = new AddressBookEntities();
        Configuration config = new Configuration();
        // GET: Address
        public ActionResult Index(int? id)
        {
            //IList<AddressModel> allAddresses = null;
            //AddressModel addr = null;
            Configuration config = new Configuration();

            if (id >= 0)
            {
                return View(config.GetAddressesById((int)id));
            }
            else
            {
                return View(config.GetAllAddresses().ToList());
            }
        }

        public ActionResult Details(int? id)
        {
            //IList<AddressModel> allAddresses = null;
            //AddressModel addr = null;
            //Configuration config = new Configuration();
            //if(ModelState.IsValid)
            //{
            //    RedirectToAction("Details");
            //}
                
            //    return View(config.GetAddressesById((int)id));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddressModel  address = config.GetAddressesById((int)id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AddressDetail addr)
        {
            if (ModelState.IsValid)
            {
                config.CreateAddress(addr);
                return RedirectToAction("Index");
            }
            return View(addr);
        }


    }
}