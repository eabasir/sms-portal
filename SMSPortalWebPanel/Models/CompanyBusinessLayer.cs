using SMSPortalDBDataLibrary;
using SMSPortalWebPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.Models
{
    public class CompanyBusinessLayer
    {

        public List<Company> getCompanies()
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    return db.Companies.Select(x => x).ToList();
                }
            }
            catch { }
            return null;
        }

        public List<CompanyViewModel> GetCompanyVMs()
        {
            try
            {
                List<CompanyViewModel> lstCompanyVM = new List<CompanyViewModel>();
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var q = (from x in db.Companies select x).ToList();

                    foreach (Company company in q)
                    {
                        CompanyViewModel CompanyVM = new CompanyViewModel()
                        {
                            Id = company.TFId,
                            Name = company.TFName,
                            Address = company.TFAddress != null ? company.TFAddress : "-",
                            Number = company.TFPhone != null ? company.TFPhone : "-"

                        };
                        lstCompanyVM.Add(CompanyVM);

                    }

                    return lstCompanyVM;

                }
            }
            catch (Exception e)
            {
            }
            return null;
        }


        public bool addNewCompany(CompanyViewModel companyViewModel , List<Guid> newContactId)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                try
                {

                    Guid newCompanyId = Guid.NewGuid();
                    Company Company = new Company()
                    {
                        TFId = newCompanyId,
                        TFName = companyViewModel.Name,
                        TFAddress = companyViewModel.Address,
                        TFPhone = companyViewModel.Number,

                    };

                    db.Companies.Add(Company);
                   
                    if (newContactId != null)
                    {
                        foreach (Guid contactId in newContactId)
                        {
                            db.Contact_Company.Add(new Contact_Company
                            {
                                TFId = Guid.NewGuid(),
                                TFCompany_Id = newCompanyId,
                                TFContact_Id = contactId
                            });

                        }
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;

                }

        }

        public bool updateCompany(CompanyViewModel companyViewModel , List<Guid> newContactId)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                try
                {
                    Company Company = getCompanyById(companyViewModel.Id);
                    if (Company != null)
                    {
                        Company.TFName = companyViewModel.Name;
                        Company.TFAddress = companyViewModel.Address;
                        Company.TFPhone = companyViewModel.Number;
                        db.Entry(Company).State = EntityState.Modified;

                        var preContact_Company = (from x in db.Contact_Company where x.TFCompany_Id == companyViewModel.Id select x).ToList();
                        db.Contact_Company.RemoveRange(preContact_Company);

                        if (newContactId != null)
                        {
                            foreach (Guid contactId in newContactId)
                            {
                                db.Contact_Company.Add(new Contact_Company
                                {
                                    TFId = Guid.NewGuid(),
                                    TFCompany_Id = companyViewModel.Id,
                                    TFContact_Id = contactId
                                });

                            }

                        }
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }


        public Company getCompanyById(Guid id)
        {

            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                Company Company = db.Companies.FirstOrDefault(x => x.TFId == id);
                return Company;
            }

        }

        public void deleteCompanyById(Guid _id)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    Company Company = db.Companies.FirstOrDefault(x => x.TFId == _id);
                    db.Companies.Remove(Company);

                    db.SaveChanges();
                }
            }
            catch { }
        }

        public List<ContactViewModel> getCompanyContactVms(Guid _id)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    List<ContactViewModel> lstContactVm = (from x in db.Contact_Company
                                                           where x.TFCompany_Id == _id
                                                           select new ContactViewModel()
                                                           {
                                                               Id = x.TFContact_Id,
                                                               Name = x.Contact.TFName,
                                                               Family = x.Contact.TFFamily,
                                                               Numbers = (from y in db.Phones where y.TFContact_Id == x.TFContact_Id select y.TFNumber).ToList()
                                                           }).ToList();
                    return lstContactVm;
                }
            }
            catch (Exception e)
            {
            }
            return null;
        }



        public object getCompanyContacts(Guid _id)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                   var q = (from p in db.Phones
                             join c in
                                 (from x in db.Contact_Company where x.TFCompany_Id == _id select x.Contact)
                             on p.TFContact_Id equals c.TFId
                             select new
                             {
                                 Name = c.TFName + " " + c.TFFamily,
                                 Number = p.TFNumber
                             }).ToList();

                    return q.ToList();

                }
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public List<ContactViewModel> GetOutsideContactVMs(Guid? Id)
        {
            try
            {
                List<ContactViewModel> lstContactVM = new List<ContactViewModel>();
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    List<Contact> lstContacts = null;
                    if (Id != null)
                    {
                        lstContacts = (from x in db.Contacts
                                       where
                                  !(from y in db.Contact_Company where y.TFCompany_Id == Id select y.TFContact_Id).Contains(x.TFId)
                                       select x).ToList();
                    }
                    else
                        lstContacts = (from x in db.Contacts select x).ToList();
                    foreach (Contact contact in lstContacts)
                    {
                        ContactViewModel contactVM = new ContactViewModel()
                        {
                            Id = contact.TFId,
                            Name = contact.TFName,
                            Family = contact.TFFamily,
                            Email = contact.TFEmail != null ? contact.TFEmail : "-",
                            Numbers = (from x in db.Phones where x.TFContact_Id == contact.TFId select x.TFNumber).ToList()

                        };
                        lstContactVM.Add(contactVM);

                    }

                    return lstContactVM;

                }
            }
            catch (Exception e)
            {
            }
            return null;
        }
    }


}
