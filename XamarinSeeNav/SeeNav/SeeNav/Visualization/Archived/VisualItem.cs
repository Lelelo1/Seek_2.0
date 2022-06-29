using System;
using System.Threading.Tasks;
using Xamarin.Forms;
//using LogicLibrary.Search.Models;

/* old visual items used by old projection and Extenions class*/
namespace SeeNav.Visualization.VisualItems.Archived
{
    /*
    public interface IVisualItem
    {
        Target Target { get; set; }
        bool IsHidden { get; set; } // to signal to visualization removal or deletion
        void ShowFullscreen(bool show);
        IContent FullscreenContent { get; }
    }
    public class VisualItem : Frame, IVisualItem
    {
        public Target Target { get; set; }
        public bool IsHidden { get; set; }

        public void ShowFullscreen(bool show)
        {
            if(show)
            {
                Margin = 0;
                Padding = 0; // frame had automatic paddding
                Content = FullscreenContent.Get();
            }
        }

        IContent _fullscreenContent;
        public IContent FullscreenContent
        {
            get
            {
                if(_fullscreenContent == null)
                {
                    IContent content = null;
                    throw new Exception("old projection with visualitem no longer supported, LabelTemplate to VisualItem is needed");
                    var with = new With(null, Target.Place);
                    switch(Target.Place.Kind)
                    {
                        case Kind.Place:
                            content = new Google(with);
                            break;
                    }
                    _fullscreenContent = content;
                }
                return _fullscreenContent;
            }
        }

        // handle resizing of content, opacity. depending on distance to result
        public double maximumOpacity = 0.3;
        public double minimumOpcity = 0.8;
        public Label label;
        public VisualItem(Target target)
        {
            Target = target;

            // in minimized format
            label = new Label();
            if (target.Place != null)
            {
                label.Text = target.Place.Name;
            }
            else
            {

            }
            Content = label;
            // Content displayed differenely depening on i'ts kind. Matters the most in fullscreen though
            // make method that can be used to set both litte and large visual items.

            // use nuget to get auto sized text: Forms9Patch
            
            

        }
        
    }
    */
    

    /*
    public class VisualFullscreenItem : IVisualItem
    {
        public Target Target { get; set; }
        public bool IsPresentedInFullscreen { get; set; }
    }
    */
}
