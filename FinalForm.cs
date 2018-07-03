using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Diagnostics;
using System.IO;

namespace Final_Kinect
{
    public partial class FinalForm : Form
    {
        #region Fields
        // Session state constants
        private const int STARTED = 22;
        private const int STOPPED = 21;
        private const int RETURNED = 20; // Returned to original position

        // Initializing Kinect Sensor
        KinectSensor mKinectSensor = null;

        // Body frame reader used to detect body by kinect
        MultiSourceFrameReader mBodyframeReader = null;

        Body[] mBody = null;
        Body[] mOriginalBody = null;

        // This is the form on which the participant will view the movie.
        SubjectMovieForm mSubjectMovieForm;

        Stopwatch mSessionStopwatch = new Stopwatch();
        Stopwatch mConditionStopwatch = new Stopwatch();

        int mSessionState = 0;

        StreamWriter mDataFile;

        bool mCurrentJointPositionsSet = false;
        bool mOriginalJointPositionsSet = false;

        CameraSpacePoint mCurrentHeadPosition,
            mCurrentNeckPosition,
            mCurrentSpineBasePosition,
            mCurrentSpineMidPosition,
            mCurrentSpineShoulderPosition,
            mCurrentShoulderLeftPosition,
            mCurrentShoulderRightPosition,
            mCurrentElbowLeftPosition,
            mCurrentElbowRightPosition;

        CameraSpacePoint mOriginalHeadPosition,
            mOriginalNeckPosition,
            mOriginalSpineBasePosition,
            mOriginalSpineMidPosition,
            mOriginalSpineShoulderPosition,
            mOriginalShoulderLeftPosition,
            mOriginalShoulderRightPosition,
            mOriginalElbowLeftPosition,
            mOriginalElbowRightPosition;

        double mHeadPositionDiff = 0,
            mNeckPositionDiff = 0,
            mSpineBasePositionDiff = 0,
            mSpineMidPositionDiff = 0,
            mSpineShoulderPositionDiff = 0,
            mShoulderLeftPositionDiff = 0,
            mShoulderRightPositionDiff = 0,
            mElbowLeftPositionDiff = 0,
            mElbowRightPositionDiff = 0;

        double mHeadPositionDiffMeanSum = 0,
            mNeckPositionDiffMeanSum = 0,
            mSpineBasePositionDiffMeanSum = 0,
            mSpineMidPositionDiffMeanSum = 0,
            mSpineShoulderPositionDiffMeanSum = 0,
            mShoulderLeftPositionDiffMeanSum = 0,
            mShoulderRightPositionDiffMeanSum = 0,
            mElbowLeftPositionDiffMeanSum = 0,
            mElbowRightPositionDiffMeanSum = 0;

        double mHeadPositionDiffMean = 0,
            mNeckPositionDiffMean = 0,
            mSpineBasePositionDiffMean = 0,
            mSpineMidPositionDiffMean = 0,
            mSpineShoulderPositionDiffMean = 0,
            mShoulderLeftPositionDiffMean = 0,
            mShoulderRightPositionDiffMean = 0,
            mElbowLeftPositionDiffMean = 0,
            mElbowRightPositionDiffMean = 0;

        int mSmoothingKernal,
            mWarning,
            mNotAllowed;

        // This should probably be set to zero and iterate earlier down below (if we ultimately need this at all).
        int mMeanDataReadIteration = 1;

        // These store particular differences in order to determine median
        double[] mHeadPositionDiffMedianArray,
            mNeckPositionDiffMedianArray,
            mSpineBasePositionDiffMedianArray,
            mSpineMidPositionDiffMedianArray,
            mSpineShoulderPositionDiffMedianArray,
            mShoulderLeftPositionDiffMedianArray,
            mShoulderRightPositionDiffMedianArray,
            mElbowLeftPositionDiffMedianArray,
            mElbowRightPositionDiffMedianArray;

        double mHeadPositionDiffMedian,
            mNeckPositionDiffMedian,
            mSpineBasePositionDiffMedian,
            mSpineMidPositionDiffMedian,
            mSpineShoulderPositionDiffMedian,
            mShoulderLeftPositionDiffMedian,
            mShoulderRightPositionDiffMedian,
            mElbowLeftPositionDiffMedian,
            mElbowRightPositionDiffMedian;

        /* This will be incremented on every tick. When the tick interval
         * is set for 1000, then mTickCount % 60 will be 0 and thus allow
         * us to perform an action every 1 second - specifically, increment
         * the progress bar.
         */
        int mTickCount = 0;
        #endregion

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

