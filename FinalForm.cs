//Final Version
//Below are the required header files
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
using System.Windows.Forms.DataVisualization.Charting;

namespace Final_Kinect
{
    public partial class FinalForm : Form
    {
        //initializing Kinect Sensor
        KinectSensor kinect_sensor = null;
        //Body frame reader used to detect body by kinect
        MultiSourceFrameReader bodyframe_reader = null;
        Body[] body = null;
        int count = 0;
        Stopwatch stp = new Stopwatch();
        //To write to file we use Streamwriter and path for three files
        static String a1;
        static string a;
        static string a2;
        static string a3;
        static string a4;
        static string a5;
        static string a6;
        static string a7;
        StreamWriter file;
        StreamWriter file1;
        StreamWriter file2;
        StreamWriter file3;
        StreamWriter file4;
        StreamWriter file5;
        StreamWriter file6;
        String s = "00:40:00";
        //variables were all the angles information is stored
        int x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, y1, y2, y3, y4, y5, y6, y7, y8, y9, y10;
        //variables used to get the average values
        int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0, sum5 = 0, sum6 = 0, sum7 = 0, sum8 = 0,counter_vary=0;
        int graph_counter = 0, graph_counter1 = 0, graph_counter2 = 0, graph_counter3 = 0, graph_counter4 = 0, graph_counter5 = 0;
        int file_writer_counter = 10;
        double avg1, avg2, avg3, avg4, avg5, avg6, avg7, avg8;
        int mean_sum1 = 0, mean_sum2 = 0, mean_sum3 = 0, mean_sum4 = 0, mean_sum5 = 0, mean_sum6 = 0,average_tagged1=0,original1=0,original2=0,original3=0,original4=0,original5=0,original6=0;
        Form7 form7;
        int c_progress = 0;
        int choice_color;
        int avg_mean1, avg_mean2, avg_mean3, avg_mean4, avg_mean5, avg_mean6,session_time=0,back_original;


        private void button6_Click(object sender, EventArgs e)
        {
            count = 21;
            button1.BackColor = Color.Red;
            file_writer_counter = 10;

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
          
        }

