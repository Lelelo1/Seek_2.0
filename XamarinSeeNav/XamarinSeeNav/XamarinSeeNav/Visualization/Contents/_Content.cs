using Xamarin.Forms;
using Seek.Visualization.Contents.Large;
using Seek.Visualization.Support;

namespace Seek.Visualization.Contents
{
    public class _Content // should probably be renamed to something like activator or something
    {
        public Templates Templates { get; set; }
        public _Content(Bubble bubble)
        {
            Templates = new Templates(bubble);
        }

       
        ContentView _small;
        ContentView Small
        {
            get
            {
                if(_small == null)
                {
                    _small = new Template<Small>(Templates).Create();
                }
                return _small;
            }
        }
        
        ContentView _large;
        ContentView Large
        {
            get
            {
                if(_large == null)
                {
                    _large = CreateLarge();
                }
                return _large;
            }
        }
        public ContentView Get => Templates.Bubble.IsSelected ? Large : Small; // can return the content - only the contentview for some reason

        ContentView CreateLarge()
        {
            /*
            ContentView content;
            switch (Templates.Visual.Place.Kind)
            {
                case Kind.Place:
                    content = new Template<Google>(Templates).Create();
                    break;
                default:
                    content = new Template<Default>(Templates).Create();
                    break;
            }
            return content;
            */

            return new Template<Default>(Templates).Create();
        }
    }

    class Template<T> where T : ContentView, new()
    {
        DataTemplate DataTemplate { get; set; }
        public Template(Templates templates)
        {
            // LogicLibrary.Utils.Log.Message("new datatemplate");
            DataTemplate = new DataTemplate(() =>
            {
                var content = (IContent)new T();
                content.Set(templates); // no parameters in constructor are alloed on ContentView sub typeswhen using generics
                return content;
            });
        }
        public T Create() => (T)DataTemplate.CreateContent();
    }
}
