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
            FinalForm finalForm = new FinalForm(
                subjectInitialsTextBox.Text,
                experimentNumberTextBox.Text,
                smoothingKernalTextBox.Text,
                smallMovementLowerLimitTextBox.Text,
                largeMovementLowerLimitTextBox.Text,    
                sessionTimeTextBox.Text,
                lowerLimitLargeMovementCheckBox.Checked ? 1 : 0,
                videoFileTextBox.Text,
                filePathTextBox.Text
            );

            finalForm.Show();           
        }
        private void videoFileBrowseButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                videoFileTextBox.Text = openFileDialog.FileName;
            }
        }
        private void dataFileBrowseButton_Click(object sender, EventArgs e)
        {
            if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                filePathTextBox.Text = fileBrowserDialog.SelectedPath;
            }
        }
    }
}