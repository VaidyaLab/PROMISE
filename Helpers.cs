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

        public static void DrawSkeleton(this PictureBox pictureBox, Body body, Graphics graphics)
        {
            if (body == null) return;
           
            foreach (Joint joint in body.Joints.Values)
            {
                pictureBox.DrawPoint(joint, graphics);
            }

            pictureBox.DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck], graphics);
            pictureBox.DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder], graphics);
            pictureBox.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight], graphics);
            pictureBox.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid], graphics);
            pictureBox.DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], graphics);
            pictureBox.DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], graphics);
            pictureBox.DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight], graphics);
            pictureBox.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight], graphics);
            pictureBox.DrawLine(body.Joints[JointType.HandTipLeft], body.Joints[JointType.ThumbLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.HandTipRight], body.Joints[JointType.ThumbRight], graphics);
            pictureBox.DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase], graphics);
            pictureBox.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight], graphics);
            pictureBox.DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight], graphics);
            pictureBox.DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight], graphics);
            pictureBox.DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft], graphics);
            pictureBox.DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight], graphics);
        }

        public static void DrawPoint(this PictureBox pictureBox, Joint joint, Graphics graphics)
        {
            if (joint.TrackingState == TrackingState.NotTracked) return;

            joint = joint.ScaleTo(pictureBox.Width, pictureBox.Height);

            SolidBrush solidBrush = new SolidBrush(Color.LightBlue);

            graphics.FillEllipse(solidBrush, new Rectangle((int) joint.Position.X - 20 / 2, (int) joint.Position.Y - 20 / 2, 20, 20));

            solidBrush.Dispose();
        }

        public static void DrawLine(this PictureBox pictureBox, Joint first, Joint second, Graphics graphics)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            first = first.ScaleTo(pictureBox.Width, pictureBox.Height);
            second = second.ScaleTo(pictureBox.Width, pictureBox.Height);

            Pen pen = new Pen(Color.LightBlue)
            {
                Width = 8
            };

            graphics.DrawLine(pen, first.Position.X, first.Position.Y, second.Position.X, second.Position.Y);
        }

        #endregion

    }
}
