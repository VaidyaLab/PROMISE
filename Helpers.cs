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

        public static void DrawSkeleton(this PictureBox pictureBox, Body body, Graphics graphics, bool originalBody)
        {
            if (body == null) return;          

            pictureBox.DrawPoint(body.Joints[JointType.Head], graphics, originalBody);
            pictureBox.DrawPoint(body.Joints[JointType.Neck], graphics, originalBody);
            pictureBox.DrawPoint(body.Joints[JointType.SpineShoulder], graphics, originalBody);
            pictureBox.DrawPoint(body.Joints[JointType.ShoulderLeft], graphics, originalBody);
            pictureBox.DrawPoint(body.Joints[JointType.ShoulderRight], graphics, originalBody);
            pictureBox.DrawPoint(body.Joints[JointType.SpineMid], graphics, originalBody);
            pictureBox.DrawPoint(body.Joints[JointType.SpineBase], graphics, originalBody);

            pictureBox.DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck], graphics, originalBody);
            pictureBox.DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder], graphics, originalBody);
            pictureBox.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft], graphics, originalBody);
            pictureBox.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight], graphics, originalBody);
            pictureBox.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.HandTipLeft], body.Joints[JointType.ThumbLeft], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.HandTipRight], body.Joints[JointType.ThumbRight], graphics, originalBody);
            pictureBox.DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft], graphics, originalBody);
            //pictureBox.DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight], graphics, originalBody);
        }

        public static void DrawPoint(this PictureBox pictureBox, Joint joint, Graphics graphics, bool originalBody)
        {
            if (joint.TrackingState == TrackingState.NotTracked) return;

            joint = joint.ScaleTo(pictureBox.Width, pictureBox.Height);

            SolidBrush solidBrush = new SolidBrush(originalBody ? Color.Black : Color.LightBlue);

            graphics.FillEllipse(solidBrush, new Rectangle((int) joint.Position.X - 20 / 2, (int) joint.Position.Y - 20 / 2, 20, 20));

            solidBrush.Dispose();
        }

        public static void DrawLine(this PictureBox pictureBox, Joint first, Joint second, Graphics graphics, bool originalBody)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            first = first.ScaleTo(pictureBox.Width, pictureBox.Height);
            second = second.ScaleTo(pictureBox.Width, pictureBox.Height);

            Pen pen = new Pen(originalBody ? Color.Black : Color.LightBlue)
            {
                Width = 8
            };

            graphics.DrawLine(pen, first.Position.X, first.Position.Y, second.Position.X, second.Position.Y);
        }

        #endregion

    }
}
