using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MyProject.vCard;

namespace testVCardReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
               textBox1.Text = File.ReadAllText(openFileDialog1.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            vCardReader vc = new vCardReader();
            vc.ParseLines(textBox1.Text);
            textBox2.Clear();
            textBox2.AppendText("Full Name=" + vc.FormattedName+ Environment.NewLine);
            textBox2.AppendText("Names=" + vc.Surname + ", " + vc.GivenName + ". " + vc.MiddleName+ Environment.NewLine);
            textBox2.AppendText("Title=" + vc.Title+ Environment.NewLine);
            if (!String.IsNullOrEmpty(vc.Prefix))
                textBox2.AppendText("Prefix=" + vc.Prefix+ Environment.NewLine);
            if (!String.IsNullOrEmpty(vc.Prefix))
                textBox2.AppendText("Suffix=" + vc.Suffix+ Environment.NewLine);

            if (vc.Birthday != null)
                textBox2.AppendText("Birthday=" + vc.Birthday.ToLongDateString() + Environment.NewLine);
            if (vc.Rev != null)
                textBox2.AppendText("Rev=" + vc.Rev.ToLongDateString()+" "+vc.Rev.ToLongTimeString() + Environment.NewLine);

            if (!String.IsNullOrEmpty(vc.Org))
                textBox2.AppendText("Org=" + vc.Org + Environment.NewLine);


            for (int i = 0; i < vc.Phones.Length; i++)
            {
                textBox2.AppendText("Phone " + vc.Phones[i].phoneType.ToString() + " " + vc.Phones[i].homeWorkType.ToString() + (vc.Phones[i].pref ? "Preferred" : "") + "=" + vc.Phones[i].number + Environment.NewLine);
            }
            for (int i = 0; i < vc.Emails.Length; i++)
            {
                textBox2.AppendText("Email " + vc.Emails[i].homeWorkType.ToString() + " " + (vc.Emails[i].pref?"Preferred":"") + "=" + vc.Emails[i].address + Environment.NewLine);
            }

            for (int i = 0; i < vc.Addresses.Length; i++)
            {
                textBox2.AppendText("Address " + vc.Addresses[i].homeWorkType.ToString() + "=" + vc.Addresses[i].po + ","
                + vc.Addresses[i].ext + ", "
                + vc.Addresses[i].street + ", "
                + vc.Addresses[i].locality + ", "
                + vc.Addresses[i].region + ", "
                + vc.Addresses[i].postcode + ", "
                + vc.Addresses[i].country + Environment.NewLine);
            }

            if (!String.IsNullOrEmpty(vc.Note))
                textBox2.AppendText("Note="+vc.Note);

           
        }

        
    }
}