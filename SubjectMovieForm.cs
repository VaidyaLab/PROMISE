using System;
using System.Windows.Forms;

namespace Final_Kinect
{
    public partial class SubjectMovieForm : Form
    {        
        public SubjectMovieForm(string videoFile, int progressBarMaximum)
        {            
            InitializeComponent();

            axWindowsMediaPlayer1.URL = videoFile;
            axWindowsMediaPlayer1.Ctlcontrols.stop();

            progressBar1.Maximum = progressBarMaximum;
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
        public void MediaPlayerPlay()
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        public void MediaPlayerPause()
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
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

            System.Drawing.Rectangle bounds = screens[1].Bounds;
            this.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            WindowState = FormWindowState.Maximized;
        }
    }
}
