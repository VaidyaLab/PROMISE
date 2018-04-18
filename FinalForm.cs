using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;

namespace Final_Kinect
{
    public partial class FinalForm : Form
    {
        // Initializing Kinect Sensor
        KinectSensor kinect_sensor = null;

        // Body frame reader used to detect body by kinect
        MultiSourceFrameReader bodyframe_reader = null;

        Body[] body = null;

        // This is the form on which the participant will view the movie.
        SubjectMovieForm subjectMovieForm;

        Stopwatch stopWatch = new Stopwatch();

        // This is being used to determine if session is started or stopped, but in some ways that I need to look further into.
        int count = 0;
  
        string mCurrentDate;
        string mFilePath;

        //To write to file we use Streamwriter and path for three files
        string rawCsvPath;
        string videoCsvPath;
        string averageCsvPath;
        string meanCsvPath;
        string medianCsvPath;
        string differenceCsvPath;
        string amplitudeCsvPath;

        StreamWriter rawCsvFile;
        StreamWriter videoCsvFile;
        StreamWriter averageCsvFile;
        StreamWriter meanCsvFile;
        StreamWriter medianCsvFile;
        StreamWriter differenceCsvFile;
        StreamWriter amplitdueCsvFile;

        // Variables where all the angles information is stored
        int x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, y1, y2, y3, y4, y5, y6, y7, y8, y9, y10;

        // Variables used to get the average values
        int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0, sum5 = 0, sum6 = 0, sum7 = 0, sum8 = 0, counter_vary=0;

        int graph_counter = 0, graph_counter1 = 0, graph_counter2 = 0, graph_counter3 = 0, graph_counter4 = 0, graph_counter5 = 0;

        bool mStarted = false;

        double avg1, avg2, avg3, avg4, avg5, avg6, avg7, avg8;

        int mean_sum1 = 0, mean_sum2 = 0, mean_sum3 = 0, mean_sum4 = 0, mean_sum5 = 0, mean_sum6 = 0,average_tagged1=0,original1=0,original2=0,original3=0,original4=0,original5=0,original6=0;

        int mColorFrameReference;

        int avg_mean1, avg_mean2, avg_mean3, avg_mean4, avg_mean5, avg_mean6,session_time=0,back_original;

        int mMovie,
            mMovieSensitive,
            mProgress,
            mProgressSensitive,
            mTrafficSensitive,
            mRawMeasures,
            mMeans,
            mMedians;

        int angle_counter;

        int mean_counter = 1;
        int average_tagged = 0;
        int mSmoothingKernal;

        int[] median_smoothing1,
            median_smoothing2,
            median_smoothing3,
            median_smoothing4,
            median_smoothing5,
            median_smoothing6;

        int warning;
        int notAllowed;

        int median1,
            median2,
            median3,
            median4,
            median5,
            median6;

        string mSubjectInitials;
        string mExperimentNumber;

        // These are used to determine the elapsed time of the session.
        DateTime startTime = new DateTime();
        TimeSpan elapsedTime = new TimeSpan();

        /* This will be incremented on every tick. When the tick interval
         * is set for 1000, then mTickCount % 60 will be 0 and thus allow
         * us to perform an action every 1 second - specifically, increment
         * the progress bar.
         */
        int mTickCount = 0;

        int counter1 = 0;
        int counter_sum = 0;
        int counter_original = 0;
        int mOverWarning = 0;
        int mOverNotAllowed = 0;

        // Arrays where the temporary values are stored
        int[] temp1 = new int[2];
        int[] temp2 = new int[2];
        int[] temp3 = new int[2];
        int[] temp4 = new int[2];
        int[] temp5 = new int[2];
        int[] temp6 = new int[2];
        int[] temp7 = new int[2];
        int[] temp8 = new int[2];

