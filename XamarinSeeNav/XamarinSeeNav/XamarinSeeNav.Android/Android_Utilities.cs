using System;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Native;

namespace Seek.Droid
{
    public class Android_Utilities : IUtilitiesService
    {
        // using essenials screen instead!

        public IUtilitiesService.ScreenStayOn ScreenStayOnMethod => (yes) => { } ;

        public Runtime Runtime => Runtime.Debug;

        public Task<User> GetUserAsync()
        {
            return Task.FromResult(new User(true, "Android_Utilities id"));
        }
    }
}
