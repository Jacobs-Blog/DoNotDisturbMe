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
		AppKit.NSMenuItem PortsMenu { get; set; }

		[Outlet]
		AppKit.NSButton StartStopButton { get; set; }

		[Outlet]
		AppKit.NSMenu StatusMenu { get; set; }

		[Outlet]
		AppKit.NSTextField TimerLabel { get; set; }

		[Action ("NumberChanged:")]
		partial void NumberChanged (Foundation.NSObject sender);

		[Action ("StartStopButtonClicked:")]
		partial void StartStopButtonClicked (Foundation.NSObject sender);

		[Action ("StatusBarExitClicked:")]
		partial void StatusBarExitClicked (Foundation.NSObject sender);

		[Action ("StatusBarTenMinutesClicked:")]
		partial void StatusBarTenMinutesClicked (Foundation.NSObject sender);

		[Action ("StatusBarTenMinutesItem:")]
		partial void StatusBarTenMinutesItem (Foundation.NSObject sender);

		[Action ("StatusBarTwentyMinutesClicked:")]
		partial void StatusBarTwentyMinutesClicked (Foundation.NSObject sender);

		[Action ("StatusBarTwentyMinutesItem:")]
		partial void StatusBarTwentyMinutesItem (Foundation.NSObject sender);
		
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

			if (PortsMenu != null) {
				PortsMenu.Dispose ();
				PortsMenu = null;
			}

			if (StartStopButton != null) {
				StartStopButton.Dispose ();
				StartStopButton = null;
			}

			if (StatusMenu != null) {
				StatusMenu.Dispose ();
				StatusMenu = null;
			}

			if (TimerLabel != null) {
				TimerLabel.Dispose ();
				TimerLabel = null;
			}
		}
	}
}
