using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using Seek.Visualization.Contents.Large;
using Rectangle = Logic.Game.Models.Rectangle;
using Size = Logic.Game.Models.Size;
using Logic;
using Logic.Game;

namespace Seek.Visualization.Support
{
    public class Animations
    {
        static Animations get;
        public static Animations Get
        {
            get
            {
                if (get == null)
                {
                    get = new Animations();
                }
                return get;
            }
        }

        public Values Values { get; set; }

        void TryInit(Bubble bubble)
        {
            if (Values != null)
            {
                return;
            }
            var small = Constants.Size;
            var smallColor = bubble.BackgroundColor;
            var diffAlfa = 0.92 - 0.80;
            var view = Logic.DependencyBox.Get<Projector>().ProjectorConfig.ViewSize;

            var width = view.Width * 0.80;
            var height = view.Height * 0.80;

            var diffW = (width - small.Width);
            var diffH = (height - small.Height);

            // animate content shift nicely:
            var shiftPortion = 0.6; // faster than normal shrink to leave room for the redisplay of the label

            Values = new Values(small, smallColor, diffAlfa, diffW, diffH, shiftPortion);

        }

        public void Enlarge(double ratio, Bubble bubble)
        {
            TryInit(bubble);

            var increaseWidth = Values.Small.Width + Values.DiffW * ratio;
            var increaseHeight = Values.Small.Height + Values.DiffH * ratio;

            var pos = bubble.Spatial.Rectangle;
            bubble.Spatial.Rectangle = new Rectangle(pos.X, pos.Y, increaseWidth, increaseHeight);

            var cornerRadius = Constants.CornerRadius_Normal - (45 * (float)ratio);
            bubble.CornerRadius = cornerRadius;

            // removing transparency
            var c = Values.SmallColor;
            var increaseAlfa = c.A + Values.DiffAlfa * ratio;
            bubble.BackgroundColor = new Color(c.R, c.G, c.B, increaseAlfa);
        }

        public void Shrink(double ratio, Bubble bubble)
        {
            TryInit(bubble);

            var diffW = (bubble.Spatial.Rectangle.Width - Values.Small.Width);
            var diffH = (bubble.Spatial.Rectangle.Height - Values.Small.Height);
            var changeWidth = bubble.Spatial.Rectangle.Width - diffW * ratio;
            var changeHeight = bubble.Spatial.Rectangle.Height - diffH * ratio;

            var pos = bubble.Spatial.Rectangle;

            bubble.Spatial.Rectangle = new Rectangle(pos.X, pos.Y, changeWidth, changeHeight);

            // increasing / resetting transparency
            var c = Values.SmallColor;
            var decreaseAlfa = c.A - Values.DiffAlfa * (ratio);
            bubble.BackgroundColor = new Color(c.R, c.G, c.B, decreaseAlfa);
        }
        public void ShrinkCornerRadius(double ratio, Bubble bubble)
        {
            var cornerRadius = Constants.CornerRadius_Enlarged + (45 * (float)(ratio));// 45 radius change appear slower than the rest of the animation
            if (cornerRadius < 0)
            {
                cornerRadius = 0;
            }
            bubble.CornerRadius = cornerRadius;
        }
        public void ShrinkShiftContent(double ratio, Bubble bubble)
        {
            TryInit(bubble);

            // until a suitable generic template for Default content is created - the Label should not shift opacity
            if (bubble._Content.Get is Default d == false)
            {
                if (ratio > 0.85) // pre start
                {
                    // var remainingTime = (uint)(Constants.Animation.Size_Deselect_Duration * (1 - Values.ShiftPortion));
                    InsertSmallContent(ratio, bubble);
                }
            }
        }
        public void InsertSmallContent(double ratio, Bubble bubble)
        {
            if(!bubble.IsSelected)
            {
                Console.WriteLine("InsertSmallContent: " + bubble.IsSelected);
                bubble.Content = bubble._Content.Get.Content;
            }
        }

        public List<DataTemplate> AnimTemplates { get; set; }

        List<string> RegistredAnimations { get; set; } = new List<string>();
        public void RegisterOnce(string name)
        {
            if (!RegistredAnimations.Contains(name))
            {
                RegistredAnimations.Add(name);
            }
        }
        public bool IsRunning(Bubble bubble)
        {
            var hasAnimationsRunning = RegistredAnimations.Any((animation) =>
            {
                return bubble.AnimationIsRunning(animation);
            });
            return hasAnimationsRunning;
        }
        public void AbortAll(Bubble bubble)
        {
            foreach (var animation in RegistredAnimations)
            {
                bubble.AbortAnimation(animation);
            }
            RegistredAnimations.Clear();
        }
    }
    public class Values
    {
        public Size Small { get; set; }
        public Color SmallColor { get; set; }
        public double DiffAlfa { get; set; }
        public double DiffW { get; set; }
        public double DiffH { get; set; }
        public double ShiftPortion { get; set; }
        public Values(Size small, Color smallColor, double diffAlfa, double diffW, double diffH, double shiftPortion )
        {
            Small = small;
            SmallColor = smallColor;
            DiffAlfa = diffAlfa;
            DiffW = diffW;
            DiffH = diffH;
            ShiftPortion = shiftPortion;
        }
    }
    
}
