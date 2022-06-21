using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Seek.iOS;
using Logic.Ahrs;

[assembly: Dependency(typeof(ResourcesPath))]
namespace Seek.iOS
{
    public class ResourcesPath : IResourcesPath
    {
        public string Path { get => NSBundle.MainBundle.BundlePath + "/"; }
    }
}