            mSmoothingKernal = Convert.ToInt32(smoothingKernal);

            mWarning = Convert.ToInt32(smallMovementLowerLimit);
            mNotAllowed = Convert.ToInt32(largeMovementLowerLimit);

            lowerLimitSmallMovementTextBox.Text = mWarning.ToString();
            lowerLimitLargeMovementTextBox.Text = mNotAllowed.ToString();

            // Creates double arrays based on the size of the smoothing kernal specified in the SettingsForm.
            mHeadPositionDiffMedianArray = new double[mSmoothingKernal];
            mNeckPositionDiffMedianArray = new double[mSmoothingKernal];
            mSpineBasePositionDiffMedianArray = new double[mSmoothingKernal];
            mSpineMidPositionDiffMedianArray = new double[mSmoothingKernal];
            mSpineShoulderPositionDiffMedianArray = new double[mSmoothingKernal];
            mShoulderLeftPositionDiffMedianArray = new double[mSmoothingKernal];
            mShoulderRightPositionDiffMedianArray = new double[mSmoothingKernal];
            mElbowLeftPositionDiffMedianArray = new double[mSmoothingKernal];
            mElbowRightPositionDiffMedianArray = new double[mSmoothingKernal];

            progressBar.Maximum = (2 * Convert.ToInt32(sessionTime));
            axWindowsMediaPlayer1.URL = videoFile;

            axWindowsMediaPlayer1.Ctlcontrols.stop();

            startButton.BackColor = Color.Red;

            InitializeDataFiles(filePath, subjectInitials, experimentNumber);

            mSubjectMovieForm = new SubjectMovieForm(videoFile, progressBar.Maximum);
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

                if (mSubjectMovieForm != null)
                {
                    mSubjectMovieForm.Show();
                }
            }

