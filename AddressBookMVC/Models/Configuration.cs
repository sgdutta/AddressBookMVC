using AddressBookMVC.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
                allAddresses = addressDb.AddressDetails.Include("AddressDetail").Where(w => w.Deleted==false)
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

        public async Task<IEnumerable<AddressModel>> GetAllAddressesAsync()
        {
           // IList<AddressModel> allAddresses = null;

            var query = (from s in addressDb.AddressDetails.Where(w => w.Deleted == false)
                         select new AddressModel()
                        {
                            InfoId = s.InfoId,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            emailaddress = s.emailaddress,
                            Zip = s.Zip
                        }).ToListAsync();
 

            return await query;

        }

        public AddressModel GetAddressesById(int id)
        {

            AddressDetail addressContext = addressDb.AddressDetails.Where(w => w.InfoId == id && w.Deleted==false).FirstOrDefault();
            ICollection<Audit> auditContext = addressDb.Audits.Where(w => w.InfoId == id).ToList();
            ICollection<Audit> audits = addressDb.Audits.Where(w => w.InfoId == id).ToList();
            AddressModel addrModel = new AddressModel();

            if(addressContext!=null)
            {
                addrModel.InfoId = addressContext.InfoId;
                addrModel.FirstName = addressContext.FirstName;
                addrModel.LastName = addressContext.LastName;
                addrModel.emailaddress = addressContext.emailaddress;
                addrModel.Zip = addressContext.Zip;
                addrModel.Audits = audits
                        .Select(s => new AuditModel()
                        {
                            Updates = s.Updates,
                            UpdatedBy = s.UpdatedBy,
                            UpdatedOn = s.UpdatedOn

                        }).ToList();
            }


            return addrModel;

        }

        public async Task<AddressModel> GetAddressesByIdAsync(int id)
        {
            ICollection<Audit> auditContext = addressDb.Audits.Where(w => w.InfoId == id).ToList();
            var query = (from s in addressDb.AddressDetails
                         where s.InfoId == id && s.Deleted == false
                         select new AddressModel()
                         {
                             InfoId = s.InfoId,
                             FirstName = s.FirstName,
                             LastName = s.LastName,
                             emailaddress = s.emailaddress,
                             Zip = s.Zip,
                             Audits = ConvertAuditDBtoAuditModel(auditContext).ToList()
                         }).SingleOrDefaultAsync();
            
            return await query; 

            //return addrModel;

        }

        private ICollection<AuditModel> ConvertAuditDBtoAuditModel(ICollection<Audit> audits)
        {
            IList<AuditModel> auditModels = null;
            auditModels = audits.Select(s=>new AuditModel()
            {
                Updates = s.Updates,
                UpdatedBy = s.UpdatedBy,
                UpdatedOn = s.UpdatedOn

            }).ToList();
            return auditModels;
        }

        public IEnumerable<AddressModel> GetAddressesBySearch(string s)
        {
            IList<AddressModel> allAddresses = null;
            if (s != null && Regex.IsMatch(s, @"^\d+$"))
            {
                int id = Convert.ToInt32(s);

                    allAddresses = addressDb.AddressDetails.Include("AddressDetail").Where(w=>w.InfoId==id && w.Deleted == false)
                        .Select(a => new AddressModel()
                        {
                            InfoId = a.InfoId,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            emailaddress = a.emailaddress,
                            Zip = a.Zip

                        }).ToList();

            }

            else
            {
                    allAddresses = addressDb.AddressDetails.Include("AddressDetail").Where(w => w.emailaddress.Contains(s) && w.Deleted == false)
                        .Select(a => new AddressModel()
                        {
                            InfoId = a.InfoId,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            emailaddress = a.emailaddress,
                            Zip = a.Zip

                        }).ToList();
            }


            return allAddresses;

        }

       

        public void CreateAddress(AddressDetail addr)
        {
            //AddressModel addrModel = new AddressModel();
            //AddressDetail addrsDtl = new AddressDetail();
            //addrsDtl.FirstName = addr.FirstName;
            //addrsDtl.LastName = addr.LastName;
            //addrsDtl.Zip = addr.Zip;
            //addrsDtl.emailaddress = addr.emailaddress;

            addr.Deleted = false;
            addressDb.AddressDetails.Add(addr);
            addressDb.SaveChanges();

            Audit auditDb = new Audit();
            auditDb.InfoId = addressDb.AddressDetails.Max(m => m.InfoId);
            auditDb.UpdatedBy = "System";//It can be replaced by users
            auditDb.Updates = "Created";
            auditDb.UpdatedOn = DateTime.Today;

            addressDb.Audits.Add(auditDb);
            addressDb.SaveChanges();

            return ;
        }

        public async Task<string> CreateAddressAsync(AddressDetail addr)
        {
            addr.Deleted = false;
            addressDb.AddressDetails.Add(addr);
            addressDb.SaveChanges();

            Audit auditDb = new Audit();
            auditDb.InfoId = addressDb.AddressDetails.Max(m => m.InfoId);
            auditDb.UpdatedBy = "System";//It can be replaced by users
            auditDb.Updates = "Created";
            auditDb.UpdatedOn = DateTime.Today;

            addressDb.Audits.Add(auditDb);
            await addressDb.SaveChangesAsync();

            return "Address Creatd";
        }

        public void EditAddress(AddressModel addr)
        {
            AddressDetail addrContext = addressDb.AddressDetails.Where(w => w.InfoId == addr.InfoId).FirstOrDefault();
            addrContext.FirstName = addr.FirstName;
            addrContext.LastName = addr.LastName;
            addrContext.Zip = addr.Zip;
            addrContext.emailaddress = addr.emailaddress;

            addressDb.SaveChanges();

            //Enter new audit for updates
            Audit auditDb = new Audit();
            auditDb.InfoId = addr.InfoId;
            auditDb.UpdatedBy = "Test";//It can be replaced by users
            auditDb.Updates = "Updated";
            auditDb.UpdatedOn = DateTime.Today;

            addressDb.Audits.Add(auditDb);
            addressDb.SaveChanges();

            return;
        }

        public void DeleteAddress(int id)
        {
            AddressDetail addrContext = addressDb.AddressDetails.Where(w => w.InfoId == id).FirstOrDefault();
            IList<Audit> auditContext = addressDb.Audits.Where(w => w.InfoId == id).ToList();

            //foreach(var aud in auditContext)
            //{
            //    addressDb.Audits.Remove(aud);
            //}

            addrContext.Deleted = true;
            //addressDb.SaveChanges();

            //Enter new audit for updates
            Audit auditDb = new Audit();
            auditDb.InfoId = id;
            auditDb.UpdatedBy = "Test"; //It can be replaced by users
            auditDb.Updates = "Deleted";
            auditDb.UpdatedOn = DateTime.Today;

            addressDb.Audits.Add(auditDb);
            addressDb.SaveChanges();

            return;
        }
    }
}