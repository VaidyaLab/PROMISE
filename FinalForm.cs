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

        Stopwatch mSessionStopwatch = new Stopwatch();
        Stopwatch mConditionStopwatch = new Stopwatch();

        // This is being used to determine if session is started or stopped, but in some ways that I need to look further into.
        int mSessionState = 0;

        StreamWriter mDataFile;

        int mNeckToElbowRightAngle = 0,
            mNeckToElbowLeftAngle = 0,
            mSpineBaseToHeadAngle = 0,
            mHeadToShoulderLeftAngle = 0,
            mHeadToShoulderRightAngle = 0,
            mHeadToSpineShoulder = 0;

        int graph_counter = 0,
            graph_counter1 = 0,
            graph_counter2 = 0,
            graph_counter3 = 0,
            graph_counter4 = 0,
            graph_counter5 = 0;

        bool mOriginalAnglesSet = false;

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

        int mSmoothingKernal,
            mWarning,
            mNotAllowed;

        int mShoulderRightMean,
            mShoulderLeftMean,
            mSpineMidMean,
            mNeckMean,
            mNeck1Mean,
            mSpineShoulderMean,
            mMovementLowerLimitAgainstOriginal;

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

        /* This will be incremented on every tick. When the tick interval
         * is set for 1000, then mTickCount % 60 will be 0 and thus allow
         * us to perform an action every 1 second - specifically, increment
         * the progress bar.
         */
        int mTickCount = 0;

        private void instructionButton_Click(object sender, EventArgs e)
        {
            UpdateLimitsTextBox("1000", "1000");
        }
        private void noContingencyButton_Click(object sender, EventArgs e)
        {
            UpdateLimitsTextBox("1000", "1000");
        }
        private void movementProbeButton_Click(object sender, EventArgs e)
        {
            UpdateLimitsTextBox("1000", "1000");
        }
        private void shapeButton_Click(object sender, EventArgs e)
        {
            UpdateLimitsTextBox("11", "13");
        }
        private void stepUpButton_Click(object sender, EventArgs e)
        {
            lowerLimitSmallMovementTextBox.Text = (Convert.ToInt32(lowerLimitSmallMovementTextBox.Text) + 2).ToString();
            lowerLimitLargeMovementTextBox.Text = (Convert.ToInt32(lowerLimitLargeMovementTextBox.Text) + 2).ToString();
        }
        private void stepDownButton_Click(object sender, EventArgs e)
        {
            lowerLimitSmallMovementTextBox.Text = (Convert.ToInt32(lowerLimitSmallMovementTextBox.Text) - 2).ToString();
            lowerLimitLargeMovementTextBox.Text = (Convert.ToInt32(lowerLimitLargeMovementTextBox.Text) - 2).ToString();
        }
        private void scanButton_Click(object sender, EventArgs e)
        {
            mOriginalAnglesSet = false;
        }
        private void initializeButton_Click(object sender, EventArgs e)
        {
            InitializeKinect();
        }
        private void startButton_Click(object sender, EventArgs e)
        {
            mSessionState = 22;
            UpdateCurrentLimits();

            startButton.BackColor = Color.DeepSkyBlue;
        }
        private void stopButton_Click(object sender, EventArgs e)
        {
            mSessionState = 21;
            startButton.BackColor = Color.Red;
        }
        private void setLimitsButton_Click(object sender, EventArgs e)
        {
            UpdateCurrentLimits();
        }

        public FinalForm(
            string subjectInitials,
            string experimentNumber,
            string smoothingKernal,
            string smallMovementLowerLimit,
            string largeMovementLowerLimit,
            string sessionTime,
            int lowerLimitLargeMovement,
            string videoFile,
            string filePath
            )
        {
            InitializeComponent();

            SetCharts();

            mSmoothingKernal = Convert.ToInt32(smoothingKernal);

            mWarning = Convert.ToInt32(smallMovementLowerLimit);
            mNotAllowed = Convert.ToInt32(largeMovementLowerLimit);

            lowerLimitSmallMovementTextBox.Text = mWarning.ToString();
            lowerLimitLargeMovementTextBox.Text = mNotAllowed.ToString();

            mMovementLowerLimitAgainstOriginal = lowerLimitLargeMovement == 1 ? mNotAllowed : mWarning;

            // Creates int arrays based on the size of the smoothing kernal specified in the SettingsForm.
            mShoulderRightMedianArray = new int[mSmoothingKernal];
            mShoulderLeftMedianArray = new int[mSmoothingKernal];
            mSpineMidMedianArray = new int[mSmoothingKernal];
            mNeckMedianArray = new int[mSmoothingKernal];
            mNeck1MedianArray = new int[mSmoothingKernal];
            mSpineShoulderMedianArray = new int[mSmoothingKernal];

            progressBar.Maximum = (2 * Convert.ToInt32(sessionTime));
            axWindowsMediaPlayer1.URL = videoFile;

            axWindowsMediaPlayer1.Ctlcontrols.stop();

            this.KeyPreview = true;

            startButton.BackColor = Color.Red;

            InitializeDataFiles(filePath, subjectInitials, experimentNumber);

            mSubjectMovieForm = new SubjectMovieForm(videoFile, progressBar.Maximum);

            InitializeKinect();
        }

        // Timer event for progress bar
        private void timer1_Tick(object sender, EventArgs e)
        {
            mTickCount++;

            // Every second, increase progress
            if (mTickCount % 60 == 0)
            {
                progressBar.Value++;
                progressBar.Update();

                if (mSubjectMovieForm != null)
                {
                    mSubjectMovieForm.UpdateProgressBar();
                }              
            }

            if (mKinectSensor.IsAvailable == false)
            {
                initializeButton.Enabled = true;
            }
        }
        public void InitializeKinect()
        {
            mKinectSensor = KinectSensor.GetDefault();

            if (mKinectSensor != null)
            {
                // Turn on kinect
                mKinectSensor.Open();

                // We are using kinect camera as well as body detection so here we have used MultiSourceFrameReader
                mBodyframeReader = mKinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);
            }

            if (mBodyframeReader != null)
            {
                mBodyframeReader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
                initializeButton.Enabled = false;

                if (mSubjectMovieForm != null)
                {
                    mSubjectMovieForm.Show();
                }
            }
            else
            {
                initializeButton.Enabled = true;
            }
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
                        if (mSessionState == 22)
                        {
                            startButton.BackColor = Color.DeepSkyBlue;
                        }
                        else if (mSessionState == 21)
                        {
                            startButton.BackColor = Color.Red; // Start button normally starts as red, fyi.
                            MediaPlayersPause();

                            timer1.Enabled = false;
                        }
                        else if (mSessionState == 20) // This is when state was not 22, and then there was too much movement, I believe.
                        {
                            startButton.BackColor = Color.Blue;
                        }
                        else
                        {
                            startButton.BackColor = Color.Green;
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

                        mNeckToElbowRightAngle = (int) neckToElbowRightAngle;
                        mNeckToElbowLeftAngle = (int) neckToElbowLeftAngle;
                        mSpineBaseToHeadAngle = (int) spineBaseToHeadAngle;
                        mHeadToShoulderLeftAngle = (int) headToShoulderLeftAngle;
                        mHeadToShoulderRightAngle = (int) headToShoulderRightAngle;
                        mHeadToSpineShoulder = (int) headToSpineShoulder;


                        // Once the user has pressed the start button                       
                        if (mSessionState == 22) // and deep sky blue button
                        {
                            if (timer1.Enabled == false)
                            {
                                timer1.Enabled = true;
                            }

                            // Session stopwatch 
                            if (mSessionStopwatch.IsRunning == false)
                            {
                                mSessionStopwatch.Start();
                            }

                            sessionElapsedTimeTextBox.Text = mSessionStopwatch.Elapsed.ToString();

                            // Condition stopwatch
                            if (mConditionStopwatch.IsRunning == false)
                            {
                                mConditionStopwatch.Start();
                            }

                            conditionElapsedTimeTextBox.Text = mConditionStopwatch.Elapsed.ToString();

                            if (!mOriginalAnglesSet)
                            {
                                SetOriginalAngles();
                            }

                            UpdateMedianArrays();
                            UpdateMeanSums();

                            if (mMeanDataReadIteration == mSmoothingKernal)
                            {   
                                MeansUpdates();

                                mMeanDataReadIteration = 0;                               
                                UpdateAllCharts();
                            }

                            // mMeanDataReadIteration was reset in the above conditional
                            mMeanDataReadIteration++;
                        }
                        // If not started
                        else if (mSessionState == 21)
                        {
                            if (mNeckToElbowRightAngle > (mOriginalNeckToElbowRightAngle - mMovementLowerLimitAgainstOriginal) &&
                                mNeckToElbowRightAngle < (mOriginalNeckToElbowRightAngle + mMovementLowerLimitAgainstOriginal) &&
                                mNeckToElbowLeftAngle > (mOriginalNeckToElbowLeftAngle - mMovementLowerLimitAgainstOriginal) &&
                                mNeckToElbowLeftAngle < (mOriginalNeckToElbowLeftAngle + mMovementLowerLimitAgainstOriginal) &&
                                mSpineBaseToHeadAngle > (mOriginalSpineBaseToHeadAngle - mMovementLowerLimitAgainstOriginal) &&
                                mSpineBaseToHeadAngle < (mOriginalSpineBaseToHeadAngle + mMovementLowerLimitAgainstOriginal) &&
                                mHeadToShoulderLeftAngle > (mOriginalHeadToShoulderLeftAngle - mMovementLowerLimitAgainstOriginal) &&
                                mHeadToShoulderLeftAngle < (mOriginalHeadToShoulderLeftAngle + mMovementLowerLimitAgainstOriginal) &&
                                mHeadToShoulderRightAngle > (mOriginalHeadToShoulderRightAngle - mMovementLowerLimitAgainstOriginal) &&
                                mHeadToShoulderRightAngle < (mOriginalHeadToShoulderRightAngle + mMovementLowerLimitAgainstOriginal) &&
                                mHeadToSpineShoulder > (mOriginalHeadToSpineShoulder - mMovementLowerLimitAgainstOriginal) &&
                                mHeadToSpineShoulder < (mOriginalHeadToSpineShoulder + mMovementLowerLimitAgainstOriginal))
                            {
                                mSessionState = 20;
                                mOriginalAnglesSet = false;
                            }
                        }
                    }
                }
            }
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
        private void UpdateChart(Chart chart, ref int graphCounter, int x, int angle)
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
            UpdateChart(chart1, ref graph_counter, mOriginalNeckToElbowRightAngle, mNeckToElbowRightAngle);
            UpdateChart(chart2, ref graph_counter1, mOriginalNeckToElbowLeftAngle, mNeckToElbowLeftAngle);
            UpdateChart(chart3, ref graph_counter2, mOriginalSpineBaseToHeadAngle, mSpineBaseToHeadAngle);
            UpdateChart(chart4, ref graph_counter3, mOriginalHeadToShoulderLeftAngle, mHeadToShoulderLeftAngle);
            UpdateChart(chart5, ref graph_counter4, mOriginalHeadToShoulderRightAngle, mHeadToShoulderRightAngle);
            UpdateChart(chart6, ref graph_counter5, mOriginalHeadToSpineShoulder, mHeadToSpineShoulder);
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

            mDataFile.WriteLine(
                "SCAN," +
                mSessionStopwatch.Elapsed.ToString() + "," +
                mOriginalHeadToShoulderRightAngle + "," +
                mOriginalHeadToShoulderLeftAngle + "," +
                mOriginalSpineBaseToHeadAngle + "," +
                mOriginalNeckToElbowRightAngle + "," +
                mOriginalNeckToElbowLeftAngle + "," +
                mOriginalHeadToSpineShoulder
            );
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

        private void MeanRedLightUpdate() // These update functions can probably be combined into one function
        {
            MediaPlayersPause();

            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
            }

            SetAllTrafficLights(true, false, false);

            mSessionStopwatch.Stop();
            mConditionStopwatch.Stop();
            mSessionState = 21;

            UpdateDataFile("Large Movement");
        }
        private void MeanYellowLightUpdate()
        {
            if (timer1.Enabled == false)
            {
                timer1.Enabled = true;
            }

            MediaPlayersPlay();

            SetAllTrafficLights(false, true, false);

            UpdateDataFile("Small Movement");
        }
        private void MeanGreenLightUpdate()
        {
            if (timer1.Enabled == false)
            {
                timer1.Enabled = true;
            }

            MediaPlayersPlay();
                              
            SetAllTrafficLights(false, false, true);

            UpdateDataFile("");
        }

        private bool MeanOverNotAllowed()
        {
            return (
                mShoulderRightMean > (mOriginalHeadToShoulderRightAngle + mNotAllowed) ||
                mShoulderRightMean < (mOriginalHeadToShoulderRightAngle - mNotAllowed) ||
                mShoulderLeftMean > (mOriginalHeadToShoulderLeftAngle + mNotAllowed) ||
                mShoulderLeftMean < (mOriginalHeadToShoulderLeftAngle - mNotAllowed) ||
                mSpineMidMean > (mOriginalSpineBaseToHeadAngle + mNotAllowed) ||
                mSpineMidMean < (mOriginalSpineBaseToHeadAngle - mNotAllowed) ||
                mNeckMean > (mOriginalNeckToElbowRightAngle + mNotAllowed) ||
                mNeckMean < (mOriginalNeckToElbowRightAngle - mNotAllowed) ||
                mNeck1Mean > (mOriginalNeckToElbowLeftAngle + mNotAllowed) ||
                mNeck1Mean < (mOriginalNeckToElbowLeftAngle - mNotAllowed) ||
                mSpineShoulderMean > (mOriginalHeadToSpineShoulder + mNotAllowed) ||
                mSpineShoulderMean < (mOriginalHeadToSpineShoulder - mNotAllowed)
            );
        }
        private bool MeanOverWarning()
        {
            return (
                mShoulderRightMean > (mOriginalHeadToShoulderRightAngle + mWarning) ||
                mShoulderRightMean < (mOriginalHeadToShoulderRightAngle - mWarning) ||
                mShoulderLeftMean > (mOriginalHeadToShoulderLeftAngle + mWarning) ||
                mShoulderLeftMean < (mOriginalHeadToShoulderLeftAngle - mWarning) ||
                mSpineMidMean > (mOriginalSpineBaseToHeadAngle + mWarning) ||
                mSpineMidMean < (mOriginalSpineBaseToHeadAngle - mWarning) ||
                mNeckMean > (mOriginalNeckToElbowRightAngle + mWarning) ||
                mNeckMean < (mOriginalNeckToElbowRightAngle - mWarning) ||
                mNeck1Mean > (mOriginalNeckToElbowLeftAngle + mWarning) ||
                mNeck1Mean < (mOriginalNeckToElbowLeftAngle - mWarning) ||
                mSpineShoulderMean > (mOriginalHeadToSpineShoulder + mWarning) ||
                mSpineShoulderMean < (mOriginalHeadToSpineShoulder - mWarning)
            );
        }

        private void MeansUpdates()
        {
            SetMeans();
            SetMedians();

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

        private void UpdateLimitsTextBox(String warning, String notAllowed)
        {
            lowerLimitSmallMovementTextBox.Text = warning;
            lowerLimitLargeMovementTextBox.Text = notAllowed;
        }
        private void UpdateCurrentLimits()
        {
            mDataFile.WriteLine("Contingency Change," + mSessionStopwatch.Elapsed.ToString() + "," + lowerLimitSmallMovementTextBox.Text + "," + lowerLimitLargeMovementTextBox.Text);

            mWarning = Convert.ToInt32(lowerLimitSmallMovementTextBox.Text);
            mNotAllowed = Convert.ToInt32(lowerLimitLargeMovementTextBox.Text);

            mConditionStopwatch.Restart();

            UpdateCurrentLimitsLabel();
        }
        private void UpdateCurrentLimitsLabel()
        {
            currentLimitsLabel.Text = "Current Limits: Warning - " + mWarning.ToString() + "  Not Allowed - " + mNotAllowed.ToString();
        }

        private void InitializeDataFiles(String filePath, String subjectInitials, String experimentNumber)
        {
            mDataFile = new StreamWriter(
                Path.Combine(filePath + "\\" + subjectInitials + "-" + experimentNumber + "-" + DateTime.Now.ToString("yyyyMMdd") + "-" + "data.csv"),
                true
            );

            mDataFile.WriteLine("Subject Initials" + "," + "Experiment No" + "," + "Smoothing Kernel" + "," + "Small Movement" + "," + "Large Movement\r\n");
            mDataFile.WriteLine(subjectInitials + "," + experimentNumber + "," + mSmoothingKernal + "," + mWarning + "," + mNotAllowed);

            mDataFile.WriteLine(
                "Event," +
                "Timestamp," +
                "MeanShoulderRight,MeanShoulderLeft,MeanSpineMid,MeanNeckLeft,MeanNeckRight,MeanSpineShoulder,," +
                "MeanDiff,,,,,,,," +
                "MedianShoulderRight,MedianShoulderLeft,MedianSpineMid,MedianNeckLeft,MedianNeckRight,MedianSpineShoulder,," +
                "MedianDiff,,,,,,,," +
                "RawShoulderRight,RawShoulderLeft,RawSpineMid,RawNeckLeft,RawNeckRight,RawSpineShoulder,," +
                "RawDiff"
            );
        }
        private void UpdateDataFile(String sessionEvent)
        {
            mDataFile.WriteLine(
                sessionEvent + "," +
                // Mean
                mSessionStopwatch.Elapsed.ToString() + "," +
                mShoulderRightMean + "," +
                mShoulderLeftMean + "," +
                mSpineMidMean + "," +
                mNeckMean + "," +
                mNeck1Mean + "," +
                mSpineShoulderMean + ",," +
                // Mean Diff
                (mOriginalHeadToShoulderRightAngle - mShoulderRightMean).ToString() + "," +
                (mOriginalHeadToShoulderLeftAngle - mShoulderLeftMean).ToString() + "," +
                (mOriginalSpineBaseToHeadAngle - mSpineMidMean).ToString() + "," +
                (mOriginalNeckToElbowRightAngle - mNeckMean).ToString() + "," +
                (mOriginalNeckToElbowLeftAngle - mNeck1Mean).ToString() + "," +
                (mOriginalHeadToSpineShoulder - mSpineShoulderMean).ToString() + ",," +
                // Median
                mShoulderRightMedian + "," +
                mShoulderLeftMedian + "," +
                mSpineMidMedian + "," +
                mNeckMedian + "," +
                mNeck1Median + "," +
                mSpineShoulderMedian + ",," +
                // Median Diff
                (mOriginalHeadToShoulderRightAngle - mShoulderRightMedian).ToString() + "," +
                (mOriginalHeadToShoulderLeftAngle - mShoulderLeftMedian).ToString() + "," +
                (mOriginalSpineBaseToHeadAngle - mSpineMidMedian).ToString() + "," +
                (mOriginalNeckToElbowRightAngle - mNeckMedian).ToString() + "," +
                (mOriginalNeckToElbowLeftAngle - mNeck1Median).ToString() + "," +
                (mOriginalHeadToSpineShoulder - mSpineShoulderMedian).ToString() + ",," +
                // Raw
                mNeckToElbowRightAngle + "," +
                mNeckToElbowLeftAngle + "," +
                mSpineBaseToHeadAngle + "," +
                mHeadToShoulderLeftAngle + "," +
                mHeadToShoulderRightAngle + "," +
                mHeadToSpineShoulder + "," +
                // Raw Diff
                (mOriginalHeadToShoulderRightAngle - mNeckToElbowRightAngle).ToString() + "," +
                (mOriginalHeadToShoulderLeftAngle - mNeckToElbowLeftAngle).ToString() + "," +
                (mOriginalSpineBaseToHeadAngle - mSpineBaseToHeadAngle).ToString() + "," +
                (mOriginalNeckToElbowRightAngle - mHeadToShoulderLeftAngle).ToString() + "," +
                (mOriginalNeckToElbowLeftAngle - mHeadToShoulderRightAngle).ToString() + "," +
                (mOriginalHeadToSpineShoulder - mHeadToSpineShoulder).ToString()
            );
        }

        private void MediaPlayersPlay()
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();

            // Play the participant's movie as well
            if (mSubjectMovieForm != null)
            {
                mSubjectMovieForm.MediaPlayerPlay();
            }
        }
        private void MediaPlayersPause()
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();

            // Pause participant's movie as well.
            if (mSubjectMovieForm != null)
            {
                mSubjectMovieForm.MediaPlayerPause();
            }
        }

        private void SetAllTrafficLights(bool redVisible, bool yellowVisible, bool greenVisible)
        {
            redLightPictureBox.Visible = redVisible;
            yellowLightPictureBox.Visible = yellowVisible;
            greenLightPictureBox.Visible = greenVisible;

            // Change the participant's lights as well
            if (mSubjectMovieForm != null)
            {
                mSubjectMovieForm.SetRedLightVisible(redVisible);
                mSubjectMovieForm.SetYellowLightVisible(yellowVisible);
                mSubjectMovieForm.SetGreenLightVisible(greenVisible);
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
            // Write notes to the end of the median file
            mDataFile.WriteLine("\r\nNotes:\r\n\r\n" + notesTextBox.Text);

            // Close all data files when form closes
            mDataFile.Close();
            mSubjectMovieForm.Close();
        }
        private void FinalForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
