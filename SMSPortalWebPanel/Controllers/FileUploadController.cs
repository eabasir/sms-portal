using Newtonsoft.Json;
using SMSPortalWebPanel.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thought.vCards;

namespace SMSPortalWebPanel.Controllers
{
    [PortalAuthorizatoin]
    public class FileUploadController : Controller
    {


        class UploadResults
        {

            public List<Person> Persons { get; set; }
            public string DuplicatesLink { get; set; }
            public string NonMobileLink { get; set; }

        }

        class Errors
        {

            public string FullName { get; set; }
            public string Number { get; set; }
        }


        class Person
        {
            public string Name { get; set; }
            public string Family { get; set; }
            public string Email { get; set; }
            public List<string> Numbers { get; set; }

        }

        [HttpPost]
        public ActionResult getFileData()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        string extension = Path.GetExtension(fname);

                        if (extension.ToLower() != ".xls" && extension.ToLower() != ".xlsx" && extension.ToLower() != ".vcf")
                            return null;

                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);

                        List<Person> lstPerson = new List<Person>();
                        List<Errors> lstDuplicates = new List<Errors>();
                        List<Errors> nonMobiles = new List<Errors>();

                        UploadResults result = new UploadResults();

                        if (extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx")
                        {

                            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fname + ";Extended Properties=Excel 12.0;";
                            OleDbConnection MyConnection;
                            DataSet ds;
                            OleDbDataAdapter MyCommand;
                            MyConnection = new OleDbConnection(connStr);
                            MyCommand = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                            ds = new DataSet();
                            MyCommand.Fill(ds);
                            MyConnection.Close();
                            System.IO.File.Delete(fname);


                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                string name = ds.Tables[0].Rows[j][0].ToString();
                                string family = ds.Tables[0].Rows[j][1].ToString();
                                string email = ds.Tables[0].Rows[j][2].ToString();
                                string strNumber = ds.Tables[0].Rows[j][3].ToString();

                                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(family))
                                {
                                    List<string> numbers = strNumber.Replace(" ", "").Split(',').ToList().Where(x => x != "").ToList();
                                    for (int k = 0; k < numbers.Count; k++)
                                    {
                                        numbers[k] = SMSPortalCross.Utils.NumberUtils.getFormattedNumber(numbers[k]);
                                    }

                                    nonMobiles.AddRange(numbers
                                              .Where(x => !SMSPortalCross.Utils.NumberUtils.isMobileNumber(x))
                                              .Select(x => new Errors
                                              {
                                                  FullName = name + " " + family,
                                                  Number = x
                                              })
                                              .ToList());

                                    numbers = numbers.Where(x => SMSPortalCross.Utils.NumberUtils.isMobileNumber(x)).ToList();

                                    if (numbers != null && numbers.Count > 0 && !string.IsNullOrEmpty(name.Trim()) && !string.IsNullOrEmpty(family.Trim()))
                                    {

                                        lstPerson.Add(new Person()
                                        {
                                            Name = name.Trim(),
                                            Family = family.Trim(),
                                            Email = email.Trim(),
                                            Numbers = numbers
                                        });
                                    }
                                }

                            }

                        }
                        else if (extension.ToLower() == ".vcf")
                        {

                            List<vCard> vCards = new List<vCard>();
                            string[] lines = System.IO.File.ReadAllLines(fname);
                            string str_vCard = string.Empty;

                            foreach (string line in lines)
                            {
                                if (line == "BEGIN:VCARD")
                                {
                                    str_vCard = "BEGIN:VCARD" + Environment.NewLine;
                                }
                                else if (line == "END:VCARD")
                                {
                                    str_vCard += "END:VCARD";
                                    using (TextReader tr = new StringReader(str_vCard))
                                    {
                                        vCards.Add(new vCard(tr));
                                    }
                                }
                                else
                                    str_vCard += line + Environment.NewLine;
                            }

                            foreach (vCard vcard in vCards)
                            {



                                if (!string.IsNullOrEmpty(vcard.GivenName) && !string.IsNullOrEmpty(vcard.FamilyName) && vcard.Phones.Count > 0)
                                {
                                    Person person = new Person
                                    {
                                        Name = vcard.GivenName,
                                        Family = vcard.FamilyName,
                                        Numbers = (from x in vcard.Phones select x.FullNumber).ToList()
                                    };

                                    for (int k = 0; k < person.Numbers.Count; k++)
                                    {
                                        person.Numbers[k] = SMSPortalCross.Utils.NumberUtils.getFormattedNumber(person.Numbers[k]);
                                    }

                                    nonMobiles.AddRange(person.Numbers
                                        .Where(x => !SMSPortalCross.Utils.NumberUtils.isMobileNumber(x))
                                        .Select(x => new Errors
                                        {
                                            FullName = person.Name + " " + person.Family,
                                            Number = x
                                        })
                                        .ToList());
                                    person.Numbers = person.Numbers.Where(x => SMSPortalCross.Utils.NumberUtils.isMobileNumber(x)).ToList();

                                    if (person.Numbers != null && person.Numbers.Count > 0)
                                        lstPerson.Add(person);

                                }

                            }



                        }



                        var all = lstPerson.SelectMany(x => x.Numbers
                        .Select(y => new Errors
                        {
                            FullName = x.Name + " " + x.Family,
                            Number = SMSPortalCross.Utils.NumberUtils.getFormattedNumber(y)
                        }
                        )).ToList();

                        all = all.GroupBy(x => new { x.FullName, x.Number }).Select(g => g.First()).ToList();

                        var duplicates = all.GroupBy(x => x.Number).Select(y => y.ToList()).Where(z => z.Count > 1).ToList();

                        

   
                        if (duplicates.Count > 0)
                        {

                            foreach (var duplicate in duplicates)
                            {
                                foreach (Errors d in duplicate)
                                {
                                    lstDuplicates.Add(new Errors
                                    {

                                        FullName = d.FullName,
                                        Number = d.Number
                                    });
                                }

                            }

                            result.DuplicatesLink = WriteErrorFile(lstDuplicates , "duplicates");

                        }
                        if (nonMobiles.Count > 0)
                        {
                            result.NonMobileLink = WriteErrorFile(nonMobiles , "nonmobiles");

                        }

                        result.Persons = lstPerson;
                        return Json(JsonConvert.SerializeObject(result));

                    }

                }
                catch (Exception)
                {

                }
            }


            return null;
        }



        private string WriteErrorFile(List<Errors> errors, string type)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Full Name";
                oSheet.Cells[1, 2] = "Number";

                //Format A1:B1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "B1").Font.Bold = true;
                oSheet.get_Range("A1", "B1").VerticalAlignment =
                    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                for (int i = 0; i < errors.Count; i++)
                {

                    oSheet.Cells[i + 2, 1] = errors[i].FullName;
                    oSheet.Cells[i + 2, 2] = errors[i].Number;

                }



                oXL.UserControl = true;

                string name = type + "-" +DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
                string fName = Path.Combine(Server.MapPath("~/Uploads/"), name);


                oWB.SaveAs(fName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();

                return name;


            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}