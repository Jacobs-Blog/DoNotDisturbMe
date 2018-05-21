using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace DoNotDisturbMe
{
    public partial class StatusMenuController : NSObject
    {
		private NSStatusItem _statusItem;
		private NSImage _activeIcon;
        private NSImage _inactiveIcon;

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();

			_statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Variable);
			_inactiveIcon = NSImage.ImageNamed("statusIconInactive");
            _activeIcon = NSImage.ImageNamed("statusIconActive");

            //icon.Template = true;
            _statusItem.Image = _inactiveIcon;
            //_statusItem.Menu = StatusMenu;
            //_statusItem.Title = "00:00:00";
		}

		#region Constructors
		//_statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Variable);
		//// Called when created from unmanaged code
		//public StatusMenuController(IntPtr handle) : base(handle)
		//{
		//    Initialize();
		//}

		// Called when created directly from a XIB file
		//[Export("initWithCoder:")]
		//public StatusMenuController(NSCoder coder) : base(coder)
		//{
		//    Initialize();
		//}

		//// Shared initialization code
		//void Initialize()
		//{
		//}

		#endregion
	}
}
