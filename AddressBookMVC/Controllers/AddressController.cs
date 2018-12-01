using AddressBookMVC.DAL;
using AddressBookMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AddressBookMVC.Controllers
{
    public class AddressController : Controller
    {
        //AddressBookEntities addressDb = new AddressBookEntities();
        Configuration config = new Configuration();
        // GET: Address
        //public ActionResult Index(int? id)
        //{
        //    //IList<AddressModel> allAddresses = null;
        //    //AddressModel addr = null;

        //    if (id >= 0)
        //    {
        //        return View(config.GetAddressesById((int)id));
        //    }
        //    else
        //    {
        //        return View(config.GetAllAddresses().ToList());
        //    }
        //}

        //public ActionResult Index(string searchString)
        //{
            
        //    if (searchString!=null)
        //    {
        //        return View(config.GetAddressesBySearch(searchString));
        //    }
        //    else
        //    {
        //        return View(config.GetAllAddresses().ToList());
        //    }
        //}

        //public ActionResult Index(string searchString)
        //{

        //    if (searchString != null)
        //    {
        //        return View(config.GetAddressesBySearch(searchString));
        //    }
        //    else
        //    {
        //        return View(config.GetAllAddresses().ToList());
        //    }
        //}

        public async Task<ActionResult> Index()
        {

            //var data = await config.GetAllAddressesAsync();
            return View(await config.GetAllAddressesAsync());
        }

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddressModel  address = config.GetAddressesById((int)id);
            if (address.InfoId<1)
            {
                return RedirectToAction("Error");
            }
            return View(address);

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Create(AddressDetail addr)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        config.CreateAddress(addr);
        //        return RedirectToAction("Index");
        //    }
        //    return View(addr);
        //}

        [HttpPost]
        public async Task<ActionResult> Create(AddressDetail addr)
        {
            if (ModelState.IsValid)
            {
                await config.CreateAddressAsync(addr);
                return RedirectToAction("Index");
            }
            return View(addr);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddressModel address = config.GetAddressesById((int)id);
            if (address == null)
            {
                return RedirectToAction("Error");
            }
            return View(address);
        }

        //[HttpGet]
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AddressModel address = await config.GetAddressesByIdAsync((int)id);
        //    if (address == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(address);
        //}

        [HttpPost]
        public ActionResult Edit(AddressModel addr)
        {
            if (ModelState.IsValid)
            {
                config.EditAddress(addr);
                return RedirectToAction("Index");
            }
            return View(addr);
        }

        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            AddressModel address = config.GetAddressesById((int)id);
            if (address == null)
            {
                return RedirectToAction("Error");
            }
            return View(address);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                config.DeleteAddress(id);
                return RedirectToAction("Index");
            }
            return View();
        }


    }
}