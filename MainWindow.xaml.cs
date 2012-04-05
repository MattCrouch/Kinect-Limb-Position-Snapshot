using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;

namespace LimbPositionSnapshot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            setupKinect();
            sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);

            comboJoint.Items.Add("Current Player Position");
            comboJoint.Items.Add("Head");
            comboJoint.Items.Add("Shoulder Center");
            comboJoint.Items.Add("Shoulder Left");
            comboJoint.Items.Add("Elbow Left");
            comboJoint.Items.Add("Wrist Left");
            comboJoint.Items.Add("Hand Left");
            comboJoint.Items.Add("Shoulder Right");
            comboJoint.Items.Add("Elbow Right");
            comboJoint.Items.Add("Wrist Right");
            comboJoint.Items.Add("Hand Right");
            comboJoint.Items.Add("Spine");
            comboJoint.Items.Add("Hip Center");
            comboJoint.Items.Add("Hip Left");
            comboJoint.Items.Add("Knee Left");
            comboJoint.Items.Add("Ankle Left");
            comboJoint.Items.Add("Foot Left");
            comboJoint.Items.Add("Hip Right");
            comboJoint.Items.Add("Knee Right");
            comboJoint.Items.Add("Ankle Right");
            comboJoint.Items.Add("Foot Right");

            comboJoint.SelectedIndex = 0;
        }

        void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                //COLOUR IMAGE CODE
                if (colorFrame == null)
                {
                    return;
                }

                byte[] pixelData = new byte[colorFrame.PixelDataLength];
                colorFrame.CopyPixelDataTo(pixelData);

                imgPlayer.Source = colorFrame.ToBitmapSource();

            }

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                {
                    return;
                }

                
                skeletonFrame.CopySkeletonDataTo(allSkeletons);

                skeleton = (from s in allSkeletons
                                     where s.TrackingState == SkeletonTrackingState.Tracked
                                     select s).FirstOrDefault();
            }
        }

        KinectSensor sensor;
        private Skeleton[] allSkeletons = new Skeleton[6];
        private Skeleton skeleton;
        private JointType jointTracked;
        private JointType jointDiff;

        public void setupKinect()
        {
            if (KinectSensor.KinectSensors.Count <= 0)
            {
                MessageBox.Show("No Kinect connected :(");
            }
            else
            {
                //use first Kinect
                sensor = KinectSensor.KinectSensors[0];

                switch (sensor.Status)
                {
                    case KinectStatus.Connected:
                        sensor.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);
                        sensor.SkeletonStream.Enable();

                        try
                        {
                            sensor.Start();
                        }
                        catch (System.IO.IOException)
                        {
                            MessageBox.Show("One program at a time, please");
                            throw;
                        }
                        break;
                    case KinectStatus.Disconnected:
                        MessageBox.Show("No Kinect connected :(");
                        break;
                }
            }
        }

        private void btnSnapshot_Click(object sender, RoutedEventArgs e)
        {
            if (skeleton != null)
            {
                if (chkFromHipCenter.IsChecked.Value)
                {
                    txtX.Text = (skeleton.Joints[jointTracked].Position.X - skeleton.Joints[JointType.HipCenter].Position.X).ToString();
                    txtY.Text = (skeleton.Joints[jointTracked].Position.Y - skeleton.Joints[JointType.HipCenter].Position.Y).ToString();
                    txtZ.Text = (skeleton.Joints[jointTracked].Position.Z- skeleton.Joints[JointType.HipCenter].Position.Z).ToString();
                }
                else
                {
                    txtX.Text = skeleton.Joints[jointTracked].Position.X.ToString();
                    txtY.Text = skeleton.Joints[jointTracked].Position.Y.ToString();
                    txtZ.Text = skeleton.Joints[jointTracked].Position.Z.ToString();
                }
            }
            else
            {
                txtX.Text = "";
                txtY.Text = "";
                txtZ.Text = "";
            }
        }

        private void comboJoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(comboJoint.SelectedItem.ToString())
            
            switch (comboJoint.SelectedItem.ToString())
            {
                case "Current Player Position":
                    jointTracked = JointType.HipCenter;
                    break;
                case "Head":
                    jointTracked = JointType.Head;
                    break;
                case "Shoulder Center":
                    jointTracked = JointType.ShoulderCenter;
                    break;
                case "Shoulder Left":
                    jointTracked = JointType.ShoulderLeft;
                    break;
                case "Elbow Left":
                    jointTracked = JointType.ElbowLeft;
                    break;
                case "Wrist Left":
                    jointTracked = JointType.WristLeft;
                    break;
                case "Hand Left":
                    jointTracked = JointType.HandLeft;
                    break;
                case "Shoulder Right":
                    jointTracked = JointType.ShoulderRight;
                    break;
                case "Elbow Right":
                    jointTracked = JointType.ElbowRight;
                    break;
                case "Wrist Right":
                    jointTracked = JointType.WristRight;
                    break;
                case "Hand Right":
                    jointTracked = JointType.HandRight;
                    break;
                case "Spine":
                    jointTracked = JointType.Spine;
                    break;
                case "Hip Center":
                    jointTracked = JointType.HipCenter;
                    break;
                case "Hip Left":
                    jointTracked = JointType.HipLeft;
                    break;
                case "Knee Left":
                    jointTracked = JointType.KneeLeft;
                    break;
                case "Ankle Left":
                    jointTracked = JointType.AnkleLeft;
                    break;
                case "Foot Left":
                    jointTracked = JointType.FootLeft;
                    break;
                case "Hip Right":
                    jointTracked = JointType.HipRight;
                    break;
                case "Knee Right":
                    jointTracked = JointType.KneeRight;
                    break;
                case "Ankle Right":
                    jointTracked = JointType.AnkleRight;
                    break;
                case "Foot Right":
                    jointTracked = JointType.FootRight;
                    break;
            }
        }

        public void stopKinect(KinectSensor theSensor)
        {
            if (theSensor != null)
            {
                theSensor.Stop();
                theSensor.AudioSource.Stop();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //MessageBox.Show("DOES NOT FAIL!");
            stopKinect(sensor);
            //StartScreen.speechRecognizer.Stop();
        }
    }
}
