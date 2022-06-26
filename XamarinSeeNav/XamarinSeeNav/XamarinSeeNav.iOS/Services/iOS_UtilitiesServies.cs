using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using LogicLibrary.Native;
using System.Threading.Tasks;
using CloudKit;
using LogicLibrary.Utils;
using LogicLibrary.Models;

[assembly: Dependency(typeof(Seek.iOS.Services.iOS_UtilitiesService))]
namespace Seek.iOS.Services
{
    public class iOS_UtilitiesService : IUtilitiesService
    {

        string DeviceId { get; } = UIDevice.CurrentDevice.IdentifierForVendor.AsString();

        User User { get; set; }

        public User GetUser() // 'FetchUserRecordIdAsync' don't give errows and goes to infinite await
        {
            if (Ext.HasValue(User))
            {
                return User;
            }

            var id = GetLoggedInId();

            User = CreateUser(id);

            return User;

        }

        User CreateUser(string id)
        {
            var loggedIn = !string.IsNullOrEmpty(id);
            id = loggedIn ? id : DeviceId;
            return new User(loggedIn, id);
        }

        string GetLoggedInId()
        {
            return NSFileManager.DefaultManager.UbiquityIdentityToken?.DebugDescription;
        }


        // Utillities...

        public Runtime Runtime
        {
            get
            {
              
#if DEBUG
                return Runtime.Debug;
#endif
               
                // https://stackoverflow.com/questions/26081543/how-to-tell-at-runtime-whether-an-ios-app-is-running-through-a-testflight-beta-i

                if (NSBundle.MainBundle.AppStoreReceiptUrl?.LastPathComponent.Contains("sandboxReceipt") == true)
                {
                    return Runtime.Testing;
                }

                return Runtime.Production;
            }
        }

        public IUtilitiesService.ScreenStayOn ScreenStayOnMethod => this.ScreenStayOn;

        public void ScreenStayOn(bool yes)
        {
            UIApplication.SharedApplication.IdleTimerDisabled = true;
        }


    }
}
