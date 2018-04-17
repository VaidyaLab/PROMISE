using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Timers;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
namespace Final_Kinect
{
    public partial class SubjectMovieForm : Form
    {
        //initializing Kinect Sensor
        KinectSensor kinect_sensor = null;
        //Body frame reader used to detect body by kinect
        MultiSourceFrameReader bodyframe_reader = null;
        Body[] body = null;
        int count = 0;

        String s = "00:40:00";
        //variables were all the angles information is stored
        int x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, y1, y2, y3, y4, y5, y6, y7, y8, y9, y10;
        //variables used to get the average values
        int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0, sum5 = 0, sum6 = 0;
        double avg1, avg2, avg3, avg4, avg5, avg6;
        int mean_sum1 = 0, mean_sum2 = 0, mean_sum3 = 0, mean_sum4 = 0, mean_sum5 = 0, mean_sum6 = 0;
        int avg_mean1, avg_mean2, avg_mean3, avg_mean4, avg_mean5, avg_mean6,session_time=0;
        int counter_value1, counter_value2, counter_value3, counter_value4, counter_value5, counter_value6, counter_value7, counter_value8, counter_value9, counter_value10, counter_value11, counter_value12;

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_2(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - startTime;

            progressBar1.Maximum = (2 * session_time);
           /* progressBar1.CreateGraphics().DrawString("  P     R     O     M     I     S     E", new Font("Arial",
                                                           (float)55, FontStyle.Bold),
                                                           Brushes.White, new System.Drawing.PointF(0, 2));*/

            counter++;
            if (counter % 60 == 0)
            {
                if (counter_value5 == 1)
                {
                    progressBar1.Increment(1);
                    progressBar1.Update();

                    //progressBar1.Value = counter;

                }
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - startTime;
            progressBar1.Maximum = (2 * session_time);

           /* progressBar1.CreateGraphics().DrawString("  P     R     O     M     I     S     E", new Font("Arial",
                                                           (float)55, FontStyle.Bold),
                                                           Brushes.White, new System.Drawing.PointF(0, 2));*/

            counter++;
            if (counter % 60 == 0)
            {
                if (counter_value5 == 1)
                {
                    progressBar1.Increment(1);
                    progressBar1.Update();

                    //progressBar1.Value = counter;

                }
            }
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            /*count = 22;

            button1.BackColor = Color.DeepSkyBlue;*/
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        int mean_counter = 1;
        int average_tagged = 0;
        int base_smoothing;
        int[] median_smoothing1;
        int[] median_smoothing2;
        int[] median_smoothing3;
        int[] median_smoothing4;
        int[] median_smoothing5;
        int[] median_smoothing6;
        int warning;
        int notallowed;
        int median1, median2, median3, median4, median5, median6;
        static string user_name;
        static string experiment_no;
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
        //Method to get the current date time and the time difference
        DateTime startTime = new DateTime();
        TimeSpan elapsedTime = new TimeSpan();
        DateTime totalTime = new DateTime();
        TimeSpan elapsedTime1 = new TimeSpan();
        int counter = 0;
        int counter1 = 0;
        int counter_sum = 0;
        int counter_original = 0;
        int counter_warning = 0;
        int counter_na = 0;
        //arrays where the temperoray values are stored
        int[] temp1 = new int[2];
        int[] temp2 = new int[2];
        int[] temp3 = new int[2];
        int[] temp4 = new int[2];
        int[] temp5 = new int[2];
        int[] temp6 = new int[2];
        int[] temp7 = new int[2];
        int[] temp8 = new int[2];
        //Timer method
        private void timer1_Tick(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - startTime;

            progressBar1.Maximum = (2 * session_time);
           /* progressBar1.CreateGraphics().DrawString("  P     R     O     M     I     S     E", new Font("Arial",
                                                           (float)55, FontStyle.Bold),
                                                           Brushes.White, new System.Drawing.PointF(0, 2));*/

            counter++;
            if (counter % 60 == 0)
            {
                if (counter_value5 == 1)
                {
                    progressBar1.Increment(1);
                    progressBar1.Update();

                    //progressBar1.Value = counter;

                }
            }
            //textBox1.Text =elapsedTime.ToString(@"hh\:mm\:ss");
        }


        public SubjectMovieForm(string pass_value3, string pass_value4, string pass_value5, int counter_new, int counter_1, int counter_2, int counter_3, int counter_4, int counter_5, int counter_6, int counter_7, int counter8, int counter_9, int counter_10, int counter_11,int time_session,string pass_value7)
        {
            
            InitializeComponent();
            //user_name = pass_value1;
            //experiment_no = pass_value2;
            base_smoothing = Convert.ToInt32(pass_value3);
            warning = Convert.ToInt32(pass_value4);
            notallowed = Convert.ToInt32(pass_value5);
            session_time = time_session;
            median_smoothing1 = new int[base_smoothing];
            median_smoothing2 = new int[base_smoothing];
            median_smoothing3 = new int[base_smoothing];
            median_smoothing4 = new int[base_smoothing];
            median_smoothing5 = new int[base_smoothing];
            median_smoothing6 = new int[base_smoothing];
            counter_value1 = counter_new;
            counter_value2 = counter_1;
            counter_value3 = counter_2;
            counter_value4 = counter_3;
            counter_value5 = counter_4;
            counter_value6 = counter_5;
            counter_value7 = counter_6;
            counter_value8 = counter_7;
            counter_value9 = counter8;
            counter_value10 = counter_9;
            counter_value11 = counter_10;
            counter_value12 = counter_11;
            axWindowsMediaPlayer1.URL = pass_value7;
           
            if (counter_value1 == 0)
            {
                axWindowsMediaPlayer1.Hide();
            }
            if (counter_value4 == 0)
            {
                progressBar1.Hide();
            }
            if (counter_value7 == 0)
            {

                pictureBox1.Hide();
                pictureBox2.Hide();
                pictureBox3.Hide();
            }
            if (counter_value8 == 0)
            {
                pictureBox1.Hide();
                pictureBox2.Hide();
                pictureBox3.Hide();
            }
           /* a1 = DateTime.Now.ToString("yyyyMMdd");
            a = Path.Combine("C:\\Users\\Manish Vaidya\\Desktop\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "raw" + ".csv");
            a2 = Path.Combine("C:\\Users\\Manish Vaidya\\Desktop\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "video" + ".csv");
            a3 = Path.Combine("C:\\Users\\Manish Vaidya\\Desktop\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "average" + ".csv");
            a4 = Path.Combine("C:\\Users\\Manish Vaidya\\Desktop\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "mean" + ".csv");
            a5 = Path.Combine("C:\\Users\\Manish Vaidya\\Desktop\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "median" + ".csv");
            file = new StreamWriter(a, true);
            file1 = new StreamWriter(a2, true);
            file2 = new StreamWriter(a3, true);
            file3 = new StreamWriter(a4, true);
            file4 = new StreamWriter(a5, true);*/
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.KeyPreview = true;
            totalTime = DateTime.Parse(s);
           // button1.BackColor = Color.Red;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000;
            timer1.Enabled = false;
            startTime = DateTime.Now;
            //Writing to three files(Main data file, video reference file and average file)
            /*file.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            file1.WriteLine("ElapsedTime" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck");
            file2.WriteLine("Date&Time" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            file3.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            file4.WriteLine("Date&Time" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
*/            
            //transfer_values();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 baseline = new Form5();
            baseline.ShowDialog();
        }

        public void transfer_values(int count,int angle_1,int angle_2,int angle_3,int angle_4,int angle_5,int angle_6,int warning1,int notallowed1,int count1)
        {
            // Console.WriteLine("Hello World");
            warning = warning1;
            notallowed = notallowed1;
            count = count1;
            if(count==21)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                timer1.Enabled = false;
            }
            //once the user has pressed the start button
                    if (count == 22)
                        {
                // axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.settings.mute =true;
                counter_sum++;
                            timer1.Enabled = true;
                            y1 = angle_1;
                            y2 = angle_2;
                            y3 = angle_3;
                            y4 = angle_4;
                            y5 = angle_5;
                            y6 = angle_6;
                            median_smoothing1[mean_counter - 1] = y1;
                            median_smoothing2[mean_counter - 1] = y2;
                            median_smoothing3[mean_counter - 1] = y3;
                            median_smoothing4[mean_counter - 1] = y4;
                            median_smoothing5[mean_counter - 1] = y5;
                            median_smoothing6[mean_counter - 1] = y6;
                            sum1 = sum1 + y1;
                            sum2 = sum2 + y2;
                            sum3 = sum3 + y3;
                            sum4 = sum4 + y4;
                            sum5 = sum5 + y5;
                            sum6 = sum6 + y6;

                            mean_sum1 = mean_sum1 + y1;
                            mean_sum2 = mean_sum2 + y2;
                            mean_sum3 = mean_sum3 + y3;
                            mean_sum4 = mean_sum4 + y4;
                            mean_sum5 = mean_sum5 + y5;
                            mean_sum6 = mean_sum6 + y6;


                            if (mean_counter == base_smoothing)
                            {
                                avg_mean1 = mean_sum1 / base_smoothing;
                                avg_mean2 = mean_sum2 / base_smoothing;
                                avg_mean3 = mean_sum3 / base_smoothing;
                                avg_mean4 = mean_sum4 / base_smoothing;
                                avg_mean5 = mean_sum5 / base_smoothing;
                                avg_mean6 = mean_sum6 / base_smoothing;
                                int tmp;
                                for (int i = 0; i < base_smoothing; i++)
                                {
                                    for (int j = i + 1; j < base_smoothing; j++)
                                    {
                                        if (median_smoothing1[j] < median_smoothing1[i])
                                        {
                                            tmp = median_smoothing1[i];
                                            median_smoothing1[i] = median_smoothing1[j];
                                            median_smoothing1[j] = tmp;
                                        }
                                    }
                                }
                                if (base_smoothing % 2 == 0)
                                    median1 = (median_smoothing1[base_smoothing / 2] + median_smoothing1[base_smoothing / 2 + 1]) / 2;
                                else
                                    median1 = median_smoothing1[(base_smoothing + 1) / 2];
                                for (int i = 0; i < base_smoothing; i++)
                                {
                                    for (int j = i + 1; j < base_smoothing; j++)
                                    {
                                        if (median_smoothing2[j] < median_smoothing2[i])
                                        {
                                            tmp = median_smoothing2[i];
                                            median_smoothing2[i] = median_smoothing2[j];
                                            median_smoothing2[j] = tmp;
                                        }
                                    }
                                }
                                if (base_smoothing % 2 == 0)
                                    median2 = (median_smoothing2[base_smoothing / 2] + median_smoothing2[base_smoothing / 2 + 1]) / 2;
                                else
                                    median2 = median_smoothing2[(base_smoothing + 1) / 2];
                                for (int i = 0; i < base_smoothing; i++)
                                {
                                    for (int j = i + 1; j < base_smoothing; j++)
                                    {
                                        if (median_smoothing3[j] < median_smoothing3[i])
                                        {
                                            tmp = median_smoothing3[i];
                                            median_smoothing3[i] = median_smoothing3[j];
                                            median_smoothing3[j] = tmp;
                                        }
                                    }
                                }
                                if (base_smoothing % 2 == 0)
                                    median3 = (median_smoothing3[base_smoothing / 2] + median_smoothing3[base_smoothing / 2 + 1]) / 2;
                                else
                                    median3 = median_smoothing3[(base_smoothing + 1) / 2];
                                for (int i = 0; i < base_smoothing; i++)
                                {
                                    for (int j = i + 1; j < base_smoothing; j++)
                                    {
                                        if (median_smoothing4[j] < median_smoothing4[i])
                                        {
                                            tmp = median_smoothing4[i];
                                            median_smoothing4[i] = median_smoothing4[j];
                                            median_smoothing4[j] = tmp;
                                        }
                                    }
                                }
                                if (base_smoothing % 2 == 0)
                                    median4 = (median_smoothing4[base_smoothing / 2] + median_smoothing4[base_smoothing / 2 + 1]) / 2;
                                else
                                    median4 = median_smoothing4[(base_smoothing + 1) / 2];
                                for (int i = 0; i < base_smoothing; i++)
                                {
                                    for (int j = i + 1; j < base_smoothing; j++)
                                    {
                                        if (median_smoothing5[j] < median_smoothing5[i])
                                        {
                                            tmp = median_smoothing5[i];
                                            median_smoothing5[i] = median_smoothing5[j];
                                            median_smoothing5[j] = tmp;
                                        }
                                    }
                                }
                                if (base_smoothing % 2 == 0)
                                    median5 = (median_smoothing5[base_smoothing / 2] + median_smoothing5[base_smoothing / 2 + 1]) / 2;
                                else
                                    median5 = median_smoothing5[(base_smoothing + 1) / 2];
                                for (int i = 0; i < base_smoothing; i++)
                                {
                                    for (int j = i + 1; j < base_smoothing; j++)
                                    {
                                        if (median_smoothing6[j] < median_smoothing6[i])
                                        {
                                            tmp = median_smoothing6[i];
                                            median_smoothing6[i] = median_smoothing6[j];
                                            median_smoothing6[j] = tmp;
                                        }
                                    }
                                }
                                if (base_smoothing % 2 == 0)
                                    median6 = (median_smoothing6[base_smoothing / 2] + median_smoothing6[base_smoothing / 2 + 1]) / 2;
                                else
                                    median6 = median_smoothing6[(base_smoothing + 1) / 2];
                                if (median1 > (x1 + notallowed) || median1 < (x1 - notallowed) || median2 > (x2 + notallowed) || median2 < (x2 - notallowed) || median3 > (x3 + notallowed) || median3 < (x3 - notallowed) || median4 > (x4 + notallowed) || median4 < (x4 - notallowed) || median5 > (x5 + notallowed) || median5 < (x5 - notallowed) || median6 > (x6 + notallowed) || median6 < (x6 - notallowed))
                                {
                                    if (counter_value12 == 1)
                                    {
                                        if (counter_value2 == 1)
                                        {
                                            if (counter_value3 == 1)
                                                axWindowsMediaPlayer1.Ctlcontrols.pause();
                                            else
                                                axWindowsMediaPlayer1.Ctlcontrols.play();
                                        }
                                        if (counter_value6 == 1)
                                        {
                                            timer1.Enabled = false;
                                        }
                                        else
                                        {
                                            timer1.Enabled = true;
                                        }
                                        if (counter_value9 == 1)
                                        {
                                            pictureBox1.Visible = true;
                                            pictureBox2.Visible = false;
                                            pictureBox3.Visible = false;
                                            count = 21;
                                        }
                                        else
                                        {
                                            pictureBox3.Visible = true;
                                        }
                                    }
                                    if (average_tagged == 1)
                                    {
                                        //ile4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6 + "," + "Tagged");
                                        //average_tagged = 0;
                                    }
                                    //if there was not tagged moment
                                    else { }
                                    //file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6);
                                }
                                else if (median1 > (x1 + warning) || median1 < (x1 - warning) || median2 > (x2 + warning) || median2 < (x2 - warning) || median3 > (x3 + warning) || median3 < (x3 - warning) || median4 > (x4 + warning) || median4 < (x4 - warning) || median5 > (x5 + warning) || median5 < (x5 - warning) || median6 > (x6 + warning) || median6 < (x6 - warning))
                                {
                                    if (counter_value12 == 1)
                                    {
                                        timer1.Enabled = true;
                                        if (counter_value2 == 1)
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                        if (counter_value9 == 1)
                                        {
                                            pictureBox1.Visible = false;
                                            pictureBox2.Visible = true;
                                            pictureBox3.Visible = false;
                                        }
                                        else
                                        {
                                            pictureBox3.Visible = true;
                                        }
                                    }
                                    if (average_tagged == 1)
                                    {
                                        //file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6 + "," + "Tagged");
                                        //average_tagged = 0;
                                    }
                                    //if there was not tagged moment
                                    else { }
                                    //file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6);
                                }
                                else
                                {
                                    if (counter_value12 == 1)
                                    {
                                        timer1.Enabled = true;
                                        if (counter_value2 == 1)
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                        if (counter_value9 == 1)
                                        {
                                            pictureBox1.Visible = false;
                                            pictureBox2.Visible = false;
                                            pictureBox3.Visible = true;
                                        }
                                        else
                                        {
                                            pictureBox3.Visible = true;
                                        }
                                    }
                                    if (average_tagged == 1)
                                    {
                                        //file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6 + "," + "Tagged");
                                        //average_tagged = 0;
                                    }
                                    //if there was not tagged moment
                                    else { }
                                    //file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6);
                                }
                                mean_counter = 0;
                                // If there was a tagged moment
                                if (avg_mean1 > (x1 + notallowed) || avg_mean1 < (x1 - notallowed) || avg_mean2 > (x2 + notallowed) || avg_mean2 < (x2 - notallowed) || avg_mean3 > (x3 + notallowed) || avg_mean3 < (x3 - notallowed) || avg_mean4 > (x4 + notallowed) || avg_mean4 < (x4 - notallowed) || avg_mean5 > (x5 + notallowed) || avg_mean5 < (x5 - notallowed) || avg_mean6 > (x6 + notallowed) || avg_mean6 < (x6 - notallowed))
                                {
                                    if (counter_value11 == 1)
                                    {
                            timer1.Enabled = false;
                                        if (counter_value2 == 1)
                                        {
                                            if (counter_value3 == 1)
                                                axWindowsMediaPlayer1.Ctlcontrols.pause();
                                            else
                                                axWindowsMediaPlayer1.Ctlcontrols.play();
                                        }
                                        if (counter_value6 == 1)
                                        {
                                            timer1.Enabled = false;
                                        }
                                        else
                                        {
                                            timer1.Enabled = true;
                                        }
                                        if (counter_value9 == 1)
                                        {
                                            pictureBox1.Visible = true;
                                            pictureBox2.Visible = false;
                                            pictureBox3.Visible = false;
                                            count = 21;
                                        }
                                        else
                                        {
                                            pictureBox3.Visible = true;
                                        }
                                    }
                                    if (average_tagged == 1)
                                    {
                                        //file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6 + "," + "Tagged");
                                        //average_tagged = 0;
                                    }
                                    //if there was not tagged moment
                                    else { }
                                    //file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6);
                                    //initialize all the sum variables to zero

                                }
                                else if (avg_mean1 > (x1 + warning) || avg_mean1 < (x1 - warning) || avg_mean2 > (x2 + warning) || avg_mean2 < (x2 - warning) || avg_mean3 > (x3 + warning) || avg_mean3 < (x3 - warning) || avg_mean4 > (x4 + warning) || avg_mean4 < (x4 - warning) || avg_mean5 > (x5 + warning) || avg_mean5 < (x5 - warning) || avg_mean6 > (x6 + warning) || avg_mean6 < (x6 - warning))
                                {
                                    if (counter_value11 == 1)
                                    {
                                        timer1.Enabled = true;
                                        if (counter_value2 == 1)
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                        if (counter_value9 == 1)
                                        {
                                            pictureBox1.Visible = false;
                                            pictureBox2.Visible = true;
                                            pictureBox3.Visible = false;
                                        }
                                        else
                                        {
                                            pictureBox3.Visible = true;
                                        }
                                    }
                                    if (average_tagged == 1)
                                    {
                                        //file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6 + "," + "Tagged");
                                        //average_tagged = 0;
                                    }
                                    //if there was not tagged moment
                                    else { }
                                    //file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6);
                                    //initialize all the sum variables to zero
                                }
                                else
                                {
                                    if (counter_value11 == 1)
                                    {
                                        timer1.Enabled = true;
                                        if (counter_value2 == 1)
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                        if (counter_value9 == 1)
                                        {
                                            pictureBox1.Visible = false;
                                            pictureBox2.Visible = false;
                                            pictureBox3.Visible = true;
                                        }
                                        else
                                        {
                                            pictureBox3.Visible = true;
                                        }
                                    }
                                    if (average_tagged == 1)
                                    {
                                        // file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6 + "," + "Tagged");
                                        //average_tagged = 0;
                                    }
                                    //if there was not tagged moment
                                    else { }
                                    //file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6);
                                    //initialize all the sum variables to zero
                                }
                                mean_sum1 = 0;
                                mean_sum2 = 0;
                                mean_sum3 = 0;
                                mean_sum4 = 0;
                                mean_sum5 = 0;
                                mean_sum6 = 0;

                            }
                            mean_counter++;
                            if (counter_sum == 15)
                            {
                                avg1 = sum1 / 15;
                                avg2 = sum2 / 15;
                                avg3 = sum3 / 15;
                                avg4 = sum4 / 15;
                                avg5 = sum5 / 15;
                                avg6 = sum6 / 15;

                                counter_sum = 0;
                                // If there was a tagged moment
                                if (average_tagged == 1)
                                {
                                    // file2.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + avg1 + "," + avg2 + "," + avg3 + "," + avg4 + "," + avg5 + "," + avg6 + "," + "Tagged");
                                    average_tagged = 0;
                                }
                                //if there was not tagged moment
                                else { }
                                //file2.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + avg1 + "," + avg2 + "," + avg3 + "," + avg4 + "," + avg5 + "," + avg6);
                                //initialize all the sum variables to zero
                                sum1 = 0;
                                sum2 = 0;
                                sum3 = 0;
                                sum4 = 0;
                                sum5 = 0;
                                sum6 = 0;

                            }
                            //if the movement is more than 3mm
                            if (y1 > (x1 + notallowed) || y1 < (x1 - notallowed) || y2 > (x2 + notallowed) || y2 < (x2 - notallowed) || y3 > (x3 + notallowed) || y3 < (x3 - notallowed) || y4 > (x4 + notallowed) || y4 < (x4 - notallowed) || y5 > (x5 + notallowed) || y5 < (x5 - notallowed) || y6 > (x6 + notallowed) || y6 < (x6 - notallowed))
                            {
                                if (counter_value10 == 1)
                                {
                                    if (counter_value2 == 1)
                                    {
                                        if (counter_value3 == 1)
                                            axWindowsMediaPlayer1.Ctlcontrols.pause();
                                        else
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                    }
                                    if (counter_value6 == 1)
                                    {
                                        timer1.Enabled = false;
                                    }
                                    else
                                    {
                                        timer1.Enabled = true;
                                    }
                                    if (counter_value9 == 1)
                                    {
                                        pictureBox1.Visible = true;
                                        pictureBox2.Visible = false;
                                        pictureBox3.Visible = false;
                                        count = 21;
                                    }
                                    else
                                    {
                                        pictureBox3.Visible = true;
                                    }
                                }
                                counter_na++;
                                if (counter_warning > 0)
                                {
                                    // file1.WriteLine("--------");
                                }
                                if (counter1 == 0)
                                {
                                    if (counter_na == 1)
                                    {
                                        //  file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement" + "," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                        //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement" + "," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                    }

                                    //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                    //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Large Movement" + "," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);

                                }
                                else
                                {
                                    //If there is a small tagged movement
                                    if (counter1 == 1)
                                    {
                                        if (counter_na == 1)
                                        {
                                            //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "SmallMovement");
                                        counter1 = 0;

                                    }
                                    //if there is a medium  tagged movement
                                    if (counter1 == 2)
                                    {
                                        if (counter_na == 1)
                                        {
                                            //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "MediumMovement");
                                        counter1 = 0;

                                    }
                                    //if there is a large tagged movement
                                    if (counter1 == 3)
                                    {
                                        if (counter_na == 1)
                                        {
                                            //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "LargeMovement");
                                        counter1 = 0;

                                    }

                                }
                            }
                            //If the movement is more than 2mm
                            else if (y1 > (x1 + warning) || y1 < (x1 - warning) || y2 > (x2 + warning) || y2 < (x2 - warning) || y3 > (x3 + warning) || y3 < (x3 - warning) || y4 > (x4 + warning) || y4 < (x4 - warning) || y5 > (x5 + warning) || y5 < (x5 - warning) || y6 > (x6 + warning) || y6 < (x6 - warning))
                            {

                                if (counter_value10 == 1)
                                {
                                    timer1.Enabled = true;
                                    if (counter_value2 == 1)
                                        axWindowsMediaPlayer1.Ctlcontrols.play();
                                    if (counter_value9 == 1)
                                    {
                                        pictureBox1.Visible = false;
                                        pictureBox2.Visible = true;
                                        pictureBox3.Visible = false;
                                    }
                                    else
                                    {
                                        pictureBox3.Visible = true;
                                    }
                                }
                                counter_warning++;
                                if (counter_na > 0)
                                {
                                    //file1.WriteLine("--------");
                                }
                                if (counter1 == 0)
                                {
                                    if (counter_warning == 1)
                                    {
                                        //  file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                        //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                    }
                                    //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                    //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                }
                                else
                                {
                                    //if the tagged movement is small movement
                                    if (counter1 == 1)
                                    {
                                        if (counter_warning == 1)
                                        {
                                            // file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "SmallMovement");
                                        counter1 = 0;

                                    }
                                    //if the tagged movement is medium movement
                                    if (counter1 == 2)
                                    {
                                        if (counter_warning == 1)
                                        {
                                            // file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "MediumMovement");
                                        counter1 = 0;

                                    }
                                    //if the tagged movement is large movement
                                    if (counter1 == 3)
                                    {
                                        if (counter_warning == 1)
                                        {
                                            //  file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            //file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "LargeMovement");
                                        counter1 = 0;

                                    }

                                }
                            }
                            else
                            {
                                //if the movement is less than 2mm
                                if (counter_value10 == 1)
                                {
                                    timer1.Enabled = true;
                                    if (counter_value2 == 1)
                                        axWindowsMediaPlayer1.Ctlcontrols.play();
                                    if (counter_value9 == 1)
                                    {
                                        pictureBox1.Visible = false;
                                        pictureBox2.Visible = false;
                                        pictureBox3.Visible = true;
                                    }
                                    else
                                    {
                                        pictureBox3.Visible = true;
                                    }
                                }
                                counter_original++;
                                //here we are storing the values to temp array
                                //here we are storing to temp original values
                                if (counter_original == 1)
                                {
                                    temp1[0] = y1;
                                    temp2[0] = y2;
                                    temp3[0] = y3;
                                    temp4[0] = y4;
                                    temp5[0] = y5;
                                    temp6[0] = y6;

                                }
                                if (counter_original == 2)
                                {
                                    temp1[1] = y1;
                                    temp2[1] = y2;
                                    temp3[1] = y3;
                                    temp4[1] = y4;
                                    temp5[1] = y5;
                                    temp6[1] = y6;

                                    counter_original = 0;
                                }
                                if (counter_na > 0)
                                {
                                    // file1.WriteLine("--------");
                                }
                                if (counter_warning > 0)
                                {
                                    //file1.WriteLine("--------");
                                }
                                counter_na = 0;
                                counter_warning = 0;
                                if (counter1 == 0) { }
                                //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                else
                                {
                                    //small tagged  movement
                                    if (counter1 == 1)
                                    {
                                        //  file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "SmallMovement");
                                        counter1 = 0;

                                    }
                                    //medium tagged movement
                                    if (counter1 == 2)
                                    {
                                        //file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "MediumMovement");
                                        counter1 = 0;

                                    }
                                    //large tagged movement
                                    if (counter1 == 3)
                                    {
                                        // file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "LargeMovement");
                                        counter1 = 0;

                                    }

                                }
                            }



                        }
                        else
                        {
                            //we refer these angle values as the original position and compare with obtained angle values(y1,y2,y3,....)
                            x1 = angle_1;
                            x2 = angle_2;
                            x3 = angle_3;
                            x4 = angle_4;
                            x5 = angle_5;
                            x6 = angle_6;

                        }


                    }

                
            

        
       

        //Form load method
        private void SubjectMovieForm_Load(object sender, EventArgs e)
        {
          
            if (count == 22)
            {
                this.BackColor = System.Drawing.Color.Black;

            }
        }
        //when we close the form
        private void SubjectMovieForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           // file.Close();
            //file1.Close();
            //file2.Close();
            //file3.Close();
            //file4.Close();
        }
        //method used for key press event
        private void SubjectMovieForm_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // button2.BackColor = Color.Green;

            Console.WriteLine(e.KeyChar);
            //for small movement press s key
            if (e.KeyChar == 's')
            {
                Console.WriteLine("Hello World");
                e.Handled = true;
                counter1 = 1;
                average_tagged = 1;

            }
            //for medium tagged movement press m
            if (e.KeyChar == 'm')
            {
                e.Handled = true;
                counter1 = 2;
                average_tagged = 1;
            }
            //for large tagged movement press l
            if (e.KeyChar == 'l')
            {
                counter1 = 3;
                average_tagged = 1;
            }
            //to start the process press r
            if (e.KeyChar == 'r')
            {
                count = 22;

                //button1.BackColor = Color.DeepSkyBlue;
            }
            if(e.KeyChar=='w'||e.KeyChar=='W')
            {
                this.WindowState = FormWindowState.Maximized;
            }

        }

    }
}
