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

namespace ClimateMonitoring
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string path = @"\\WINSERVER\data\measurement.db3";
            string path1 = System.Environment.CurrentDirectory + @"\measurement.db3";

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

                        ELlabel1.Text += reader["name"];
                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        SNELlabel.Text += s2; //reader["EUI64"];
                        ElTemplabel.Text += reader["temperature"] + " °C";
                        ElWetlabel.Text += reader["wetness"];
                        ElBatlabel.Text += reader["battery"];
                    }
                    else if (reader["sensor_id"].ToString() == "3")
                    {
                        Fizhimlabel1.Text = null;
                        SNFizhimlabel.Text = null;
                        FizhimTemplabel.Text = null;
                        FizhimWetlabel.Text = null;
                        FizhimBatlabel.Text = null;

                        Fizhimlabel1.Text += reader["name"];
                        SNFizhimlabel.Text += reader["EUI64"];
                        FizhimTemplabel.Text += reader["temperature"] + " °C";
                        FizhimWetlabel.Text += reader["wetness"];
                        FizhimBatlabel.Text += reader["battery"];
                    }
                    else if (reader["sensor_id"].ToString() == "4")
                    {
                        Podlabel1.Text = null;
                        SNPodlabel.Text = null;
                        PodTemplabel.Text = null;
                        PodWetlabel.Text = null;
                        PodBatlabel.Text = null;

                        Podlabel1.Text += reader["name"];
                        SNPodlabel.Text += reader["EUI64"];
                        PodTemplabel.Text += reader["temperature"] + " °C";
                        PodWetlabel.Text += reader["wetness"];
                        PodBatlabel.Text += reader["battery"];
                    }
                    else if (reader["sensor_id"].ToString() == "5")
                    {
                        Manlabel1.Text = null;
                        SNManlabel.Text = null;
                        ManTemplabel.Text = null;
                        ManWetlabel.Text = null;
                        ManBatlabel.Text = null;

                        Manlabel1.Text += reader["name"];
                        SNManlabel.Text += reader["EUI64"];
                        ManTemplabel.Text += reader["temperature"] + " °C";
                        ManWetlabel.Text += reader["wetness"];
                        ManBatlabel.Text += reader["battery"];

                    }
                    else if (reader["sensor_id"].ToString() == "8")
                    {
                        Teplabel1.Text = null;
                        SNTeplabel.Text = null;
                        TempTeplabel.Text = null;
                        WetTeplabel.Text = null;
                        BatTeplabel.Text = null;

                        Teplabel1.Text += reader["name"];
                        if (Teplabel1.Text == "теплотехника")
                        {
                            Teplabel1.Text = null;
                            Teplabel1.Text += "Теплотехника";
                        }
                        SNTeplabel.Text += reader["EUI64"];
                        TempTeplabel.Text += reader["temperature"] + " °C";
                        WetTeplabel.Text += reader["wetness"];
                        BatTeplabel.Text += reader["battery"];

                    }
                    else if (reader["sensor_id"].ToString() == "10")
                    {
                        Prilabel1.Text = null;
                        SNPrilabel.Text = null;
                        TempPrilabel.Text = null;
                        WetPrilabel.Text = null;
                        BatPrilabel.Text = null;

                        Prilabel1.Text += reader["name"];
                        SNPrilabel.Text += reader["EUI64"];
                        TempPrilabel.Text += reader["temperature"] + " °C";
                        WetPrilabel.Text += reader["wetness"];
                        BatPrilabel.Text += reader["battery"];
                    }             

                }


                connections.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"\\WINSERVER\data\measurement.db3";
            string path1 = System.Environment.CurrentDirectory + @"\measurement.db3";

            File.Copy(path, path1, true);
            MessageBox.Show(path1);

            /*using (SQLiteConnection connections = new SQLiteConnection(@"Data Source=\\\\WINSERVER\data\measurement.db3"))
            {
                connections.Open();
                SQLiteCommand command = new SQLiteCommand
                    (
                    "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id = 2", 
                    connections
                    );
                SQLiteDataReader reader = command.ExecuteReader();                
                
                ELlabel1.Text = null;
                SNELlabel.Text = null;
                ElTemplabel.Text = null;
                ElWetlabel.Text = null;
                ElBatlabel.Text = null;

                while (reader.Read())
                {                    
                    ELlabel1.Text += reader["name"];
                    SNELlabel.Text += reader["EUI64"];
                    ElTemplabel.Text += reader["temperature"];
                    ElWetlabel.Text += reader["wetness"];
                    ElBatlabel.Text += reader["battery"];
                }

                 command = new SQLiteCommand
                   (
                   "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id = 3",
                   connections
                   );
                 reader = command.ExecuteReader();

                Fizhimlabel1.Text = null;
                SNFizhimlabel.Text = null;
                FizhimTemplabel.Text = null;
                FizhimWetlabel.Text = null;
                FizhimBatlabel.Text = null;

                while (reader.Read())
                {
                    Fizhimlabel1.Text += reader["name"];
                    SNFizhimlabel.Text += reader["EUI64"];
                    FizhimTemplabel.Text += reader["temperature"];
                    FizhimWetlabel.Text += reader["wetness"];
                    FizhimBatlabel.Text += reader["battery"];
                }

                command = new SQLiteCommand
                 (
                 "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id = 4",
                 connections
                 );
                reader = command.ExecuteReader();

                Podlabel1.Text = null;
                SNPodlabel.Text = null;
                PodTemplabel.Text = null;
                PodWetlabel.Text = null;
                PodBatlabel.Text = null;

                while (reader.Read())
                {
                    Podlabel1.Text += reader["name"];
                    SNPodlabel.Text += reader["EUI64"];
                    PodTemplabel.Text += reader["temperature"];
                    PodWetlabel.Text += reader["wetness"];
                    PodBatlabel.Text += reader["battery"];
                }

                command = new SQLiteCommand
                 (
                 "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id = 5",
                 connections
                 );
                reader = command.ExecuteReader();

                Manlabel1.Text = null;
                SNManlabel.Text = null;
                ManTemplabel.Text = null;
                ManWetlabel.Text = null;
                ManBatlabel.Text = null;

                while (reader.Read())
                {
                    Manlabel1.Text += reader["name"];
                    SNManlabel.Text += reader["EUI64"];
                    ManTemplabel.Text += reader["temperature"];
                    ManWetlabel.Text += reader["wetness"];
                    ManBatlabel.Text += reader["battery"];
                }

                command = new SQLiteCommand
                 (
                 "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id = 8",
                 connections
                 );
                reader = command.ExecuteReader();

                Teplabel1.Text = null;
                SNTeplabel.Text = null;
                TempTeplabel.Text = null;
                WetTeplabel.Text = null;
                BatTeplabel.Text = null;

                while (reader.Read())
                {
                    Teplabel1.Text += reader["name"];
                    if (Teplabel1.Text == "теплотехника")
                    {
                        Teplabel1.Text = null;
                        Teplabel1.Text += "Теплотехника";
                    }
                    SNTeplabel.Text += reader["EUI64"];
                    TempTeplabel.Text += reader["temperature"];
                    WetTeplabel.Text += reader["wetness"];
                    BatTeplabel.Text += reader["battery"];
                }

                command = new SQLiteCommand
                 (
                 "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id = 10",
                 connections
                 );
                reader = command.ExecuteReader();

                Prilabel1.Text = null;
                SNPrilabel.Text = null;
                TempPrilabel.Text = null;
                WetPrilabel.Text = null;
                BatPrilabel.Text = null;

                while (reader.Read())
                {
                    Prilabel1.Text += reader["name"];
                    SNPrilabel.Text += reader["EUI64"];
                    TempPrilabel.Text += reader["temperature"];
                    WetPrilabel.Text += reader["wetness"];
                    BatPrilabel.Text += reader["battery"];
                }

                connections.Close();
            }*/

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/vitaliy-petukhov-206a3a156/");
        }
    }
}

