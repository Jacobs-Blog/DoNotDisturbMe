// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DoNotDisturbMe
{
	partial class AppController
	{
		[Outlet]
		AppKit.NSMenuItem Exit { get; set; }

		[Outlet]
		AppKit.NSMenuItem FiftyMinutes { get; set; }

		[Outlet]
		AppKit.NSMenuItem FortyMinutes { get; set; }

		[Outlet]
		AppKit.NSMenuItem SixtyMinutes { get; set; }

		[Outlet]
		AppKit.NSMenu StatusMenu { get; set; }

		[Outlet]
		AppKit.NSMenuItem TenMinutes { get; set; }

		[Outlet]
		AppKit.NSMenuItem ThirtyMinutes { get; set; }

		[Outlet]
		AppKit.NSMenuItem TwentyMinutes { get; set; }

		[Action ("ExitClicked:")]
		partial void ExitClicked (Foundation.NSObject sender);

		[Action ("MinutesSelected:")]
		partial void MinutesSelected (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (StatusMenu != null) {
				StatusMenu.Dispose ();
				StatusMenu = null;
			}

			if (TenMinutes != null) {
				TenMinutes.Dispose ();
				TenMinutes = null;
			}

			if (TwentyMinutes != null) {
				TwentyMinutes.Dispose ();
				TwentyMinutes = null;
			}

			if (ThirtyMinutes != null) {
				ThirtyMinutes.Dispose ();
				ThirtyMinutes = null;
			}

			if (FortyMinutes != null) {
				FortyMinutes.Dispose ();
				FortyMinutes = null;
			}

			if (FiftyMinutes != null) {
				FiftyMinutes.Dispose ();
				FiftyMinutes = null;
			}

			if (SixtyMinutes != null) {
				SixtyMinutes.Dispose ();
				SixtyMinutes = null;
			}

			if (Exit != null) {
				Exit.Dispose ();
				Exit = null;
			}
		}
	}
}
