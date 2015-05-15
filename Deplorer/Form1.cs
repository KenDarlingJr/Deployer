using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Deplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void currentTime_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToLongTimeString();
            if (checkBox1.Checked)
                checkTimes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            currentTime.Enabled = true;
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker1.ShowUpDown = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblSelectedTime.Text = dateTimePicker1.Value.ToLongTimeString();
            saveItem();
            //checkBox1.Checked = true;

        }

        private void checkTimes() 
        {
            DateTime selTime = DateTime.Parse(lblSelectedTime.Text, System.Globalization.CultureInfo.CurrentCulture);

            int result = DateTime.Compare(DateTime.Now, selTime);

            string strSource = "C:\\Staging\\Receiving Insp FE  1.4.accdb";
            string strDest = "C:\\Staging\\Receiving Insp FE.accdb";

            if (result == 1)
            {
                //Trigger the event here
                File.Copy(strSource, strDest, true);
                lblTimeMatch.Text = "Trigger Event";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileName;
            DialogResult result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                txtSource.Text = fileName;
                //txtDestFile.Text = fileName.LastIndexOf("\\").ToString();
                txtDestFile.Text = fileName.Substring(fileName.LastIndexOf("\\")+1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtDestPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void saveItem()
        {

            if (File.Exists("C:\\Staging\\myFile.xml") == false)
            {
                XmlWriterSettings xmlSettings = new XmlWriterSettings();
                xmlSettings.Indent = true;
                xmlSettings.NewLineOnAttributes = true;

                using (XmlWriter writer = XmlWriter.Create("C:\\Staging\\myFile.xml",xmlSettings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Items");

                    writer.WriteStartElement("Item");
                    writer.WriteElementString("Source", txtSource.Text);
                    writer.WriteElementString("Destination", txtDestPath.Text + "\\" + txtDestFile.Text);
                    writer.WriteElementString("Time", lblSelectedTime.Text);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            else
            {
                XDocument xDoc = XDocument.Load("C:\\Staging\\myFile.xml");
                XElement root = xDoc.Element("Items");
                IEnumerable<XElement> rows = root.Descendants("Item");
                XElement firstRow = rows.First();
                firstRow.AddBeforeSelf(
                    new XElement("Item", 
                    new XElement("Source", txtSource.Text),
                    new XElement("Destination", txtDestPath.Text + "\\" + txtDestFile.Text),
                    new XElement("Time", lblSelectedTime.Text)));

                xDoc.Save("C:\\Staging\\myFile.xml");
            }

        }
    }
}
