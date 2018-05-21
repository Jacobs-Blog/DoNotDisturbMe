using System;
using AppKit;
using Foundation;
using System.Timers;
using System.Linq;
using System.IO.Ports;
using DoNotDisturbMe.Models;

namespace DoNotDisturbMe
{
	[Register("AppController")]
	public partial class AppController : NSObject
	{
		private NSStatusItem _statusItem;
		private NSImage _activeIcon;
		private NSImage _inactiveIcon;
		private NSImage _noArduinoIcon;
		private NSMenuItem _stopItem;

		private SerialPort _serialPort;

		private Timer _timer;
		private int _timeLeft;
		private int _timerDuration;

		private bool _manualStopRequested;
		private bool _testRequested;

		public AppController()
		{
		}

		public override void AwakeFromNib()
		{
			SetupMenu();

			SetupArduinoMenu();

			Start();
		}

		private void SetupMenu()
		{
			_inactiveIcon = NSImage.ImageNamed("statusIconInactive");
			_activeIcon = NSImage.ImageNamed("statusIconActive");
			_noArduinoIcon = NSImage.ImageNamed("statusIconNoArduino");

			_statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Variable);
			SetSystemIcon();
			_statusItem.Menu = StatusMenu;
			_statusItem.HighlightMode = true;

			_stopItem = _statusItem.Menu.ItemArray().FirstOrDefault(x => x.Title == "Stop");

			InvokeOnMainThread(() => _stopItem.Enabled = false);
		}

        private void SetSystemIcon()
		{
			if(_serialPort == null || !_serialPort.IsOpen)
			{
				_statusItem.Image = _noArduinoIcon;
				_statusItem.ToolTip = "No arduino connected";
			}   
			else 
			{
				_statusItem.Image = _inactiveIcon;
				_statusItem.ToolTip = "Arduino connected";
			}         
		}

		private void SetupArduinoMenu()
		{
			var menuItems = new NSMenu();

			var ports = SerialPort.GetPortNames().Where(x => x.Contains("usbmodem"));

			foreach (var port in ports)
			{
				menuItems.AddItem(new NSMenuItem(port, (object sender, EventArgs e) =>
				{
					var selectedPort = sender as NSMenuItem;
					ConnectToArduino(selectedPort.Title);
				}));
			}

			ArduinoPortsMenu.Submenu = menuItems;
		}

		private void Start()
		{
			// Fire the timer once a second
			_timer = new Timer(1000);
			_timer.Elapsed += _timer_Elapsed;
		}

		#region Timer Methods

		void _timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			_timeLeft--;

			SetTimerLabel();

			if (_timeLeft == 0 || _manualStopRequested)
			{
				TimerEnd();
			}
		}

		private void SetTimerLabel()
		{
			// Format the remaining time nicely for the label
			TimeSpan time = TimeSpan.FromSeconds(_timeLeft);
			string timeString = time.ToString(@"mm\:ss");
			InvokeOnMainThread(() => _statusItem.Title = timeString);
		}

		private void StartNewTimer()
		{
			if(_testRequested)
			    _timeLeft = _timerDuration;
			else
			    _timeLeft = _timerDuration * 60;

			SetSystemIcon();

			SetTimerLabel();

			WriteDataToArduino(ArduinoCommands.Up);
			_timer.Start();

			_stopItem.Enabled = true;
		}

		private void TimerEnd()
		{
			_timer.Stop();
			WriteDataToArduino(ArduinoCommands.Down);

			InvokeOnMainThread(() =>
			{
				_stopItem.Enabled = false;

				_statusItem.Title = string.Empty;

				SetSystemIcon();

				UncheckMenuItems();
			});

			if (!_manualStopRequested)
			{
				//Trigger a local notification after the time has elapsed
				var notification = new NSUserNotification();
				notification.Title = "OH NO! RUN! HIDE!";
				notification.InformativeText = $"{_timerDuration} minutes are up!";
				notification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
				NSUserNotificationCenter.DefaultUserNotificationCenter.DeliverNotification(notification);
			}

			_manualStopRequested = false;
		}

		#endregion

		#region Menu Click Handlers

		partial void MinutesSelected(NSObject sender)
		{
			UncheckMenuItems();

			var item = sender as NSMenuItem;
			_timerDuration = (int)item.Tag;

			Console.WriteLine("Changed to {0}", item.Tag);
			item.State = NSCellStateValue.On;

			StartNewTimer();
		}

		private void UncheckMenuItems()
		{
			foreach (var menuItem in _statusItem.Menu.ItemArray().Where(x => x.Title.ToLower().Contains("minutes") && x.State == NSCellStateValue.On))
			{
				menuItem.State = NSCellStateValue.Off;
			}
		}

		partial void StopClicked(NSObject sender)
		{
			_manualStopRequested = true;
		}

		partial void TestClicked(NSObject sender)
		{
			_testRequested = !_testRequested;

			if (_testRequested)
				_statusItem.Menu.ItemArray().FirstOrDefault(x => x.Title.ToLower().Contains("test")).State = NSCellStateValue.On;
			else
				_statusItem.Menu.ItemArray().FirstOrDefault(x => x.Title.ToLower().Contains("test")).State = NSCellStateValue.Off;         
		}

		partial void ExitClicked(NSObject sender)
		{
			if (_timer.Enabled)
				_timer.Stop();

			if (_serialPort.IsOpen)
			    TryCloseSerial();

			NSApplication.SharedApplication.Terminate(this);
		}

		#endregion

		#region Arduino Methods

		private void ConnectToArduino(string port)
        {         
            try
            {
				UncheckArduinoMenuItems();

                TryCloseSerial();

                _serialPort = new SerialPort(port, 9600);
				if (_serialPort != null)
                {
					_serialPort.Open();
					if(_serialPort.IsOpen)
					{
						ArduinoPortsMenu.Submenu.ItemArray().FirstOrDefault(x => x.Title.Equals(port)).State = NSCellStateValue.On;
						//var item = ArduinoPortsMenu.Submenu.ItemArray().FirstOrDefault(x => x.Title.Equals(port));
						//item.State = NSCellStateValue.On;                  
					}
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
				SetSystemIcon();
            }
        }

		private void UncheckArduinoMenuItems()
        {
			foreach (var menuItem in ArduinoPortsMenu.Submenu.ItemArray().Where(x => x.State == NSCellStateValue.On))
            {
                menuItem.State = NSCellStateValue.Off;
            }
        }

		private void TryCloseSerial()
        {
			if (_serialPort != null && _serialPort.IsOpen)
            {
				_serialPort.Close();
				_serialPort.Dispose();
				_serialPort = null;
            }
        }

		private void WriteDataToArduino(ArduinoCommands command)
		{
			if(_serialPort != null && _serialPort.IsOpen)
			{
				switch(command)
				{
					case ArduinoCommands.Up:
						_serialPort.WriteLine("100");
						break;

					case ArduinoCommands.Down:
						_serialPort.WriteLine("0");
                        break;

					case ArduinoCommands.Angle:
						_serialPort.WriteLine("angle");
						break;
				}
			}
		}

		#endregion
	}
}
