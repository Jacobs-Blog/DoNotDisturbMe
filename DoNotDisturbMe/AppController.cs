using System;
using AppKit;
using Foundation;

namespace DoNotDisturbMe
{
	[Register("AppController")]
	public partial class AppController : NSObject
	{
		private NSStatusItem _statusItem;
		private NSImage _activeIcon;
		private NSImage _inactiveIcon;

		public AppController()
		{

		}

		public override void AwakeFromNib()
		{
			SetupMenu();
		}

		private void SetupMenu()
		{
			_inactiveIcon = NSImage.ImageNamed("statusIconInactive");
			_activeIcon = NSImage.ImageNamed("statusIconActive");

			_statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Variable);
			_statusItem.Image = _inactiveIcon;
			_statusItem.Menu = StatusMenu;
			_statusItem.HighlightMode = true;
		}

#region Menu Click Handlers

		partial void MinutesSelected(Foundation.NSObject sender)
		{
			var item = sender as NSMenuItem;
			Console.WriteLine("Changed to {0}", item.Tag);
		}

		partial void ExitClicked(NSObject sender)
		{
			//if (_timer.Enabled)
			//    _timer.Stop();

			//if (_serial.IsOpen)
			//TryCloseSerial();

			NSApplication.SharedApplication.Terminate(this);
		}

#endregion
	}
}
