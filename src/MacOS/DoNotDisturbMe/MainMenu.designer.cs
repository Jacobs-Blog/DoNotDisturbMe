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
	[Register ("MainMenu")]
	partial class MainMenu
	{
		[Outlet]
		AppKit.NSMenuItem MenuItemExit { get; set; }
        
		[Action ("ExitClicked:")]
		partial void ExitClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (MenuItemExit != null) {
				MenuItemExit.Dispose ();
				MenuItemExit = null;
			}
		}
	}
}
