using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSPortalDBDataLibrary;
using SMSPortalWebPanel.ViewModels;
using System.Data.Entity;
using System.Web.Mvc;

namespace SMSPortalWebPanel.Models
{
    public class ContactBusinessLayer
    {
        public List<ContactViewModel> GetContactVMs()
        {
            try
            {
                List<ContactViewModel> lstContactVM = new List<ContactViewModel>();
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var q = (from x in db.Contacts select x).ToList();

                    foreach (Contact contact in q)
                    {
                        ContactViewModel contactVM = new ContactViewModel()
                        {
                            Id = contact.TFId,
                            Name = contact.TFName,
                            Family = contact.TFFamily,
                            Email = contact.TFEmail != null ? contact.TFEmail : "-",
                            Address = contact.TFAddress != null ? contact.TFAddress : "-",
                            Companies = (from x in db.Contact_Company where x.TFContact_Id == contact.TFId select x.Company.TFName).ToList(),
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



        /// <summary>
        /// The numbers in the list which are used in ths method, passed the validation before.
        /// </summary>
        /// <param name="contactVM"></param>
        /// <returns></returns>
        public bool addNewContact(ContactViewModel contactVM)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                try
                {
                    Contact contact = new Contact()
                    {
                        TFId = Guid.NewGuid(),
                        TFName = contactVM.Name,
                        TFFamily = contactVM.Family,
                        TFAddress = contactVM.Address,
                        TFEmail = contactVM.Email,

                    };
                    if (contactVM.Companies != null)
                    {
                        foreach (string companyName in contactVM.Companies.Distinct())
                        {
                            db.Contact_Company.Add(new Contact_Company
                            {
                                TFId = Guid.NewGuid(),
                                TFContact_Id = contact.TFId,
                                TFCompany_Id = (from x in db.Companies where x.TFName == companyName select x.TFId).FirstOrDefault()
                            });
                        }
                    }
                    db.Contacts.Add(contact);

                    foreach (string number in contactVM.Numbers.Distinct())
                    {
                        string formatedNumber = SMSPortalCross.Utils.NumberUtils.getFormattedNumber(number);
                        Phone prePhone = db.Phones.FirstOrDefault(x => x.TFNumber == formatedNumber);

                        if (prePhone == null)
                        {
                            db.Phones.Add(new Phone()
                            {
                                TFId = Guid.NewGuid(),
                                TFNumber = formatedNumber,
                                TFContact_Id = contact.TFId
                            });
                        }
                        else
                        {
                            prePhone.TFContact_Id = contact.TFId;
                            db.Entry(prePhone).State = EntityState.Modified;
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


        /// <summary>
        /// Numbers in this method are received through ajax post. So the didn't pass validation check.
        /// It must be checked here that whether they are validated formatted numbers or not.
        /// Although, during file upload, validation is executed over numbers. But it must be re-checked
        /// </summary>
        /// <param name="contactGroupAdd"></param>
        /// <returns></returns>
        public bool groupAddContact(ContactGroupAddViewModel contactGroupAdd)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                try
                {
                    foreach (FileContactsViewModel fileContact in contactGroupAdd.Contacts)
                    {
                        try
                        {
                            Contact contact = new Contact()
                            {
                                TFId = Guid.NewGuid(),
                                TFName = fileContact.Name,
                                TFFamily = fileContact.Family,
                                TFEmail = fileContact.Email,
                            };

                            if (contactGroupAdd.Companies != null)
                            {
                                foreach (string companyName in contactGroupAdd.Companies.Distinct())
                                {
                                    db.Contact_Company.Add(new Contact_Company
                                    {
                                        TFId = Guid.NewGuid(),
                                        TFContact_Id = contact.TFId,
                                        TFCompany_Id = (from x in db.Companies where x.TFName == companyName select x.TFId).FirstOrDefault()
                                    });
                                }
                            }
                            db.Contacts.Add(contact);

                            foreach (string number in fileContact.Numbers.Distinct())
                            {
                                if (SMSPortalCross.Utils.NumberUtils.isMobileNumber(number))
                                {

                                    string formatedNumber = SMSPortalCross.Utils.NumberUtils.getFormattedNumber(number);

                                    if (string.IsNullOrEmpty(formatedNumber))
                                        continue;


                                    Phone prePhone = db.Phones.FirstOrDefault(x => x.TFNumber == formatedNumber);

                                    if (prePhone == null)
                                    {
                                        db.Phones.Add(new Phone()
                                        {
                                            TFId = Guid.NewGuid(),
                                            TFNumber = formatedNumber,
                                            TFContact_Id = contact.TFId
                                        });
                                    }
                                    else
                                    {
                                        prePhone.TFContact_Id = contact.TFId;
                                        db.Entry(prePhone).State = EntityState.Modified;
                                    }

                                }
                            }

                            db.SaveChanges();

                        }
                        catch {
                        }
                    }
                    
                    return true;
                }
                catch (Exception e)
                {
                    return false;

                }

        }

        /// <summary>
        /// The number which is used in ths method, passed the validation before.
        /// </summary>
        /// <param name="contactVM"></param>
        /// <returns></returns>
        public bool updateContact(ContactViewModel contactVM)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                try
                {
                    Contact contact = getContactById(contactVM.Id);
                    if (contact != null)
                    {
                        contact.TFName = contactVM.Name;
                        contact.TFFamily = contactVM.Family;
                        contact.TFEmail = contactVM.Email;
                        contact.TFAddress = contactVM.Address;

                        db.Entry(contact).State = EntityState.Modified;


                        var q = (from x in db.Contact_Company where x.TFContact_Id == contact.TFId select x).ToList();
                        db.Contact_Company.RemoveRange(q);

                        if (contactVM.Companies != null)
                        {
                            foreach (string companyName in contactVM.Companies.Distinct())
                            {
                                db.Contact_Company.Add(new Contact_Company
                                {
                                    TFId = Guid.NewGuid(),
                                    TFContact_Id = contact.TFId,
                                    TFCompany_Id = (from x in db.Companies where x.TFName == companyName select x.TFId).FirstOrDefault()
                                });

                            }
                        }

                        List<Phone> lstPrePhones = (from x in db.Phones where x.TFContact_Id == contact.TFId select x).ToList();
                        foreach (Phone phone in lstPrePhones)
                        {
                            phone.TFContact_Id = null;
                            db.Entry(phone).State = EntityState.Modified;
                        }
                        foreach (string number in contactVM.Numbers.Distinct())
                        {
                            string formatedNumber = SMSPortalCross.Utils.NumberUtils.getFormattedNumber(number);

                            Phone phone = (from x in db.Phones where x.TFNumber == formatedNumber select x).FirstOrDefault();
                            if (phone != null)
                            {
                                phone.TFContact_Id = contactVM.Id;
                                db.Entry(phone).State = EntityState.Modified;
                            }
                            else
                            {
                                db.Phones.Add(new Phone()
                                {
                                    TFId = Guid.NewGuid(),
                                    TFNumber = formatedNumber,
                                    TFContact_Id = contactVM.Id
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


        public Contact getContactById(Guid id)
        {

            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                Contact contact = db.Contacts.FirstOrDefault(x => x.TFId == id);
                return contact;
            }

        }

        public ContactAddUpdateViewModel getContactViewModelById(Guid id)
        {

            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                Contact contact = db.Contacts.FirstOrDefault(x => x.TFId == id);


                if (contact != null)
                {

                    return new ContactAddUpdateViewModel()
                    {
                        Id = contact.TFId,
                        Name = contact.TFName,
                        Family = contact.TFFamily,
                        Address = contact.TFAddress,
                        Email = contact.TFEmail,
                        CompanyList = getSelectListItemsForCompanies(),
                        Companies = db.Contact_Company.Where(x => x.TFContact_Id == id).Select(x => x.Company.TFName).ToList(),
                        Numbers = (from x in db.Phones where x.TFContact_Id == contact.TFId select x.TFNumber).ToList()
                    };


                }

                return null;
            }

        }

        public List<SelectListItem> getSelectListItemsForCompanies()
        {
            return new CompanyBusinessLayer().getCompanies().Select(x => new SelectListItem
            {
                Text = x.TFName,
                Value = x.TFId.ToString()
            }).ToList();


        }

        public void deleteContactById(Guid _id)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    Contact contact = db.Contacts.FirstOrDefault(x => x.TFId == _id);
                    db.Contacts.Remove(contact);

                    var lstContactCompany = (from x in db.Contact_Company where x.TFContact_Id == _id select x).ToList();
                    db.Contact_Company.RemoveRange(lstContactCompany);

                    List<Phone> lstPhones = (from x in db.Phones where x.TFContact_Id == _id select x).ToList();

                    foreach (Phone phone in lstPhones)
                    {
                        phone.TFContact_Id = null;

                    }

                    db.SaveChanges();
                }
            }
            catch { }
        }


    }
}
