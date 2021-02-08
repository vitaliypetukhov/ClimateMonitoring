using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;

namespace ClimateMonitoring
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            string path = @"\\WINSERVER\data\measurement.db3";
            string path1 = System.Environment.CurrentDirectory + @"\measurement.db3";

            string info = File.GetLastWriteTime(path).ToString();
            infolabel.Text = "Информация на " + info; 

           // MessageBox.Show(File.GetLastWriteTime(path).ToString());

            FileInfo check = new FileInfo(path1);
           
            if(check.Exists)
            {
                File.Delete(path1);
                File.Copy(path, path1, true);               
            }
            else
            {
                File.Copy(path, path1, true);                
            }

            //MessageBox.Show(path1);
          
            using (SQLiteConnection connections = new SQLiteConnection(@"Data Source="+path1))
            {
                connections.Open();
                /*
                 bool test1 = PodBatlabel.Text.Substring(0, 1).Equals("1");
                    if (test1 == true)
                    {
                        PodBatlabel.ForeColor = Color.Red;
                    }
                    else
                    {
                        PodBatlabel.ForeColor = Color.Green;
                    }
                */

                SQLiteCommand command = new SQLiteCommand
                    (
                    "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id > 0 group by sensor_id",
                    connections
                    );
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["sensor_id"].ToString() == "2")
                    {
                        //MessageBox.Show("111");
                        ELlabel1.Text = null;
                        SNELlabel.Text = null;
                        ElTemplabel.Text = null;
                        ElWetlabel.Text = null;
                        ElBatlabel.Text = null;

                        //MessageBox.Show(date.ToString());

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        ELlabel1.Text += reader["name"]+" ("+s2+")";
                        
                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNELlabel.Text += date_last_update.ToString();

                        ElTemplabel.Text += reader["temperature"] + " °C";
                        ElWetlabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ElBatlabel.Text += s4 + " V";// reader["battery"];
                    }
                    else if (reader["sensor_id"].ToString() == "3")
                    {
                        Fizhimlabel1.Text = null;
                        SNFizhimlabel.Text = null;
                        FizhimTemplabel.Text = null;
                        FizhimWetlabel.Text = null;
                        FizhimBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Fizhimlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNFizhimlabel.Text += date_last_update.ToString();

                        /*Fizhimlabel1.Text += reader["name"];

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        SNFizhimlabel.Text += s2; */
                        FizhimTemplabel.Text += reader["temperature"] + " °C";
                        FizhimWetlabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        FizhimBatlabel.Text += s4 + " V";// reader["battery"];
                    }
                    else if (reader["sensor_id"].ToString() == "4")
                    {
                        Podlabel1.Text = null;
                        SNPodlabel.Text = null;
                        PodTemplabel.Text = null;
                        PodWetlabel.Text = null;
                        PodBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Podlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNPodlabel.Text += date_last_update.ToString();

                        /*Podlabel1.Text += reader["name"];

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        SNPodlabel.Text += s2;*/
                        PodTemplabel.Text += reader["temperature"] + " °C";
                        PodWetlabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        PodBatlabel.Text += s4 + " V";
                    }
                    else if (reader["sensor_id"].ToString() == "5")
                    {
                        Manlabel1.Text = null;
                        SNManlabel.Text = null;
                        ManTemplabel.Text = null;
                        ManWetlabel.Text = null;
                        ManBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Manlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNManlabel.Text += date_last_update.ToString();

                        /* Manlabel1.Text += reader["name"];

                         String s = reader["EUI64"].ToString();
                         String s2 = s.Substring(s.Length - 7);

                         SNManlabel.Text += s2; */
                        ManTemplabel.Text += reader["temperature"] + " °C";
                        ManWetlabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ManBatlabel.Text += s4 + " V";// reader["battery"];

                    }
                    else if (reader["sensor_id"].ToString() == "8")
                    {
                        Teplabel1.Text = null;
                        SNTeplabel.Text = null;
                        TempTeplabel.Text = null;
                        WetTeplabel.Text = null;
                        BatTeplabel.Text = null;

                        /*Teplabel1.Text += reader["name"];
                        if (Teplabel1.Text == "теплотехника")
                        {
                            Teplabel1.Text = null;
                            Teplabel1.Text += "Теплотехника";
                        }

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        SNTeplabel.Text += s2;*/

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Teplabel1.Text += reader["name"];
                        if (Teplabel1.Text == "теплотехника")
                        {
                            Teplabel1.Text = null;
                            Teplabel1.Text += "Теплотехника" + " (" + s2 + ")";
                        }
                        

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNTeplabel.Text += date_last_update.ToString();

                        TempTeplabel.Text += reader["temperature"] + " °C";
                        WetTeplabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatTeplabel.Text += s4 + " V";

                    }
                    else if (reader["sensor_id"].ToString() == "10")
                    {
                        Prilabel1.Text = null;
                        SNPrilabel.Text = null;
                        TempPrilabel.Text = null;
                        WetPrilabel.Text = null;
                        BatPrilabel.Text = null;

                        /*Prilabel1.Text += reader["name"];

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        SNPrilabel.Text += s2;*/

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Prilabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNPrilabel.Text += date_last_update.ToString();

                        TempPrilabel.Text += reader["temperature"] + " °C";
                        WetPrilabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatPrilabel.Text += s4 + " V";// reader["battery"];
                    }             

                }

                //File.Create(path1).Close();
                
                connections.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {           

            string path = @"\\WINSERVER\data\measurement.db3";
            string path1 = System.Environment.CurrentDirectory + @"\measurement.db3";

            string info = File.GetLastWriteTime(path).ToString();
            infolabel.Text = "Информация на " + info;


            FileInfo check = new FileInfo(path1);
            if (check.Exists)
            {
               // File.Delete(path1);
                File.Copy(path, path1, true);
                //File.Create(path1).Close();
            }
            else
            {
                File.Copy(path, path1, true);
               // File.Create(path1).Close();
            }

            //MessageBox.Show(path1);

            using (SQLiteConnection connections = new SQLiteConnection(@"Data Source=" + path1))
            {
                connections.Open();
                /*
                 bool test1 = PodBatlabel.Text.Substring(0, 1).Equals("1");
                    if (test1 == true)
                    {
                        PodBatlabel.ForeColor = Color.Red;
                    }
                    else
                    {
                        PodBatlabel.ForeColor = Color.Green;
                    }
                */

                SQLiteCommand command = new SQLiteCommand
                    (
                    "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id > 0 group by sensor_id",
                    connections
                    );
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["sensor_id"].ToString() == "2")
                    {
                        //MessageBox.Show("111");
                        ELlabel1.Text = null;
                        SNELlabel.Text = null;
                        ElTemplabel.Text = null;
                        ElWetlabel.Text = null;
                        ElBatlabel.Text = null;

                        //MessageBox.Show(date.ToString());

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        ELlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNELlabel.Text += date_last_update.ToString();

                        ElTemplabel.Text += reader["temperature"] + " °C";
                        ElWetlabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ElBatlabel.Text += s4 + " V";// reader["battery"];
                    }
                    else if (reader["sensor_id"].ToString() == "3")
                    {
                        Fizhimlabel1.Text = null;
                        SNFizhimlabel.Text = null;
                        FizhimTemplabel.Text = null;
                        FizhimWetlabel.Text = null;
                        FizhimBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Fizhimlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNFizhimlabel.Text += date_last_update.ToString();

                        /*Fizhimlabel1.Text += reader["name"];

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        SNFizhimlabel.Text += s2; */
                        FizhimTemplabel.Text += reader["temperature"] + " °C";
                        FizhimWetlabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        FizhimBatlabel.Text += s4 + " V";// reader["battery"];
                    }
                    else if (reader["sensor_id"].ToString() == "4")
                    {
                        Podlabel1.Text = null;
                        SNPodlabel.Text = null;
                        PodTemplabel.Text = null;
                        PodWetlabel.Text = null;
                        PodBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Podlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNPodlabel.Text += date_last_update.ToString();

                        /*Podlabel1.Text += reader["name"];

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        SNPodlabel.Text += s2;*/
                        PodTemplabel.Text += reader["temperature"] + " °C";
                        PodWetlabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        PodBatlabel.Text += s4 + " V";
                    }
                    else if (reader["sensor_id"].ToString() == "5")
                    {
                        Manlabel1.Text = null;
                        SNManlabel.Text = null;
                        ManTemplabel.Text = null;
                        ManWetlabel.Text = null;
                        ManBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Manlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNManlabel.Text += date_last_update.ToString();

                        /* Manlabel1.Text += reader["name"];

                         String s = reader["EUI64"].ToString();
                         String s2 = s.Substring(s.Length - 7);

                         SNManlabel.Text += s2; */
                        ManTemplabel.Text += reader["temperature"] + " °C";
                        ManWetlabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ManBatlabel.Text += s4 + " V";// reader["battery"];

                    }
                    else if (reader["sensor_id"].ToString() == "8")
                    {
                        Teplabel1.Text = null;
                        SNTeplabel.Text = null;
                        TempTeplabel.Text = null;
                        WetTeplabel.Text = null;
                        BatTeplabel.Text = null;

                        /*Teplabel1.Text += reader["name"];
                        if (Teplabel1.Text == "теплотехника")
                        {
                            Teplabel1.Text = null;
                            Teplabel1.Text += "Теплотехника";
                        }

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        SNTeplabel.Text += s2;*/

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Teplabel1.Text += reader["name"];
                        if (Teplabel1.Text == "теплотехника")
                        {
                            Teplabel1.Text = null;
                            Teplabel1.Text += "Теплотехника" + " (" + s2 + ")";
                        }


                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNTeplabel.Text += date_last_update.ToString();

                        TempTeplabel.Text += reader["temperature"] + " °C";
                        WetTeplabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatTeplabel.Text += s4 + " V";

                    }
                    else if (reader["sensor_id"].ToString() == "10")
                    {
                        Prilabel1.Text = null;
                        SNPrilabel.Text = null;
                        TempPrilabel.Text = null;
                        WetPrilabel.Text = null;
                        BatPrilabel.Text = null;

                        /*Prilabel1.Text += reader["name"];

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        SNPrilabel.Text += s2;*/

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Prilabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNPrilabel.Text += date_last_update.ToString();

                        TempPrilabel.Text += reader["temperature"] + " °C";
                        WetPrilabel.Text += reader["wetness"] + " %";

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatPrilabel.Text += s4 + " V";// reader["battery"];
                    }

                }

                connections.Close();
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/vitaliy-petukhov-206a3a156/");
        }

        
    }
}

