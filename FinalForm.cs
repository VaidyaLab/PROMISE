using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Final_Kinect
{
    public partial class FinalForm : Form
    {
        // Initializing Kinect Sensor
        KinectSensor mKinectSensor = null;

        // Body frame reader used to detect body by kinect
        MultiSourceFrameReader mBodyframeReader = null;

        Body[] mBody = null;

        // This is the form on which the participant will view the movie.
        SubjectMovieForm mSubjectMovieForm;

        Stopwatch mStopwatch = new Stopwatch();

        // This is being used to determine if session is started or stopped, but in some ways that I need to look further into.
        int mSessionState = 0;

        string mCurrentDate,
            mFilePath,
            mSubjectInitials,
            mExperimentNumber;

        StreamWriter rawCsvFile,
            videoCsvFile,
            meanCsvFile,
            medianCsvFile,
            differenceCsvFile,
            amplitdueCsvFile;

        int mStartedOriginalShoulderRight,
            mStartedOriginalShoulderLeft,
            mStartedOriginalSpineMid,
            mStartedOriginalNeck,
            mStartedOriginalNeck1,
            mStartedOriginalSpineShoulder;

        int mNeckToElbowRightAngle,
            mNeckToElbowLeftAngle,
            mSpineBaseToHeadAngle,
            mHeadToShoulderLeftAngle,
            mHeadToShoulderRightAngle,
            mHeadToSpineShoulder;

        int graph_counter = 0,
            graph_counter1 = 0,
            graph_counter2 = 0,
            graph_counter3 = 0,
            graph_counter4 = 0,
            graph_counter5 = 0;

        bool mStarted = false,
            mOriginalAnglesSet = false;

        int mShouderRightMeanSum = 0,
            mShoulderLeftMeanSum = 0,
            mSpineMidMeanSum = 0,
            mNeckMeanSum = 0,
            mNeck1MeanSum = 0,
            mSpineShoulderMeanSum = 0;

        int mOriginalNeckToElbowRightAngle = 0,
            mOriginalNeckToElbowLeftAngle = 0,
            mOriginalSpineBaseToHeadAngle = 0,
            mOriginalHeadToShoulderLeftAngle = 0,
            mOriginalHeadToShoulderRightAngle = 0,
            mOriginalHeadToSpineShoulder = 0;

        int mDepthFrameReference,
            mJointTrackingType,
            mMarkTagged = 0,
            mTagType = 0,
            mTempArrayElementIteration = 0,
            mSmoothingKernal,
            mWarning,
            mNotAllowed;

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

        // This should probably be set to zero and iterate earlier down below (if we ultimately need this at all).
        int mMeanDataReadIteration = 1;

        // Will store values in array, and then will find median of the array.
        int[] mShoulderRightMedianArray,
            mShoulderLeftMedianArray,
            mSpineMidMedianArray,
            mNeckMedianArray,
            mNeck1MedianArray,
            mSpineShoulderMedianArray;

        // Will store the median, which will be obtained with the help of the MedianArrays.
        int mShoulderRightMedian,
            mShoulderLeftMedian,
            mSpineMidMedian,
            mNeckMedian,
            mNeck1Median,
            mSpineShoulderMedian;

        // These are used to determine the elapsed time of the session.
        DateTime mStartTime = new DateTime();
        TimeSpan mElapsedTime = new TimeSpan();

        /* This will be incremented on every tick. When the tick interval
         * is set for 1000, then mTickCount % 60 will be 0 and thus allow
         * us to perform an action every 1 second - specifically, increment
         * the progress bar.
         */
        int mTickCount = 0;

        // These two are only used in Raw measures updates.
        int mOverWarning = 0;
        int mOverNotAllowed = 0;

        // These are also used in the Raw measures updates.
        int[] temp1 = new int[2],
            temp2 = new int[2],
            temp3 = new int[2],
            temp4 = new int[2],
            temp5 = new int[2],
            temp6 = new int[2],
            temp7 = new int[2],
            temp8 = new int[2];

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

            // Creates int arrays based on the size of the smoothing kernal specified in the SettingsForm.
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

            // Hide controls if specified in SettingsForm to be hidden
            HideControls(movieFrame, progressFrame, trafficFrame, traffic);

            mCurrentDate = DateTime.Now.ToString("yyyyMMdd");
            mFilePath = filePath;

            axWindowsMediaPlayer1.Ctlcontrols.stop();

            this.KeyPreview = true;

            startButton.BackColor = Color.Red;
            mStartTime = DateTime.Now;

            SetTimer();

            InitializeDataFiles();

            mSubjectMovieForm = new SubjectMovieForm(
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

            InitializeKinect();
        }

        // Do we need amplitude and difference files? If so, ought their creation be moved out of here and with rest?
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

        public void InitializeKinect()
        {
            mKinectSensor = KinectSensor.GetDefault();

            if (mKinectSensor != null)
            {
                // Turn on kinect
                mKinectSensor.Open();
            }

            // We are using kinect camera as well as body detection so here we have used MultiSourceFrameReader
            mBodyframeReader = mKinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);

            if (mBodyframeReader != null)
            {
                mBodyframeReader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }

            // What if already showing?
            mSubjectMovieForm.Show();
        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            bool dataReceived = false;
            var reference = e.FrameReference.AcquireFrame();

            // Get body data
            using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (mBody == null)
                    {
                        /* TODO: Should only detect one body and provide message if
                         * more than one body is detected. Then we can not use foreach
                         * below. Also. mBody seems like it can be a local variable.
                         * Also. Is this creating a new body every time? Would that cause
                         * and issue with trying to track fine movements?
                         */
                        mBody = new Body[bodyFrame.BodyFrameSource.BodyCount];
                    }
                    bodyFrame.GetAndRefreshBodyData(mBody);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                foreach (Body body in mBody)
                {
                    // Process if the body has been detected
                    if (body.IsTracked)
                    {
                        if (mSessionState == 21)
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

                        mSubjectMovieForm.transfer_values(
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
                            startButton.BackColor = Color.DeepSkyBlue;
                            mStopwatch.Start(); // May need to add if (stopWatch.Started == false) check or something
                            elapsedTimeTextBox.Text = mStopwatch.Elapsed.ToString();
                            timer1.Enabled = true; // May need to add if (timer1.Enabled == false) check or something.

                            mNeckToElbowRightAngle = (int) neckToElbowRightAngle;
                            mNeckToElbowLeftAngle = (int) neckToElbowLeftAngle;
                            mSpineBaseToHeadAngle = (int) spineBaseToHeadAngle;
                            mHeadToShoulderLeftAngle = (int) headToShoulderLeftAngle;
                            mHeadToShoulderRightAngle = (int) headToShoulderRightAngle;
                            mHeadToSpineShoulder = (int) headToSpineShoulder;

                            SetHeadOrTorso();

                            if (!mOriginalAnglesSet)
                            {
                                SetOriginalAngles();
                            }

                            UpdateMedianArrays();
                            UpdateMeanSums();

                            // Median and mean logic
                            if (mMeanDataReadIteration == mSmoothingKernal)
                            {                     
                                if (mMedians == 1)
                                {
                                    MediansUpdates();
                                }
                                else if (mMeans == 1)
                                {
                                    // TODO: Check to make sure Tag is set appropriately
                                    MeansUpdates();
                                }

                                mMeanDataReadIteration = 0;                               
                                UpdateAllCharts();
                            }

                            // mMeanDataReadIteration was reset in the above conditional
                            mMeanDataReadIteration++;

                            if (mRawMeasures == 1)
                            {
                                RawUpdates();
                            }

                            ResetTag();
                        }
                        // If not started
                        else // If 20 or 21
                        {
                            // We refer these angle values as the original position and compare with obtained angle values
                            mStartedOriginalShoulderRight = (int) neckToElbowRightAngle;
                            mStartedOriginalShoulderLeft = (int) neckToElbowLeftAngle;
                            mStartedOriginalSpineMid = (int) spineBaseToHeadAngle;
                            mStartedOriginalNeck = (int) headToShoulderLeftAngle;
                            mStartedOriginalNeck1 = (int) headToShoulderRightAngle;
                            mStartedOriginalSpineShoulder = (int) headToSpineShoulder;

                            /* This might indicate that the Original angles are set and are checking that movement hasn't
                             * occurred since. Otherwise, Change the session state and set mOriginalAnglesSet to false.
                             * A session state of 20 might prompt user to start process over.
                             * 
                             * Actually, I think state 21 indicates paused. And if user has moved too much, then we need to
                             * start over.
                             * 
                             * Still not sure. If it is set to 20, there is no way for it to get set to anything else except
                             * by clicking start or stop.
                             */
                            if (mSessionState == 21)
                            {
                                if (mStartedOriginalShoulderRight > (mOriginalNeckToElbowRightAngle - mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalShoulderRight < (mOriginalNeckToElbowRightAngle + mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalShoulderLeft > (mOriginalNeckToElbowLeftAngle - mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalShoulderLeft < (mOriginalNeckToElbowLeftAngle + mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalSpineMid > (mOriginalSpineBaseToHeadAngle - mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalSpineMid < (mOriginalSpineBaseToHeadAngle + mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalNeck > (mOriginalHeadToShoulderLeftAngle - mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalNeck < (mOriginalHeadToShoulderLeftAngle + mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalNeck1 > (mOriginalHeadToShoulderRightAngle - mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalNeck1 < (mOriginalHeadToShoulderRightAngle + mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalSpineShoulder > (mOriginalHeadToSpineShoulder - mMovementLowerLimitAgainstOriginal) &&
                                    mStartedOriginalSpineShoulder < (mOriginalHeadToSpineShoulder + mMovementLowerLimitAgainstOriginal))
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

        private String GetCsvPath(String fileType)
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
            UpdateChart(chart1, graph_counter, mStartedOriginalShoulderRight, mNeckToElbowRightAngle);
            UpdateChart(chart2, graph_counter1, mStartedOriginalShoulderLeft, mNeckToElbowLeftAngle);
            UpdateChart(chart3, graph_counter2, mStartedOriginalSpineMid, mSpineBaseToHeadAngle);
            UpdateChart(chart4, graph_counter3, mStartedOriginalNeck, mHeadToShoulderLeftAngle);
            UpdateChart(chart5, graph_counter4, mStartedOriginalNeck1, mHeadToShoulderRightAngle);
            UpdateChart(chart6, graph_counter5, mStartedOriginalSpineShoulder, mHeadToSpineShoulder);
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

        private void UpdateMeanSums()
        {
            mShouderRightMeanSum += mNeckToElbowRightAngle;
            mShoulderLeftMeanSum += mNeckToElbowLeftAngle;
            mSpineMidMeanSum += mSpineBaseToHeadAngle;
            mNeckMeanSum += mHeadToShoulderLeftAngle;
            mNeck1MeanSum += mHeadToShoulderRightAngle;
            mSpineShoulderMeanSum += mHeadToSpineShoulder;
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
            if (mShoulderRightMean > (mStartedOriginalShoulderRight + mNotAllowed) || mShoulderRightMean < (mStartedOriginalShoulderRight - mNotAllowed))
            {
                shoulderRightLabel.ForeColor = Color.Red;
            }

            if (mShoulderLeftMean > (mStartedOriginalShoulderLeft + mNotAllowed) || mShoulderLeftMean < (mStartedOriginalShoulderLeft - mNotAllowed))
            {
                shoulderLeftLabel.ForeColor = Color.Red;
            }

            if (mSpineMidMean > (mStartedOriginalSpineMid + mNotAllowed) || mSpineMidMean < (mStartedOriginalSpineMid - mNotAllowed))
            {
                spineMidLabel.ForeColor = Color.Red;
            }

            if (mNeckMean > (mStartedOriginalNeck + mNotAllowed) || mNeckMean < (mStartedOriginalNeck - mNotAllowed))
            {
                neckLabel.ForeColor = Color.Red;
            }

            if (mNeck1Mean > (mStartedOriginalNeck1 + mNotAllowed) || mNeck1Mean < (mStartedOriginalNeck1 - mNotAllowed))
            {
                neck1Label.ForeColor = Color.Red;
            }

            if (mSpineShoulderMean > (mStartedOriginalSpineShoulder + mNotAllowed) || mSpineShoulderMean < (mStartedOriginalSpineShoulder - mNotAllowed))
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

        private void InitializeDataFiles()
        {
            rawCsvFile = new StreamWriter(GetCsvPath("raw"), true);
            videoCsvFile = new StreamWriter(GetCsvPath("video"), true);
            meanCsvFile = new StreamWriter(GetCsvPath("mean"), true);
            medianCsvFile = new StreamWriter(GetCsvPath("median"), true);
            differenceCsvFile = new StreamWriter(GetCsvPath("difference"), true);
            amplitdueCsvFile = new StreamWriter(GetCsvPath("amplitude"), true);

            rawCsvFile.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            videoCsvFile.WriteLine("ElapsedTime" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck");
            meanCsvFile.WriteLine("Date&Time" + "," + "Position" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
            medianCsvFile.WriteLine("Date&Time" + "," + "ShoulderRight" + "," + "ShoulderLeft" + "," + "SpineMid" + "," + "Neck" + "," + "Neck" + "," + "Neck" + "," + "Movement");
        }

        private void MeanRedLightUpdate()
        {
            // Check for and update any labels that need to be turned red.
            UpdateLabelColors(); // diff

            double final_amplitude = GetAmplitude();

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
                        // What if already playing?
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                    }
                }

                if (mProgressSensitive == 1)
                {
                    timer1.Enabled = false;
                }
                else
                {
                    // What if already enabled?
                    timer1.Enabled = true;
                }

                if (mTrafficSensitive == 1)
                {
                    redLightPictureBox.Visible = true;
                    yellowLightPictureBox.Visible = false;
                    greenLightPictureBox.Visible = false;
                    mStopwatch.Stop(); // diff
                    mSessionState = 21;
                }
                else
                {
                    // What if already visible?
                    greenLightPictureBox.Visible = true;
                }
            }

            differenceCsvFile.WriteLine((mStartedOriginalShoulderRight - mNeckToElbowRightAngle).ToString() + "," + (mStartedOriginalShoulderLeft - mNeckToElbowLeftAngle).ToString() + "," + (mStartedOriginalSpineMid - mSpineBaseToHeadAngle).ToString() + "," + (mStartedOriginalNeck - mHeadToShoulderLeftAngle).ToString() + "," + (mStartedOriginalNeck1 - mHeadToShoulderRightAngle).ToString() + "," + (mStartedOriginalSpineShoulder - mHeadToSpineShoulder).ToString() + (mMarkTagged == 1 ? ",Tagged" : ""));
            differenceCsvFile.WriteLine(" " + "," + " " + "," + " " + "," + " " + "," + " " + "," + " ");
            amplitdueCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);
            amplitdueCsvFile.WriteLine(" " + " ");
            meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Large Movement" + "," + mShoulderRightMean + "," + mShoulderLeftMean + "," + mSpineMidMean + "," + mNeckMean + "," + mNeck1Mean + "," + mSpineShoulderMean + (mMarkTagged == 1 ? ",Tagged" : ""));
            
        }

        private void MeanYellowLightUpdate()
        {
            ResetLabelColors();

            double final_amplitude = GetAmplitude();

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

            meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Small Movement" + "," + mShoulderRightMean + "," + mShoulderLeftMean + "," + mSpineMidMean + "," + mNeckMean + "," + mNeck1Mean + "," + mSpineShoulderMean + (mMarkTagged == 1 ? ",Tagged" : ""));
            differenceCsvFile.WriteLine((mStartedOriginalShoulderRight - mNeckToElbowRightAngle).ToString() + "," + (mStartedOriginalShoulderLeft - mNeckToElbowLeftAngle).ToString() + "," + (mStartedOriginalSpineMid - mSpineBaseToHeadAngle).ToString() + "," + (mStartedOriginalNeck - mHeadToShoulderLeftAngle).ToString() + "," + (mStartedOriginalNeck1 - mHeadToShoulderRightAngle).ToString() + "," + (mStartedOriginalSpineShoulder - mHeadToSpineShoulder).ToString() + (mMarkTagged == 1 ? ",Tagged" : ""));
            amplitdueCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);           
        }

        private void MeanGreenLightUpdate()
        {
            ResetLabelColors();

            double final_amplitude = GetAmplitude();

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

            meanCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + "Original" + "," + mShoulderRightMean + "," + mShoulderLeftMean + "," + mSpineMidMean + "," + mNeckMean + "," + mNeck1Mean + "," + mSpineShoulderMean + (mMarkTagged == 1 ? ",Tagged" : ""));
            differenceCsvFile.WriteLine((mStartedOriginalShoulderRight - mNeckToElbowRightAngle).ToString() + "," + (mStartedOriginalShoulderLeft - mNeckToElbowLeftAngle).ToString() + "," + (mStartedOriginalSpineMid - mSpineBaseToHeadAngle).ToString() + "," + (mStartedOriginalNeck - mHeadToShoulderLeftAngle).ToString() + "," + (mStartedOriginalNeck1 - mHeadToShoulderRightAngle).ToString() + "," + (mStartedOriginalSpineShoulder - mHeadToSpineShoulder).ToString() + (mMarkTagged == 1 ? ",Tagged" : ""));
            amplitdueCsvFile.WriteLine(mElapsedTime.ToString(@"hh\:mm\:ss") + "," + final_amplitude);            
        }

        private void MedianRedLightUpdate()
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

            medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightMedian + "," + mShoulderLeftMedian + "," + mSpineMidMedian + "," + mNeckMedian + "," + mNeck1Median + "," + mSpineShoulderMedian + (mMarkTagged == 1 ? ",Tagged" : ""));

        }

        private void MedianYellowLightUpdate()
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

            medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightMedian + "," + mShoulderLeftMedian + "," + mSpineMidMedian + "," + mNeckMedian + "," + mNeck1Median + "," + mSpineShoulderMedian + (mMarkTagged == 1 ? ",Tagged" : ""));
        }

        private void MedianGreenLightUpdate()
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

            medianCsvFile.WriteLine(DateTime.Now.ToString("yyyy//MM//dd hh:mm:ss.fff") + "," + mShoulderRightMedian + "," + mShoulderLeftMedian + "," + mSpineMidMedian + "," + mNeckMedian + "," + mNeck1Median + "," + mSpineShoulderMedian + (mMarkTagged == 1 ? ",Tagged" : ""));
        }

        private void RawRedLightUpdate()
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

        private void RawYellowLightUpdate()
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

            if (mOverNotAllowed > 0) // Should this be mOverWarning, or is it correct the way it is?
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

        private void RawGreenLightUpdate()
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

        private bool MeanOverNotAllowed()
        {
            return (
                mShoulderRightMean > (mStartedOriginalShoulderRight + mNotAllowed) ||
                mShoulderRightMean < (mStartedOriginalShoulderRight - mNotAllowed) ||
                mShoulderLeftMean > (mStartedOriginalShoulderLeft + mNotAllowed) ||
                mShoulderLeftMean < (mStartedOriginalShoulderLeft - mNotAllowed) ||
                mSpineMidMean > (mStartedOriginalSpineMid + mNotAllowed) ||
                mSpineMidMean < (mStartedOriginalSpineMid - mNotAllowed) ||
                mNeckMean > (mStartedOriginalNeck + mNotAllowed) ||
                mNeckMean < (mStartedOriginalNeck - mNotAllowed) ||
                mNeck1Mean > (mStartedOriginalNeck1 + mNotAllowed) ||
                mNeck1Mean < (mStartedOriginalNeck1 - mNotAllowed) ||
                mSpineShoulderMean > (mStartedOriginalSpineShoulder + mNotAllowed) ||
                mSpineShoulderMean < (mStartedOriginalSpineShoulder - mNotAllowed)
            );
        }

        private bool MeanOverWarning()
        {
            return (
                mShoulderRightMean > (mStartedOriginalShoulderRight + mWarning) ||
                mShoulderRightMean < (mStartedOriginalShoulderRight - mWarning) ||
                mShoulderLeftMean > (mStartedOriginalShoulderLeft + mWarning) ||
                mShoulderLeftMean < (mStartedOriginalShoulderLeft - mWarning) ||
                mSpineMidMean > (mStartedOriginalSpineMid + mWarning) ||
                mSpineMidMean < (mStartedOriginalSpineMid - mWarning) ||
                mNeckMean > (mStartedOriginalNeck + mWarning) ||
                mNeckMean < (mStartedOriginalNeck - mWarning) ||
                mNeck1Mean > (mStartedOriginalNeck1 + mWarning) ||
                mNeck1Mean < (mStartedOriginalNeck1 - mWarning) ||
                mSpineShoulderMean > (mStartedOriginalSpineShoulder + mWarning) ||
                mSpineShoulderMean < (mStartedOriginalSpineShoulder - mWarning)
            );
        }

        private bool MedianOverNotAllowed()
        {
            return (
                mShoulderRightMedian > (mStartedOriginalShoulderRight + mNotAllowed) ||
                mShoulderRightMedian < (mStartedOriginalShoulderRight - mNotAllowed) ||
                mShoulderLeftMedian > (mStartedOriginalShoulderLeft + mNotAllowed) ||
                mShoulderLeftMedian < (mStartedOriginalShoulderLeft - mNotAllowed) ||
                mSpineMidMedian > (mStartedOriginalSpineMid + mNotAllowed) ||
                mSpineMidMedian < (mStartedOriginalSpineMid - mNotAllowed) ||
                mNeckMedian > (mStartedOriginalNeck + mNotAllowed) ||
                mNeckMedian < (mStartedOriginalNeck - mNotAllowed) ||
                mNeck1Median > (mStartedOriginalNeck1 + mNotAllowed) ||
                mNeck1Median < (mStartedOriginalNeck1 - mNotAllowed) ||
                mSpineShoulderMedian > (mStartedOriginalSpineShoulder + mNotAllowed) ||
                mSpineShoulderMedian < (mStartedOriginalSpineShoulder - mNotAllowed)
            );
        }

        private bool MedianOverWarning()
        {
            return (
                mShoulderRightMedian > (mStartedOriginalShoulderRight + mWarning) ||
                mShoulderRightMedian < (mStartedOriginalShoulderRight - mWarning) ||
                mShoulderLeftMedian > (mStartedOriginalShoulderLeft + mWarning) ||
                mShoulderLeftMedian < (mStartedOriginalShoulderLeft - mWarning) ||
                mSpineMidMedian > (mStartedOriginalSpineMid + mWarning) ||
                mSpineMidMedian < (mStartedOriginalSpineMid - mWarning) ||
                mNeckMedian > (mStartedOriginalNeck + mWarning) ||
                mNeckMedian < (mStartedOriginalNeck - mWarning) ||
                mNeck1Median > (mStartedOriginalNeck1 + mWarning) ||
                mNeck1Median < (mStartedOriginalNeck1 - mWarning) ||
                mSpineShoulderMedian > (mStartedOriginalSpineShoulder + mWarning) ||
                mSpineShoulderMedian < (mStartedOriginalSpineShoulder - mWarning)
            );
        }

        private bool RawOverNotAllowed()
        {
            return (
                mNeckToElbowRightAngle > (mStartedOriginalShoulderRight + mNotAllowed) ||
                mNeckToElbowRightAngle < (mStartedOriginalShoulderRight - mNotAllowed) ||
                mNeckToElbowLeftAngle > (mStartedOriginalShoulderLeft + mNotAllowed) ||
                mNeckToElbowLeftAngle < (mStartedOriginalShoulderLeft - mNotAllowed) ||
                mSpineBaseToHeadAngle > (mStartedOriginalSpineMid + mNotAllowed) ||
                mSpineBaseToHeadAngle < (mStartedOriginalSpineMid - mNotAllowed) ||
                mHeadToShoulderLeftAngle > (mStartedOriginalNeck + mNotAllowed) ||
                mHeadToShoulderLeftAngle < (mStartedOriginalNeck - mNotAllowed) ||
                mHeadToShoulderRightAngle > (mStartedOriginalNeck1 + mNotAllowed) ||
                mHeadToShoulderRightAngle < (mStartedOriginalNeck1 - mNotAllowed) ||
                mHeadToSpineShoulder > (mStartedOriginalSpineShoulder + mNotAllowed) ||
                mHeadToSpineShoulder < (mStartedOriginalSpineShoulder - mNotAllowed)
            );
        }

        private bool RawOverWarning()
        {
            return (
                mNeckToElbowRightAngle > (mStartedOriginalShoulderRight + mWarning) ||
                mNeckToElbowRightAngle < (mStartedOriginalShoulderRight - mWarning) ||
                mNeckToElbowLeftAngle > (mStartedOriginalShoulderLeft + mWarning) ||
                mNeckToElbowLeftAngle < (mStartedOriginalShoulderLeft - mWarning) ||
                mSpineBaseToHeadAngle > (mStartedOriginalSpineMid + mWarning) ||
                mSpineBaseToHeadAngle < (mStartedOriginalSpineMid - mWarning) ||
                mHeadToShoulderLeftAngle > (mStartedOriginalNeck + mWarning) ||
                mHeadToShoulderLeftAngle < (mStartedOriginalNeck - mWarning) ||
                mHeadToShoulderRightAngle > (mStartedOriginalNeck1 + mWarning) ||
                mHeadToShoulderRightAngle < (mStartedOriginalNeck1 - mWarning) ||
                mHeadToSpineShoulder > (mStartedOriginalSpineShoulder + mWarning) ||
                mHeadToSpineShoulder < (mStartedOriginalSpineShoulder - mWarning)
            );
        }

        private void MeansUpdates()
        {
            SetMeans();

            if (MeanOverNotAllowed())
            {
                MeanRedLightUpdate();
            }
            else if (MeanOverWarning())
            {
                MeanYellowLightUpdate();
            }
            else
            {
                MeanGreenLightUpdate();
            }

            ResetMeanSums();
        }

        private void MediansUpdates()
        {
            SetMedians();

            if (MedianOverNotAllowed())
            {
                MedianRedLightUpdate();
            }
            else if (MedianOverWarning())
            {
                MedianYellowLightUpdate();
            }
            else
            {
                MedianGreenLightUpdate();
            }
        }

        private void RawUpdates()
        {
            if (RawOverNotAllowed())
            {
                RawRedLightUpdate();
            }
            else if (RawOverWarning())
            {
                RawYellowLightUpdate();
            }
            else
            {
                RawGreenLightUpdate();
            }
        }

        private double GetAmplitude()
        {
            // The Math.Pow function could probably usefully be used here.
            double amplitude = Math.Sqrt(
                ((mStartedOriginalShoulderRight - mNeckToElbowRightAngle) * (mStartedOriginalShoulderRight - mNeckToElbowRightAngle)) +
                ((mStartedOriginalShoulderLeft - mNeckToElbowLeftAngle) * (mStartedOriginalShoulderLeft - mNeckToElbowLeftAngle)) +
                ((mStartedOriginalSpineMid - mSpineBaseToHeadAngle) * (mStartedOriginalSpineMid - mSpineBaseToHeadAngle)) +
                ((mStartedOriginalNeck - mHeadToShoulderLeftAngle) * (mStartedOriginalNeck - mHeadToShoulderLeftAngle)) +
                ((mStartedOriginalNeck1 - mHeadToShoulderRightAngle) * (mStartedOriginalNeck1 - mHeadToShoulderRightAngle)) +
                ((mStartedOriginalSpineShoulder - mHeadToSpineShoulder) * (mStartedOriginalSpineShoulder - mHeadToSpineShoulder))
            );

            double final_amplitude = (amplitude / 6);

            return final_amplitude;
        }

        private void HideControls(int movieFrame, int progressFrame, int trafficFrame, int traffic)
        {
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
        }

        private void SetHeadOrTorso()
        {
            if (mJointTrackingType == 1)
            {
                mStartedOriginalNeck = 0;
                mHeadToShoulderLeftAngle = 0;
                mStartedOriginalNeck1 = 0;
                mHeadToShoulderRightAngle = 0;
                mStartedOriginalSpineShoulder = 0;
                mHeadToSpineShoulder = 0;

                // Maybe add checks for whether or not they are already hidden? Or, move this outside of the loop.
                neckLabel.Hide();
                neck1Label.Hide();
                spineShoulderLabel.Hide();
            }
            if (mJointTrackingType == 2)
            {
                mStartedOriginalShoulderRight = 0;
                mNeckToElbowRightAngle = 0;
                mStartedOriginalShoulderLeft = 0;
                mNeckToElbowLeftAngle = 0;
                mStartedOriginalSpineMid = 0;
                mSpineBaseToHeadAngle = 0;

                shoulderRightLabel.Hide();
                shoulderLeftLabel.Hide();
                spineMidLabel.Hide();
            }
        }

        private void ResetTag()
        {
            if (mMarkTagged == 1)
            {
                mMarkTagged = 0;
            }
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
                mMarkTagged = 1;
            }

            // For medium tagged movement press m
            if (e.KeyChar == 'm')
            {
                e.Handled = true;
                mTagType = 2;
                mMarkTagged = 1;
            }

            // For large tagged movement press l
            if (e.KeyChar == 'l')
            {
                mTagType = 3;
                mMarkTagged = 1;
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
