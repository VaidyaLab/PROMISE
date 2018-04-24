// Initial Window for specifying settings and file paths.

using System;
using System.Windows.Forms;

namespace Final_Kinect
{
    public partial class SettingsForm : Form
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        FolderBrowserDialog fileBrowserDialog = new FolderBrowserDialog();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            // If not depth, then color
            int depthFrameReference = depthRadioButton.Checked ? 1 : 0;

            int counter_angle = 0;

            if (headRadioButton.Checked)
            {
                counter_angle = 2;
            }
            else if (torsoRadioButton.Checked)
            {
                counter_angle = 1;
            }
            else if (allRadioButton.Checked)
            {
                counter_angle = 3;
            }

            // Run Baseline - I believe this is not used at all and may not even work
            if (baselineVersionRadioButton.Checked)
            {
                BaselineForm baselineForm = new BaselineForm(
                    subjectInitialsTextBox.Text,
                    experimentNumberTextBox.Text,
                    smoothingKernalTextBox.Text,
                    smallMovementLowerLimitTextBox.Text,
                    largeMovementLowerLimitTextBox.Text,
                    filePathTextBox.Text,
                    counter_angle,
                    depthFrameReference
                );

                baselineForm.Show();
            }

           // Run Final (with all components)
            else if (finalVersionRadioButton.Checked)
            {
                FinalForm baseline = new FinalForm(
                    subjectInitialsTextBox.Text,
                    experimentNumberTextBox.Text,
                    smoothingKernalTextBox.Text,
                    smallMovementLowerLimitTextBox.Text,
                    largeMovementLowerLimitTextBox.Text,
                    movieFrameCheckBox.Checked ? 1 : 0,
                    movieCheckBox.Checked ? 1 : 0,
                    movieSensitiveCheckBox.Checked ? 1 : 0,
                    progressFrameCheckBox.Checked ? 1 : 0,
                    progressCheckBox.Checked ? 1 : 0,
                    progressSensitiveCheckBox.Checked ? 1 : 0,
                    trafficFrameCheckBox.Checked ? 1 : 0,
                    trafficCheckBox.Checked ? 1 : 0,
                    trafficSensitiveCheckBox.Checked ? 1 : 0,
                    rawMeasuresRadioButton.Checked ? 1 : 0,
                    meansRadioButton.Checked ? 1 : 0,
                    mediansRadioButton.Checked ? 1 : 0,
                    sessionTimeTextBox.Text,
                    lowerLimitLargeMovementCheckBox.Checked ? 1 : 0,
                    videoFileTextBox.Text,
                    filePathTextBox.Text,
                    counter_angle,
                    depthFrameReference
                );

                baseline.Show();
            }
        }

        // To select the video file
        private void videoFileBrowseButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                videoFileTextBox.Text = openFileDialog.FileName;
            }
        }

        // To select the path for storing the data files
        private void dataFileBrowseButton_Click(object sender, EventArgs e)
        {
            if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                filePathTextBox.Text = fileBrowserDialog.SelectedPath;
            }
        }
    }
}