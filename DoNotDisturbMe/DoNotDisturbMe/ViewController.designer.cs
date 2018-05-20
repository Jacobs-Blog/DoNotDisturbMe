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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSProgressIndicator ArduinoLoader { get; set; }

		[Outlet]
		AppKit.NSTextField ArduinoStatus { get; set; }

		[Outlet]
		AppKit.NSPopUpButton PortSelector { get; set; }

		[Outlet]
		AppKit.NSButton StartStopButton { get; set; }

		[Outlet]
		AppKit.NSTextField TimerLabel { get; set; }

		[Action ("NumberChanged:")]
		partial void NumberChanged (Foundation.NSObject sender);

		[Action ("StartStopButtonClicked:")]
		partial void StartStopButtonClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ArduinoLoader != null) {
				ArduinoLoader.Dispose ();
				ArduinoLoader = null;
			}

			if (ArduinoStatus != null) {
				ArduinoStatus.Dispose ();
				ArduinoStatus = null;
			}

			if (PortSelector != null) {
				PortSelector.Dispose ();
				PortSelector = null;
			}

			if (TimerLabel != null) {
				TimerLabel.Dispose ();
				TimerLabel = null;
			}

			if (StartStopButton != null) {
				StartStopButton.Dispose ();
				StartStopButton = null;
			}
		}
	}
}
