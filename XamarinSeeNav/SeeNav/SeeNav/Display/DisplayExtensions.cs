using System;
using System.Linq;
using Xamarin.Forms;

namespace SeeNav.Display
{
    public static class DisplayExtensions
    {
        public static void Fill(this BindableObject bindable, AbsoluteLayoutFlags flag, double x = 0, double y = 0, double w = 0, double h = 0)
        {
            AbsoluteLayout.SetLayoutFlags(bindable, flag);
            AbsoluteLayout.SetLayoutBounds(bindable, new Rectangle(x, y, w, h));
        }
        public static void Add(this Layout<View> layout, params View[] views)
        {
            views.ToList().ForEach(b => layout.Children.Add(b));
        }

        public static void AddWhenNotExist(this AbsoluteLayout layout, View view)
        {
            if(view == null)
            {
                return;
            }

            if(!layout.Children.Contains(view))
            {
                layout.Children.Add(view);
            }
        }

        public static void RemoveWhenExist(this AbsoluteLayout layout, View view)
        {
            if(view == null)
            {
                return;
            }

            if (layout.Children.Contains(view))
            {
                layout.Children.Remove(view);
            }
        }
    }
}
