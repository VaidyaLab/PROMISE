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

        public static void DrawSkeleton(this Panel panel, Body body)
        {
            if (body == null) return;

            foreach (Joint joint in body.Joints.Values)
            {
                panel.DrawPoint(joint);
            }

            panel.DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck]);
            panel.DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder]);
            panel.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft]);
            panel.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight]);
            panel.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid]);
            panel.DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft]);
            panel.DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight]);
            panel.DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft]);
            panel.DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight]);
            panel.DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft]);
            panel.DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]);
            panel.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft]);
            panel.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight]);
            panel.DrawLine(body.Joints[JointType.HandTipLeft], body.Joints[JointType.ThumbLeft]);
            panel.DrawLine(body.Joints[JointType.HandTipRight], body.Joints[JointType.ThumbRight]);
            panel.DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase]);
            panel.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft]);
            panel.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight]);
            panel.DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft]);
            panel.DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]);
            panel.DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft]);
            panel.DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight]);
            panel.DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft]);
            panel.DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight]);
        }

        public static void DrawPoint(this Panel panel, Joint joint)
        {
            if (joint.TrackingState == TrackingState.NotTracked) return;

            joint = joint.ScaleTo(panel.Width, panel.Height);

            Graphics panelGraphics = panel.CreateGraphics();
            SolidBrush solidBrush = new SolidBrush(Color.LightBlue);

            panelGraphics.FillEllipse(solidBrush, new Rectangle((int) joint.Position.X - 20 / 2, (int) joint.Position.Y - 20 / 2, 20, 20));

            solidBrush.Dispose();
            panelGraphics.Dispose();
        }

        public static void DrawLine(this Panel panel, Joint first, Joint second)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            first = first.ScaleTo(panel.Width, panel.Height);
            second = second.ScaleTo(panel.Width, panel.Height);

            Pen pen = new Pen(Color.LightBlue)
            {
                Width = 8
            };

            Graphics panelGraphics = panel.CreateGraphics();
            panelGraphics.DrawLine(pen, first.Position.X, first.Position.Y, second.Position.X, second.Position.Y);
        }

        #endregion

    }
}
