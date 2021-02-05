using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClimateMonitoring
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            using (SQLiteConnection connections = new SQLiteConnection(@"Data Source=\\\\WINSERVER\data\measurement.db3"))
            {
                connections.Open();
                SQLiteCommand command = new SQLiteCommand
                    (
                    "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id = 2",
                    connections
                    );
                SQLiteDataReader reader = command.ExecuteReader();

                var allRecords = new StringBuilder();

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

                connections.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection connections = new SQLiteConnection(@"Data Source=\\\\WINSERVER\data\measurement.db3"))
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

                connections.Close();
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/vitaliy-petukhov-206a3a156/");
        }
    }
}

