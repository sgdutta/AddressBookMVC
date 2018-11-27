using AddressBookMVC.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddressBookMVC.Models
{
    public class Configuration
    {
        AddressBookEntities addressDb = new AddressBookEntities();

        public IEnumerable<AddressModel> GetAllAddresses()
        {
            IList<AddressModel> allAddresses = null;

            using (var context = new AddressBookEntities())
            {
                allAddresses = addressDb.AddressDetails.Include("AddressDetail")
                    .Select(s => new AddressModel()
                    {
                        InfoId = s.InfoId,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        emailaddress = s.emailaddress,
                        Zip = s.Zip

                    }).ToList();
            }

            return allAddresses;

        }

        public AddressModel GetAddressesById(int id)
        {

           // AddressModel address = null;
            AddressDetail addressContext = addressDb.AddressDetails.Where(w => w.InfoId == id).FirstOrDefault();
            ICollection<Audit> auditContext = addressDb.Audits.Where(w => w.InfoId == id).ToList();
            ICollection<Audit> audits = addressDb.Audits.Where(w => w.InfoId == id).ToList();
            AddressModel addrModel = new AddressModel();
            //using (var context = new AddressBookEntities())
            //{
            //    address = addressDb.AddressDetails.Include("AddressDetail").Where(w=>w.InfoId==id).ToList()
            //        .Select(s => new AddressModel()
            //        {
            //            InfoId = s.InfoId,
            //            FirstName = s.FirstName,
            //            LastName = s.LastName,
            //            emailaddress = s.emailaddress,
            //            Zip = s.Zip

            //        }).FirstOrDefault();
            //}

            addrModel.InfoId = addressContext.InfoId;
            addrModel.FirstName = addressContext.FirstName;
            addrModel.LastName = addressContext.LastName;
            addrModel.emailaddress = addressContext.emailaddress;
            addrModel.Zip = addressContext.Zip;
            addrModel.Audits = audits
                    .Select(s => new AuditModel()
                    {
                        Updates= s.Updates,
                        UpdatedBy = s.UpdatedBy,
                        UpdatedOn = s.UpdatedOn

                    }).ToList();

            return addrModel;

        }

        //public AddressModel CreateAddress(AddressModel addr)
        //{
        //    AddressModel addrModel = new AddressModel();
        //    AddressDetail addrsDtl = new AddressDetail();
        //    addrsDtl.FirstName = addr.FirstName;
        //    addrsDtl.LastName = addr.LastName;
        //    addrsDtl.Zip = addr.Zip;
        //    addrsDtl.emailaddress = addr.emailaddress;


        //    addressDb.AddressDetails.Add(addrsDtl);
        //    addressDb.SaveChanges();

        //    Audit auditDb = new Audit();
        //    auditDb.InfoId = addressDb.AddressDetails.Max(m => m.InfoId);
        //    auditDb.UpdatedBy = "System";
        //    auditDb.Updates = "Created";
        //    auditDb.UpdatedOn = DateTime.Today;

        //    addressDb.Audits.Add(auditDb);
        //    addressDb.SaveChanges();

        //    return addrModel;
        //}

        public void CreateAddress(AddressDetail addr)
        {
            //AddressModel addrModel = new AddressModel();
            //AddressDetail addrsDtl = new AddressDetail();
            //addrsDtl.FirstName = addr.FirstName;
            //addrsDtl.LastName = addr.LastName;
            //addrsDtl.Zip = addr.Zip;
            //addrsDtl.emailaddress = addr.emailaddress;


            addressDb.AddressDetails.Add(addr);
            addressDb.SaveChanges();

            Audit auditDb = new Audit();
            auditDb.InfoId = addressDb.AddressDetails.Max(m => m.InfoId);
            auditDb.UpdatedBy = "System";
            auditDb.Updates = "Created";
            auditDb.UpdatedOn = DateTime.Today;

            addressDb.Audits.Add(auditDb);
            addressDb.SaveChanges();

            return ;
        }
    }
}