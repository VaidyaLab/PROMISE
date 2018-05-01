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
            int bodyTrackingArea = 0;

            if (headRadioButton.Checked)
            {
                bodyTrackingArea = 2;
            }
            else if (torsoRadioButton.Checked)
            {
                bodyTrackingArea = 1;
            }
            else if (allRadioButton.Checked)
            {
                bodyTrackingArea = 3;
            }

            FinalForm finalForm = new FinalForm(
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
                bodyTrackingArea,
                depthRadioButton.Checked ? 1 : 0 // If not depth, then color
            );

            finalForm.Show();           
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