using Android.Graphics;
using Android.Views;
using FeedMe.Droid.Effects;
using FeedMe.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("FeedMe")]
[assembly: ExportEffect(typeof(FrameCornerEffectAndroid), "FrameCornerEffect")]
namespace FeedMe.Droid.Effects
{
    public class FrameCornerEffectAndroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            Control.ClipToOutline = true;
            Control.OutlineProvider = new RoundedOutlineProvider(FrameCornerEffect.GetRadius(Element));
        }

        protected override void OnDetached()
        {
        }
    }

    class RoundedOutlineProvider : ViewOutlineProvider
    {
        private readonly double radius;

        public RoundedOutlineProvider(double radius)
        {
            this.radius = radius;
        }

        public override void GetOutline(Android.Views.View view, Outline outline)
        {
            outline?.SetRoundRect(0, 0, view.Width, view.Height, (float)radius);
        }
    }
}
