using AppKit;
using Foundation;
using System.Runtime.Remoting;

namespace DoNotDisturbMe
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
		NSStatusItem _statusItem;

        public AppDelegate()
        {
			_statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Variable);
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
			//http://footle.org/WeatherBar/



			//			let icon = NSImage(named: "statusIcon")
			//icon?.isTemplate = true // best for dark mode
			//statusItem.image = icon
			//statusItem.menu = statusMenu
			//var icon = NSImage.FromStream("statusIcon");
			//var icon =UiImage UIImage.FromBundle("statusicon");

			//NSImage.F
			var icon = NSImage.ImageNamed("statusIcon");
			//var icon = new NSImage("statusIcon");
			icon.Template = true;
			_statusItem.Image = icon;
			//_statusItem.Title = "WeatherBar";
			_statusItem.Menu = StatusMenu;

        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
