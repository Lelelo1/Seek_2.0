using System;
using UIKit;


namespace Seek.iOS
{
    public class Application
    {



        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
            
            // (does never go belpw UIAppliaction.Main call..)
        }
    }
}