            if (startButton.Enabled == false)
            {
                startButton.Enabled = true;
            }
            if (stopButton.Enabled == false)
            {
                stopButton.Enabled = true;
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
                    if (mOriginalBody == null)
                    {
                        mOriginalBody = new Body[bodyFrame.BodyFrameSource.BodyCount];
                    }

                    bodyFrame.GetAndRefreshBodyData(mBody);

                    if (mOriginalJointPositionsSet == false)
                    {
                        bodyFrame.GetAndRefreshBodyData(mOriginalBody);
                    }

                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                bodyPictureBox.Refresh();

                foreach (Body body in mBody)
                {
                    // Process if the body has been detected
                    if (body.IsTracked)
                    {
                        SetCurrentJointPoisitions(body);
                        SetPositionDiff();

                        if (mSessionState == STARTED)
                        {
                            startButton.BackColor = Color.DeepSkyBlue;

                            UpdateStopwatches();

                            if (timer1.Enabled == false)
                            {
                                timer1.Enabled = true;
                            }

                            if (!mOriginalJointPositionsSet)
                            {
                                SetOriginalJointPositions();
                            }

                            UpdateMedianArrays();
                            UpdateMeanSums();

                            if (mMeanDataReadIteration == mSmoothingKernal)
                            {
                                MeansUpdates();

                                mMeanDataReadIteration = 0;
                            }

                            // mMeanDataReadIteration was reset in the above conditional
                            mMeanDataReadIteration++;
                        }
                        else if (mSessionState == STOPPED)
                        {
                            startButton.BackColor = Color.Red;
                            MediaPlayersPause();

                            if (timer1.Enabled)
                            {
                                timer1.Enabled = false;
                            }

                            if (!RawDiffOver(mNotAllowed))
                            {
                                mSessionState = RETURNED;
                            }
                        }
                        else if (mSessionState == RETURNED)
                        {
                            startButton.BackColor = Color.Blue;

                            if (RawDiffOver(mNotAllowed))
                            {
                                mSessionState = STOPPED;
                            }
                        }
                        else
                        {
                            startButton.BackColor = Color.Green;
                        }
                    }
                }
            }
        }

        #region Joint positions and diffs
        private void SetOriginalJointPositions()
        {
            if (mCurrentJointPositionsSet)
            {
                mOriginalHeadPosition = mCurrentHeadPosition;
                mOriginalNeckPosition = mCurrentNeckPosition;
                mOriginalSpineBasePosition = mCurrentSpineBasePosition;
                mOriginalSpineMidPosition = mCurrentSpineMidPosition;
                mOriginalSpineShoulderPosition = mCurrentSpineShoulderPosition;
                mOriginalShoulderLeftPosition = mCurrentShoulderLeftPosition;
                mOriginalShoulderRightPosition = mCurrentShoulderRightPosition;
                mOriginalElbowLeftPosition = mCurrentElbowLeftPosition;
                mOriginalElbowRightPosition = mCurrentElbowRightPosition;

                if (!mOriginalJointPositionsSet)
                {
                    mOriginalJointPositionsSet = true;
                }

                mDataFile.WriteLine("SCAN," + mSessionStopwatch.Elapsed.ToString());
            }
        }
        private void SetCurrentJointPoisitions(Body body)
        {
            // Setting current CameraSpacePoints
            mCurrentHeadPosition = body.Joints[JointType.Head].Position;
            mCurrentNeckPosition = body.Joints[JointType.Neck].Position;
            mCurrentSpineBasePosition = body.Joints[JointType.SpineBase].Position;
            mCurrentSpineMidPosition = body.Joints[JointType.SpineMid].Position;
            mCurrentSpineShoulderPosition = body.Joints[JointType.SpineShoulder].Position;
            mCurrentShoulderLeftPosition = body.Joints[JointType.ShoulderLeft].Position;
            mCurrentShoulderRightPosition = body.Joints[JointType.ShoulderRight].Position;
            mCurrentElbowLeftPosition = body.Joints[JointType.ElbowLeft].Position;
            mCurrentElbowRightPosition = body.Joints[JointType.ElbowRight].Position;

            if (!mCurrentJointPositionsSet)
            {
                mCurrentJointPositionsSet = true;
            }
        }
        private void SetPositionDiff()
        {
            if (mOriginalJointPositionsSet)
            {
                mHeadPositionDiff = MathExtensions.Length(mCurrentHeadPosition, mOriginalHeadPosition) * 1000;
                mNeckPositionDiff = MathExtensions.Length(mCurrentNeckPosition, mOriginalNeckPosition) * 1000;
                mSpineBasePositionDiff = MathExtensions.Length(mCurrentSpineBasePosition, mOriginalSpineBasePosition) * 1000;
                mSpineMidPositionDiff = MathExtensions.Length(mCurrentSpineMidPosition, mOriginalSpineMidPosition) * 1000;
                mSpineShoulderPositionDiff = MathExtensions.Length(mCurrentSpineShoulderPosition, mOriginalSpineShoulderPosition) * 1000;
                mShoulderLeftPositionDiff = MathExtensions.Length(mCurrentShoulderLeftPosition, mOriginalShoulderLeftPosition) * 1000;
                mShoulderRightPositionDiff = MathExtensions.Length(mCurrentShoulderRightPosition, mOriginalShoulderRightPosition) * 1000;
                mElbowLeftPositionDiff = MathExtensions.Length(mCurrentElbowLeftPosition, mOriginalElbowLeftPosition) * 1000;
                mElbowRightPositionDiff = MathExtensions.Length(mCurrentElbowRightPosition, mOriginalElbowRightPosition) * 1000;

                headTextBox.Text = ((int)mHeadPositionDiff).ToString();
                neckTextBox.Text = ((int)mNeckPositionDiff).ToString();
                spineBaseTextBox.Text = ((int)mSpineBasePositionDiff).ToString();
                spineMidTextBox.Text = ((int)mSpineMidPositionDiff).ToString();
                spineShoulderTextBox.Text = ((int)mSpineShoulderPositionDiff).ToString();
                shoulderLeftTextBox.Text = ((int)mShoulderLeftPositionDiff).ToString();
                shoulderRightTextBox.Text = ((int)mShoulderRightPositionDiff).ToString();
                elbowLeftTextBox.Text = ((int)mElbowLeftPositionDiff).ToString();
                elbowRightTextBox.Text = ((int)mElbowRightPositionDiff).ToString();
            }

        }
        #endregion

        private bool RawDiffOver(int limit)
        {
            return (
                Math.Abs(mHeadPositionDiff) > limit ||
                Math.Abs(mNeckPositionDiff) > limit ||
                Math.Abs(mSpineBasePositionDiff) > limit ||
                Math.Abs(mSpineMidPositionDiff) > limit ||
                Math.Abs(mSpineShoulderPositionDiff) > limit ||
                Math.Abs(mShoulderLeftPositionDiff) > limit ||
                Math.Abs(mShoulderRightPositionDiff) > limit //||
            //    Math.Abs(mElbowLeftPositionDiff) > limit ||
             //   Math.Abs(mElbowLeftPositionDiff) > limit
            );
        }

        #region Median functions
        private void SetMedians()
        {
            mHeadPositionDiffMedian = GetMedian(mHeadPositionDiffMedianArray);
            mNeckPositionDiffMedian = GetMedian(mNeckPositionDiffMedianArray);
            mSpineBasePositionDiffMedian = GetMedian(mSpineBasePositionDiffMedianArray);
            mSpineMidPositionDiffMedian = GetMedian(mSpineMidPositionDiffMedianArray);
            mSpineShoulderPositionDiffMedian = GetMedian(mSpineShoulderPositionDiffMedianArray);
            mShoulderLeftPositionDiffMedian = GetMedian(mShoulderLeftPositionDiffMedianArray);
            mShoulderRightPositionDiffMedian = GetMedian(mShoulderRightPositionDiffMedianArray);
            mElbowLeftPositionDiffMedian = GetMedian(mElbowLeftPositionDiffMedianArray);
            mElbowRightPositionDiffMedian = GetMedian(mElbowRightPositionDiffMedianArray);
        }
        private double GetMedian(double[] medianSmoothing)
        {
            double tmp;

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
             //   double returnVal = (medianSmoothing[mSmoothingKernal / 2 - 1] + medianSmoothing[mSmoothingKernal / 2]) / 2;
                return (medianSmoothing[mSmoothingKernal / 2 - 1] + medianSmoothing[mSmoothingKernal / 2]) / 2;
            }
            else
            {
                return medianSmoothing[(mSmoothingKernal/ 2)];
            }
        }
        private void UpdateMedianArrays()
        {
            int element = mMeanDataReadIteration - 1; // Perhaps using a parameter would be better than mMeanDataReadIteration.

            mHeadPositionDiffMedianArray[element] = mHeadPositionDiff;
            mNeckPositionDiffMedianArray[element] = mNeckPositionDiff;
            mSpineBasePositionDiffMedianArray[element] = mSpineBasePositionDiff;
            mSpineMidPositionDiffMedianArray[element] = mSpineMidPositionDiff;
            mSpineShoulderPositionDiffMedianArray[element] = mSpineShoulderPositionDiff;
            mShoulderLeftPositionDiffMedianArray[element] = mShoulderLeftPositionDiff;
            mShoulderRightPositionDiffMedianArray[element] = mShoulderRightPositionDiff;
            mElbowLeftPositionDiffMedianArray[element] = mElbowLeftPositionDiff;
            mElbowRightPositionDiffMedianArray[element] = mElbowRightPositionDiff;
        }
        private bool MedianOver(int limit)
        {
            return (
                Math.Abs(mHeadPositionDiffMedian) > limit ||
                Math.Abs(mNeckPositionDiffMedian) > limit ||
                Math.Abs(mSpineBasePositionDiffMedian) > limit ||
                Math.Abs(mSpineMidPositionDiffMedian) > limit ||
                Math.Abs(mSpineShoulderPositionDiffMedian) > limit ||
                Math.Abs(mShoulderLeftPositionDiffMedian) > limit ||
                Math.Abs(mShoulderRightPositionDiffMedian) > limit //||
               // Math.Abs(mElbowLeftPositionDiffMedian) > limit ||
                //Math.Abs(mElbowRightPositionDiffMedian) > limit
            );
        }
        #endregion

        #region Mean functions
        private void SetMeans()
        {
            mHeadPositionDiffMean = mHeadPositionDiffMeanSum / mSmoothingKernal;
            mNeckPositionDiffMean = mNeckPositionDiffMeanSum / mSmoothingKernal;
            mSpineBasePositionDiffMean = mSpineBasePositionDiffMeanSum / mSmoothingKernal;
            mSpineMidPositionDiffMean = mSpineMidPositionDiffMeanSum / mSmoothingKernal;
            mSpineShoulderPositionDiffMean = mSpineShoulderPositionDiffMeanSum / mSmoothingKernal;
            mShoulderLeftPositionDiffMean = mShoulderLeftPositionDiffMeanSum / mSmoothingKernal;
            mShoulderRightPositionDiffMean = mShoulderRightPositionDiffMeanSum / mSmoothingKernal;
            mElbowLeftPositionDiffMean = mElbowLeftPositionDiffMeanSum / mSmoothingKernal;
            mElbowRightPositionDiffMean = mElbowRightPositionDiffMeanSum / mSmoothingKernal;
        }
        private void UpdateMeanSums()
        {
            mHeadPositionDiffMeanSum += mHeadPositionDiff;
            mNeckPositionDiffMeanSum += mNeckPositionDiff;
            mSpineBasePositionDiffMeanSum += mSpineBasePositionDiff;
            mSpineMidPositionDiffMeanSum += mSpineMidPositionDiff;
            mSpineShoulderPositionDiffMeanSum += mSpineShoulderPositionDiff;
            mShoulderLeftPositionDiffMeanSum += mShoulderLeftPositionDiff;
            mShoulderRightPositionDiffMeanSum += mShoulderRightPositionDiff;
            mElbowLeftPositionDiffMeanSum += mElbowLeftPositionDiff;
            mElbowRightPositionDiffMeanSum += mElbowRightPositionDiff;
        }
        private void ResetMeanSums()
        {
            mHeadPositionDiffMeanSum = 0;
            mNeckPositionDiffMeanSum = 0;
            mSpineBasePositionDiffMeanSum = 0;
            mSpineMidPositionDiffMeanSum = 0;
            mSpineShoulderPositionDiffMeanSum = 0;
            mShoulderLeftPositionDiffMeanSum = 0;
            mShoulderRightPositionDiffMeanSum = 0;
            mElbowLeftPositionDiffMeanSum = 0;
            mElbowRightPositionDiffMeanSum = 0;
        }
        private void MeansUpdates()
        {
            SetMeans();
            SetMedians();

            if (MedianOver(mNotAllowed))
            {
                MeanRedLightUpdate();
            }
            else if (MedianOver(mWarning))
            {
                MeanYellowLightUpdate();
            }
            else
            {
                MeanGreenLightUpdate();
            }

            ResetMeanSums();
        }
        #endregion

        #region LightUpdates
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
        #endregion

        #region Limits
        private void UpdateLimitsTextBox(String warning, String notAllowed)
        {
            lowerLimitSmallMovementTextBox.Text = warning;
            lowerLimitLargeMovementTextBox.Text = notAllowed;
        }
        private void UpdateCurrentLimits(bool updateDateFile)
        {
            if (updateDateFile == true)
            {
                mDataFile.WriteLine("Contingency Change," + mSessionStopwatch.Elapsed.ToString() + "," + lowerLimitSmallMovementTextBox.Text + "," + lowerLimitLargeMovementTextBox.Text);
            }

            mWarning = Convert.ToInt32(lowerLimitSmallMovementTextBox.Text);
            mNotAllowed = Convert.ToInt32(lowerLimitLargeMovementTextBox.Text);

            mConditionStopwatch.Restart();

            UpdateCurrentLimitsLabel();
        }
        private void UpdateCurrentLimitsLabel()
        {
            currentLimitsLabel.Text = "Current Limits: Warning - " + mWarning.ToString() + "  Not Allowed - " + mNotAllowed.ToString();
        }
        #endregion

        private void InitializeDataFiles(String filePath, String subjectInitials, String experimentNumber)
        {
            mDataFile = new StreamWriter(
                Path.Combine(filePath + "\\" + subjectInitials + "-" + experimentNumber + "-" + DateTime.Now.ToString("yyyyMMdd") + "-" + "data.csv"),
                true
            );

            mDataFile.WriteLine("Subject Initials" + "," + "Experiment No" + "," + "Smoothing Kernel" + "," + "Small Movement" + "," + "Large Movement");
            mDataFile.WriteLine(subjectInitials + "," + experimentNumber + "," + mSmoothingKernal + "," + mWarning + "," + mNotAllowed + "\r\n");

            mDataFile.WriteLine(
                "Event," +
                "Timestamp," +
                "MeanHeadDiff,MeanNeckDiff,MeanSpineBaseDiff,MeanSpineMidDiff,MeanSpineShoulderDiff,MeanShoulderLeftDiff,MeanShoulderRightDiff,MeanElbowLeftDiff,MeanElbowRightDiff,," +
                "MedianHeadDiff,MedianNeckDiff,MedianSpineBaseDiff,MedianSpineMidDiff,MedianSpineShoulderDiff,MedianShoulderLeftDiff,MedianShoulderRightDiff,MedianElbowLeftDiff,MedianElbowRightDiff,," +
                "RawHeadDiff,RawNeckDiff,RawSpineBaseDiff,RawSpineMidDiff,RawSpineShoulderDiff,RawShoulderLeftDiff,RawShoulderRightDiff,RawElbowLeftDiff,RawElbowRightDiff"
            );
        }
        private void UpdateDataFile(String sessionEvent)
        {
            mDataFile.WriteLine(
                sessionEvent + "," +
                mSessionStopwatch.Elapsed.ToString() + "," +
                // Mean Diff
                mHeadPositionDiffMean.ToString() + "," +
                mNeckPositionDiffMean.ToString() + "," +
                mSpineBasePositionDiffMean.ToString() + "," +
                mSpineMidPositionDiffMean.ToString() + "," +
                mSpineShoulderPositionDiffMean.ToString() + "," +
                mShoulderLeftPositionDiffMean.ToString() + "," +
                mShoulderRightPositionDiffMean.ToString() + "," +
                mElbowLeftPositionDiffMean.ToString() + "," +
                mElbowRightPositionDiffMean.ToString() + ",," +
                // Median Diff
                mHeadPositionDiffMedian.ToString() + "," +
                mNeckPositionDiffMedian.ToString() + "," +
                mSpineBasePositionDiffMedian.ToString() + "," +
                mSpineMidPositionDiffMedian.ToString() + "," +
                mSpineShoulderPositionDiffMedian.ToString() + "," +
                mShoulderLeftPositionDiffMedian.ToString() + "," +
                mShoulderRightPositionDiffMedian.ToString() + "," +
                mElbowLeftPositionDiffMedian.ToString() + "," +
                mElbowRightPositionDiffMedian.ToString() + ",," +
                // Raw Diff
                mHeadPositionDiff.ToString() + "," +
                mNeckPositionDiff.ToString() + "," +
                mSpineBasePositionDiff.ToString() + "," +
                mSpineMidPositionDiff.ToString() + "," +
                mSpineShoulderPositionDiff.ToString() + "," +
                mShoulderLeftPositionDiff.ToString() + "," +
                mShoulderRightPositionDiff.ToString() + "," +
                mElbowLeftPositionDiff.ToString() + "," +
                mElbowRightPositionDiff.ToString()
            );
        }

        private void UpdateStopwatches()
        {
            // Session stopwatch 
            if (mSessionStopwatch.IsRunning == false)
            {
                mSessionStopwatch.Start();
            }

            sessionElapsedTimeTextBox.Text = mSessionStopwatch.Elapsed.ToString("mm\\:ss\\.ff");

            // Condition stopwatch
            if (mConditionStopwatch.IsRunning == false)
            {
                mConditionStopwatch.Start();
            }

            conditionElapsedTimeTextBox.Text = mConditionStopwatch.Elapsed.ToString("mm\\:ss\\.ff");
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
            if (axWindowsMediaPlayer1 != null)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }

            // Pause participant's movie as well.
            if (mSubjectMovieForm != null)
            {
                mSubjectMovieForm.MediaPlayerPause();
            }
        }

        #region Events (except Kinect's FrameArrived event)
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
        }
        private void bodyPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            // Draw original skeleton
            if (mOriginalBody != null)
            {
                foreach (Body body in mOriginalBody)
                {
                    if (body.IsTracked)
                    {
                        Helpers.DrawSkeleton(bodyPictureBox, body, graphics, true);
                    }
                }
            }

            // Draw current skeleton
            if (mBody != null)
            {
                foreach (Body body in mBody)
                {
                    if (body.IsTracked)
                    {
                        Helpers.DrawSkeleton(bodyPictureBox, body, graphics, false);
                    }
                }
            }
        }
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
            mOriginalJointPositionsSet = false;
        }
        private void initializeButton_Click(object sender, EventArgs e)
        {
            InitializeKinect();
        }
        private void startButton_Click(object sender, EventArgs e)
        {
            mSessionState = 22;
            UpdateCurrentLimits(false);

            startButton.BackColor = Color.DeepSkyBlue;
        }
        private void stopButton_Click(object sender, EventArgs e)
        {
            mSessionState = 21;
            startButton.BackColor = Color.Red;
        }
        private void setLimitsButton_Click(object sender, EventArgs e)
        {
            UpdateCurrentLimits(true);
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
            timer1.Dispose();
            mKinectSensor.Close();
            mBodyframeReader.Dispose();
        }
        private void FinalForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
