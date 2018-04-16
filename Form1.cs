// Initial Window for specifying settings and file paths.

using System;
using System.Windows.Forms;

namespace Final_Kinect
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        FolderBrowserDialog fileBrowserDialog = new FolderBrowserDialog();

        public Form1()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            // If not depth, then color
            int color_choice = depthRadioButton.Checked ? 1 : 0;

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

            // Run Baseline
            if (baselineVersionRadioButton.Checked)
            {
                Form2 baselineForm = new Form2(
                    subjectInitialsTextBox.Text,
                    experimentNumberTextBox.Text,
                    smootingKernalTextBox.Text,
                    smallMovementLowerLimitTextBox.Text,
                    largeMovementLowerLimitTextBox.Text,
                    filePathTextBox.Text,
                    counter_angle,
                    color_choice
                );

                baselineForm.Show();
            }
           // Run Final (with all components)
            else if (finalVersionRadioButton.Checked)
            {
                Form6 baseline = new Form6(
                    subjectInitialsTextBox.Text,
                    experimentNumberTextBox.Text,
                    smootingKernalTextBox.Text,
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
                    color_choice
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