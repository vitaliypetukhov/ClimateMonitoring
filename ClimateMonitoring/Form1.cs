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

            ViewGrid_main.Visible = false;
            button3.Visible = false;
            button4.Visible = false;

            date_otchet.Visible = false;
            date_otchet2.Visible = false;

            combo_otch_datchik.Visible = false;

            Otdelcombo.SelectedIndex = 0;

            if (Otdelcombo.SelectedIndex == 0)
            {
                Prilabel1.Visible = false;
                SNPrilabel.Visible = false;
                TempPrilabel.Visible = false;
                WetPrilabel.Visible = false;
                BatPrilabel.Visible = false;
                this.Text = "Система мониторинга микроклимата помощений (отдел ЭМиТТИ)";
            }
            else
            {
                Prilabel1.Visible = true;
                SNPrilabel.Visible = true;
                TempPrilabel.Visible = true;
                WetPrilabel.Visible = true;
                BatPrilabel.Visible = true;
                this.Text = "Система мониторинга микроклимата помощений (отдел МиЛУИ)";
            }

            string path = @"\\WINSERVER\data\measurement.db3";
            string path1 = System.Environment.CurrentDirectory + @"\measurement.db3";

            string info = File.GetLastWriteTime(path).ToString();
            infolabel.Text = "Информация на " + info;

            //DateTime dateForButton = DateTime.Now.AddDays(-3);           
            //MessageBox.Show(month.ToString());

            FileInfo check = new FileInfo(path1);
           
            if(check.Exists)
            {
                //File.Delete(path1);
                File.Copy(path, path1, true);               
            }
            else
            {
                File.Copy(path, path1, true);                
            }
                      
            using (SQLiteConnection connections = new SQLiteConnection(@"Data Source="+path1))
            {
                connections.Open();
                
                SQLiteCommand command = new SQLiteCommand
                    (
                    "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id > 0 group by sensor_id",
                    connections
                    );
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (Otdelcombo.SelectedIndex == 0)
                    {
                        Prilabel1.Visible = false;
                        SNPrilabel.Visible = false;
                        TempPrilabel.Visible = false;
                        WetPrilabel.Visible = false;
                        BatPrilabel.Visible = false;
                        this.Text = "Система мониторинга микроклимата помощений (отдел ЭМиТТИ)";
                    }
                    //Пенал (4B702D7)
                    if (reader["sensor_id"].ToString() == "1" && Otdelcombo.SelectedIndex == 1)
                    {                        
                        ELlabel1.Text = null;
                        SNELlabel.Text = null;
                        ElTemplabel.Text = null;
                        ElWetlabel.Text = null;
                        ElBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        ELlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNELlabel.Text += date_last_update.ToString();

                        ElTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            ElTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            ElTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            ElWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                ElWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            ElWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                ElWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ElBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                            //ELlabel1.Text = null;
                            SNELlabel.Text = null;
                            ElTemplabel.Text = null;
                            ElWetlabel.Text = null;
                            ElBatlabel.Text = null;

                            //ELlabel1.Text += "NONE";
                            SNELlabel.Text += "NONE";
                            ElTemplabel.Text += "NONE";
                            ElWetlabel.Text += "NONE";
                            ElBatlabel.Text += "NONE";

                            //ELlabel1.ForeColor = Color.Red;
                            SNELlabel.ForeColor = Color.Red;
                            ElTemplabel.ForeColor = Color.Red;
                            ElWetlabel.ForeColor = Color.Red;
                            ElBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Пенал (4B702D7) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }

                    }
                    //Электрики (4B70900)
                    else if (reader["sensor_id"].ToString() == "2" && Otdelcombo.SelectedIndex == 0)
                    {
                        //MessageBox.Show("111");
                        ELlabel1.Text = null;
                        SNELlabel.Text = null;
                        ElTemplabel.Text = null;
                        ElWetlabel.Text = null;
                        ElBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        ELlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNELlabel.Text += date_last_update.ToString();

                        ElTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {                            
                            ElTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            ElTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {                            
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;

                            ElWetlabel.Text += wetnes_int.ToString() + " %";

                            if(wetnes_int>30.0 && wetnes_int<80.0)
                            {
                                ElWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else 
                        {
                            ElWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1>30.0 && wetnes_int1<80.0)
                            {
                                ElWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ElBatlabel.Text += s4 + " V";// reader["battery"];

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                            //ELlabel1.Text = null;
                            SNELlabel.Text = null;
                            ElTemplabel.Text = null;
                            ElWetlabel.Text = null;
                            ElBatlabel.Text = null;

                            //ELlabel1.Text += "NONE";
                            SNELlabel.Text += "NONE";
                            ElTemplabel.Text += "NONE";
                            ElWetlabel.Text += "NONE";
                            ElBatlabel.Text += "NONE";

                            //ELlabel1.ForeColor = Color.Red;
                            SNELlabel.ForeColor = Color.Red;
                            ElTemplabel.ForeColor = Color.Red;
                            ElWetlabel.ForeColor = Color.Red;
                            ElBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Электрики (4B70900) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Физ-химия (4AF798F)
                    else if (reader["sensor_id"].ToString() == "3" && Otdelcombo.SelectedIndex == 0)
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

                        FizhimTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            FizhimTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            FizhimTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            FizhimWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                FizhimWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            FizhimWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                FizhimWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimWetlabel.ForeColor = Color.Red;
                            }
                        }                        

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);
                        FizhimBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                            //Fizhimlabel1.Text = null;
                            SNFizhimlabel.Text = null;
                            FizhimTemplabel.Text = null;
                            FizhimWetlabel.Text = null;
                            FizhimBatlabel.Text = null;

                            //Fizhimlabel1.Text += "NONE";
                            SNFizhimlabel.Text += "NONE";
                            FizhimTemplabel.Text += "NONE";
                            FizhimWetlabel.Text += "NONE";
                            FizhimBatlabel.Text += "NONE";

                            //Fizhimlabel1.ForeColor = Color.Red;
                            SNFizhimlabel.ForeColor = Color.Red;
                            FizhimTemplabel.ForeColor = Color.Red;
                            FizhimWetlabel.ForeColor = Color.Red;
                            FizhimBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Физ-химия (4AF798F) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Подвал (4B6FС97)
                    else if (reader["sensor_id"].ToString() == "4" && Otdelcombo.SelectedIndex == 1)
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
                                                
                        FizhimTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            FizhimTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            FizhimTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            FizhimWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                FizhimWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            FizhimWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                FizhimWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimWetlabel.ForeColor = Color.Red;
                            }
                        }                        

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);
                        FizhimBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Fizhimlabel1.Text = null;
                            SNFizhimlabel.Text = null;
                            FizhimTemplabel.Text = null;
                            FizhimWetlabel.Text = null;
                            FizhimBatlabel.Text = null;

                            //Fizhimlabel1.Text += "NONE";
                            SNFizhimlabel.Text += "NONE";
                            FizhimTemplabel.Text += "NONE";
                            FizhimWetlabel.Text += "NONE";
                            FizhimBatlabel.Text += "NONE";

                           // Fizhimlabel1.ForeColor = Color.Red;
                            SNFizhimlabel.ForeColor = Color.Red;
                            FizhimTemplabel.ForeColor = Color.Red;
                            FizhimWetlabel.ForeColor = Color.Red;
                            FizhimBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Подвал (4B6FС97) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Манометры (4B70ADC)
                    else if (reader["sensor_id"].ToString() == "5" && Otdelcombo.SelectedIndex == 0)
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
                                               
                        PodTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            PodTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            PodTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            PodWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                PodWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            PodWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                PodWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodWetlabel.ForeColor = Color.Red;
                            }
                        }                        

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        PodBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                            //Podlabel1.Text = null;
                            SNPodlabel.Text = null;
                            PodTemplabel.Text = null;
                            PodWetlabel.Text = null;
                            PodBatlabel.Text = null;

                            //Podlabel1.Text += "NONE";
                            SNPodlabel.Text += "NONE";
                            PodTemplabel.Text += "NONE";
                            PodWetlabel.Text += "NONE";
                            PodBatlabel.Text += "NONE";

                            //Podlabel1.ForeColor = Color.Red;
                            SNPodlabel.ForeColor = Color.Red;
                            PodTemplabel.ForeColor = Color.Red;
                            PodWetlabel.ForeColor = Color.Red;
                            PodBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Манометры (4B70ADC) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Мерники (4B6E621)
                    else if (reader["sensor_id"].ToString() == "6" && Otdelcombo.SelectedIndex == 1)
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
                                                
                        PodTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            PodTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            PodTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            PodWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                PodWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            PodWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                PodWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        PodBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                            //Podlabel1.Text = null;
                            SNPodlabel.Text = null;
                            PodTemplabel.Text = null;
                            PodWetlabel.Text = null;
                            PodBatlabel.Text = null;

                           // Podlabel1.Text += "NONE";
                            SNPodlabel.Text += "NONE";
                            PodTemplabel.Text += "NONE";
                            PodWetlabel.Text += "NONE";
                            PodBatlabel.Text += "NONE";

                            //Podlabel1.ForeColor = Color.Red;
                            SNPodlabel.ForeColor = Color.Red;
                            PodTemplabel.ForeColor = Color.Red;
                            PodWetlabel.ForeColor = Color.Red;
                            PodBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Мерники (4B6E621) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Весы (4B6FB5A)
                    else if (reader["sensor_id"].ToString() == "7" && Otdelcombo.SelectedIndex == 1)
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

                        ManTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            ManTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            ManTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            ManWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                ManWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            ManWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                ManWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManWetlabel.ForeColor = Color.Red;
                            }
                        }                        

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ManBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                            //Manlabel1.Text = null;
                            SNManlabel.Text = null;
                            ManTemplabel.Text = null;
                            ManWetlabel.Text = null;
                            ManBatlabel.Text = null;

                            //Manlabel1.Text += "NONE";
                            SNManlabel.Text += "NONE";
                            ManTemplabel.Text += "NONE";
                            ManWetlabel.Text += "NONE";
                            ManBatlabel.Text += "NONE";

                            //Manlabel1.ForeColor = Color.Red;
                            SNManlabel.ForeColor = Color.Red;
                            ManTemplabel.ForeColor = Color.Red;
                            ManWetlabel.ForeColor = Color.Red;
                            ManBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Весы (4B6FB5A) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Теплотехника (4B709FD)
                    else if (reader["sensor_id"].ToString() == "8" && Otdelcombo.SelectedIndex == 0)
                    {
                        Manlabel1.Text = null;
                        SNManlabel.Text = null;
                        ManTemplabel.Text = null;
                        ManWetlabel.Text = null;
                        ManBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Manlabel1.Text += reader["name"];

                        if (Manlabel1.Text == "теплотехника")
                        {
                            Manlabel1.Text = null;
                            Manlabel1.Text += "Теплотехника" + " (" + s2 + ")";
                        }

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNManlabel.Text += date_last_update.ToString();

                        ManTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            ManTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            ManTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            ManWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                ManWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            ManWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                ManWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ManBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                            //Manlabel1.Text = null;
                            SNManlabel.Text = null;
                            ManTemplabel.Text = null;
                            ManWetlabel.Text = null;
                            ManBatlabel.Text = null;

                            //Manlabel1.Text += "NONE";
                            SNManlabel.Text += "NONE";
                            ManTemplabel.Text += "NONE";
                            ManWetlabel.Text += "NONE";
                            ManBatlabel.Text += "NONE";

                           // Manlabel1.ForeColor = Color.Red;
                            SNManlabel.ForeColor = Color.Red;
                            ManTemplabel.ForeColor = Color.Red;
                            ManWetlabel.ForeColor = Color.Red;
                            ManBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Теплотехника (4B709FD) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }

                    }
                    //Механики (4B6FBA5)
                    else if (reader["sensor_id"].ToString() == "10" && Otdelcombo.SelectedIndex == 1)
                    {
                        Teplabel1.Text = null;
                        SNTeplabel.Text = null;
                        TempTeplabel.Text = null;
                        WetTeplabel.Text = null;
                        BatTeplabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Teplabel1.Text += reader["name"] + " (" + s2 + ")";
                       
                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNTeplabel.Text += date_last_update.ToString();

                        TempTeplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            TempTeplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            TempTeplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            WetTeplabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                WetTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetTeplabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            WetTeplabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                WetTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetTeplabel.ForeColor = Color.Red;
                            }
                        }                        

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatTeplabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Teplabel1.Text = null;
                            SNTeplabel.Text = null;
                            TempTeplabel.Text = null;
                            WetTeplabel.Text = null;
                            BatTeplabel.Text = null;

                           // Teplabel1.Text += "NONE";
                            SNTeplabel.Text += "NONE";
                            TempTeplabel.Text += "NONE";
                            WetTeplabel.Text += "NONE";
                            BatTeplabel.Text += "NONE";

                          //  Teplabel1.ForeColor = Color.Red;
                            SNTeplabel.ForeColor = Color.Red;
                            TempTeplabel.ForeColor = Color.Red;
                            WetTeplabel.ForeColor = Color.Red;
                            BatTeplabel.ForeColor = Color.Red;

                            MessageBox.Show("Механики (4B6FBA5) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }

                    }
                    //Приемка (4B707D0)
                    else if (reader["sensor_id"].ToString() == "10" && Otdelcombo.SelectedIndex == 0)
                    {
                        Teplabel1.Text = null;
                        SNTeplabel.Text = null;
                        TempTeplabel.Text = null;
                        WetTeplabel.Text = null;
                        BatTeplabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Teplabel1.Text += reader["name"] + " (" + s2 + ")";
                        
                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNTeplabel.Text += date_last_update.ToString();

                        TempTeplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            TempTeplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            TempTeplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            WetTeplabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                WetTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetTeplabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            WetTeplabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                WetTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetTeplabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatTeplabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                            //Teplabel1.Text = null;
                            SNTeplabel.Text = null;
                            TempTeplabel.Text = null;
                            WetTeplabel.Text = null;
                            BatTeplabel.Text = null;

                           // Teplabel1.Text += "NONE";
                            SNTeplabel.Text += "NONE";
                            TempTeplabel.Text += "NONE";
                            WetTeplabel.Text += "NONE";
                            BatTeplabel.Text += "NONE";

                           // Teplabel1.ForeColor = Color.Red;
                            SNTeplabel.ForeColor = Color.Red;
                            TempTeplabel.ForeColor = Color.Red;
                            WetTeplabel.ForeColor = Color.Red;
                            BatTeplabel.ForeColor = Color.Red;

                            MessageBox.Show("Приемка (4B707D0) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }

                    }
                    //Линейно-угловые (4B70245)
                    else if (reader["sensor_id"].ToString() == "11" && Otdelcombo.SelectedIndex == 1)
                    {
                        Prilabel1.Text = null;
                        SNPrilabel.Text = null;
                        TempPrilabel.Text = null;
                        WetPrilabel.Text = null;
                        BatPrilabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Prilabel1.Text += reader["name"];

                        if (Prilabel1.Text == "Линейно-угловые")
                        {
                            Prilabel1.Text = null;
                            Prilabel1.Text += "Лин-угловые" + " (" + s2 + ")";
                        }

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNPrilabel.Text += date_last_update.ToString();

                        TempPrilabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            TempPrilabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            TempPrilabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            WetTeplabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                WetTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetTeplabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            WetPrilabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                WetPrilabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetPrilabel.ForeColor = Color.Red;
                            }
                        }
                        

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatPrilabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Prilabel1.Text = null;
                            SNPrilabel.Text = null;
                            TempPrilabel.Text = null;
                            WetPrilabel.Text = null;
                            BatPrilabel.Text = null;

                           // Prilabel1.Text += "NONE";
                            SNPrilabel.Text += "NONE";
                            TempPrilabel.Text += "NONE";
                            WetPrilabel.Text += "NONE";
                            BatPrilabel.Text += "NONE";

                           // Prilabel1.ForeColor = Color.Red;
                            SNPrilabel.ForeColor = Color.Red;
                            TempPrilabel.ForeColor = Color.Red;
                            WetPrilabel.ForeColor = Color.Red;
                            BatPrilabel.ForeColor = Color.Red;

                            MessageBox.Show("Линейно-угловые (4B70245) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                }                                                
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
                File.Copy(path, path1, true);               
            }
            else
            {
                File.Copy(path, path1, true);
            }

            using (SQLiteConnection connections = new SQLiteConnection(@"Data Source=" + path1))
            {
                connections.Open();
                
                if (Otdelcombo.SelectedIndex == 0)
                {
                    Prilabel1.Visible = false;
                    SNPrilabel.Visible = false;
                    TempPrilabel.Visible = false;
                    WetPrilabel.Visible = false;
                    BatPrilabel.Visible = false;
                    this.Text = "Система мониторинга микроклимата помощений (отдел ЭМиТТИ)";
                }
                else
                {
                    Prilabel1.Visible = true;
                    SNPrilabel.Visible = true;
                    TempPrilabel.Visible = true;
                    WetPrilabel.Visible = true;
                    BatPrilabel.Visible = true;
                    this.Text = "Система мониторинга микроклимата помощений (отдел МиЛУИ)";
                }

                SQLiteCommand command = new SQLiteCommand
                    (
                    "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id > 0 group by sensor_id",
                    connections
                    );
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //Пенал (4B702D7)
                    if (reader["sensor_id"].ToString() == "1" && Otdelcombo.SelectedIndex == 1)
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

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            ElTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            ElTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            ElWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                ElWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            ElWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                ElWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ElBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // ELlabel1.Text = null;
                            SNELlabel.Text = null;
                            ElTemplabel.Text = null;
                            ElWetlabel.Text = null;
                            ElBatlabel.Text = null;

                           // ELlabel1.Text += "NONE";
                            SNELlabel.Text += "NONE";
                            ElTemplabel.Text += "NONE";
                            ElWetlabel.Text += "NONE";
                            ElBatlabel.Text += "NONE";

                           // ELlabel1.ForeColor = Color.Red;
                            SNELlabel.ForeColor = Color.Red;
                            ElTemplabel.ForeColor = Color.Red;
                            ElWetlabel.ForeColor = Color.Red;
                            ElBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Пенал (4B702D7) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }

                    }
                    //Электрики (4B70900)
                    else if (reader["sensor_id"].ToString() == "2" && Otdelcombo.SelectedIndex == 0)
                    {                        
                        ELlabel1.Text = null;
                        SNELlabel.Text = null;
                        ElTemplabel.Text = null;
                        ElWetlabel.Text = null;
                        ElBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        ELlabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNELlabel.Text += date_last_update.ToString();

                        ElTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            ElTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            ElTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            ElWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                ElWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            ElWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                ElWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ElBatlabel.Text += s4 + " V";// reader["battery"];

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // ELlabel1.Text = null;
                            SNELlabel.Text = null;
                            ElTemplabel.Text = null;
                            ElWetlabel.Text = null;
                            ElBatlabel.Text = null;

                           // ELlabel1.Text += "NONE";
                            SNELlabel.Text += "NONE";
                            ElTemplabel.Text += "NONE";
                            ElWetlabel.Text += "NONE";
                            ElBatlabel.Text += "NONE";

                           // ELlabel1.ForeColor = Color.Red;
                            SNELlabel.ForeColor = Color.Red;
                            ElTemplabel.ForeColor = Color.Red;
                            ElWetlabel.ForeColor = Color.Red;
                            ElBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Электрики (4B70900) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Физ-химия (4AF798F)
                    else if (reader["sensor_id"].ToString() == "3" && Otdelcombo.SelectedIndex == 0)
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

                        FizhimTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            FizhimTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            FizhimTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            FizhimWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                FizhimWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            FizhimWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                FizhimWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);
                        FizhimBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Fizhimlabel1.Text = null;
                            SNFizhimlabel.Text = null;
                            FizhimTemplabel.Text = null;
                            FizhimWetlabel.Text = null;
                            FizhimBatlabel.Text = null;

                           // Fizhimlabel1.Text += "NONE";
                            SNFizhimlabel.Text += "NONE";
                            FizhimTemplabel.Text += "NONE";
                            FizhimWetlabel.Text += "NONE";
                            FizhimBatlabel.Text += "NONE";

                           // Fizhimlabel1.ForeColor = Color.Red;
                            SNFizhimlabel.ForeColor = Color.Red;
                            FizhimTemplabel.ForeColor = Color.Red;
                            FizhimWetlabel.ForeColor = Color.Red;
                            FizhimBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Физ-химия (4AF798F) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Подвал (4B6FС97)
                    else if (reader["sensor_id"].ToString() == "4" && Otdelcombo.SelectedIndex == 1)
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

                        FizhimTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            FizhimTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            FizhimTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            FizhimWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                FizhimWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            FizhimWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                FizhimWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);
                        FizhimBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Fizhimlabel1.Text = null;
                            SNFizhimlabel.Text = null;
                            FizhimTemplabel.Text = null;
                            FizhimWetlabel.Text = null;
                            FizhimBatlabel.Text = null;

                          //  Fizhimlabel1.Text += "NONE";
                            SNFizhimlabel.Text += "NONE";
                            FizhimTemplabel.Text += "NONE";
                            FizhimWetlabel.Text += "NONE";
                            FizhimBatlabel.Text += "NONE";

                           // Fizhimlabel1.ForeColor = Color.Red;
                            SNFizhimlabel.ForeColor = Color.Red;
                            FizhimTemplabel.ForeColor = Color.Red;
                            FizhimWetlabel.ForeColor = Color.Red;
                            FizhimBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Подвал (4B6FС97) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Манометры (4B70ADC)
                    else if (reader["sensor_id"].ToString() == "5" && Otdelcombo.SelectedIndex == 0)
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

                        PodTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            PodTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            PodTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            PodWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                PodWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            PodWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                PodWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        PodBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                          //  Podlabel1.Text = null;
                            SNPodlabel.Text = null;
                            PodTemplabel.Text = null;
                            PodWetlabel.Text = null;
                            PodBatlabel.Text = null;

                          //  Podlabel1.Text += "NONE";
                            SNPodlabel.Text += "NONE";
                            PodTemplabel.Text += "NONE";
                            PodWetlabel.Text += "NONE";
                            PodBatlabel.Text += "NONE";

                          //  Podlabel1.ForeColor = Color.Red;
                            SNPodlabel.ForeColor = Color.Red;
                            PodTemplabel.ForeColor = Color.Red;
                            PodWetlabel.ForeColor = Color.Red;
                            PodBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Манометры (4B70ADC) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Мерники (4B6E621)
                    else if (reader["sensor_id"].ToString() == "6" && Otdelcombo.SelectedIndex == 1)
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

                        PodTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.00 && temp < 25.00)
                        {
                            PodTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            PodTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            PodWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                PodWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            PodWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                PodWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        PodBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Podlabel1.Text = null;
                            SNPodlabel.Text = null;
                            PodTemplabel.Text = null;
                            PodWetlabel.Text = null;
                            PodBatlabel.Text = null;

                           // Podlabel1.Text += "NONE";
                            SNPodlabel.Text += "NONE";
                            PodTemplabel.Text += "NONE";
                            PodWetlabel.Text += "NONE";
                            PodBatlabel.Text += "NONE";

                           // Podlabel1.ForeColor = Color.Red;
                            SNPodlabel.ForeColor = Color.Red;
                            PodTemplabel.ForeColor = Color.Red;
                            PodWetlabel.ForeColor = Color.Red;
                            PodBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Мерники (4B6E621) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Весы (4B6FB5A)
                    else if (reader["sensor_id"].ToString() == "7" && Otdelcombo.SelectedIndex == 1)
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

                        ManTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            ManTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            ManTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            ManWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                ManWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            ManWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                ManWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ManBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Manlabel1.Text = null;
                            SNManlabel.Text = null;
                            ManTemplabel.Text = null;
                            ManWetlabel.Text = null;
                            ManBatlabel.Text = null;

                           // Manlabel1.Text += "NONE";
                            SNManlabel.Text += "NONE";
                            ManTemplabel.Text += "NONE";
                            ManWetlabel.Text += "NONE";
                            ManBatlabel.Text += "NONE";

                           // Manlabel1.ForeColor = Color.Red;
                            SNManlabel.ForeColor = Color.Red;
                            ManTemplabel.ForeColor = Color.Red;
                            ManWetlabel.ForeColor = Color.Red;
                            ManBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Весы (4B6FB5A) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                    //Теплотехника (4B709FD)
                    else if (reader["sensor_id"].ToString() == "8" && Otdelcombo.SelectedIndex == 0)
                    {
                        Manlabel1.Text = null;
                        SNManlabel.Text = null;
                        ManTemplabel.Text = null;
                        ManWetlabel.Text = null;
                        ManBatlabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Manlabel1.Text += reader["name"];

                        if (Manlabel1.Text == "теплотехника")
                        {
                            Manlabel1.Text = null;
                            Manlabel1.Text += "Теплотехника" + " (" + s2 + ")";
                        }

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNManlabel.Text += date_last_update.ToString();

                        ManTemplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            ManTemplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            ManTemplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            ManWetlabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                ManWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManWetlabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            ManWetlabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                ManWetlabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManWetlabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        ManBatlabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                          //  Manlabel1.Text = null;
                            SNManlabel.Text = null;
                            ManTemplabel.Text = null;
                            ManWetlabel.Text = null;
                            ManBatlabel.Text = null;

                          //  Manlabel1.Text += "NONE";
                            SNManlabel.Text += "NONE";
                            ManTemplabel.Text += "NONE";
                            ManWetlabel.Text += "NONE";
                            ManBatlabel.Text += "NONE";

                           // Manlabel1.ForeColor = Color.Red;
                            SNManlabel.ForeColor = Color.Red;
                            ManTemplabel.ForeColor = Color.Red;
                            ManWetlabel.ForeColor = Color.Red;
                            ManBatlabel.ForeColor = Color.Red;

                            MessageBox.Show("Теплотехника (4B709FD) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }

                    }
                    //Механики (4B6FBA5)
                    else if (reader["sensor_id"].ToString() == "10" && Otdelcombo.SelectedIndex == 1)
                    {
                        Teplabel1.Text = null;
                        SNTeplabel.Text = null;
                        TempTeplabel.Text = null;
                        WetTeplabel.Text = null;
                        BatTeplabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Teplabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNTeplabel.Text += date_last_update.ToString();

                        TempTeplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            TempTeplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            TempTeplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            WetTeplabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                WetTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetTeplabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            WetTeplabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                WetTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetTeplabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatTeplabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Teplabel1.Text = null;
                            SNTeplabel.Text = null;
                            TempTeplabel.Text = null;
                            WetTeplabel.Text = null;
                            BatTeplabel.Text = null;

                           // Teplabel1.Text += "NONE";
                            SNTeplabel.Text += "NONE";
                            TempTeplabel.Text += "NONE";
                            WetTeplabel.Text += "NONE";
                            BatTeplabel.Text += "NONE";

                           // Teplabel1.ForeColor = Color.Red;
                            SNTeplabel.ForeColor = Color.Red;
                            TempTeplabel.ForeColor = Color.Red;
                            WetTeplabel.ForeColor = Color.Red;
                            BatTeplabel.ForeColor = Color.Red;

                            MessageBox.Show("Механики (4B6FBA5) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }

                    }
                    //Приемка (4B707D0)
                    else if (reader["sensor_id"].ToString() == "10" && Otdelcombo.SelectedIndex == 0)
                    {
                        Teplabel1.Text = null;
                        SNTeplabel.Text = null;
                        TempTeplabel.Text = null;
                        WetTeplabel.Text = null;
                        BatTeplabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Teplabel1.Text += reader["name"] + " (" + s2 + ")";

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNTeplabel.Text += date_last_update.ToString();

                        TempTeplabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            TempTeplabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            TempTeplabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            WetTeplabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                WetTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetTeplabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            WetTeplabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                WetTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetTeplabel.ForeColor = Color.Red;
                            }
                        }

                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatTeplabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Teplabel1.Text = null;
                            SNTeplabel.Text = null;
                            TempTeplabel.Text = null;
                            WetTeplabel.Text = null;
                            BatTeplabel.Text = null;

                           // Teplabel1.Text += "NONE";
                            SNTeplabel.Text += "NONE";
                            TempTeplabel.Text += "NONE";
                            WetTeplabel.Text += "NONE";
                            BatTeplabel.Text += "NONE";

                           // Teplabel1.ForeColor = Color.Red;
                            SNTeplabel.ForeColor = Color.Red;
                            TempTeplabel.ForeColor = Color.Red;
                            WetTeplabel.ForeColor = Color.Red;
                            BatTeplabel.ForeColor = Color.Red;

                            MessageBox.Show("Приемка (4B707D0) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }

                    }
                    //Линейно-угловые (4B70245)
                    else if (reader["sensor_id"].ToString() == "11" && Otdelcombo.SelectedIndex == 1)
                    {
                        Prilabel1.Text = null;
                        SNPrilabel.Text = null;
                        TempPrilabel.Text = null;
                        WetPrilabel.Text = null;
                        BatPrilabel.Text = null;

                        String s = reader["EUI64"].ToString();
                        String s2 = s.Substring(s.Length - 7);

                        Prilabel1.Text += reader["name"];

                        if (Prilabel1.Text == "Линейно-угловые")
                        {
                            Prilabel1.Text = null;
                            Prilabel1.Text += "Лин-угловые" + " (" + s2 + ")";
                        }

                        int timestamp = Convert.ToInt32(reader["timemeasure"]);
                        DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                        SNPrilabel.Text += date_last_update.ToString();

                        TempPrilabel.Text += reader["temperature"] + " °C";

                        double temp = Convert.ToDouble(reader["temperature"]);
                        if (temp > 15.0 && temp < 25.0)
                        {
                            TempPrilabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            TempPrilabel.ForeColor = Color.Red;
                        }

                        DateTime currdate = DateTime.Now;
                        int month = currdate.Month;
                        if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                        {
                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            double wetnes_int = wetnes_int1 + 10;
                            WetPrilabel.Text += wetnes_int.ToString() + " %";

                            if (wetnes_int > 30.0 && wetnes_int < 80.0)
                            {
                                WetPrilabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetPrilabel.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            WetPrilabel.Text += reader["wetness"] + " %";

                            double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                            if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                            {
                                WetPrilabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                WetPrilabel.ForeColor = Color.Red;
                            }
                        }


                        String s3 = reader["battery"].ToString();
                        String s4 = s3.Substring(0, 4);

                        BatPrilabel.Text += s4 + " V";

                        DateTime check_status = DateTime.Now.AddDays(-1);
                        if (check_status > date_last_update)
                        {
                           // Prilabel1.Text = null;
                            SNPrilabel.Text = null;
                            TempPrilabel.Text = null;
                            WetPrilabel.Text = null;
                            BatPrilabel.Text = null;

                           // Prilabel1.Text += "NONE";
                            SNPrilabel.Text += "NONE";
                            TempPrilabel.Text += "NONE";
                            WetPrilabel.Text += "NONE";
                            BatPrilabel.Text += "NONE";

                           // Prilabel1.ForeColor = Color.Red;
                            SNPrilabel.ForeColor = Color.Red;
                            TempPrilabel.ForeColor = Color.Red;
                            WetPrilabel.ForeColor = Color.Red;
                            BatPrilabel.ForeColor = Color.Red;

                            MessageBox.Show("Линейно-угловые (4B70245) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                        }
                    }
                }
                connections.Close();
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/vitaliy-petukhov-206a3a156/");
        }

        private void Otdelcombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if(month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11|| month == 12)
            if (Otdelcombo.SelectedIndex == 0)
            {
                string path = @"\\WINSERVER\data\measurement.db3";
                string path1 = System.Environment.CurrentDirectory + @"\measurement.db3";

                string info = File.GetLastWriteTime(path).ToString();
                infolabel.Text = "Информация на " + info;

                Prilabel1.Visible = false;
                SNPrilabel.Visible = false;
                TempPrilabel.Visible = false;
                WetPrilabel.Visible = false;
                BatPrilabel.Visible = false;
                this.Text = "Система мониторинга микроклимата помощений (отдел ЭМиТТИ)";

                FileInfo check = new FileInfo(path1);
                if (check.Exists)
                {
                    File.Copy(path, path1, true);
                }
                else
                {
                    File.Copy(path, path1, true);
                }

                using (SQLiteConnection connections = new SQLiteConnection(@"Data Source=" + path1))
                {
                    connections.Open();
                

                    SQLiteCommand command = new SQLiteCommand
                        (
                        "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id > 0 group by sensor_id",
                        connections
                        );
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {                        
                        //Электрики (4B70900)
                         if (reader["sensor_id"].ToString() == "2" && Otdelcombo.SelectedIndex == 0)
                        {
                            ELlabel1.Text = null;
                            SNELlabel.Text = null;
                            ElTemplabel.Text = null;
                            ElWetlabel.Text = null;
                            ElBatlabel.Text = null;

                            String s = reader["EUI64"].ToString();
                            String s2 = s.Substring(s.Length - 7);

                            ELlabel1.Text += reader["name"] + " (" + s2 + ")";

                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                            SNELlabel.Text += date_last_update.ToString();

                            ElTemplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                ElTemplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElTemplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                ElWetlabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    ElWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    ElWetlabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                ElWetlabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    ElWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    ElWetlabel.ForeColor = Color.Red;
                                }
                            }

                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);

                            ElBatlabel.Text += s4 + " V";// reader["battery"];

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                               // ELlabel1.Text = null;
                                SNELlabel.Text = null;
                                ElTemplabel.Text = null;
                                ElWetlabel.Text = null;
                                ElBatlabel.Text = null;

                                //ELlabel1.Text += "NONE";
                                SNELlabel.Text += "NONE";
                                ElTemplabel.Text += "NONE";
                                ElWetlabel.Text += "NONE";
                                ElBatlabel.Text += "NONE";

                               //ELlabel1.ForeColor = Color.Red;
                                SNELlabel.ForeColor = Color.Red;
                                ElTemplabel.ForeColor = Color.Red;
                                ElWetlabel.ForeColor = Color.Red;
                                ElBatlabel.ForeColor = Color.Red;

                                MessageBox.Show("Электрики (4B70900) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }
                        }
                        //Физ-химия (4AF798F)
                        else if (reader["sensor_id"].ToString() == "3" && Otdelcombo.SelectedIndex == 0)
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

                            FizhimTemplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                FizhimTemplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimTemplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                FizhimWetlabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    FizhimWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    FizhimWetlabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                FizhimWetlabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    FizhimWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    FizhimWetlabel.ForeColor = Color.Red;
                                }
                            }

                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);
                            FizhimBatlabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                               // Fizhimlabel1.Text = null;
                                SNFizhimlabel.Text = null;
                                FizhimTemplabel.Text = null;
                                FizhimWetlabel.Text = null;
                                FizhimBatlabel.Text = null;

                               // Fizhimlabel1.Text += "NONE";
                                SNFizhimlabel.Text += "NONE";
                                FizhimTemplabel.Text += "NONE";
                                FizhimWetlabel.Text += "NONE";
                                FizhimBatlabel.Text += "NONE";

                               // Fizhimlabel1.ForeColor = Color.Red;
                                SNFizhimlabel.ForeColor = Color.Red;
                                FizhimTemplabel.ForeColor = Color.Red;
                                FizhimWetlabel.ForeColor = Color.Red;
                                FizhimBatlabel.ForeColor = Color.Red;

                                MessageBox.Show("Физ-химия (4AF798F) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }
                        }
                        //Манометры (4B70ADC)
                        else if (reader["sensor_id"].ToString() == "5" && Otdelcombo.SelectedIndex == 0)
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

                            PodTemplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                PodTemplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodTemplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                PodWetlabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    PodWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    PodWetlabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                PodWetlabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    PodWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    PodWetlabel.ForeColor = Color.Red;
                                }
                            }

                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);

                            PodBatlabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                                //Podlabel1.Text = null;
                                SNPodlabel.Text = null;
                                PodTemplabel.Text = null;
                                PodWetlabel.Text = null;
                                PodBatlabel.Text = null;

                               // Podlabel1.Text += "NONE";
                                SNPodlabel.Text += "NONE";
                                PodTemplabel.Text += "NONE";
                                PodWetlabel.Text += "NONE";
                                PodBatlabel.Text += "NONE";

                               // Podlabel1.ForeColor = Color.Red;
                                SNPodlabel.ForeColor = Color.Red;
                                PodTemplabel.ForeColor = Color.Red;
                                PodWetlabel.ForeColor = Color.Red;
                                PodBatlabel.ForeColor = Color.Red;

                                MessageBox.Show("Манометры (4B70ADC) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }
                        }
                        //Теплотехника (4B709FD)
                        else if (reader["sensor_id"].ToString() == "8" && Otdelcombo.SelectedIndex == 0)
                        {
                            Manlabel1.Text = null;
                            SNManlabel.Text = null;
                            ManTemplabel.Text = null;
                            ManWetlabel.Text = null;
                            ManBatlabel.Text = null;

                            String s = reader["EUI64"].ToString();
                            String s2 = s.Substring(s.Length - 7);

                            Manlabel1.Text += reader["name"];

                            if (Manlabel1.Text == "теплотехника")
                            {
                                Manlabel1.Text = null;
                                Manlabel1.Text += "Теплотехника" + " (" + s2 + ")";
                            }

                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                            SNManlabel.Text += date_last_update.ToString();

                            ManTemplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                ManTemplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManTemplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                ManWetlabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    ManWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    ManWetlabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                ManWetlabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    ManWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    ManWetlabel.ForeColor = Color.Red;
                                }
                            }

                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);

                            ManBatlabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                               // Manlabel1.Text = null;
                                SNManlabel.Text = null;
                                ManTemplabel.Text = null;
                                ManWetlabel.Text = null;
                                ManBatlabel.Text = null;

                              //  Manlabel1.Text += "NONE";
                                SNManlabel.Text += "NONE";
                                ManTemplabel.Text += "NONE";
                                ManWetlabel.Text += "NONE";
                                ManBatlabel.Text += "NONE";

                              //  Manlabel1.ForeColor = Color.Red;
                                SNManlabel.ForeColor = Color.Red;
                                ManTemplabel.ForeColor = Color.Red;
                                ManWetlabel.ForeColor = Color.Red;
                                ManBatlabel.ForeColor = Color.Red;

                                MessageBox.Show("Теплотехника (4B709FD) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }

                        }
                        //Приемка (4B707D0)
                        else if (reader["sensor_id"].ToString() == "10" && Otdelcombo.SelectedIndex == 0)
                        {
                            Teplabel1.Text = null;
                            SNTeplabel.Text = null;
                            TempTeplabel.Text = null;
                            WetTeplabel.Text = null;
                            BatTeplabel.Text = null;

                            String s = reader["EUI64"].ToString();
                            String s2 = s.Substring(s.Length - 7);

                            Teplabel1.Text += reader["name"] + " (" + s2 + ")";

                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                            SNTeplabel.Text += date_last_update.ToString();

                            TempTeplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                TempTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                TempTeplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                WetTeplabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    WetTeplabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    WetTeplabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                WetTeplabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    WetTeplabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    WetTeplabel.ForeColor = Color.Red;
                                }
                            }

                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);

                            BatTeplabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                              //  Teplabel1.Text = null;
                                SNTeplabel.Text = null;
                                TempTeplabel.Text = null;
                                WetTeplabel.Text = null;
                                BatTeplabel.Text = null;

                             //   Teplabel1.Text += "NONE";
                                SNTeplabel.Text += "NONE";
                                TempTeplabel.Text += "NONE";
                                WetTeplabel.Text += "NONE";
                                BatTeplabel.Text += "NONE";

                              //  Teplabel1.ForeColor = Color.Red;
                                SNTeplabel.ForeColor = Color.Red;
                                TempTeplabel.ForeColor = Color.Red;
                                WetTeplabel.ForeColor = Color.Red;
                                BatTeplabel.ForeColor = Color.Red;

                                MessageBox.Show("Приемка (4B707D0) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }

                        }
                       
                    }
                    connections.Close();
                }

            }
            else
            {
                string path = @"\\WINSERVER\data\measurement.db3";
                string path1 = System.Environment.CurrentDirectory + @"\measurement.db3";

                string info = File.GetLastWriteTime(path).ToString();
                infolabel.Text = "Информация на " + info;

                Prilabel1.Visible = true;
                SNPrilabel.Visible = true;
                TempPrilabel.Visible = true;
                WetPrilabel.Visible = true;
                BatPrilabel.Visible = true;
                this.Text = "Система мониторинга микроклимата помощений (отдел МиЛУИ)";

                FileInfo check = new FileInfo(path1);
                if (check.Exists)
                {
                    File.Copy(path, path1, true);
                }
                else
                {
                    File.Copy(path, path1, true);
                }

                using (SQLiteConnection connections = new SQLiteConnection(@"Data Source=" + path1))
                {
                    connections.Open();                                    

                    SQLiteCommand command = new SQLiteCommand
                        (
                        "SELECT sensor_id, MAX(timemeasure) AS timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id > 0 group by sensor_id",
                        connections
                        );
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //Пенал (4B702D7)
                        if (reader["sensor_id"].ToString() == "1" && Otdelcombo.SelectedIndex == 1)
                        {
                            ELlabel1.Text = null;
                            SNELlabel.Text = null;
                            ElTemplabel.Text = null;
                            ElWetlabel.Text = null;
                            ElBatlabel.Text = null;

                            ELlabel1.ForeColor = Color.Black;
                            SNELlabel.ForeColor = Color.Black;
                            ElTemplabel.ForeColor = Color.Green;
                            ElWetlabel.ForeColor = Color.Green;
                            ElBatlabel.ForeColor = Color.Black;

                            String s = reader["EUI64"].ToString();
                            String s2 = s.Substring(s.Length - 7);

                            ELlabel1.Text += reader["name"] + " (" + s2 + ")";

                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                            SNELlabel.Text += date_last_update.ToString();

                            ElTemplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                ElTemplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ElTemplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                ElWetlabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    ElWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    ElWetlabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                ElWetlabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    ElWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    ElWetlabel.ForeColor = Color.Red;
                                }
                            }

                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);

                            ElBatlabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                               // ELlabel1.Text = null;
                                SNELlabel.Text = null;
                                ElTemplabel.Text = null;
                                ElWetlabel.Text = null;
                                ElBatlabel.Text = null;

                               // ELlabel1.Text += "NONE";
                                SNELlabel.Text += "NONE";
                                ElTemplabel.Text += "NONE";
                                ElWetlabel.Text += "NONE";
                                ElBatlabel.Text += "NONE";

                               // ELlabel1.ForeColor = Color.Red;
                                SNELlabel.ForeColor = Color.Red;
                                ElTemplabel.ForeColor = Color.Red;
                                ElWetlabel.ForeColor = Color.Red;
                                ElBatlabel.ForeColor = Color.Red;

                                MessageBox.Show("Пенал (4B702D7) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }

                        }
                        //Подвал (4B6FС97)
                        else if (reader["sensor_id"].ToString() == "4" && Otdelcombo.SelectedIndex == 1)
                        {
                            Fizhimlabel1.Text = null;
                            SNFizhimlabel.Text = null;
                            FizhimTemplabel.Text = null;
                            FizhimWetlabel.Text = null;
                            FizhimBatlabel.Text = null;

                            Fizhimlabel1.ForeColor = Color.Black;
                            SNFizhimlabel.ForeColor = Color.Black;
                            FizhimTemplabel.ForeColor = Color.Green;
                            FizhimWetlabel.ForeColor = Color.Green;
                            FizhimBatlabel.ForeColor = Color.Black;

                            String s = reader["EUI64"].ToString();
                            String s2 = s.Substring(s.Length - 7);

                            Fizhimlabel1.Text += reader["name"] + " (" + s2 + ")";

                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                            SNFizhimlabel.Text += date_last_update.ToString();

                            FizhimTemplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                FizhimTemplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                FizhimTemplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                FizhimWetlabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    FizhimWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    FizhimWetlabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                FizhimWetlabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    FizhimWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    FizhimWetlabel.ForeColor = Color.Red;
                                }
                            }


                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);
                            FizhimBatlabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                              //  Fizhimlabel1.Text = null;
                                SNFizhimlabel.Text = null;
                                FizhimTemplabel.Text = null;
                                FizhimWetlabel.Text = null;
                                FizhimBatlabel.Text = null;

                               // Fizhimlabel1.Text += "NONE";
                                SNFizhimlabel.Text += "NONE";
                                FizhimTemplabel.Text += "NONE";
                                FizhimWetlabel.Text += "NONE";
                                FizhimBatlabel.Text += "NONE";

                               // Fizhimlabel1.ForeColor = Color.Red;
                                SNFizhimlabel.ForeColor = Color.Red;
                                FizhimTemplabel.ForeColor = Color.Red;
                                FizhimWetlabel.ForeColor = Color.Red;
                                FizhimBatlabel.ForeColor = Color.Red;

                                MessageBox.Show("Подвал (4B6FС97) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }
                        }
                        //Мерники (4B6E621)
                        else if (reader["sensor_id"].ToString() == "6" && Otdelcombo.SelectedIndex == 1)
                        {
                            Podlabel1.Text = null;
                            SNPodlabel.Text = null;
                            PodTemplabel.Text = null;
                            PodWetlabel.Text = null;
                            PodBatlabel.Text = null;

                            Podlabel1.ForeColor = Color.Black;
                            SNPodlabel.ForeColor = Color.Black;
                            PodTemplabel.ForeColor = Color.Green;
                            PodWetlabel.ForeColor = Color.Green;
                            PodBatlabel.ForeColor = Color.Black;

                            String s = reader["EUI64"].ToString();
                            String s2 = s.Substring(s.Length - 7);

                            Podlabel1.Text += reader["name"] + " (" + s2 + ")";

                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                            SNPodlabel.Text += date_last_update.ToString();

                            PodTemplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                PodTemplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                PodTemplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                PodWetlabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    PodWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    PodWetlabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                PodWetlabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    PodWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    PodWetlabel.ForeColor = Color.Red;
                                }
                            }

                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);

                            PodBatlabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                               // Podlabel1.Text = null;
                                SNPodlabel.Text = null;
                                PodTemplabel.Text = null;
                                PodWetlabel.Text = null;
                                PodBatlabel.Text = null;

                              //  Podlabel1.Text += "NONE";
                                SNPodlabel.Text += "NONE";
                                PodTemplabel.Text += "NONE";
                                PodWetlabel.Text += "NONE";
                                PodBatlabel.Text += "NONE";

                               // Podlabel1.ForeColor = Color.Red;
                                SNPodlabel.ForeColor = Color.Red;
                                PodTemplabel.ForeColor = Color.Red;
                                PodWetlabel.ForeColor = Color.Red;
                                PodBatlabel.ForeColor = Color.Red;

                                MessageBox.Show("Мерники (4B6E621) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }
                        }
                        //Весы (4B6FB5A)
                        else if (reader["sensor_id"].ToString() == "7" && Otdelcombo.SelectedIndex == 1)
                        {
                            Manlabel1.Text = null;
                            SNManlabel.Text = null;
                            ManTemplabel.Text = null;
                            ManWetlabel.Text = null;
                            ManBatlabel.Text = null;

                            Manlabel1.ForeColor = Color.Black;
                            SNManlabel.ForeColor = Color.Black;
                            ManTemplabel.ForeColor = Color.Green;
                            ManWetlabel.ForeColor = Color.Green;
                            ManBatlabel.ForeColor = Color.Black;

                            String s = reader["EUI64"].ToString();
                            String s2 = s.Substring(s.Length - 7);

                            Manlabel1.Text += reader["name"] + " (" + s2 + ")";

                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                            SNManlabel.Text += date_last_update.ToString();

                            ManTemplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                ManTemplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                ManTemplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                ManWetlabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    ManWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    ManWetlabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                ManWetlabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    ManWetlabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    ManWetlabel.ForeColor = Color.Red;
                                }
                            }

                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);

                            ManBatlabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                              //  Manlabel1.Text = null;
                                SNManlabel.Text = null;
                                ManTemplabel.Text = null;
                                ManWetlabel.Text = null;
                                ManBatlabel.Text = null;

                               // Manlabel1.Text += "NONE";
                                SNManlabel.Text += "NONE";
                                ManTemplabel.Text += "NONE";
                                ManWetlabel.Text += "NONE";
                                ManBatlabel.Text += "NONE";

                               // Manlabel1.ForeColor = Color.Red;
                                SNManlabel.ForeColor = Color.Red;
                                ManTemplabel.ForeColor = Color.Red;
                                ManWetlabel.ForeColor = Color.Red;
                                ManBatlabel.ForeColor = Color.Red;

                                MessageBox.Show("Весы (4B6FB5A) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }
                        }
                        //Механики (4B6FBA5)
                        else if (reader["sensor_id"].ToString() == "10" && Otdelcombo.SelectedIndex == 1)
                        {
                            Teplabel1.Text = null;
                            SNTeplabel.Text = null;
                            TempTeplabel.Text = null;
                            WetTeplabel.Text = null;
                            BatTeplabel.Text = null;

                            Teplabel1.ForeColor = Color.Black;
                            SNTeplabel.ForeColor = Color.Black;
                            TempTeplabel.ForeColor = Color.Green;
                            WetTeplabel.ForeColor = Color.Green;
                            BatTeplabel.ForeColor = Color.Black;

                            String s = reader["EUI64"].ToString();
                            String s2 = s.Substring(s.Length - 7);

                            Teplabel1.Text += reader["name"] + " (" + s2 + ")";

                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                            SNTeplabel.Text += date_last_update.ToString();

                            TempTeplabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                TempTeplabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                TempTeplabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                WetTeplabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    WetTeplabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    WetTeplabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                WetTeplabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    WetTeplabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    WetTeplabel.ForeColor = Color.Red;
                                }
                            }

                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);

                            BatTeplabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                               // Teplabel1.Text = null;
                                SNTeplabel.Text = null;
                                TempTeplabel.Text = null;
                                WetTeplabel.Text = null;
                                BatTeplabel.Text = null;

                               // Teplabel1.Text += "NONE";
                                SNTeplabel.Text += "NONE";
                                TempTeplabel.Text += "NONE";
                                WetTeplabel.Text += "NONE";
                                BatTeplabel.Text += "NONE";

                               // Teplabel1.ForeColor = Color.Red;
                                SNTeplabel.ForeColor = Color.Red;
                                TempTeplabel.ForeColor = Color.Red;
                                WetTeplabel.ForeColor = Color.Red;
                                BatTeplabel.ForeColor = Color.Red;

                                MessageBox.Show("Механики (4B6FBA5) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }

                        }
                        //Линейно-угловые (4B70245)
                        else if (reader["sensor_id"].ToString() == "11" && Otdelcombo.SelectedIndex == 1)
                        {
                            Prilabel1.Text = null;
                            SNPrilabel.Text = null;
                            TempPrilabel.Text = null;
                            WetPrilabel.Text = null;
                            BatPrilabel.Text = null;

                            Prilabel1.ForeColor = Color.Black;
                            SNPrilabel.ForeColor = Color.Black;
                            TempPrilabel.ForeColor = Color.Green;
                            WetPrilabel.ForeColor = Color.Green;
                            BatPrilabel.ForeColor = Color.Black;

                            String s = reader["EUI64"].ToString();
                            String s2 = s.Substring(s.Length - 7);

                            Prilabel1.Text += reader["name"];

                            if (Prilabel1.Text == "Линейно-угловые")
                            {
                                Prilabel1.Text = null;
                                Prilabel1.Text += "Лин-угловые" + " (" + s2 + ")";
                            }

                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                            SNPrilabel.Text += date_last_update.ToString();

                            TempPrilabel.Text += reader["temperature"] + " °C";

                            double temp = Convert.ToDouble(reader["temperature"]);
                            if (temp > 15.0 && temp < 25.0)
                            {
                                TempPrilabel.ForeColor = Color.Green;
                            }
                            else
                            {
                                TempPrilabel.ForeColor = Color.Red;
                            }

                            DateTime currdate = DateTime.Now;
                            int month = currdate.Month;
                            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 10 || month == 11 || month == 12)
                            {
                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                double wetnes_int = wetnes_int1 + 10;
                                WetPrilabel.Text += wetnes_int.ToString() + " %";

                                if (wetnes_int > 30.0 && wetnes_int < 80.0)
                                {
                                    WetPrilabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    WetPrilabel.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                WetPrilabel.Text += reader["wetness"] + " %";

                                double wetnes_int1 = Convert.ToDouble(reader["wetness"]);
                                if (wetnes_int1 > 30.0 && wetnes_int1 < 80.0)
                                {
                                    WetPrilabel.ForeColor = Color.Green;
                                }
                                else
                                {
                                    WetPrilabel.ForeColor = Color.Red;
                                }
                            }


                            String s3 = reader["battery"].ToString();
                            String s4 = s3.Substring(0, 4);

                            BatPrilabel.Text += s4 + " V";

                            DateTime check_status = DateTime.Now.AddDays(-1);
                            if (check_status > date_last_update)
                            {
                              //  Prilabel1.Text = null;
                                SNPrilabel.Text = null;
                                TempPrilabel.Text = null;
                                WetPrilabel.Text = null;
                                BatPrilabel.Text = null;

                                ///Prilabel1.Text += "NONE";
                                SNPrilabel.Text += "NONE";
                                TempPrilabel.Text += "NONE";
                                WetPrilabel.Text += "NONE";
                                BatPrilabel.Text += "NONE";

                               // Prilabel1.ForeColor = Color.Red;
                                SNPrilabel.ForeColor = Color.Red;
                                TempPrilabel.ForeColor = Color.Red;
                                WetPrilabel.ForeColor = Color.Red;
                                BatPrilabel.ForeColor = Color.Red;

                                MessageBox.Show("Линейно-угловые (4B70245) не отвечает!\nЗамените батарейки в датчике!", "ВНИМАНИЕ");
                            }
                        }
                    }
                    connections.Close();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //устанавливает флаг отмены события в истину
                e.Cancel = true;
                //спрашивает стоит ли завершится
                if (MessageBox.Show("Вы уверены что хотите закрыть программу?", "Система мониторинга микроклимата помощений", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Text = "Система мониторинга микроклимата помощений (формирование отчета)";

            date_otchet.Visible = true;
            date_otchet2.Visible = true;
            combo_otch_datchik.Visible = true;

            date_otchet.Format = DateTimePickerFormat.Custom;
            date_otchet.CustomFormat = "dd/MM/yyyy hh:mm:ss";

            date_otchet2.Format = DateTimePickerFormat.Custom;
            date_otchet2.CustomFormat = "dd/MM/yyyy hh:mm:ss";


            button3.Visible = true;
            button4.Visible = true;

            ViewGrid_main.Visible = true;
            button1.Visible = false;
            button2.Visible = false;
            Otdelcombo.Visible = false;
            infolabel.Visible = false;

            ELlabel1.Visible = false;
            Fizhimlabel1.Visible = false;
            Podlabel1.Visible = false;
            Manlabel1.Visible = false;
            Teplabel1.Visible = false;
            Prilabel1.Visible = false;
            label5.Visible = false;

            label1.Visible = false;
            SNELlabel.Visible = false;
            SNFizhimlabel.Visible = false;
            SNPodlabel.Visible = false;
            SNManlabel.Visible = false;
            SNTeplabel.Visible = false;
            SNPrilabel.Visible = false;

            label4.Visible = false;
            ElTemplabel.Visible = false;
            FizhimTemplabel.Visible = false;
            PodTemplabel.Visible = false;
            ManTemplabel.Visible = false;
            TempTeplabel.Visible = false;
            TempPrilabel.Visible = false;

            label3.Visible = false;
            ElWetlabel.Visible = false;
            FizhimWetlabel.Visible = false;
            PodWetlabel.Visible = false;
            ManWetlabel.Visible = false;
            WetTeplabel.Visible = false;
            WetPrilabel.Visible = false;

            label2.Visible = false;
            ElBatlabel.Visible = false;
            FizhimBatlabel.Visible = false;
            PodBatlabel.Visible = false;
            ManBatlabel.Visible = false;
            BatTeplabel.Visible = false;
            BatPrilabel.Visible = false;            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Text = "Система мониторинга микроклимата помощений (отдел ЭМиТТИ)";

            ViewGrid_main.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            Otdelcombo.Visible = true;
            infolabel.Visible = true;

            ELlabel1.Visible = true;
            Fizhimlabel1.Visible = true;
            Podlabel1.Visible = true;
            Manlabel1.Visible = true;
            Teplabel1.Visible = true;
            Prilabel1.Visible = false;
            label5.Visible = true;

            label1.Visible = true;
            SNELlabel.Visible = true;
            SNFizhimlabel.Visible = true;
            SNPodlabel.Visible = true;
            SNManlabel.Visible = true;
            SNTeplabel.Visible = true;
            SNPrilabel.Visible = false;

            label4.Visible = true;
            ElTemplabel.Visible = true;
            FizhimTemplabel.Visible = true;
            PodTemplabel.Visible = true;
            ManTemplabel.Visible = true;
            TempTeplabel.Visible = true;
            TempPrilabel.Visible = false;

            label3.Visible = true;
            ElWetlabel.Visible = true;
            FizhimWetlabel.Visible = true;
            PodWetlabel.Visible = true;
            ManWetlabel.Visible = true;
            WetTeplabel.Visible = true;
            WetPrilabel.Visible = false;

            label2.Visible = true;
            ElBatlabel.Visible = true;
            FizhimBatlabel.Visible = true;
            PodBatlabel.Visible = true;
            ManBatlabel.Visible = true;
            BatTeplabel.Visible = true;
            BatPrilabel.Visible = false;

            button4.Visible = false;
            button3.Visible = false;

            date_otchet.Visible = false;
            date_otchet2.Visible = false;
            combo_otch_datchik.Visible = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(combo_otch_datchik.Text))
            {
                MessageBox.Show("Не выбран датчик для создания отчёта","Ошибка создания отчёта");
            }
            else
            {
                ViewGrid_main.Rows.Clear();

                DateTime date = date_otchet.Value;
                DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                TimeSpan tsInterval = date.Subtract(dt1970);
                Int32 iSeconds = Convert.ToInt32(tsInterval.TotalSeconds);
                iSeconds = iSeconds - 1;
                //MessageBox.Show(date.ToString()+" ->"+iSeconds.ToString());

                DateTime date_2 = date_otchet2.Value;
                DateTime dt1970_2 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                TimeSpan tsInterval_2 = date_2.Subtract(dt1970_2);
                Int32 iSeconds_2 = Convert.ToInt32(tsInterval_2.TotalSeconds);
                iSeconds_2 = iSeconds_2 - 1;
                //MessageBox.Show(date.ToString() + " ->" + iSeconds.ToString()+"\n"+date_2.ToString() + " ->" + iSeconds_2.ToString());

                Int32 comboindex = combo_otch_datchik.SelectedIndex;
                comboindex += 1;

                //MessageBox.Show("index ->"+ comboindex);

                string path1 = System.Environment.CurrentDirectory + @"\measurement.db3";


                using (SQLiteConnection connections = new SQLiteConnection(@"Data Source=" + path1))
                {
                    connections.Open();
                    SQLiteCommand command = new SQLiteCommand
                    (
                    "SELECT sensor_id, timemeasure, temperature, wetness, battery, name, EUI64 FROM measure, sensors WHERE sensors.id = sensor_id AND sensor_id =" + comboindex + " AND timemeasure >" +
                    iSeconds.ToString() + " AND timemeasure <" + iSeconds_2.ToString() + ";",
                    connections
                    );

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int timestamp = Convert.ToInt32(reader["timemeasure"]);
                            DateTime date_last_update = new DateTime(1970, 1, 1).AddSeconds(timestamp);

                            ViewGrid_main.Rows.Add(new object[]
                            {
                                reader.GetValue(reader.GetOrdinal("sensor_id")),
                                date_last_update.ToString(),
                               // reader.GetValue(reader.GetOrdinal("timemeasure")),
                                reader.GetValue(reader.GetOrdinal("temperature")),
                                reader.GetValue(reader.GetOrdinal("wetness")),
                                reader.GetValue(reader.GetOrdinal("battery")),
                                reader.GetValue(reader.GetOrdinal("name")),
                                reader.GetValue(reader.GetOrdinal("EUI64"))
                            }); 
                        }
                    }

                }

            }
            
        }
    }
}

