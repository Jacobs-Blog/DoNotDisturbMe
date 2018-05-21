using System;
using AppKit;
using Foundation;
using System.Timers;
using System.Linq;

namespace DoNotDisturbMe
{
	[Register("AppController")]
	public partial class AppController : NSObject
	{
		private NSStatusItem _statusItem;
		private NSImage _activeIcon;
		private NSImage _inactiveIcon;
		private NSMenuItem _stopItem;

		private Timer _timer;
        private int _timeLeft;
		private int _timerDuration;

		private bool _manualStopRequested;

		public AppController()
		{

		}

		public override void AwakeFromNib()
		{
			SetupMenu();

			Start();
		}

		private void SetupMenu()
		{
			_inactiveIcon = NSImage.ImageNamed("statusIconInactive");
			_activeIcon = NSImage.ImageNamed("statusIconActive");

			_statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Variable);
			_statusItem.Image = _inactiveIcon;
			_statusItem.Menu = StatusMenu;
			_statusItem.HighlightMode = true;

			_stopItem = _statusItem.Menu.ItemArray().FirstOrDefault(x => x.Title == "Stop");

			InvokeOnMainThread(() => _stopItem.Enabled = false);
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
            InvokeOnMainThread(() =>
            {
                _statusItem.Image = _activeIcon;
                _statusItem.Title = timeString;
            });
        }

        private void StartNewTimer()
		{
#if DEBUG
			_timeLeft = _timerDuration;
#else
			_timeLeft = _timerDuration * 60;
#endif

			_timer.Start();

			_stopItem.Enabled = true;
		}

		private void TimerEnd()
		{
			_timer.Stop();
            
			InvokeOnMainThread(() =>
			{
				_stopItem.Enabled = false;

				_statusItem.Image = _inactiveIcon;
                _statusItem.Title = string.Empty;

				UncheckMenuItems();
			});         

			if(!_manualStopRequested)
			{
				//Trigger a local notification after the time has elapsed
                var notification = new NSUserNotification();
                notification.Title = "OH NO! They can disturb you again!";
                notification.InformativeText = $"{_timerDuration} minutes are up!";
                notification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
                NSUserNotificationCenter.DefaultUserNotificationCenter.DeliverNotification(notification);  
			}

			_manualStopRequested = false;
		}

		#endregion

		#region Menu Click Handlers

		partial void MinutesSelected(Foundation.NSObject sender)
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
			foreach (var menuItem in _statusItem.Menu.ItemArray())
            {
                if (menuItem.Title.ToLower().Contains("minutes"))
                    menuItem.State = NSCellStateValue.Off;
            }
		}
        
		partial void StopClicked(NSObject sender)
		{
			_manualStopRequested = true;
		}

		partial void ExitClicked(NSObject sender)
		{
			if (_timer.Enabled)
			    _timer.Stop();

			//if (_serial.IsOpen)
			//TryCloseSerial();

			NSApplication.SharedApplication.Terminate(this);
		}

#endregion

	}
}
