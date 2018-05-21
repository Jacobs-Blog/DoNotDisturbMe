using AppKit;
using Foundation;
using System.Runtime.Remoting;

namespace DoNotDisturbMe
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
		

        public AppDelegate()
        {
			
        }

  //      [Export("awakeFromNib")]
		//public void AwakeFromNib()
		//{
		//	throw new System.NotImplementedException();
		//}

        public override void DidFinishLaunching(NSNotification notification)
        {
			       
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
