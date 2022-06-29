using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SeeNav.Display.Internal;

namespace SeeNav.Display
{
    // handles visibility of views, with opacity and inputtransparent, and sesnorfade depending application state
    public class Extended
    {

        static List<ExtendedView> ExtendedViews { get; } = new List<ExtendedView>();
        
        public static ExtendedView Get(View view)
        {
            var extendedView = ExtendedViews.Find(ev => ev.View == view);
            if(extendedView == null)
            {
                extendedView = new ExtendedView(view);
                ExtendedViews.Add(extendedView);
            }

            return extendedView;
        }
    }
}
