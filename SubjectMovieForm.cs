using System;
using System.Windows.Forms;

namespace Final_Kinect
{
    public partial class SubjectMovieForm : Form
    {
        int mVolume;
        System.Drawing.Color mBGColor;

        public SubjectMovieForm(string videoFile, int progressBarMaximum)
        {            
            InitializeComponent();

            axWindowsMediaPlayer1.URL = videoFile;
            axWindowsMediaPlayer1.uiMode = "None";
            axWindowsMediaPlayer1.Ctlcontrols.stop();

            progressBar1.Maximum = progressBarMaximum;
            mVolume = axWindowsMediaPlayer1.settings.volume;
            mBGColor = this.BackColor;
         
        }
        private void SubjectMovieForm_Load(object sender, EventArgs e)
        {
            /*
            if (count == 22)
            {
                this.BackColor = System.Drawing.Color.Black;
            }
            */

            PositionOnSecondScreen();           
        }
        public void UpdateProgressBar()
        {
            progressBar1.Value++;
            progressBar1.Update();
        }
        public void BaselineScreen(bool baseline)
        {
            if (baseline)
            {
                this.BackColor = System.Drawing.Color.Black;
            }
            else
            {
                this.BackColor = mBGColor;
            }

            axWindowsMediaPlayer1.Visible = !baseline;
            panel1.Visible = !baseline;
            label1.Visible = !baseline;
            progressBar1.Visible = !baseline;
            pictureBox4.Visible = !baseline;
            pictureBox5.Visible = !baseline;

        }
        public void MediaPlayerPlay()
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        public void MediaPlayerPause()
        {
            if (axWindowsMediaPlayer1 != null)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
        }
        public void MediaPlayerMute(bool mute)
        {
            if (mute)
            {
                axWindowsMediaPlayer1.settings.volume = 0;
                
            }
            else
            {
                axWindowsMediaPlayer1.settings.volume = mVolume;
            }
        }
        public void SetRedLightVisible(bool visible)
        {
            mRedLightPictureBox.Visible = visible;
        }
        public void SetYellowLightVisible(bool visible)
        {
            mYellowLightPictureBox.Visible = visible;
        }
        public void SetGreenLightVisible(bool visible)
        {
            mGreenLightPictureBox.Visible = visible;
        }
        public void PositionOnSecondScreen()
        {
            Screen[] screens = Screen.AllScreens;

            if (screens.Length > 1)
            {
                System.Drawing.Rectangle bounds = screens[1].Bounds;
                this.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            }

            WindowState = FormWindowState.Maximized;

        }
    }
}
