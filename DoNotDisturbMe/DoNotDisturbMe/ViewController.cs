using AppKit;
using CoreGraphics;
using Foundation;
using System;
using System.IO.Ports;
using System.Linq;
using System.Timers;
using DoNotDisturbMe.Helpers;

namespace DoNotDisturbMe
{
	public partial class ViewController : NSViewController
	{
		private SerialPort _serial;
		private Timer _timer;
		private int _timeLeft;   
		private NSImage _activeIcon;
        private NSImage _inactiveIcon;

		private NSStatusItem _statusItem;

		public ViewController(IntPtr handle) : base(handle)
		{
			_statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Variable);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			SetupMenu();         

			PortSelector.Activated += PortSelector_Activated;

			SetTimer();

			SetTimerLabel();         

			FillArduinoPortSelector();

			Start();
		}

		private void Start()
        {
            // Fire the timer once a second
            _timer = new Timer(1000);
            _timer.Elapsed += (sender, e) =>
            {
                _timeLeft--;

                SetTimerLabel();
                // If 25 minutes have passed
                if (_timeLeft == 0)
                {
                    // Stop the timer and reset
                    _timer.Stop();

                    SetTimer();
                    EndTimer();
                    //ClearTimerLabel();
                    //SetTimerLabel();

                    // Trigger a local notification after the time has elapsed
                    var notification = new NSUserNotification();
                    // Add text and sound to the notification
                    notification.Title = "OH NO! They can disturb you again!";
                    notification.InformativeText = $"{Settings.LastTimer} minutes are up!";
                    notification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
                    //notification.HasActionButton = true; // Show "close" and "show" buttons when the notification is displayed as an alert
                    NSUserNotificationCenter.DefaultUserNotificationCenter.DeliverNotification(notification);

                    //InvokeOnMainThread(() =>
                    //{
                    //  SetTimerLabel();
                    //  StartStopButton.Title = "Start";
                    //});
                }
            };
        }


		public override void ViewDidDisappear()
		{
			PortSelector.Activated -= PortSelector_Activated;

			base.ViewDidDisappear();
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}
        
        private void SetupMenu()
		{
			//http://footle.org/WeatherBar/
			_inactiveIcon = NSImage.ImageNamed("statusIconInactive");
            _activeIcon = NSImage.ImageNamed("statusIconActive");

            //icon.Template = true;
			_statusItem.Image = _inactiveIcon;
            _statusItem.Menu = StatusMenu;
			_statusItem.Title = "00:00:00";
		}

		partial void StatusBarTenMinutesClicked(NSObject sender)
		{
		}

		partial void StatusBarExitClicked(NSObject sender)
		{
			if (_timer.Enabled)
				_timer.Stop();

			if (_serial.IsOpen)
				TryCloseSerial();

			NSApplication.SharedApplication.Terminate(this);	
		}

		private void FillArduinoPortSelector()
		{
			try
			{
				var ports = SerialPort.GetPortNames();
				var menuItems = new NSMenu();
				foreach (var port in ports)
				{
					PortSelector.AddItem(port);
					menuItems.AddItem(new NSMenuItem(port, (object sender, EventArgs e) => 
					{
						
					}));
				}

				PortsMenu.Submenu = menuItems;
                         
				if (ports.Any(x => x.Contains("usbmodem")))
				{
					PortSelector.SelectItem(ports.ToList().IndexOf(ports.FirstOrDefault(x => x.Contains("usbmodem"))));
					Connect(ports.FirstOrDefault(x => x.Contains("usbmodem")));
				}

			}
			catch (Exception ex)
			{

			}
		}

		private void PortSelector_Activated(object sender, EventArgs e)
		{
			Connect(PortSelector.SelectedItem.Title);
		}

		private void Connect(string port)
		{
			ArduinoLoader.Hidden = false;
			ArduinoStatus.StringValue = "Connecting...";

			try
			{
				TryCloseSerial();

				_serial = new SerialPort(port, 9600);
				if (_serial != null)
				{
					_serial.Open();

					if (_serial.IsOpen)
						ArduinoStatus.StringValue = "Connected";
				}
			}
			catch (Exception ex)
			{
				ArduinoStatus.StringValue = "Error!";
			}
			finally
			{
				ArduinoLoader.Hidden = true;
			}
		}

		private void TryCloseSerial()
		{
			if (_serial != null && _serial.IsOpen)
			{
				_serial.Close();
				_serial.Dispose();
				_serial = null;
			}
		}

		partial void NumberChanged(Foundation.NSObject sender)
		{
			var check = sender as NSButton;
			Console.WriteLine("Changed to {0}", check.Tag);

			if (_timer.Enabled)
				_timer.Stop();

			Settings.LastTimer = (int)check.Tag;
			SetTimer();
			SetTimerLabel();
		}

		partial void StartStopButtonClicked(NSObject sender)
		{
			if (_timer.Enabled)
			{
				_timer.Stop();
				StartStopButton.Title = "Start";
			}
			else
			{
				_timer.Start();
				StartStopButton.Title = "Stop";
			}
		}

        private void SetTimer()
		{
#if DEBUG
            _timeLeft = Settings.LastTimer;
#else
            _timeLeft = Settings.LastTimer * 60;
#endif
		}

		private void SetTimerLabel()
		{         
			// Format the remaining time nicely for the label
			TimeSpan time = TimeSpan.FromSeconds(_timeLeft);
			string timeString = time.ToString(@"mm\:ss");
			InvokeOnMainThread(() =>
			{
				_statusItem.Image = _activeIcon;
				TimerLabel.StringValue = timeString;
				_statusItem.Title = timeString;
			});
		}

		//private void ClearTimerLabel()
    //    {
    //        InvokeOnMainThread(() =>
    //        {
				//_statusItem.Title = string.Empty;
        //    });
        //}

		      
		private void BeginTimer()
		{
			
		}

		private void EndTimer()
		{
			InvokeOnMainThread(() =>
            {
			    _statusItem.Image = _inactiveIcon;
                _statusItem.Title = string.Empty;
            });
		}

    }
}
