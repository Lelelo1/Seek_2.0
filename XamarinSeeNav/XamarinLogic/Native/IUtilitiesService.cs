using System;
using System.Threading.Tasks;
using XamarinLogic.Models;

namespace XamarinLogic.Native
{
    // https://stackoverflow.com/questions/43687689/keeping-screen-turned-on-for-certain-pages
    public interface IUtilitiesService : INative
    {
        User GetUser();
        delegate void ScreenStayOn(bool yes);
        ScreenStayOn ScreenStayOnMethod { get; }
        Runtime Runtime { get; }
    }
    // currently binded together with playfab analtyics in backend
    public enum Runtime
    {
        Debug,
        Testing,// test flight ios
        Production
    }
}
