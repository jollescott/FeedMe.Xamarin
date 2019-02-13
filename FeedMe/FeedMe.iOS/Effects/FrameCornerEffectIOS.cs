using System;
using FeedMe.Effects;
using FeedMe.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("FeedMe")]
[assembly: ExportEffect(typeof(FrameCornerEffectIOS), "FrameCornerEffect")]
namespace FeedMe.iOS.Effects
{
    public class FrameCornerEffectIOS : PlatformEffect
    {

        protected override void OnAttached()
        {
            var radius = (float)FrameCornerEffect.GetRadius(Element);
            Container.Layer.CornerRadius = radius;
            Container.Layer.MasksToBounds = true;
        }

        protected override void OnDetached()
        {
        }
    }
}
