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

        Body[] mBody = null;

        // This is the form on which the participant will view the movie.
        SubjectMovieForm subjectMovieForm;

        Stopwatch stopWatch = new Stopwatch();

        // This is being used to determine if session is started or stopped, but in some ways that I need to look further into.
        int mSessionState = 0;
  
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
        int x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, mNeckToElbowRightAngle, mNeckToElbowLeftAngle, mSpineBaseToHeadAngle, mHeadToShoulderLeftAngle, mHeadToShoulderRightAngle, mHeadToSpineShoulder, y7, y8, y9, y10;

        // Variables used to get the average values
        int mShoulderRightAverageSum = 0,
            mShoulderLeftAverageSum = 0,
            mSpineMidAverageSum = 0,
            mNeckAverageSum = 0,
            mNeck1AverageSum = 0,
            mSpineShoulderAverageSum = 0;

        int graph_counter = 0,
            graph_counter1 = 0,
            graph_counter2 = 0,
            graph_counter3 = 0,
            graph_counter4 = 0,
            graph_counter5 = 0;

        bool mStarted = false;
        bool mOriginalAnglesSet = false;

        double mShoulderRightAverage, mShoulderLeftAverage, mSpineMidAverage, mNeckAverage, mNeck1Average, mSpineShoulderAverage, avg7, avg8;

        int mShouderRightMeanSum = 0,
            mShoulderLeftMeanSum = 0,
            mSpineMidMeanSum = 0,
            mNeckMeanSum = 0,
            mNeck1MeanSum = 0,
            mSpineShoulderMeanSum = 0,
            average_tagged1 = 0,
            mOriginalNeckToElbowRightAngle = 0,
            mOriginalNeckToElbowLeftAngle = 0,
            mOriginalSpineBaseToHeadAngle = 0,
            mOriginalHeadToShoulderLeftAngle = 0,
            mOriginalHeadToShoulderRightAngle = 0,
            mOriginalHeadToSpineShoulder=0;

        int mDepthFrameReference;

        int mShoulderRightMean,
            mShoulderLeftMean,
            mSpineMidMean,
            mNeckMean,
            mNeck1Mean,
            mSpineShoulderMean,
            mSessionTime = 0,
            mMovementLowerLimitAgainstOriginal;

        int mMovie,
            mMovieSensitive,
            mProgress,
            mProgressSensitive,
            mTrafficSensitive,
            mRawMeasures,
            mMeans,
            mMedians;

        int mJointTrackingType;

        // This should probably be set to zero and iterate earlier down below (if we ultimately need this at all).
        int mMeanDataReadIteration = 1;

        // This gets incremented at each data receipt if started and resets to zero before it hits 16.
        int mAverageDataReadIteration = 0;

        int average_tagged = 0;
        int mSmoothingKernal;

        int[] mShoulderRightMedianArray,
            mShoulderLeftMedianArray,
            mSpineMidMedianArray,
            mNeckMedianArray,
            mNeck1MedianArray,
            mSpineShoulderMedianArray;

        int mWarning;
        int mNotAllowed;

        int mShoulderRightMedian,
            mShoulderLeftMedian,
            mSpineMidMedian,
            mNeckMedian,
            mNeck1Median,
            mSpineShoulderMedian;

        string mSubjectInitials;
        string mExperimentNumber;

        // These are used to determine the elapsed time of the session.
        DateTime mStartTime = new DateTime();
        TimeSpan mElapsedTime = new TimeSpan();

        /* This will be incremented on every tick. When the tick interval
         * is set for 1000, then mTickCount % 60 will be 0 and thus allow
         * us to perform an action every 1 second - specifically, increment
         * the progress bar.
         */
        int mTickCount = 0;

        int mTagType = 0;     

        int mTempArrayElementIteration = 0;
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
            int jointTrackingType,
            int depthFrameReference
            )
        {
            InitializeComponent();

            SetCharts();

            mDepthFrameReference = depthFrameReference;
            mSubjectInitials = subjectInitials;
            mExperimentNumber = experimentNumber;
            mSmoothingKernal = Convert.ToInt32(smoothingKernal);

            mWarning = Convert.ToInt32(smallMovementLowerLimit);
            mNotAllowed = Convert.ToInt32(largeMovementLowerLimit);

            lowerLimitSmallMovementTextBox.Text = mWarning.ToString();
            lowerLimitLargeMovementTextBox.Text = mNotAllowed.ToString();

            mSessionTime = Convert.ToInt32(sessionTime);
            mJointTrackingType = jointTrackingType;

            mShoulderRightMedianArray = new int[mSmoothingKernal];
            mShoulderLeftMedianArray = new int[mSmoothingKernal];
            mSpineMidMedianArray = new int[mSmoothingKernal];
            mNeckMedianArray = new int[mSmoothingKernal];
            mNeck1MedianArray = new int[mSmoothingKernal];
            mSpineShoulderMedianArray = new int[mSmoothingKernal];

            mMovie = movie;
            mMovieSensitive = movieSensitive;
            mProgress = progress;
            mProgressSensitive = progressSensitive;
            mTrafficSensitive = trafficSensitive;
            mRawMeasures = rawMeasures;
            mMeans = means;
            mMedians = medians;

            progressBar.Maximum = (2 * mSessionTime);
            axWindowsMediaPlayer1.URL = videoFile;

            mMovementLowerLimitAgainstOriginal = lowerLimitLargeMovement == 1 ? mNotAllowed : mWarning;

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
            mFilePath = filePath;

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
            mStartTime = DateTime.Now;

            SetTimer();

            // Writing to three files(Main data file, video reference file and average file)
            rawCsvFile.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            videoCsvFile.WriteLine("ElapsedTime" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck");
            averageCsvFile.WriteLine("Date&Time" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            meanCsvFile.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            medianCsvFile.WriteLine("Date&Time" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");

            subjectMovieForm = new SubjectMovieForm(
                mSmoothingKernal.ToString(),
                mWarning.ToString(),
                mNotAllowed.ToString(),
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
                mSessionTime,
                videoFile
                );
           
            initializeKinect();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            mSessionState = 22;
            mWarning = Convert.ToInt32(lowerLimitSmallMovementTextBox.Text);
            mNotAllowed = Convert.ToInt32(lowerLimitLargeMovementTextBox.Text);
            startButton.BackColor = Color.DeepSkyBlue;

            if (mStarted == false)
            {
                // Headers

                differenceCsvFile.WriteLine("Subject Initials" + "," + "Experiment No" + "," + "Smoothing Kernel" + "," + "Small Movement" + "," + "Large Movement");
                differenceCsvFile.WriteLine(mSubjectInitials + "," + mExperimentNumber + "," + mSmoothingKernal + "," + mWarning + "," + mNotAllowed);
                differenceCsvFile.WriteLine(" " + "," + " " + "," + " " + "," + " " + "," + " ");
                differenceCsvFile.WriteLine("ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");

                amplitdueCsvFile.WriteLine("Subject Initials" + "," + "Experiment No" + "," + "Smoothing Kernel" + "," + "Small Movement" + "," + "Large Movement");
                amplitdueCsvFile.WriteLine(mSubjectInitials + "," + mExperimentNumber + "," + mSmoothingKernal + "," + mWarning + "," + mNotAllowed);
                amplitdueCsvFile.WriteLine("Elapsed_Time" + "," + "Values");
                amplitdueCsvFile.WriteLine(" " + "," + " ");

                mStarted = true;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            mSessionState = 21;
            startButton.BackColor = Color.Red;
            mStarted = false;
        }

        // Timer event for progress bar
        private void timer1_Tick(object sender, EventArgs e)
        {
            mElapsedTime = DateTime.Now - mStartTime;

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
            if (mDepthFrameReference == 0)
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
            if (mDepthFrameReference == 1)
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
                    if (mBody == null)
                    {
                        /* TODO: Should only detect one body and provide message if
                         * more than one body is detected. Then we can not use foreach
                         * below. Also. mBody seems like it can be a local variable.
                         * Also. Is this creating a new body every time? Would that cause
                         * and issue with trying to track fine movements?
                         */
                        mBody = new Body[body_frame.BodyFrameSource.BodyCount];
                    }
                    body_frame.GetAndRefreshBodyData(mBody);
                    data_received = true;
                }
            }

            if (data_received)
            {
                foreach (Body body in mBody)
                {
                    // Process if the body has been detected
                    if (body.IsTracked)
                    {
                        if (mSessionState == 22) // I believe state is set to 22 if start button has been clicked.
                        {
                            startButton.BackColor = Color.DeepSkyBlue;
                        }
                        else if (mSessionState == 21)
                        {
                            startButton.BackColor = Color.Red; // Start button normally starts as red, fyi.
                            axWindowsMediaPlayer1.Ctlcontrols.pause();
                            timer1.Enabled = false;
                        }
                        else if (mSessionState == 20) // This is when state was not 22, and then there was too much movement, I believe.
                        {
                            startButton.BackColor = Color.Blue;
                        }

                        // Joints
                        var head = body.Joints[JointType.Head];
                        var neck = body.Joints[JointType.Neck];
                        var spineBase = body.Joints[JointType.SpineBase];
                        var spineMid = body.Joints[JointType.SpineMid];
                        var spineShoulder = body.Joints[JointType.SpineShoulder];
                        var shoulderLeft = body.Joints[JointType.ShoulderLeft];
                        var shoulderRight = body.Joints[JointType.ShoulderRight];
                        var elbowLeft = body.Joints[JointType.ElbowLeft];
                        var elbowRight = body.Joints[JointType.ElbowRight];

                        // Angles - doubles are returned by Angle(). Yet, later they are cast to int. Is this an issue?
                        var neckToElbowRightAngle = shoulderRight.Angle(neck, elbowRight);
                        var neckToElbowLeftAngle = shoulderLeft.Angle(neck, elbowLeft);
                        var spineBaseToHeadAngle = spineMid.Angle(spineBase, head);
                        var headToShoulderLeftAngle = neck.Angle(head, shoulderLeft);
                        var headToShoulderRightAngle = neck.Angle(head, shoulderRight);
                        var headToSpineShoulder = neck.Angle(head, spineShoulder);
                        
                        subjectMovieForm.transfer_values(
                            mSessionState,
                            (int) neckToElbowRightAngle,
                            (int) neckToElbowLeftAngle,
                            (int) spineBaseToHeadAngle,
                            (int) headToShoulderLeftAngle,
                            (int) headToShoulderRightAngle,
                            (int) headToSpineShoulder,
                            mWarning,
                            mNotAllowed,
                            mSessionState
                        );

                        // Once the user has pressed the start button                       
                        if (mSessionState == 22) // and deep sky blue button
                        {
                            stopWatch.Start();
                            elapsedTimeTextBox.Text = stopWatch.Elapsed.ToString();
                            timer1.Enabled = true;

                            mNeckToElbowRightAngle = (int) neckToElbowRightAngle;
                            mNeckToElbowLeftAngle = (int) neckToElbowLeftAngle;
                            mSpineBaseToHeadAngle = (int) spineBaseToHeadAngle;
                            mHeadToShoulderLeftAngle = (int) headToShoulderLeftAngle;
                            mHeadToShoulderRightAngle = (int) headToShoulderRightAngle;
                            mHeadToSpineShoulder = (int) headToSpineShoulder;

                            if (mJointTrackingType == 1)
                            {
                                x4 = 0;
                                mHeadToShoulderLeftAngle = 0;
                                x5 = 0;
                                mHeadToShoulderRightAngle = 0;
                                x6 = 0;
                                mHeadToSpineShoulder = 0;

                               neckLabel.Hide();
                               neck1Label.Hide();
                               spineShoulderLabel.Hide();
                            }
                            if (mJointTrackingType == 2)
                            {
                                x1 = 0;
                                mNeckToElbowRightAngle = 0;
                                x2 = 0;
                                mNeckToElbowLeftAngle = 0;
                                x3 = 0;
                                mSpineBaseToHeadAngle = 0;

                                shoulderRightLabel.Hide();
                                shoulderLeftLabel.Hide();
                                spineMidLabel.Hide();
                            }

                            if (!mOriginalAnglesSet)
                            {
                                SetOriginalAngles();
                            }

                            UpdateMedianArrays();
                            UpdateAverageSums();
                            UpdateMeanSums();

                            if (mMeanDataReadIteration == mSmoothingKernal)
                            {
                                mMeanDataReadIteration = 0;

                                SetMeans();
                                SetMedians();

                                // Red Light
                                if (mShoulderRightMedian > (x1 + mNotAllowed) || mShoulderRightMedian < (x1 - mNotAllowed) || mShoulderLeftMedian > (x2 + mNotAllowed) || mShoulderLeftMedian < (x2 - mNotAllowed) || mSpineMidMedian > (x3 + mNotAllowed) || mSpineMidMedian < (x3 - mNotAllowed) || mNeckMedian > (x4 + mNotAllowed) || mNeckMedian < (x4 - mNotAllowed) || mNeck1Median > (x5 + mNotAllowed) || mNeck1Median < (x5 - mNotAllowed) || mSpineShoulderMedian > (x6 + mNotAllowed) || mSpineShoulderMedian < (x6 - mNotAllowed))
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
                                            mSessionState = 21;
                                        }
                                        else
                                        {
                                            greenLightPictureBox.Visible = true;
                                        }
                                    }
                                    if (average_tagged1 == 1)
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightMedian + "," + mShoulderLeftMedian + "," + mSpineMidMedian + "," + mNeckMedian + "," + mNeck1Median + "," + mSpineShoulderMedian + "," + "Tagged");
                                    }
                                    // If there was not tagged moment
                                    else
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightMedian + "," + mShoulderLeftMedian + "," + mSpineMidMedian + "," + mNeckMedian + "," + mNeck1Median + "," + mSpineShoulderMedian);
                                    }
                                }
                                // Yellow Light
                                else if (mShoulderRightMedian > (x1 + mWarning) || mShoulderRightMedian < (x1 - mWarning) || mShoulderLeftMedian > (x2 + mWarning) || mShoulderLeftMedian < (x2 - mWarning) || mSpineMidMedian > (x3 + mWarning) || mSpineMidMedian < (x3 - mWarning) || mNeckMedian > (x4 + mWarning) || mNeckMedian < (x4 - mWarning) || mNeck1Median > (x5 + mWarning) || mNeck1Median < (x5 - mWarning) || mSpineShoulderMedian > (x6 + mWarning) || mSpineShoulderMedian < (x6 - mWarning))
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
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightMedian + "," + mShoulderLeftMedian + "," + mSpineMidMedian + "," + mNeckMedian + "," + mNeck1Median + "," + mSpineShoulderMedian + "," + "Tagged");
                                    }
                                    // If there was not tagged moment
                                    else
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightMedian + "," + mShoulderLeftMedian + "," + mSpineMidMedian + "," + mNeckMedian + "," + mNeck1Median + "," + mSpineShoulderMedian);
                                    }
                                }
                                // Green Light
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
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightMedian + "," + mShoulderLeftMedian + "," + mSpineMidMedian + "," + mNeckMedian + "," + mNeck1Median + "," + mSpineShoulderMedian + "," + "Tagged");
                                    }
                                    // If there was not tagged moment
                                    else
                                    {
                                        medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightMedian + "," + mShoulderLeftMedian + "," + mSpineMidMedian + "," + mNeckMedian + "," + mNeck1Median + "," + mSpineShoulderMedian);
                                    }
                                }

                                // If there was a tagged moment
                                double amplitude = Math.Sqrt(((x1 - mNeckToElbowRightAngle) * (x1 - mNeckToElbowRightAngle)) + ((x2 - mNeckToElbowLeftAngle) * (x2 - mNeckToElbowLeftAngle)) + ((x3 - mSpineBaseToHeadAngle) * (x3 - mSpineBaseToHeadAngle)) + ((x4 - mHeadToShoulderLeftAngle) * (x4 - mHeadToShoulderLeftAngle)) + ((x5 - mHeadToShoulderRightAngle) * (x5 - mHeadToShoulderRightAngle)) + ((x6 - mHeadToSpineShoulder) * (x6 - mHeadToSpineShoulder)));
                                double final_amplitude = (amplitude / 6);

                                UpdateAllCharts();

                                // Red Light
                                if (mShoulderRightMean > (x1 + mNotAllowed) || mShoulderRightMean < (x1 - mNotAllowed) || mShoulderLeftMean > (x2 + mNotAllowed) || mShoulderLeftMean < (x2 - mNotAllowed) || mSpineMidMean > (x3 + mNotAllowed) || mSpineMidMean < (x3 - mNotAllowed) || mNeckMean > (x4 + mNotAllowed) || mNeckMean < (x4 - mNotAllowed) || mNeck1Mean > (x5 + mNotAllowed) || mNeck1Mean < (x5 - mNotAllowed) || mSpineShoulderMean > (x6 + mNotAllowed) || mSpineShoulderMean < (x6 - mNotAllowed))
                                {
                                    // Check for and update any labels that need to be turned red.
                                    UpdateLabelColors(); // diff

                                    if (mMeans == 1)
                                    {
                                        timer1.Enabled = false; // diff

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
                                            greenLightPictureBox.Visible =false;
                                            stopWatch.Stop(); // diff
                                            mSessionState = 21;
                                        }
                                        else
                                        {
                                            greenLightPictureBox.Visible = true;
                                        }
                                    }

                                    // If there was a tagged movement
                                    if (average_tagged1 == 1)
                                    {
                                        differenceCsvFile.WriteLine((x1 - mNeckToElbowRightAngle).ToString() + "," + (x2 - mNeckToElbowLeftAngle).ToString() + "," + (x3 - mSpineBaseToHeadAngle).ToString() + "," + (x4 - mHeadToShoulderLeftAngle).ToString() + "," + (x5 - mHeadToShoulderRightAngle).ToString() + "," + (x6 - mHeadToSpineShoulder).ToString()+","+"Tagged");
                                        differenceCsvFile.WriteLine(" " + "," + " " + "," + " " + "," + " "+ "," +" " + "," +" ");
                                        amplitdueCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        amplitdueCsvFile.WriteLine(" "+" ");
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + mShoulderRightMean + "," + mShoulderLeftMean + "," + mSpineMidMean + "," + mNeckMean + "," + mNeck1Mean + "," + mSpineShoulderMean + "," + "Tagged");
                                        average_tagged1 = 0;
                                    }
                                    // If there was not tagged movement
                                    else
                                    {
                                        differenceCsvFile.WriteLine((x1 - mNeckToElbowRightAngle).ToString() + "," + (x2 - mNeckToElbowLeftAngle).ToString() + "," + (x3 - mSpineBaseToHeadAngle).ToString() + "," + (x4 - mHeadToShoulderLeftAngle).ToString() + "," + (x5 - mHeadToShoulderRightAngle).ToString() + "," + (x6 - mHeadToSpineShoulder).ToString());
                                        differenceCsvFile.WriteLine(" " + "," + " " + "," + " " + "," + " " + "," + " " + "," + " ");
                                        amplitdueCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        amplitdueCsvFile.WriteLine(" " + " ");
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + mShoulderRightMean + "," + mShoulderLeftMean + "," + mSpineMidMean + "," + mNeckMean + "," + mNeck1Mean + "," + mSpineShoulderMean);
                                    }                                    
                                }
                                // Yellow Light
                                else if (mShoulderRightMean > (x1 + mWarning) || mShoulderRightMean < (x1 - mWarning) || mShoulderLeftMean > (x2 + mWarning) || mShoulderLeftMean < (x2 - mWarning) || mSpineMidMean > (x3 + mWarning) || mSpineMidMean < (x3 - mWarning) || mNeckMean > (x4 + mWarning) || mNeckMean < (x4 - mWarning) || mNeck1Mean > (x5 + mWarning) || mNeck1Mean < (x5 - mWarning) || mSpineShoulderMean > (x6 + mWarning) || mSpineShoulderMean < (x6 - mWarning))
                                {
                                    ResetLabelColors();
                                    
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
                                    // If there was a tagged movement
                                    if (average_tagged1 == 1)
                                    {
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement" + "," + mShoulderRightMean + "," + mShoulderLeftMean + "," + mSpineMidMean + "," + mNeckMean + "," + mNeck1Mean + "," + mSpineShoulderMean + "," + "Tagged");
                                        differenceCsvFile.WriteLine((x1 - mNeckToElbowRightAngle).ToString() + "," + (x2 - mNeckToElbowLeftAngle).ToString() + "," + (x3 - mSpineBaseToHeadAngle).ToString() + "," + (x4 - mHeadToShoulderLeftAngle).ToString() + "," + (x5 - mHeadToShoulderRightAngle).ToString() + "," + (x6 - mHeadToSpineShoulder).ToString()+","+"Tagged");
                                        amplitdueCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        average_tagged1 = 0;
                                    }
                                    // If there was not tagged movement
                                    else
                                    {
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement" + "," + mShoulderRightMean + "," + mShoulderLeftMean + "," + mSpineMidMean + "," + mNeckMean + "," + mNeck1Mean + "," + mSpineShoulderMean);
                                        differenceCsvFile.WriteLine((x1 - mNeckToElbowRightAngle).ToString() + "," + (x2 - mNeckToElbowLeftAngle).ToString() + "," + (x3 - mSpineBaseToHeadAngle).ToString() + "," + (x4 - mHeadToShoulderLeftAngle).ToString() + "," + (x5 - mHeadToShoulderRightAngle).ToString() + "," + (x6 - mHeadToSpineShoulder).ToString());
                                        amplitdueCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                    }
                                }
                                // Green Light
                                else
                                {
                                    ResetLabelColors();

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
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original" + "," + mShoulderRightMean + "," + mShoulderLeftMean + "," + mSpineMidMean + "," + mNeckMean + "," + mNeck1Mean + "," + mSpineShoulderMean + "," + "Tagged");
                                        differenceCsvFile.WriteLine((x1 - mNeckToElbowRightAngle).ToString() + "," + (x2 - mNeckToElbowLeftAngle).ToString() + "," + (x3 - mSpineBaseToHeadAngle).ToString() + "," + (x4 - mHeadToShoulderLeftAngle).ToString() + "," + (x5 - mHeadToShoulderRightAngle).ToString() + "," + (x6 - mHeadToSpineShoulder).ToString()+","+"Tagged");
                                        amplitdueCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                        average_tagged1 = 0;
                                    }
                                    // If there was not tagged movement
                                    else
                                    {
                                        differenceCsvFile.WriteLine((x1 - mNeckToElbowRightAngle).ToString() + "," + (x2 - mNeckToElbowLeftAngle).ToString() + "," + (x3 - mSpineBaseToHeadAngle).ToString() + "," + (x4 - mHeadToShoulderLeftAngle).ToString() + "," + (x5 - mHeadToShoulderRightAngle).ToString() + "," + (x6 - mHeadToSpineShoulder).ToString());
                                        meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original" + "," + mShoulderRightMean + "," + mShoulderLeftMean + "," + mSpineMidMean + "," + mNeckMean + "," + mNeck1Mean + "," + mSpineShoulderMean);
                                        amplitdueCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
                                    }
                                }

                                ResetMeanSums();
                            }

                            // mMeanDataReadIteration was reset in the above conditional
                            mMeanDataReadIteration++;

                            // This is iterated before setting averages. mMeanDataReadIterations is iterated after setting means. Is this an issue?
                            mAverageDataReadIteration++;

                            // Logic for calculating Averages. Maytbe this should be combined with condtional for MeanData
                            if (mAverageDataReadIteration == 15) // 15 should maybe be a variable (maybe mSmoothingKernal).
                            {
                                SetAverages();
                                ResetAverageSums();

                                // This must be reset after SetAverages() as it uses mAverageDataReadIteration (though maybe it ought to use mSmoothingKernal).
                                mAverageDataReadIteration = 0;

                                // If there was a tagged moment
                                if (average_tagged == 1)
                                {
                                    averageCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightAverage + "," + mShoulderLeftAverage + "," + mSpineMidAverage + "," + mNeckAverage + "," + mNeck1Average + "," + mSpineShoulderAverage + "," + "Tagged");
                                    average_tagged = 0;
                                }
                                // If there was not tagged moment
                                else
                                {
                                    averageCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightAverage + "," + mShoulderLeftAverage + "," + mSpineMidAverage + "," + mNeckAverage + "," + mNeck1Average + "," + mSpineShoulderAverage);
                                }
                            }

                            // If the movement is more than 3mm
                            if (mNeckToElbowRightAngle > (x1 + mNotAllowed) || mNeckToElbowRightAngle < (x1 - mNotAllowed) || mNeckToElbowLeftAngle > (x2 + mNotAllowed) || mNeckToElbowLeftAngle < (x2 - mNotAllowed) || mSpineBaseToHeadAngle > (x3 + mNotAllowed) || mSpineBaseToHeadAngle < (x3 - mNotAllowed) ||mHeadToShoulderLeftAngle > (x4 + mNotAllowed) || mHeadToShoulderLeftAngle < (x4 - mNotAllowed) || mHeadToShoulderRightAngle > (x5 + mNotAllowed) || mHeadToShoulderRightAngle < (x5 - mNotAllowed) || mHeadToSpineShoulder > (x6 + mNotAllowed) || mHeadToSpineShoulder < (x6 - mNotAllowed))
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
                                        mSessionState = 21;
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

                                // Not tagged
                                if (mTagType == 0)
                                {
                                    if (mOverNotAllowed == 1)
                                    {
                                        videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement" + "," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                        videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement" + "," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                    }

                                    rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder);
                                    videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Large Movement" + "," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder);

                                }
                                // If tagged
                                else
                                {
                                    // If there is a small tagged movement
                                    if (mTagType == 1)
                                    {
                                        if (mOverNotAllowed == 1)
                                        {
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder + "," + "SmallMovement");
                                        mTagType = 0;

                                    }

                                    // If there is a medium tagged movement
                                    if (mTagType == 2)
                                    {
                                        if (mOverNotAllowed == 1)
                                        {
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder + "," + "MediumMovement");
                                        mTagType = 0;
                                    }

                                    // If there is a large tagged movement
                                    if (mTagType == 3)
                                    {
                                        if (mOverNotAllowed == 1)
                                        {
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder + "," + "LargeMovement");
                                        mTagType = 0;
                                    }
                                }
                            }
                            // If the movement is more than 2mm
                            else if (mNeckToElbowRightAngle > (x1 + mWarning) || mNeckToElbowRightAngle < (x1 - mWarning) || mNeckToElbowLeftAngle > (x2 + mWarning) || mNeckToElbowLeftAngle < (x2 - mWarning) || mSpineBaseToHeadAngle > (x3 + mWarning) || mSpineBaseToHeadAngle < (x3 - mWarning) || mHeadToShoulderLeftAngle > (x4 + mWarning) || mHeadToShoulderLeftAngle < (x4 - mWarning) || mHeadToShoulderRightAngle > (x5 + mWarning) || mHeadToShoulderRightAngle < (x5 - mWarning) || mHeadToSpineShoulder > (x6 + mWarning) || mHeadToSpineShoulder < (x6 - mWarning))
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

                                if (mTagType == 0)
                                {
                                    if (mOverWarning == 1)
                                    {
                                        videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                        videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                    }

                                    rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder);
                                    videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Small Movement," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder);
                                }
                                else
                                {
                                    // If the tagged movement is small movement
                                    if (mTagType == 1)
                                    {
                                        if (mOverWarning == 1)
                                        {
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder + "," + "SmallMovement");
                                        mTagType = 0;
                                    }

                                    // If the tagged movement is medium movement
                                    if (mTagType == 2)
                                    {
                                        if (mOverWarning == 1)
                                        {
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder + "," + "MediumMovement");
                                        mTagType = 0;
                                    }

                                    // If the tagged movement is large movement
                                    if (mTagType == 3)
                                    {
                                        if (mOverWarning == 1)
                                        {
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[0] + "," + temp2[0] + "," + temp3[0] + "," + temp4[0] + "," + temp5[0] + "," + temp6[0]);
                                            videoCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + "Original Before Movement," + temp1[1] + "," + temp2[1] + "," + temp3[1] + "," + temp4[1] + "," + temp5[1] + "," + temp6[1]);
                                        }

                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder + "," + "LargeMovement");
                                        mTagType = 0;
                                    }
                                }
                            }
                            // If the movement is less than 2mm
                            else
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
                                        yellowLightPictureBox.Visible = false;
                                        greenLightPictureBox.Visible = true;
                                    }
                                    else
                                    {
                                        greenLightPictureBox.Visible = true;
                                    }
                                }
                               
                                mTempArrayElementIteration++;

                                //here we are storing the values to temp array
                                //here we are storing to temp original values
                                if (mTempArrayElementIteration == 1)
                                {
                                    temp1[0] = mNeckToElbowRightAngle;
                                    temp2[0] = mNeckToElbowLeftAngle;
                                    temp3[0] = mSpineBaseToHeadAngle;
                                    temp4[0] = mHeadToShoulderLeftAngle;
                                    temp5[0] = mHeadToShoulderRightAngle;
                                    temp6[0] = mHeadToSpineShoulder;
                                }

                                if (mTempArrayElementIteration == 2)
                                {
                                    temp1[1] = mNeckToElbowRightAngle;
                                    temp2[1] = mNeckToElbowLeftAngle;
                                    temp3[1] = mSpineBaseToHeadAngle;
                                    temp4[1] = mHeadToShoulderLeftAngle;
                                    temp5[1] = mHeadToShoulderRightAngle;
                                    temp6[1] = mHeadToSpineShoulder;

                                    mTempArrayElementIteration = 0;
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

                                if (mTagType == 0)
                                { 
                                    rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder);
                                }
                                else
                                {
                                    // Small tagged movement
                                    if (mTagType == 1)
                                    {
                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder + "," + "SmallMovement");
                                        mTagType = 0;
                                    }
                                    // Medium tagged movement
                                    if (mTagType == 2)
                                    {
                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder + "," + "MediumMovement");
                                        mTagType = 0;
                                    }
                                    // Large tagged movement
                                    if (mTagType == 3)
                                    {
                                        rawCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original," + mNeckToElbowRightAngle + "," + mNeckToElbowLeftAngle + "," + mSpineBaseToHeadAngle + "," + mHeadToShoulderLeftAngle + "," + mHeadToShoulderRightAngle + "," + mHeadToSpineShoulder + "," + "LargeMovement");
                                        mTagType = 0;
                                    }
                                }
                            }
                        }
                        // If not started
                        else // If 20 or 21
                        {
                            // We refer these angle values as the original position and compare with obtained angle values
                            x1 = (int) neckToElbowRightAngle;
                            x2 = (int) neckToElbowLeftAngle;
                            x3 = (int) spineBaseToHeadAngle;
                            x4 = (int) headToShoulderLeftAngle;
                            x5 = (int) headToShoulderRightAngle;
                            x6 = (int) headToSpineShoulder;

                            /* This might indicate that the Original angles are set and are checking that movement hasn't
                             * occurred since. Otherwise, Change the session state and set mOriginalAnglesSet to false.
                             * A session state of 20 might prompt user to start process over.
                             * 
                             * Actually, I think state 21 indicates paused. And if user has moved too much, then we need to
                             * start over.
                             */
                            if (mSessionState == 21)
                            {
                                if (x1 > (mOriginalNeckToElbowRightAngle - mMovementLowerLimitAgainstOriginal) && 
                                    x1 < (mOriginalNeckToElbowRightAngle + mMovementLowerLimitAgainstOriginal) && 
                                    x2 > (mOriginalNeckToElbowLeftAngle - mMovementLowerLimitAgainstOriginal) && 
                                    x2 < (mOriginalNeckToElbowLeftAngle + mMovementLowerLimitAgainstOriginal) &&
                                    x3 > (mOriginalSpineBaseToHeadAngle - mMovementLowerLimitAgainstOriginal) &&
                                    x3 < (mOriginalSpineBaseToHeadAngle + mMovementLowerLimitAgainstOriginal) &&
                                    x4 > (mOriginalHeadToShoulderLeftAngle - mMovementLowerLimitAgainstOriginal) &&
                                    x4 < (mOriginalHeadToShoulderLeftAngle + mMovementLowerLimitAgainstOriginal) &&
                                    x5 > (mOriginalHeadToShoulderRightAngle - mMovementLowerLimitAgainstOriginal) &&
                                    x5 < (mOriginalHeadToShoulderRightAngle + mMovementLowerLimitAgainstOriginal) &&
                                    x6 > (mOriginalHeadToSpineShoulder - mMovementLowerLimitAgainstOriginal) &&
                                    x6 < (mOriginalHeadToSpineShoulder + mMovementLowerLimitAgainstOriginal))
                                {
                                    mSessionState = 20;
                                    mOriginalAnglesSet = false;
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

        private void SetCharts()
        {
            // Below code implements the representation on charts of angle data of the angles on all the charts
            Chart[] chartsArray = new Chart[] { chart1, chart2, chart3, chart4, chart5, chart6 };

            foreach (Chart chart in chartsArray)
            {
                chart.ChartAreas[0].AxisY.ScaleView.Zoom(-20, 20);
                chart.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
                chart.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            }
        }

        private void UpdateChart(Chart chart, int graphCounter, int x, int angle)
        {
            chart.Series[0].Points.AddXY(graphCounter, (x - angle));

            // TODO: Pretty sure the ChartType can be moved to SetCharts()
            chart.Series[0].ChartType = SeriesChartType.Line;

            chart.ChartAreas[0].AxisX.ScaleView.Position = chart.Series[0].Points.Count - chart.ChartAreas[0].AxisX.ScaleView.Size;

            if (graphCounter % 100 == 0)
            {
                chart.ChartAreas[0].AxisX.ScaleView.Zoom(0, 100);
            }

            graphCounter++;
        }

        private void UpdateAllCharts()
        {
            UpdateChart(chart1, graph_counter, x1, mNeckToElbowRightAngle);
            UpdateChart(chart2, graph_counter1, x2, mNeckToElbowLeftAngle);
            UpdateChart(chart3, graph_counter2, x3, mSpineBaseToHeadAngle);
            UpdateChart(chart4, graph_counter3, x4, mHeadToShoulderLeftAngle);
            UpdateChart(chart5, graph_counter4, x5, mHeadToShoulderRightAngle);
            UpdateChart(chart6, graph_counter5, x6, mHeadToSpineShoulder);
        }

        private void SetTimer()
        {
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000;
            timer1.Enabled = false;
        }

        private void SetOriginalAngles()
        {
            mOriginalNeckToElbowRightAngle = mNeckToElbowRightAngle;
            mOriginalNeckToElbowLeftAngle = mNeckToElbowLeftAngle;
            mOriginalSpineBaseToHeadAngle = mSpineBaseToHeadAngle;
            mOriginalHeadToShoulderLeftAngle = mHeadToShoulderLeftAngle;
            mOriginalHeadToShoulderRightAngle = mHeadToShoulderRightAngle;
            mOriginalHeadToSpineShoulder = mHeadToSpineShoulder;

            mOriginalAnglesSet = true;
        }

        private int GetMedian(int[] medianSmoothing)
        {
            int tmp;

            for (int i = 0; i < mSmoothingKernal; i++)
            {
                for (int j = i + 1; j < mSmoothingKernal; j++)
                {
                    if (medianSmoothing[j] < medianSmoothing[i])
                    {
                        tmp = medianSmoothing[i];
                        medianSmoothing[i] = medianSmoothing[j];
                        medianSmoothing[j] = tmp;
                    }
                }
            }

            if (mSmoothingKernal % 2 == 0)
            {
                return (medianSmoothing[mSmoothingKernal / 2] + medianSmoothing[mSmoothingKernal / 2 + 1]) / 2;
            }
            else
            {
                return medianSmoothing[(mSmoothingKernal + 1) / 2];
            }
        }

        private void UpdateMedianArrays()
        {
            int element = mMeanDataReadIteration - 1; // Perhaps using a parameter would be better than mMeanDataReadIteration.

            mShoulderRightMedianArray[element] = mNeckToElbowRightAngle;
            mShoulderLeftMedianArray[element] = mNeckToElbowLeftAngle;
            mSpineMidMedianArray[element] = mSpineBaseToHeadAngle;
            mNeckMedianArray[element] = mHeadToShoulderLeftAngle;
            mNeck1MedianArray[element] = mHeadToShoulderRightAngle;
            mSpineShoulderMedianArray[element] = mHeadToSpineShoulder;
        }

        private void UpdateAverageSums()
        {
            mShoulderRightAverageSum += mNeckToElbowRightAngle;
            mShoulderLeftAverageSum += mNeckToElbowLeftAngle;
            mSpineMidAverageSum += mSpineBaseToHeadAngle;
            mNeckAverageSum += mHeadToShoulderLeftAngle;
            mNeck1AverageSum += mHeadToShoulderRightAngle;
            mSpineShoulderAverageSum += mHeadToSpineShoulder;
        }

        private void UpdateMeanSums()
        {
            mShouderRightMeanSum += mNeckToElbowRightAngle;
            mShoulderLeftMeanSum += mNeckToElbowLeftAngle;
            mSpineMidMeanSum += mSpineBaseToHeadAngle;
            mNeckMeanSum += mHeadToShoulderLeftAngle;
            mNeck1MeanSum += mHeadToShoulderRightAngle;
            mSpineShoulderMeanSum += mHeadToSpineShoulder;
        }

        private void ResetAverageSums()
        {
            mShoulderRightAverageSum = 0;
            mShoulderLeftAverageSum = 0;
            mSpineMidAverageSum = 0;
            mNeckAverageSum = 0;
            mNeck1AverageSum = 0;
            mSpineShoulderAverageSum = 0;
        }

        private void ResetMeanSums()
        {
            mShouderRightMeanSum = 0;
            mShoulderLeftMeanSum = 0;
            mSpineMidMeanSum = 0;
            mNeckMeanSum = 0;
            mNeck1MeanSum = 0;
            mSpineShoulderMeanSum = 0;
        }

        private void SetAverages()
        {
            mShoulderRightAverage = mShoulderRightAverageSum / mAverageDataReadIteration;
            mShoulderLeftAverage = mShoulderLeftAverageSum / mAverageDataReadIteration;
            mSpineMidAverage = mSpineMidAverageSum / mAverageDataReadIteration;
            mNeckAverage = mNeckAverageSum / mAverageDataReadIteration;
            mNeck1Average = mNeck1AverageSum / mAverageDataReadIteration;
            mSpineShoulderAverage = mSpineShoulderAverageSum / mAverageDataReadIteration;
        }

        private void SetMeans()
        {
            mShoulderRightMean = mShouderRightMeanSum / mSmoothingKernal;
            mShoulderLeftMean = mShoulderLeftMeanSum / mSmoothingKernal;
            mSpineMidMean = mSpineMidMeanSum / mSmoothingKernal;
            mNeckMean = mNeckMeanSum / mSmoothingKernal;
            mNeck1Mean = mNeck1MeanSum / mSmoothingKernal;
            mSpineShoulderMean = mSpineShoulderMeanSum / mSmoothingKernal;
        }

        private void SetMedians()
        {
            mShoulderRightMedian = GetMedian(mShoulderRightMedianArray);
            mShoulderLeftMedian = GetMedian(mShoulderLeftMedianArray);
            mSpineMidMedian = GetMedian(mSpineMidMedianArray);
            mNeckMedian = GetMedian(mNeckMedianArray);
            mNeck1Median = GetMedian(mNeck1MedianArray);
            mSpineShoulderMedian = GetMedian(mSpineShoulderMedianArray);
        }

        private void UpdateLabelColors()
        {
            if (mShoulderRightMean > (x1 + mNotAllowed) || mShoulderRightMean < (x1 - mNotAllowed))
            {
                shoulderRightLabel.ForeColor = Color.Red;
            }

            if (mShoulderLeftMean > (x2 + mNotAllowed) || mShoulderLeftMean < (x2 - mNotAllowed))
            {
                shoulderLeftLabel.ForeColor = Color.Red;
            }

            if (mSpineMidMean > (x3 + mNotAllowed) || mSpineMidMean < (x3 - mNotAllowed))
            {
                spineMidLabel.ForeColor = Color.Red;
            }

            if (mNeckMean > (x4 + mNotAllowed) || mNeckMean < (x4 - mNotAllowed))
            {
                neckLabel.ForeColor = Color.Red;
            }

            if (mNeck1Mean > (x5 + mNotAllowed) || mNeck1Mean < (x5 - mNotAllowed))
            {
                neck1Label.ForeColor = Color.Red;
            }

            if (mSpineShoulderMean > (x6 + mNotAllowed) || mSpineShoulderMean < (x6 - mNotAllowed))
            {
                spineShoulderLabel.ForeColor = Color.Red;
            }
        }

        private void ResetLabelColors()
        {
            shoulderRightLabel.ForeColor = Color.Black;
            shoulderLeftLabel.ForeColor = Color.Black;
            spineMidLabel.ForeColor = Color.Black;
            neckLabel.ForeColor = Color.Black;
            neck1Label.ForeColor = Color.Black;
            spineShoulderLabel.ForeColor = Color.Black;
        }

        private void FinalForm_Load(object sender, EventArgs e)
        {            
            if (mSessionState == 22)
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
                mTagType = 1;
                average_tagged = 1;
                average_tagged1 = 1;
            }

            // For medium tagged movement press m
            if (e.KeyChar == 'm')
            {
                e.Handled = true;
                mTagType = 2;
                average_tagged = 1;
                average_tagged1 = 1;
            }

            // For large tagged movement press l
            if (e.KeyChar == 'l')
            {
                mTagType = 3;
                average_tagged = 1;
                average_tagged1 = 1;
            }

            // To start the process press r
            if (e.KeyChar == 'r')
            {
                mSessionState = 22;

                startButton.BackColor = Color.DeepSkyBlue;
            }
        }
    }
}
