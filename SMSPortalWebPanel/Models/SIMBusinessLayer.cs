using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSPortalDBDataLibrary;
using System.Data.Entity;
using SMSPortalWebPanel.ViewModels;

namespace SMSPortalWebPanel.Models
{
    public class SIMBusinessLayer
    {

        public List<SIM> GetSims()
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                var q = (from x in db.SIMs select x).ToList();
                return q;

            }
        }



        public bool addNewSim(SIMViewModel simVM)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                try
                {
                    SIM sim = new SIM()
                    {
                        TFId = Guid.NewGuid(),
                        TFNumber = simVM.Number,
                        TFPort = Convert.ToInt32(simVM.Port)
                    };
                    
                    db.SIMs.Add(sim);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;

                }

        }

        public bool updateSim(SIMViewModel simVM)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                try
                {
                    SIM sim = getSimById(simVM.Id);
                    if (sim != null)
                    {
                        sim.TFNumber = simVM.Number;
                        sim.TFPort = Convert.ToInt32(simVM.Port);
                        db.Entry(sim).State = EntityState.Modified;
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                catch
                {
                    return false;
                }

            }
        }


        public SIM getSimById(Guid id)
        {

            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                SIM sim = (from x in db.SIMs where x.TFId == id select x).FirstOrDefault();
                return sim;
            }

        }


        public SIM getSimByNumber(string _number)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                SIM sim =  (from x in db.SIMs where x.TFNumber == _number select x).FirstOrDefault();
                return sim;
            }

        }


        public void deleteSimById(Guid _id)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    SIM sim = (from x in db.SIMs where x.TFId == _id select x).FirstOrDefault();
                    db.SIMs.Remove(sim);
                    db.SaveChanges();
                }
            }
            catch { }
        }


    }
}
