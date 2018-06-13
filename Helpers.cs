using System.Drawing;
using System.Windows.Forms;
using Microsoft.Kinect;


namespace Final_Kinect
{
    public static class Helpers
    {

        #region Body

        public static Joint ScaleTo(this Joint joint, double width, double height, float skeletonMaxX, float skeletonMaxY)
        {
            joint.Position = new CameraSpacePoint
            {
                X = Scale(width, skeletonMaxX, joint.Position.X),
                Y = Scale(height, skeletonMaxY, -joint.Position.Y),
                Z = joint.Position.Z
            };

            return joint;
        }

        public static Joint ScaleTo(this Joint joint, double width, double height)
        {
            return ScaleTo(joint, width, height, 1.0f, 1.0f);
        }

        private static float Scale(double maxPixel, double maxSkeleton, float position)
        {
            float value = (float)((((maxPixel / maxSkeleton) / 2) * position) + (maxPixel / 2));

            if (value > maxPixel)
            {
                return (float)maxPixel;
            }

            if (value < 0)
            {
                return 0;
            }

            return value;
        }

        #endregion

        #region Drawing

        public static void DrawSkeleton(this PictureBox pictureBox, Body body)
        {
            if (body == null) return;

            pictureBox.Invalidate();

            foreach (Joint joint in body.Joints.Values)
            {
                pictureBox.DrawPoint(joint);
            }

            pictureBox.DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck]);
            pictureBox.DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder]);
            pictureBox.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft]);
            pictureBox.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight]);
            pictureBox.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid]);
            pictureBox.DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft]);
            pictureBox.DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight]);
            pictureBox.DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft]);
            pictureBox.DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight]);
            pictureBox.DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft]);
            pictureBox.DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]);
            pictureBox.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft]);
            pictureBox.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight]);
            pictureBox.DrawLine(body.Joints[JointType.HandTipLeft], body.Joints[JointType.ThumbLeft]);
            pictureBox.DrawLine(body.Joints[JointType.HandTipRight], body.Joints[JointType.ThumbRight]);
            pictureBox.DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase]);
            pictureBox.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft]);
            pictureBox.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight]);
            pictureBox.DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft]);
            pictureBox.DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]);
            pictureBox.DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft]);
            pictureBox.DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight]);
            pictureBox.DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft]);
            pictureBox.DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight]);
        }

        public static void DrawPoint(this PictureBox pictureBox, Joint joint)
        {
            if (joint.TrackingState == TrackingState.NotTracked) return;

            joint = joint.ScaleTo(pictureBox.Width, pictureBox.Height);

            Graphics pictureBoxGraphics = pictureBox.CreateGraphics();
            SolidBrush solidBrush = new SolidBrush(Color.LightBlue);

            pictureBoxGraphics.FillEllipse(solidBrush, new Rectangle((int) joint.Position.X - 20 / 2, (int) joint.Position.Y - 20 / 2, 20, 20));

            solidBrush.Dispose();
            pictureBoxGraphics.Dispose();
        }

        public static void DrawLine(this PictureBox pictureBox, Joint first, Joint second)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            first = first.ScaleTo(pictureBox.Width, pictureBox.Height);
            second = second.ScaleTo(pictureBox.Width, pictureBox.Height);

            Pen pen = new Pen(Color.LightBlue)
            {
                Width = 8
            };

            Graphics pictureBoxGraphics = pictureBox.CreateGraphics();
            pictureBoxGraphics.DrawLine(pen, first.Position.X, first.Position.Y, second.Position.X, second.Position.Y);
        }

        #endregion

    }
}
