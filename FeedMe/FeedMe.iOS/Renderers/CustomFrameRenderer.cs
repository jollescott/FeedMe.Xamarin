using CoreGraphics;
using FeedMe.iOS.Renderers;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

[assembly: ExportRenderer(typeof(Frame), typeof(CustomFrameRenderer))]
namespace FeedMe.iOS.Renderers
{
    public class CustomFrameRenderer : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
        }
    }
}