        public FinalForm(
            string subjectInitials,
            string experimentNumber,
            string smoothingKernal,
            string smallMovementLowerLimit,
            string largeMovementLowerLimit,
            int movieFrame,
            int movie,
            int movieSensitive,
            int progressFrame,
            int progress,
            int progressSensitive,
            int trafficFrame,
            int traffic,
            int trafficSensitive,
            int rawMeasures,
            int means,
            int medians,
            string sessionTime,
            int lowerLimitLargeMovement,
            string videoFile,
            string filePath,
            int counterAngle,
            int colorFrameReference
            )
        {
            InitializeComponent();
            mColorFrameReference = colorFrameReference;

            // Below code implements how we represent data of the angles on all the charts

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

            mSubjectInitials = subjectInitials;
            mExperimentNumber = experimentNumber;
            mSmoothingKernal = Convert.ToInt32(smoothingKernal);

            warning = Convert.ToInt32(smallMovementLowerLimit);
            notAllowed = Convert.ToInt32(largeMovementLowerLimit);

            lowerLimitSmallMovementTextBox.Text = warning.ToString();
            lowerLimitLargeMovementTextBox.Text = notAllowed.ToString();

            session_time = Convert.ToInt32(sessionTime);
            angle_counter = counterAngle;

            median_smoothing1 = new int[mSmoothingKernal];
            median_smoothing2 = new int[mSmoothingKernal];
            median_smoothing3 = new int[mSmoothingKernal];
            median_smoothing4 = new int[mSmoothingKernal];
            median_smoothing5 = new int[mSmoothingKernal];
            median_smoothing6 = new int[mSmoothingKernal];

            mMovie = movie;
            mMovieSensitive = movieSensitive;
            mProgress = progress;
            mProgressSensitive = progressSensitive;
            mTrafficSensitive = trafficSensitive;
            mRawMeasures = rawMeasures;
            mMeans = means;
            mMedians = medians;

            progressBar.Maximum = (2 * session_time);
            axWindowsMediaPlayer1.URL = videoFile;

            back_original = lowerLimitLargeMovement == 1 ? notAllowed : warning;

            if (movieFrame == 0)
            {
                axWindowsMediaPlayer1.Hide();
            }

            if (progressFrame == 0)
            {
                progressBar.Hide();
            }

            if (trafficFrame == 0 || traffic == 0)
            {              
               redLightPictureBox.Hide();
               yellowLightPictureBox.Hide();
               greenLightPictureBox.Hide();
            }

            mCurrentDate = DateTime.Now.ToString("yyyyMMdd");
            this.mFilePath = filePath;

            rawCsvFile = new StreamWriter(getCsvPath("raw"), true);
            videoCsvFile = new StreamWriter(getCsvPath("video"), true);
            averageCsvFile = new StreamWriter(getCsvPath("average"), true);
            meanCsvFile = new StreamWriter(getCsvPath("mean"), true);
            medianCsvFile = new StreamWriter(getCsvPath("median"), true);
            differenceCsvFile = new StreamWriter(getCsvPath("difference"), true);
            amplitdueCsvFile = new StreamWriter(getCsvPath("amplitude"), true);

            axWindowsMediaPlayer1.Ctlcontrols.stop();

            this.KeyPreview = true;
            startButton.BackColor = Color.Red;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000;
            timer1.Enabled = false;
            startTime = DateTime.Now;

            // Writing to three files(Main data file, video reference file and average file)
            rawCsvFile.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            videoCsvFile.WriteLine("ElapsedTime" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck");
            averageCsvFile.WriteLine("Date&Time" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            meanCsvFile.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            medianCsvFile.WriteLine("Date&Time" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");

            subjectMovieForm = new SubjectMovieForm(
                mSmoothingKernal.ToString(),
                warning.ToString(),
                notAllowed.ToString(),
                movieFrame,
                mMovie,
                mMovieSensitive,
                progressFrame,
                mProgress,
                mProgressSensitive,
                trafficFrame,
                traffic,
                mTrafficSensitive,
                mRawMeasures,
                mMeans,
                mMedians,
                session_time,
                videoFile
                );
           
            initializeKinect();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            count = 22;
            warning = Convert.ToInt32(lowerLimitSmallMovementTextBox.Text);
            notAllowed = Convert.ToInt32(lowerLimitLargeMovementTextBox.Text);
            startButton.BackColor = Color.DeepSkyBlue;

            if (mStarted == false)
            {
                // Headers

                differenceCsvFile.WriteLine("Subject Initials" + "," + "Experiment No" + "," + "Smoothing Kernel" + "," + "Small Movement" + "," + "Large Movement");
                differenceCsvFile.WriteLine(mSubjectInitials + "," + mExperimentNumber + "," + mSmoothingKernal + "," + warning + "," + notAllowed);
                differenceCsvFile.WriteLine(" " + "," + " " + "," + " " + "," + " " + "," + " ");
                differenceCsvFile.WriteLine("ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");

                amplitdueCsvFile.WriteLine("Subject Initials" + "," + "Experiment No" + "," + "Smoothing Kernel" + "," + "Small Movement" + "," + "Large Movement");
                amplitdueCsvFile.WriteLine(mSubjectInitials + "," + mExperimentNumber + "," + mSmoothingKernal + "," + warning + "," + notAllowed);
                amplitdueCsvFile.WriteLine("Elapsed_Time" + "," + "Values");
                amplitdueCsvFile.WriteLine(" " + "," + " ");

                mStarted = true;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            count = 21;
            startButton.BackColor = Color.Red;
            mStarted = false;
        }

        // Timer event for progress bar
        private void timer1_Tick(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - startTime;

            mTickCount++;

            // Every second, increase progress
            if (mTickCount % 60 == 0 && mProgress == 1)
            {
                progressBar.Value++;
                progressBar.Update();                
            }
        }

        public void initializeKinect()
        {
            kinect_sensor = KinectSensor.GetDefault();

            if (kinect_sensor != null)
            {
                kinect_sensor.Open(); // Turn on kinect
            }

            // We are using kinect camera as well as body detection so here we have used MultiSourceFrameReader
            bodyframe_reader = kinect_sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Body);

            if (bodyframe_reader != null)
            {
                bodyframe_reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }

            subjectMovieForm.Show();
        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            bool data_received = false;
            var reference = e.FrameReference.AcquireFrame();

            // Method for getting video in depth view
            if (mColorFrameReference == 0)
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
                    }
                }
            }
            if (mColorFrameReference == 1)
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
                    // Process if the body has been detected
                    if (body_1.IsTracked)
                    {
                        if (count == 22)
                        {
                            startButton.BackColor = Color.DeepSkyBlue;
                        }
                        else if (count == 21)
                        {
                            startButton.BackColor = Color.Red;
                            axWindowsMediaPlayer1.Ctlcontrols.pause();
                            timer1.Enabled = false;
                        }
                        else if (count == 20)
                        {
                            startButton.BackColor = Color.Blue;
                        }
                        else
                        {
                            startButton.BackColor = Color.Green;
                        }

                        IReadOnlyDictionary<JointType, Joint> joints = body_1.Joints;
                        Dictionary<JointType, Point> joint_points = new Dictionary<JointType, Point>();
                        Joint Midspine = joints[JointType.Neck];
                        float x = Midspine.Position.X;
                        float y = Midspine.Position.Y;
                        float z = Midspine.Position.Z;
                        var point = body_1.Joints[JointType.SpineBase].Position;
                        CameraSpacePoint myPoint = Midspine.Position;
                        // All the angles: "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "ElbowLeft" + "," + "ElbowRight" + "," + "Movement"
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
                        
                        subjectMovieForm.transfer_values(
                            count,
                            (int) angle_1,
                            (int) angle_2,
                            (int) angle_3,
                            (int) angle_4,
                            (int) angle_5,
                            (int) angle_6,
                            warning,
                            notAllowed,
                            count
                        );

                        // Once the user has pressed the start button
                       
                        if (count == 22)
                        {
                            stopWatch.Start();
                            elapsedTimeTextBox.Text = stopWatch.Elapsed.ToString();
                            
                            counter_sum++;
                            timer1.Enabled = true;

                            y1 = (int) angle_1;
                            y2 = (int) angle_2;
                            y3 = (int) angle_3;
                            y4 = (int) angle_4;
                            y5 = (int) angle_5;
                            y6 = (int) angle_6;

                            if (angle_counter == 1)
                            {
                                x4 = 0;
                                y4 = 0;
                                x5 = 0;
                                y5 = 0;
                                x6 = 0;
                                y6 = 0;

                               neckLabel.Hide();
                               neck1Label.Hide();
                               spineShoulderLabel.Hide();
                            }
                            if (angle_counter == 2)
                            {
                                x1 = 0;
                                y1 = 0;
                                x2 = 0;
                                y2 = 0;
                                x3 = 0;
                                y3 = 0;

                                shoulderRightLabel.Hide();
                                shoulderLeftLabel.Hide();
                                spineMidLabel.Hide();
                            }
                            if (counter_vary == 0)
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

                            if (mean_counter == mSmoothingKernal)
                            {
                                avg_mean1 = mean_sum1 / mSmoothingKernal;
                                avg_mean2 = mean_sum2 / mSmoothingKernal;
                                avg_mean3 = mean_sum3 / mSmoothingKernal;
                                avg_mean4 = mean_sum4 / mSmoothingKernal;
                                avg_mean5 = mean_sum5 / mSmoothingKernal;
                                avg_mean6 = mean_sum6 / mSmoothingKernal;

                                int tmp;
                                for (int i = 0; i < mSmoothingKernal; i++)
                                {
                                    for (int j = i + 1; j < mSmoothingKernal; j++)
                                    {
                                        if (median_smoothing1[j] < median_smoothing1[i])
                                        {
                                            tmp = median_smoothing1[i];
                                            median_smoothing1[i] = median_smoothing1[j];
                                            median_smoothing1[j] = tmp;
                                        }
                                    }
                                }
                                if (mSmoothingKernal % 2 == 0)
                                {
                                    median1 = (median_smoothing1[mSmoothingKernal / 2] + median_smoothing1[mSmoothingKernal / 2 + 1]) / 2;
                                }
                                else
                                {
                                    median1 = median_smoothing1[(mSmoothingKernal + 1) / 2];
                                }
                                for (int i = 0; i < mSmoothingKernal; i++)
                                {
                                    for (int j = i + 1; j < mSmoothingKernal; j++)
                                    {
                                        if (median_smoothing2[j] < median_smoothing2[i])
                                        {
                                            tmp = median_smoothing2[i];
                                            median_smoothing2[i] = median_smoothing2[j];
                                            median_smoothing2[j] = tmp;
                                        }
                                    }
                                }
                                if (mSmoothingKernal % 2 == 0)
                                {
                                    median2 = (median_smoothing2[mSmoothingKernal / 2] + median_smoothing2[mSmoothingKernal / 2 + 1]) / 2;
                                }
                                else
                                {
                                    median2 = median_smoothing2[(mSmoothingKernal + 1) / 2];
                                }

                                for (int i = 0; i < mSmoothingKernal; i++)
                                {
                                    for (int j = i + 1; j < mSmoothingKernal; j++)
                                    {
                                        if (median_smoothing3[j] < median_smoothing3[i])
                                        {
                                            tmp = median_smoothing3[i];
                                            median_smoothing3[i] = median_smoothing3[j];
                                            median_smoothing3[j] = tmp;
                                        }
                                    }
                                }

                                if (mSmoothingKernal % 2 == 0)
                                {
                                    median3 = (median_smoothing3[mSmoothingKernal / 2] + median_smoothing3[mSmoothingKernal / 2 + 1]) / 2;
                                }
                                else
                                {
                                    median3 = median_smoothing3[(mSmoothingKernal + 1) / 2];
                                }

                                for (int i = 0; i < mSmoothingKernal; i++)
                                {
                                    for (int j = i + 1; j < mSmoothingKernal; j++)
                                    {
                                        if (median_smoothing4[j] < median_smoothing4[i])
                                        {
                                            tmp = median_smoothing4[i];
                                            median_smoothing4[i] = median_smoothing4[j];
                                            median_smoothing4[j] = tmp;
                                        }
                                    }
                                }

                                if (mSmoothingKernal % 2 == 0)
                                {
                                    median4 = (median_smoothing4[mSmoothingKernal / 2] + median_smoothing4[mSmoothingKernal / 2 + 1]) / 2;
                                }
                                else
                                {
                                    median4 = median_smoothing4[(mSmoothingKernal + 1) / 2];
                                }

                                for (int i = 0; i < mSmoothingKernal; i++)
                                {
                                    for (int j = i + 1; j < mSmoothingKernal; j++)
                                    {
                                        if (median_smoothing5[j] < median_smoothing5[i])
                                        {
                                            tmp = median_smoothing5[i];
                                            median_smoothing5[i] = median_smoothing5[j];
                                            median_smoothing5[j] = tmp;
                                        }
                                    }
                                }

                                if (mSmoothingKernal % 2 == 0)
                                {
                                    median5 = (median_smoothing5[mSmoothingKernal / 2] + median_smoothing5[mSmoothingKernal / 2 + 1]) / 2;
                                }
                                else
                                {
                                    median5 = median_smoothing5[(mSmoothingKernal + 1) / 2];
                                }

                                for (int i = 0; i < mSmoothingKernal; i++)
                                {
                                    for (int j = i + 1; j < mSmoothingKernal; j++)
                                    {
                                        if (median_smoothing6[j] < median_smoothing6[i])
                                        {
                                            tmp = median_smoothing6[i];
                                            median_smoothing6[i] = median_smoothing6[j];
                                            median_smoothing6[j] = tmp;
                                        }
                                    }
                                }

                                if (mSmoothingKernal % 2 == 0)
                                {
                                    median6 = (median_smoothing6[mSmoothingKernal / 2] + median_smoothing6[mSmoothingKernal / 2 + 1]) / 2;
                                }
                                else
                                {
                                    median6 = median_smoothing6[(mSmoothingKernal + 1) / 2];
                                }

                                if (median1 > (x1 + notAllowed) || median1 < (x1 - notAllowed) || median2 > (x2 + notAllowed) || median2 < (x2 - notAllowed) || median3 > (x3 + notAllowed) || median3 < (x3 - notAllowed) || median4 > (x4 + notAllowed) || median4 < (x4 - notAllowed) || median5 > (x5 + notAllowed) || median5 < (x5 - notAllowed) || median6 > (x6 + notAllowed) || median6 < (x6 - notAllowed))
                                {
                                    if (mMedians == 1)
                                    {
                                        if (mMovie == 1)
                                        {
                                            if (mMovieSensitive == 1)
                                                axWindowsMediaPlayer1.Ctlcontrols.pause();
                                            else
                                                axWindowsMediaPlayer1.Ctlcontrols.play();
                                        }
                                        if (mProgressSensitive == 1)
                                        {
                                            timer1.Enabled = false;
                                        }
                                        else
                                        {
                                            timer1.Enabled = true;
                                        }
                                        if (mTrafficSensitive == 1)
                                        {
                                            redLightPictureBox.Visible = true;
                                            yellowLightPictureBox.Visible = false;
                                            greenLightPictureBox.Visible = false;
                                            count = 21;
                                        }
                                        else
                                        {
                                            greenLightPictureBox.Visible = true;
                                        }
                                    }
                                    if (average_tagged1 == 1)
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6 + "," + "Tagged");
                                    }
                                    // If there was not tagged moment
                                    else
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6);
                                    }
                                }
                                else if (median1 > (x1 + warning) || median1 < (x1 - warning) || median2 > (x2 + warning) || median2 < (x2 - warning) || median3 > (x3 + warning) || median3 < (x3 - warning) || median4 > (x4 + warning) || median4 < (x4 - warning) || median5 > (x5 + warning) || median5 < (x5 - warning) || median6 > (x6 + warning) || median6 < (x6 - warning))
                                {
                                    if (mMedians == 1)
                                    {
                                        timer1.Enabled = true;
                                        if (mMovie == 1)
                                        {
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                        }

                                        if (mTrafficSensitive == 1)
                                        {
                                            redLightPictureBox.Visible = false;
                                            yellowLightPictureBox.Visible = true;
                                            greenLightPictureBox.Visible = false;
                                        }
                                        else
                                        {
                                            greenLightPictureBox.Visible = true;
                                        }
                                    }

                                    if (average_tagged1 == 1)
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6 + "," + "Tagged");
                                    }
                                    // If there was not tagged moment
                                    else
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6);
                                    }
                                }
                                else
                                {
                                    if (mMedians == 1)
                                    {
                                        timer1.Enabled = true;

                                        if (mMovie == 1)
                                        {
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                        }

                                        if (mTrafficSensitive == 1)
                                        {
                                            redLightPictureBox.Visible = false;
                                            yellowLightPictureBox.Visible = false;
                                            greenLightPictureBox.Visible = true;
                                        }
                                        else
                                        {
                                            greenLightPictureBox.Visible = true;
                                        }
                                    }
                                    if (average_tagged1 == 1)
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6 + "," + "Tagged");
                                    }
                                    // If there was not tagged moment
                                    else
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + median1 + "," + median2 + "," + median3 + "," + median4 + "," + median5 + "," + median6);
                                    }
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
                                {
                                    graph_counter++;
                                }

                                chart2.Series[0].Points.AddXY(graph_counter1, (x2 - y2));
                                chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart2.ChartAreas[0].AxisX.ScaleView.Position = chart2.Series[0].Points.Count - chart2.ChartAreas[0].AxisX.ScaleView.Size;

                                if (graph_counter1 % 100 == 0)
                                {
                                    chart2.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter1++;
                                }
                                else
                                {
                                    graph_counter1++;
                                }

                                chart3.Series[0].Points.AddXY(graph_counter2, (x3 - y3));
                                chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart3.ChartAreas[0].AxisX.ScaleView.Position = chart3.Series[0].Points.Count - chart3.ChartAreas[0].AxisX.ScaleView.Size;

                                if (graph_counter2 % 100 == 0)
                                {
                                    chart3.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter2++;
                                }
                                else
                                {
                                    graph_counter2++;
                                }

                                chart4.Series[0].Points.AddXY(graph_counter3, (x4 - y4));
                                chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart4.ChartAreas[0].AxisX.ScaleView.Position = chart4.Series[0].Points.Count - chart4.ChartAreas[0].AxisX.ScaleView.Size;

                                if (graph_counter3 % 100 == 0)
                                {
                                    chart4.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter3++;
                                }
                                else
                                {
                                    graph_counter3++;
                                }

                                chart5.Series[0].Points.AddXY(graph_counter4, (x5 - y5));
                                chart5.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart5.ChartAreas[0].AxisX.ScaleView.Position = chart5.Series[0].Points.Count - chart5.ChartAreas[0].AxisX.ScaleView.Size;

                                if (graph_counter4 % 100 == 0)
                                {
                                    chart5.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter4++;
                                }
                                else
                                {
                                    graph_counter4++;
                                }

                                chart6.Series[0].Points.AddXY(graph_counter5, (x6 - y6));
                                chart6.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                chart6.ChartAreas[0].AxisX.ScaleView.Position = chart6.Series[0].Points.Count - chart6.ChartAreas[0].AxisX.ScaleView.Size;

                                if (graph_counter5 % 100 == 0)
                                {
                                    chart6.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                                    graph_counter5++;
                                }
                                else
                                {
                                    graph_counter5++;
                                }

                                if (avg_mean1 > (x1 + notAllowed) || avg_mean1 < (x1 - notAllowed) || avg_mean2 > (x2 + notAllowed) || avg_mean2 < (x2 - notAllowed) || avg_mean3 > (x3 + notAllowed) || avg_mean3 < (x3 - notAllowed) || avg_mean4 > (x4 + notAllowed) || avg_mean4 < (x4 - notAllowed) || avg_mean5 > (x5 + notAllowed) || avg_mean5 < (x5 - notAllowed) || avg_mean6 > (x6 + notAllowed) || avg_mean6 < (x6 - notAllowed))
                                {
                                    if (avg_mean1 > (x1 + notAllowed) || avg_mean1 < (x1 - notAllowed))
                                    {
                                        shoulderRightLabel.ForeColor = Color.Red;
                                    }

                                    if (avg_mean2 > (x2 + notAllowed) || avg_mean2 < (x2 - notAllowed))
                                    {
                                        shoulderLeftLabel.ForeColor = Color.Red;
                                    }

                                    if (avg_mean3 > (x3 + notAllowed) || avg_mean3 < (x3 - notAllowed))
                                    {
                                        spineMidLabel.ForeColor = Color.Red;
                                    }

                                    if (avg_mean4 > (x4 + notAllowed) || avg_mean4 < (x4 - notAllowed))
                                    {
                                        neckLabel.ForeColor = Color.Red;
                                    }

                                    if (avg_mean5 > (x5 + notAllowed) || avg_mean5 < (x5 - notAllowed))
                                    {
                                        neck1Label.ForeColor = Color.Red;
                                    }

                                    if (avg_mean6 > (x6 + notAllowed) || avg_mean6 < (x6 - notAllowed))
                                    {
                                        spineShoulderLabel.ForeColor = Color.Red;
                                    }

                                    if (mMeans == 1)
                                    {
                                        timer1.Enabled = false;

                                        if (mMovie == 1)
                                        {
                                            if (mMovieSensitive == 1)
                                            {
                                                axWindowsMediaPlayer1.Ctlcontrols.pause();
                                            }
                                            else
                                            {
                                                axWindowsMediaPlayer1.Ctlcontrols.play();
                                            }
                                        }
                                        if (mProgressSensitive == 1)
                                        {
                                            timer1.Enabled = false;
                                        }
                                        else
                                        {
                                            timer1.Enabled = true;
                                        }
                                        if (mTrafficSensitive == 1)
                                        {
                                            redLightPictureBox.Visible =true;
                                            yellowLightPictureBox.Visible = false;
                                            greenLightPictureBox.Visible =false;
                                            stopWatch.Stop();
                                            count = 21;
                                        }
                                        else
                                        {
                                            greenLightPictureBox.Visible = true;
                                        }
                                    }
                                    if (average_tagged1 == 1)
                                    {
                                        differenceCsvFile.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString()+","+"Tagged");
                                        differenceCsvFile.WriteLine(" " + "," + " " + "," + " " + "," + " "+ "," +" " + "," +" ");
                                        amplitdueCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + ","+final_amplitude);
                                        amplitdueCsvFile.WriteLine(" "+" ");
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6 + "," + "Tagged");
                                        average_tagged1 = 0;
                                    }
                                    // If there was not tagged moment
                                    else
                                    {
                                        differenceCsvFile.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString());
                                        differenceCsvFile.WriteLine(" " + "," + " " + "," + " " + "," + " " + "," + " " + "," + " ");
                                        amplitdueCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        amplitdueCsvFile.WriteLine(" " + " ");
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6);
                                    } // Initialize all the sum variables to zero
                                    
                                }
                                else if (avg_mean1 > (x1 + warning) || avg_mean1 < (x1 - warning) || avg_mean2 > (x2 + warning) || avg_mean2 < (x2 - warning) || avg_mean3 > (x3 + warning) || avg_mean3 < (x3 - warning) || avg_mean4 > (x4 + warning) || avg_mean4 < (x4 - warning) || avg_mean5 > (x5 + warning) || avg_mean5 < (x5 - warning) || avg_mean6 > (x6 + warning) || avg_mean6 < (x6 - warning))
                                {
                                    shoulderRightLabel.ForeColor = Color.Black;
                                    shoulderLeftLabel.ForeColor = Color.Black;
                                    spineMidLabel.ForeColor = Color.Black;
                                    neckLabel.ForeColor = Color.Black;
                                    neck1Label.ForeColor = Color.Black;
                                    spineShoulderLabel.ForeColor = Color.Black;
                                    if (mMeans == 1)
                                    {
                                        timer1.Enabled = true;
                                        if (mMovie == 1)
                                        {
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                        }

                                        if (mTrafficSensitive == 1)
                                        {
                                            redLightPictureBox.Visible = false;
                                            yellowLightPictureBox.Visible = true;
                                            greenLightPictureBox.Visible = false;
                                        }
                                        else
                                        {
                                            greenLightPictureBox.Visible = true;
                                        }
                                    }
                                    if (average_tagged1 == 1)
                                    {
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6 + "," + "Tagged");
                                        differenceCsvFile.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString()+","+"Tagged");
                                        amplitdueCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        average_tagged1 = 0;
                                    }
                                    // If there was not tagged moment
                                    else
                                    {
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6);
                                        differenceCsvFile.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString());
                                        amplitdueCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                    } // Initialize all the sum variables to zero
                                }
                                else
                                {
                                    shoulderRightLabel.ForeColor = Color.Black;
                                    shoulderLeftLabel.ForeColor = Color.Black;
                                    spineMidLabel.ForeColor = Color.Black;
                                    neckLabel.ForeColor = Color.Black;
                                    neck1Label.ForeColor = Color.Black;
                                    spineShoulderLabel.ForeColor = Color.Black;

                                    if (mMeans == 1)
                                    {
                                        timer1.Enabled = true;
                                        if (mMovie == 1)
                                        {
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                        }

                                        if (mTrafficSensitive == 1)
                                        {
                                            redLightPictureBox.Visible = false;
                                            yellowLightPictureBox.Visible = false;
                                            greenLightPictureBox.Visible = true;
                                        }
                                        else
                                        {
                                            greenLightPictureBox.Visible = true;
                                        }
                                    }

                                    if (average_tagged1 == 1)
                                    {
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6 + "," + "Tagged");
                                        differenceCsvFile.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString()+","+"Tagged");
                                        amplitdueCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        average_tagged1 = 0;
                                    }
                                    // If there was not tagged moment
                                    else
                                    {
                                        differenceCsvFile.WriteLine((x1 - y1).ToString() + "," + (x2 - y2).ToString() + "," + (x3 - y3).ToString() + "," + (x4 - y4).ToString() + "," + (x5 - y5).ToString() + "," + (x6 - y6).ToString());
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original" + "," + avg_mean1 + "," + avg_mean2 + "," + avg_mean3 + "," + avg_mean4 + "," + avg_mean5 + "," + avg_mean6);
                                        amplitdueCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                    } // Initialize all the sum variables to zero
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
                                    averageCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + avg1 + "," + avg2 + "," + avg3 + "," + avg4 + "," + avg5 + "," + avg6 + "," + "Tagged");
                                    average_tagged = 0;
                                }
                                // If there was not tagged moment
                                else
                                {
                                    averageCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + avg1 + "," + avg2 + "," + avg3 + "," + avg4 + "," + avg5 + "," + avg6);
                                }

                                // Initialize all the sum variables to zero
                                sum1 = 0;
                                sum2 = 0;
                                sum3 = 0;
                                sum4 = 0;
                                sum5 = 0;
                                sum6 = 0;
                                sum7 = 0;
                                sum8 = 0;
                            }
                            // If the movement is more than 3mm
                            if (y1 > (x1 + notAllowed) || y1 < (x1 - notAllowed) || y2 > (x2 + notAllowed) || y2 < (x2 - notAllowed) || y3 > (x3 + notAllowed) || y3 < (x3 - notAllowed) ||y4 > (x4 + notAllowed) || y4 < (x4 - notAllowed) || y5 > (x5 + notAllowed) || y5 < (x5 - notAllowed) || y6 > (x6 + notAllowed) || y6 < (x6 - notAllowed))
                            {
                                if (mRawMeasures == 1)
                                {
                                    if (mMovie == 1)
                                    {
                                        if (mMovieSensitive == 1)
                                        {
                                            axWindowsMediaPlayer1.Ctlcontrols.pause();
                                        }
                                        else
                                        {
                                            axWindowsMediaPlayer1.Ctlcontrols.play();
                                        }
                                    }

                                    if (mProgressSensitive == 1)
                                    {
                                        timer1.Enabled = false;
                                    }
                                    else
                                    {
                                        timer1.Enabled = true;
                                    }

                                    if (mTrafficSensitive == 1)
                                    {
                                        redLightPictureBox.Visible = true;
                                        yellowLightPictureBox.Visible = false;
                                        greenLightPictureBox.Visible = false;
                                        count = 21;
                                    }
                                    else
                                    {
                                        greenLightPictureBox.Visible = true;
                                    }
                                }

                                mOverNotAllowed++;

                                if (mOverWarning > 0)
                                {
                                    videoCsvFile.WriteLine("--------");
                                }

                                if (counter1 == 0)
                                {
                                    if (mOverNotAllowed == 1)
                                    {
                                        videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement" + "," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                        videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement" + "," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                    }

                                    rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                    videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Large Movement" + "," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);

                                }
                                else
                                {
                                    // If there is a small tagged movement
                                    if (counter1 == 1)
                                    {
                                        if (mOverNotAllowed == 1)
                                        {
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "SmallMovement");
                                        counter1 = 0;

                                    }
                                    // If there is a medium tagged movement
                                    if (counter1 == 2)
                                    {
                                        if (mOverNotAllowed == 1)
                                        {
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "MediumMovement");
                                        counter1 = 0;
                                    }

                                    // If there is a large tagged movement
                                    if (counter1 == 3)
                                    {
                                        if (mOverNotAllowed == 1)
                                        {
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "LargeMovement");
                                        counter1 = 0;
                                    }
                                }
                            }

                            // If the movement is more than 2mm
                            else if (y1 > (x1 + warning) || y1 < (x1 - warning) || y2 > (x2 + warning) || y2 < (x2 - warning) || y3 > (x3 + warning) || y3 < (x3 - warning) || y4 > (x4 + warning) || y4 < (x4 - warning) || y5 > (x5 + warning) || y5 < (x5 - warning) || y6 > (x6 + warning) || y6 < (x6 - warning))
                            {
                                if (mRawMeasures == 1)
                                {
                                    timer1.Enabled = true;

                                    if (mMovie == 1)
                                    {
                                        axWindowsMediaPlayer1.Ctlcontrols.play();
                                    }

                                    if (mTrafficSensitive == 1)
                                    {
                                        redLightPictureBox.Visible = false;
                                        yellowLightPictureBox.Visible = true;
                                        greenLightPictureBox.Visible = false;
                                    }
                                    else
                                    {
                                        greenLightPictureBox.Visible = true;
                                    }
                                }

                                mOverWarning++;

                                if (mOverNotAllowed > 0)
                                {
                                    videoCsvFile.WriteLine("--------");
                                }

                                if (counter1 == 0)
                                {
                                    if (mOverWarning == 1)
                                    {
                                        videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                        videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                    }

                                    rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                    videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                }
                                else
                                {
                                    // If the tagged movement is small movement
                                    if (counter1 == 1)
                                    {
                                        if (mOverWarning == 1)
                                        {
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "SmallMovement");
                                        counter1 = 0;
                                    }

                                    // If the tagged movement is medium movement
                                    if (counter1 == 2)
                                    {
                                        if (mOverWarning == 1)
                                        {
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "MediumMovement");
                                        counter1 = 0;
                                    }

                                    // If the tagged movement is large movement
                                    if (counter1 == 3)
                                    {
                                        if (mOverWarning == 1)
                                        {
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(elapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "LargeMovement");
                                        counter1 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //if the movement is less than 2mm
                                if (mRawMeasures == 1)
                                {
                                    timer1.Enabled = true;

                                    if (mMovie == 1)
                                    {
                                        axWindowsMediaPlayer1.Ctlcontrols.play();
                                    }

                                    if (mTrafficSensitive == 1)
                                    {
                                        redLightPictureBox.Visible = false;
                                        yellowLightPictureBox.Visible = false;
                                        greenLightPictureBox.Visible = true;
                                    }
                                    else
                                    {
                                        greenLightPictureBox.Visible = true;
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

                                if (mOverNotAllowed > 0)
                                {
                                    videoCsvFile.WriteLine("--------");
                                }

                                if (mOverWarning > 0)
                                {
                                    videoCsvFile.WriteLine("--------");
                                }

                                mOverNotAllowed = 0;
                                mOverWarning = 0;

                                if (counter1 == 0)
                                { 
                                    rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6);
                                }
                                else
                                {
                                    // Small tagged movement
                                    if (counter1 == 1)
                                    {
                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "SmallMovement");
                                        counter1 = 0;
                                    }
                                    // Medium tagged movement
                                    if (counter1 == 2)
                                    {
                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "MediumMovement");
                                        counter1 = 0;
                                    }
                                    // Large tagged movement
                                    if (counter1 == 3)
                                    {
                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + y1 + "," + y2 + "," + y3 + "," + y4 + "," + y5 + "," + y6 + "," + "LargeMovement");
                                        counter1 = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // We refer these angle values as the original position and compare with obtained angle values(y1,y2,y3,....)
                            x1 = (int)angle_1;
                            x2 = (int)angle_2;
                            x3 = (int)angle_3;
                            x4 = (int)angle_4;
                            x5 = (int)angle_5;
                            x6 = (int)angle_6;

                            if (count == 21)
                            {
                                if (x1 > (original1 - (back_original)) && 
                                    x1 < (original1 + (back_original)) && 
                                    x2 > (original2 - back_original) && 
                                    x2 < (original2 + back_original) &&
                                    x3 > (original3 - back_original) &&
                                    x3 < (original3 + back_original) &&
                                    x4 > (original4 - back_original) &&
                                    x4 < (original4 + back_original) &&
                                    x5 > (original5 - back_original) &&
                                    x5 < (original5 + back_original) &&
                                    x6 > (original6 - back_original) &&
                                    x6 < (original6 + back_original))
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

        private String getCsvPath(String fileType)
        {
            return Path.Combine(mFilePath + "\\" + mSubjectInitials + "-" + mExperimentNumber + "-" + mCurrentDate + "-" + fileType + ".csv");
        }

        private void FinalForm_Load(object sender, EventArgs e)
        {            
            if (count == 22)
            {
                this.BackColor = System.Drawing.Color.Black;
            }
        }

        private void FinalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close all data files when form closes

            rawCsvFile.Close();
            videoCsvFile.Close();
            averageCsvFile.Close();
            meanCsvFile.Close();
            medianCsvFile.Close();
            differenceCsvFile.Close();
            amplitdueCsvFile.Close();
        }

        private void FinalForm_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // For small tagged movement press s key
            if (e.KeyChar == 's')
            {
                e.Handled = true;
                counter1 = 1;
                average_tagged = 1;
                average_tagged1 = 1;
            }

            // For medium tagged movement press m
            if (e.KeyChar == 'm')
            {
                e.Handled = true;
                counter1 = 2;
                average_tagged = 1;
                average_tagged1 = 1;
            }

            // For large tagged movement press l
            if (e.KeyChar == 'l')
            {
                counter1 = 3;
                average_tagged = 1;
                average_tagged1 = 1;
            }

            // To start the process press r
            if (e.KeyChar == 'r')
            {
                count = 22;

                startButton.BackColor = Color.DeepSkyBlue;
            }
        }
    }
}