        int counter_value1, counter_value2, counter_value3, counter_value4, counter_value5, counter_value6, counter_value7, counter_value8,counter_value9,counter_value10,counter_value11,counter_value12;
        string pass7;
        int angle_counter;
        private void timer1_Tick_1(object sender, EventArgs e)
        {

            elapsedTime = DateTime.Now - startTime;
            progressBar1.Maximum = (2 * session_time);

            counter++;
            if (counter % 60 == 0)
            {
                if (counter_value5 == 1)
                {
                    progressBar1.Increment(1);
                   
                    progressBar1.Refresh();
                   

                }
            }
           

        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            count = 22;
            // form7.Show();
            warning1 = Convert.ToInt32(textBox2.Text);
            notallowed1 = Convert.ToInt32(textBox3.Text);
            warning = warning1;
            notallowed = notallowed1;
            button1.BackColor = Color.DeepSkyBlue;
            if (file_writer_counter == 10)
            {
                file5.WriteLine("Subject Initials" + "," + "Experiment No" + "," + "Smoothing Kernel" + "," + "Small Movement" + "," + "Large Movement");
                file5.WriteLine(user_name + "," + experiment_no + "," + base_smoothing + "," + warning + "," + notallowed);
                file5.WriteLine(" " + "," + " " + "," + " " + "," + " " + "," + " ");
                file5.WriteLine("ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
                file6.WriteLine("Subject Initials" + "," + "Experiment No" + "," + "Smoothing Kernel" + "," + "Small Movement" + "," + "Large Movement");
                file6.WriteLine(user_name + "," + experiment_no + "," + base_smoothing + "," + warning + "," + notallowed);
                file6.WriteLine("Elapsed_Time"+","+"Values");
                file6.WriteLine(" " + "," + " ");
                file_writer_counter = 0;
            }
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
        int warning,warning1;
        int notallowed,notallowed1;
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
        int i = 0;
        //Timer method
        private void timer1_Tick(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - startTime;


           
            counter++;
            if (counter % 60 == 0)
            {
                if (counter_value5 == 1)
                {
                    progressBar1.Value=++i;
                    progressBar1.Update();
                    
                    //progressBar1.Value = counter;

                }
            }

        }


        public FinalForm(string pass_value1, string pass_value2, string pass_value3, string pass_value4, string pass_value5,int counter_new,int counter_1,int counter_2,int counter_3,int counter_4,int counter_5,int counter_6,int counter_7,int counter8,int counter_9,int counter_10,int counter_11,string pass_value6,int counter_12,string pass_value7,string file_value,int counter_angle,int color_choice)
        {
            InitializeComponent();
            choice_color = color_choice;
            //Below code represents of how we represent data of the angles on all the charts
            chart1.ChartAreas[0].AxisY.ScaleView.Zoom(-20, 20);
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            chart2.ChartAreas[0].AxisY.ScaleView.Zoom(-20, 20);
            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
            chart2.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart2.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart2.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart2.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            chart3.ChartAreas[0].AxisY.ScaleView.Zoom(-20, 20);
            chart3.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
            chart3.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart3.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart3.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart3.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            chart4.ChartAreas[0].AxisY.ScaleView.Zoom(-20,20);
            chart4.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
            chart4.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart4.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart4.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart4.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            chart5.ChartAreas[0].AxisY.ScaleView.Zoom(-20, 20);
            chart5.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
            chart5.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart5.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart5.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart5.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            chart6.ChartAreas[0].AxisY.ScaleView.Zoom(-20, 20);
            chart6.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
            chart6.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart6.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart6.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart6.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            user_name = pass_value1;
            experiment_no = pass_value2;
            base_smoothing = Convert.ToInt32(pass_value3);
            warning = Convert.ToInt32(pass_value4);
            notallowed = Convert.ToInt32(pass_value5);
            warning1 = warning;
            notallowed1 = notallowed;
            session_time = Convert.ToInt32(pass_value6);
            angle_counter = counter_angle;
            median_smoothing1 = new int[base_smoothing];
            median_smoothing2 = new int[base_smoothing];
            median_smoothing3 = new int[base_smoothing];
            median_smoothing4 = new int[base_smoothing];
            median_smoothing5 = new int[base_smoothing];
            median_smoothing6 = new int[base_smoothing];
            textBox2.Text = warning1.ToString();
            textBox3.Text = notallowed1.ToString();
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
            pass7 = pass_value7;
            progressBar1.Maximum = (2 * session_time);
            axWindowsMediaPlayer1.URL = pass_value7;
            
            if (counter_12==1)
            {
                back_original = notallowed;
            }
            if(counter_12==0)
            {
                back_original = warning;
            }
            if (counter_value1==0)
            {
                axWindowsMediaPlayer1.Hide();
            }
            if(counter_value4==0)
            {
                progressBar1.Hide();
            }
            if(counter_value7==0)
            {
               
               pictureBox1.Hide();
                pictureBox2.Hide();
                pictureBox3.Hide();
            }
            if(counter_value8==0)
            {
                pictureBox1.Hide();
                pictureBox2.Hide();
                pictureBox3.Hide();
            }
            a1 = DateTime.Now.ToString("yyyyMMdd");
            a = Path.Combine(file_value + "\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "raw" + ".csv");
            a2 = Path.Combine(file_value + "\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "video" + ".csv");
            a3 = Path.Combine(file_value + "\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "average" + ".csv");
            a4 = Path.Combine(file_value + "\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "mean" + ".csv");
            a5 = Path.Combine(file_value + "\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "median" + ".csv");
            a6 = Path.Combine(file_value + "\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "difference" + ".csv");
            a7 = Path.Combine(file_value+"\\" + user_name + "-" + experiment_no + "-" + a1 + "-" + "amplitude" + ".csv");
            file = new StreamWriter(a, true);
            file1 = new StreamWriter(a2, true);
            file2 = new StreamWriter(a3, true);
            file3 = new StreamWriter(a4, true);
            file4 = new StreamWriter(a5, true);
            file5 = new StreamWriter(a6, true);
            file6 = new StreamWriter(a7, true);
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.KeyPreview = true;
            totalTime = DateTime.Parse(s);
            button1.BackColor = Color.Red;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000;
            timer1.Enabled = false;
            startTime = DateTime.Now;
            //Writing to three files(Main data file, video reference file and average file)
            file.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            file1.WriteLine("ElapsedTime" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck");
            file2.WriteLine("Date&Time" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            file3.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            file4.WriteLine("Date&Time" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            form7 = new Form7(base_smoothing.ToString(), warning.ToString(), notallowed.ToString(), counter_value1, counter_value2, counter_value3, counter_value4, counter_value5, counter_value6, counter_value7, counter_value8, counter_value9, counter_value10, counter_value11, counter_value12,session_time,pass7);
           
            initializeKinect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 baseline = new Form5();
            baseline.ShowDialog();
        }

        public void initializeKinect()
        {
            kinect_sensor = KinectSensor.GetDefault();
            if (kinect_sensor != null)
            {
                kinect_sensor.Open();//turn on kinect
            }
            //as we are using kinect camera as well as body detection so here we have used multisourceframereader
            bodyframe_reader = kinect_sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Body);
            if (bodyframe_reader != null)
            {
                bodyframe_reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
            form7.Show();
        }
        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            bool data_received = false;
            var reference = e.FrameReference.AcquireFrame();
            //method for getting video in depth view
            if (choice_color == 0)
            {
                using (var frame = reference.ColorFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        var width = frame.FrameDescription.Width;
                        var height = frame.FrameDescription.Height;
                        ushort[] data = new ushort[width * height];
                        byte[] pixels = new byte[width * height * 32 / 8];
                        if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
                        {
                            frame.CopyRawFrameDataToArray(pixels);
                        }
                        else
                        {
                            frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
                        }
                        var bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                        Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);
                        bitmap.UnlockBits(bitmapData);
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
                       // pictureBox6.Image = bitmap;
                    }
                }
            }
            if (choice_color == 1)
            {
                using (var frame = reference.DepthFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        var width = frame.FrameDescription.Width;
                        var height = frame.FrameDescription.Height;
                        ushort minDepth = frame.DepthMinReliableDistance;
                        ushort maxDepth = frame.DepthMaxReliableDistance;
                        ushort[] data = new ushort[width * height];
                        byte[] pixels = new byte[width * height * 32 / 8];
                        frame.CopyFrameDataToArray(data);
                        int colorIndex = 0;
                        for (int depthIndex = 0; depthIndex < data.Length; ++depthIndex)
                        {
                            ushort depth = data[depthIndex];
                            byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);
                            pixels[colorIndex++] = intensity;
                            pixels[colorIndex++] = intensity;
                            pixels[colorIndex++] = intensity;
                            ++colorIndex;
                        }
                        var bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                        Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);
                        bitmap.UnlockBits(bitmapData);
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
                        //pictureBox6.Image = bitmap;
                    }
                }
            }
           
            using (var body_frame = reference.BodyFrameReference.AcquireFrame())
            {

                if (body_frame != null)
                {

                    if (body == null)
                    {
                        body = new Body[body_frame.BodyFrameSource.BodyCount];
                    }
                    body_frame.GetAndRefreshBodyData(body);
                    data_received = true;
                }

            }
            if (data_received)
            {

                foreach (Body body_1 in body)
                {
                    //process if the body has been detected
                    if (body_1.IsTracked)
                    {
                        if (count == 22)
                            button1.BackColor = Color.DeepSkyBlue;
                        else if (count == 21)
                        {
                            button1.BackColor = Color.Red;
                            axWindowsMediaPlayer1.Ctlcontrols.pause();
                            timer1.Enabled = false;
                        }
                        else if (count == 20)
                            button1.BackColor = Color.Blue;
                        else
                            button1.BackColor = Color.Green;
                        IReadOnlyDictionary<JointType, Joint> joints = body_1.Joints;
                        Dictionary<JointType, Point> joint_points = new Dictionary<JointType, Point>();
                        Joint Midspine = joints[JointType.Neck];
                        float x = Midspine.Position.X;
                        float y = Midspine.Position.Y;
                        float z = Midspine.Position.Z;
                        var point = body_1.Joints[JointType.SpineBase].Position;
                        CameraSpacePoint myPoint = Midspine.Position;
                        //All the angles: "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "ElbowLeft" + "," + "ElbowRight" + "," + "Movement"
                        var start_1 = body_1.Joints[JointType.Neck];
                        var center_1 = body_1.Joints[JointType.ShoulderRight];
                        var end_1 = body_1.Joints[JointType.ElbowRight];
                        var angle_1 = center_1.Angle(start_1, end_1);
                        var start_2 = body_1.Joints[JointType.ElbowLeft];
                        var center_2 = body_1.Joints[JointType.ShoulderLeft];
                        var end_2 = body_1.Joints[JointType.Neck];
                        var angle_2 = center_2.Angle(start_2, end_2);
                        var start_3 = body_1.Joints[JointType.SpineBase];
                        var center_3 = body_1.Joints[JointType.SpineMid];
                        var end_3 = body_1.Joints[JointType.Head];
                        var angle_3 = center_3.Angle(start_3, end_3);
                        var start_4 = body_1.Joints[JointType.Head];
                        var center_4 = body_1.Joints[JointType.Neck];
                        var end_4 = body_1.Joints[JointType.ShoulderLeft];
                        var angle_4 = center_4.Angle(start_4, end_4);
                        var start_5 = body_1.Joints[JointType.Head];
                        var center_5 = body_1.Joints[JointType.Neck];
                        var end_5 = body_1.Joints[JointType.ShoulderRight];
                        var angle_5 = center_5.Angle(start_5, end_5);
                        var start_6 = body_1.Joints[JointType.Head];
                        var center_6 = body_1.Joints[JointType.Neck];
                        var end_6 = body_1.Joints[JointType.SpineShoulder];
                        var angle_6 = center_6.Angle(start_6, end_6);
                        
                        form7.transfer_values(count,(int)angle_1, (int)angle_2, (int)angle_3, (int)angle_4, (int)angle_5, (int)angle_6,warning,notallowed,count);
                        //once the user has pressed the start button
                       
                        if (count == 22)
                        {
                            // form7.transfer_values();
                            
                           // axWindowsMediaPlayer1.settings.mute = false;
                            stp.Start();
                            textBox1.Text = stp.Elapsed.ToString();
                            
                            counter_sum++;
                           timer1.Enabled = true;
                            y1 = (int)angle_1;
                            y2 = (int)angle_2;
                            y3 = (int)angle_3;
                            y4 = (int)angle_4;
                            y5 = (int)angle_5;
                            y6 = (int)angle_6;
                            if (angle_counter == 1)
                            {
                                x4 = 0;
                                y4 = 0;
                                x5 = 0;
                                y5 = 0;
                                x6 = 0;
                                y6 = 0;
                               label6.Hide();
                               label7.Hide();
                               label8.Hide();
                            }
                            if (angle_counter == 2)
                            {
                                x1 = 0;
                                y1 = 0;
                                x2 = 0;
                                y2 = 0;
                                x3 = 0;
                                y3 = 0;
                                label3.Hide();
                                label4.Hide();
                                label5.Hide();
                            }
                            if (counter_vary==0)
                            {
                                original1 = y1;
                                original2 = y2;
                                original3 = y3;
                                original4 = y4;
                                original5 = y5;
                                original6 = y6;
                                counter_vary++;
                            }
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
                                    if (average_tagged1== 1)
                                    {
                                        file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6 + "," + "Tagged");
                                        //average_tagged = 0;
                                    }
                                    //if there was not tagged moment
                                    else
                                        file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6);
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
                                    if (average_tagged1 == 1)
                                    {
                                        file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6 + "," + "Tagged");
                                        //average_tagged = 0;
                                    }
                                    //if there was not tagged moment
                                    else
                                        file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6);
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
                                    if (average_tagged1== 1)
                                    {
                                        file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6 + "," + "Tagged");
                                        //average_tagged = 0;
                                    }
                                    //if there was not tagged moment
                                    else
                                        file4.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6);
                                }
                                mean_counter = 0;
                                // If there was a tagged moment
                                double amplitude = Math.Sqrt(((x1 - y1) * (x1 - y1)) + ((x2 - y2) * (x2 - y2)) + ((x3 - y3) * (x3 - y3)) + ((x4 - y4) * (x4 - y4)) + ((x5 - y5) * (x5 - y5)) + ((x6 - y6) * (x6 - y6)));
                                double final_amplitude = (amplitude / 6);
                                chart1.Series[0].Points.AddXY(graph_counter, (x1-y1));
                                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart1.ChartAreas[0].AxisX.ScaleView.Position = chart1.Series[0].Points.Count - chart1.ChartAreas[0].AxisX.ScaleView.Size;
                                if (graph_counter % 100 == 0)
                                {
                                    chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter++;
                                }
                                else
                                    graph_counter++;
                                chart2.Series[0].Points.AddXY(graph_counter1, (x2 - y2));
                                chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart2.ChartAreas[0].AxisX.ScaleView.Position = chart2.Series[0].Points.Count - chart2.ChartAreas[0].AxisX.ScaleView.Size;
                                if (graph_counter1 % 100 == 0)
                                {
                                    chart2.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter1++;
                                }
                                else
                                    graph_counter1++;
                                chart3.Series[0].Points.AddXY(graph_counter2, (x3 - y3));
                                chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart3.ChartAreas[0].AxisX.ScaleView.Position = chart3.Series[0].Points.Count - chart3.ChartAreas[0].AxisX.ScaleView.Size;
                                if (graph_counter2 % 100 == 0)
                                {
                                    chart3.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter2++;
                                }
                                else
                                    graph_counter2++;
                                chart4.Series[0].Points.AddXY(graph_counter3, (x4 - y4));
                                chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart4.ChartAreas[0].AxisX.ScaleView.Position = chart4.Series[0].Points.Count - chart4.ChartAreas[0].AxisX.ScaleView.Size;
                                if (graph_counter3 % 100 == 0)
                                {
                                    chart4.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter3++;
                                }
                                else
                                    graph_counter3++;
                                chart5.Series[0].Points.AddXY(graph_counter4, (x5 - y5));
                                chart5.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart5.ChartAreas[0].AxisX.ScaleView.Position = chart5.Series[0].Points.Count - chart5.ChartAreas[0].AxisX.ScaleView.Size;
                                if (graph_counter4 % 100 == 0)
                                {
                                    chart5.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter4++;
                                }
                                else
                                    graph_counter4++;
                                chart6.Series[0].Points.AddXY(graph_counter5, (x6 - y6));
                                chart6.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart6.ChartAreas[0].AxisX.ScaleView.Position = chart6.Series[0].Points.Count - chart6.ChartAreas[0].AxisX.ScaleView.Size;
                                if (graph_counter5 % 100 == 0)
                                {
                                    chart6.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter5++;
                                }
                                else
                                    graph_counter5++;
                                //file6.WriteLine(final_amplitude);
                                if (avg_mean1 > (x1 + notallowed) || avg_mean1 < (x1 - notallowed) || avg_mean2 > (x2 + notallowed) || avg_mean2 < (x2 - notallowed) || avg_mean3 > (x3 + notallowed) || avg_mean3 < (x3 - notallowed) || avg_mean4 > (x4 + notallowed) || avg_mean4 < (x4 - notallowed) || avg_mean5 > (x5 + notallowed) || avg_mean5 < (x5 - notallowed) || avg_mean6 > (x6 + notallowed) || avg_mean6 < (x6 - notallowed))
                                {
                                    if (avg_mean1 > (x1 + notallowed) || avg_mean1 < (x1 - notallowed))
                                    {
                                        label3.ForeColor = Color.Red;
                                    }
                                    if (avg_mean2 > (x2 + notallowed) || avg_mean2 < (x2 - notallowed))
                                    {
                                        label4.ForeColor = Color.Red;
                                    }
                                    if (avg_mean3 > (x3 + notallowed) || avg_mean3 < (x3 - notallowed))
                                    {
                                        label5.ForeColor = Color.Red;
                                    }
                                    if (avg_mean4 > (x4 + notallowed) || avg_mean4 < (x4 - notallowed))
                                    {
                                        label6.ForeColor = Color.Red;
                                    }
                                    if (avg_mean5 > (x5 + notallowed) || avg_mean5 < (x5 - notallowed))
                                    {
                                        label7.ForeColor = Color.Red;
                                    }
                                    if (avg_mean6 > (x6 + notallowed) || avg_mean6 < (x6 - notallowed))
                                    {
                                        label8.ForeColor = Color.Red;
                                    }
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
                                            pictureBox1.Visible =true;
                                            pictureBox2.Visible = false;
                                            pictureBox3.Visible =false;
                                            stp.Stop();
                                            count = 21;
                                        }
                                        else
                                        {
                                            pictureBox3.Visible = true;
                                        }
                                    }
                                    if (average_tagged1 == 1)
                                    {
                                        file5.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString()+","+"Tagged");
                                        file5.WriteLine(" " + "," + " " + "," + " " + "," + " "+ "," +" " + "," +" ");
                                        file6.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + ","+final_amplitude);
                                        file6.WriteLine(" "+" ");
                                        file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6 + "," + "Tagged");
                                        average_tagged1 = 0;
                                    }
                                    //if there was not tagged moment
                                    else
                                    {
                                        file5.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString());
                                        file5.WriteLine(" " + "," + " " + "," + " " + "," + " " + "," + " " + "," + " ");
                                        file6.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        file6.WriteLine(" " + " ");
                                        file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6);
                                    }//initialize all the sum variables to zero
                                    
                                }
                                else if (avg_mean1 > (x1 + warning) || avg_mean1 < (x1 - warning) || avg_mean2 > (x2 + warning) || avg_mean2 < (x2 - warning) || avg_mean3 > (x3 + warning) || avg_mean3 < (x3 - warning) || avg_mean4 > (x4 + warning) || avg_mean4 < (x4 - warning) || avg_mean5 > (x5 + warning) || avg_mean5 < (x5 - warning) || avg_mean6 > (x6 + warning) || avg_mean6 < (x6 - warning))
                                {
                                    label3.ForeColor = Color.Black;
                                    label4.ForeColor = Color.Black;
                                    label5.ForeColor = Color.Black;
                                    label6.ForeColor = Color.Black;
                                    label7.ForeColor = Color.Black;
                                    label8.ForeColor = Color.Black;
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
                                    if (average_tagged1 == 1)
                                    {
                                        file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6 + "," + "Tagged");
                                        file5.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString()+","+"Tagged");
                                        file6.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        average_tagged1 = 0;
                                    }
                                    //if there was not tagged moment
                                    else
                                    {
                                        file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6);
                                        file5.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString());
                                        file6.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                    }//initialize all the sum variables to zero
                                }
                                else
                                {
                                    label3.ForeColor = Color.Black;
                                    label4.ForeColor = Color.Black;
                                    label5.ForeColor = Color.Black;
                                    label6.ForeColor = Color.Black;
                                    label7.ForeColor = Color.Black;
                                    label8.ForeColor = Color.Black;
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
                                    if (average_tagged1 == 1)
                                    {
                                        file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6 + "," + "Tagged");
                                        file5.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString()+","+"Tagged");
                                        file6.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        average_tagged1 = 0;
                                    }
                                    //if there was not tagged moment
                                    else
                                    {
                                        file5.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString());
                                        file3.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6);
                                        file6.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                    }//initialize all the sum variables to zero
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
                                    file2.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + avg1 + "," + avg2 + "," + avg3 + "," + avg4 + "," + avg5 + "," + avg6 + "," + "Tagged");
                                    average_tagged = 0;
                                }
                                //if there was not tagged moment
                                else
                                    file2.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + avg1 + "," + avg2 + "," + avg3 + "," + avg4 + "," + avg5 + "," + avg6);
                                //initialize all the sum variables to zero
                                sum1 = 0;
                                sum2 = 0;
                                sum3 = 0;
                                sum4 = 0;
                                sum5 = 0;
                                sum6 = 0;
                                sum7 = 0;
                                sum8 = 0;
                            }
                            //if the movement is more than 3mm
                            if (y1 > (x1 + notallowed) || y1 < (x1 - notallowed) || y2 > (x2 + notallowed) || y2 < (x2 - notallowed) || y3 > (x3 + notallowed) || y3 < (x3 - notallowed) ||y4 > (x4 + notallowed) || y4 < (x4 - notallowed) || y5 > (x5 + notallowed) || y5 < (x5 - notallowed) || y6 > (x6 + notallowed) || y6 < (x6 - notallowed))
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
                                    file1.WriteLine("--------");
                                }
                                if (counter1 == 0)
                                {
                                    if (counter_na == 1)
                                    {
                                        file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement" + "," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                        file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement" + "," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                    }

                                    file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                    file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Large Movement" + "," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);

                                }
                                else
                                {
                                    //If there is a small tagged movement
                                    if (counter1 == 1)
                                    {
                                        if (counter_na == 1)
                                        {
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "SmallMovement");
                                        counter1 = 0;

                                    }
                                    //if there is a medium  tagged movement
                                    if (counter1 == 2)
                                    {
                                        if (counter_na == 1)
                                        {
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "MediumMovement");
                                        counter1 = 0;

                                    }
                                    //if there is a large tagged movement
                                    if (counter1 == 3)
                                    {
                                        if (counter_na == 1)
                                        {
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "LargeMovement");
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
                                    file1.WriteLine("--------");
                                }
                                if (counter1 == 0)
                                {
                                    if (counter_warning == 1)
                                    {
                                        file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                        file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                    }
                                    file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                    file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                }
                                else
                                {
                                    //if the tagged movement is small movement
                                    if (counter1 == 1)
                                    {
                                        if (counter_warning == 1)
                                        {
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "SmallMovement");
                                        counter1 = 0;

                                    }
                                    //if the tagged movement is medium movement
                                    if (counter1 == 2)
                                    {
                                        if (counter_warning == 1)
                                        {
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "MediumMovement");
                                        counter1 = 0;

                                    }
                                    //if the tagged movement is large movement
                                    if (counter1 == 3)
                                    {
                                        if (counter_warning == 1)
                                        {
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            file1.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }
                                        file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "LargeMovement");
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
                                    file1.WriteLine("--------");
                                }
                                if (counter_warning > 0)
                                {
                                    file1.WriteLine("--------");
                                }
                                counter_na = 0;
                                counter_warning = 0;
                                if (counter1 == 0)
                                    file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                else
                                {
                                    //small tagged  movement
                                    if (counter1 == 1)
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "SmallMovement");
                                        counter1 = 0;

                                    }
                                    //medium tagged movement
                                    if (counter1 == 2)
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "MediumMovement");
                                        counter1 = 0;

                                    }
                                    //large tagged movement
                                    if (counter1 == 3)
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "LargeMovement");
                                        counter1 = 0;

                                    }

                                }
                            }



                        }
                        else
                        {
                            //we refer these angle values as the original position and compare with obtained angle values(y1,y2,y3,....)
                            x1 = (int)angle_1;
                            x2 = (int)angle_2;
                            x3 = (int)angle_3;
                            x4 = (int)angle_4;
                            x5 = (int)angle_5;
                            x6 = (int)angle_6;
                            if (count == 21)
                            {
                                if (x1 > (original1 - (back_original)) && x1 < (original1 + (back_original)) && x2 > (original2 - back_original) && x2 < (original2 + back_original) && x3 > (original3 - back_original) && x3 < (original3 + back_original) && x4 > (original4 - back_original) && x4 < (original4 + back_original) && x5 > (original5 - back_original) && x5 < (original5 + back_original) && x6 > (original6 - back_original) && x6 < (original6 + back_original))
                                {
                                    count = 20;
                                    counter_vary = 0;
                                }
                            }

                        }


                    }

                }
            }

        }

        //Form load method
        private void Form6_Load(object sender, EventArgs e)
        {
           
            //label1.Parent = progressBar1;
            //label1.BackColor = Color.Transparent;
            
            if (count == 22)
            {
                this.BackColor = System.Drawing.Color.Black;

            }
           

        }
        //when we close the form
        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            file.Close();
            file1.Close();
            file2.Close();
            file3.Close();
            file4.Close();
            file5.Close();
            file6.Close();
        }
        //method used for key press event
        private void Form6_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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
                average_tagged1 = 1;

            }
            //for medium tagged movement press m
            if (e.KeyChar == 'm')
            {
                e.Handled = true;
                counter1 = 2;
                average_tagged = 1;
                average_tagged1 = 1;
            }
            //for large tagged movement press l
            if (e.KeyChar == 'l')
            {
                counter1 = 3;
                average_tagged = 1;
                average_tagged1 = 1;
            }
            //to start the process press r
            if (e.KeyChar == 'r')
            {
                count = 22;

                button1.BackColor = Color.DeepSkyBlue;
            }

        }

    }
}
