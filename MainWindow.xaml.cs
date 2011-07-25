/////////////////////////////////////////////////////////////////////////
//
// Copyright © Microsoft Corporation.  All rights reserved.  
// This code is a “supplement” under the terms of the 
// Microsoft Kinect for Windows SDK (Beta) from Microsoft Research 
// License Agreement: http://research.microsoft.com/KinectSDK-ToU
// and is licensed under the terms of that license agreement. 
//
/////////////////////////////////////////////////////////////////////////
//implementação baseada no projeto KinectMouse encontrado em: http://kinectmouse.codeplex.com/

using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Coding4Fun.Kinect.Wpf;
using Microsoft.Research.Kinect.Nui;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

[assembly:CLSCompliant(true)]
namespace Microsoft.Research.Kinect.Samples.CursorControl
{
	public partial class MainWindow : Window
	{
		private const float ClickThreshold = 0.28f;
		private const float SkeletonMaxX = 0.60f;
		private const float SkeletonMaxY = 0.40f;

		private Nui.Runtime _runtime = new Nui.Runtime();
		private NotifyIcon _notifyIcon = new NotifyIcon();


		public MainWindow()
		{
			InitializeComponent();

			{
				this.Show();
				this.WindowState = WindowState.Normal;
				this.Focus();
			};
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_runtime.SkeletonFrameReady += _runtime_SkeletonFrameReady;

			try
			{
				_runtime.Initialize(RuntimeOptions.UseDepth | RuntimeOptions.UseSkeletalTracking);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Could not initialize Kinect device: " + ex.Message);
			}

			_runtime.SkeletonEngine.TransformSmooth = true;
			TransformSmoothParameters parameters = new TransformSmoothParameters();
			parameters.Smoothing = 0.7f;
			parameters.Correction = 0.3f;
			parameters.Prediction = 0.4f;
			parameters.JitterRadius = 1.0f;
			parameters.MaxDeviationRadius = 0.5f;
			_runtime.SkeletonEngine.SmoothParameters = parameters;

			try
			{
				_runtime.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.Depth);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Could not open depth stream: " + ex.Message);
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			_notifyIcon.Visible = false;
			_runtime.Uninitialize();
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
				this.Hide();
		}

		void _runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
		{
			foreach(SkeletonData sd in e.SkeletonFrame.Skeletons)
			{
				if(sd.TrackingState == SkeletonTrackingState.Tracked)
				{
					if(
						sd.Joints[JointID.HandRight].TrackingState == JointTrackingState.Tracked)
					{
						int cursorX, cursorY;

						Joint jointRight = sd.Joints[JointID.HandRight];
						Joint jointLeft = sd.Joints[JointID.HandLeft];

						Joint scaledRight = jointRight.ScaleTo((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight, SkeletonMaxX, SkeletonMaxY);
						Joint scaledLeft = jointLeft.ScaleTo((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight, SkeletonMaxX, SkeletonMaxY);

						cursorX = (int)scaledRight.Position.X;
						cursorY = (int)scaledRight.Position.Y;




						bool leftClick;

						if(jointLeft.Position.Y > ClickThreshold)
							leftClick = true;
						else
							leftClick = false;
						NativeMethods.SendMouseInput(cursorX, cursorY, (int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight, leftClick);

						return;
					}
				}
			}
		}

	}
}
