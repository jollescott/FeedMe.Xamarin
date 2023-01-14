using FeedMe.Effects;
using FeedMe.iOS.Effects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

[assembly: ResolutionGroupName("FeedMe")]
[assembly: ExportEffect(typeof(FrameCornerEffectIOS), "FrameCornerEffect")]
namespace FeedMe.iOS.Effects
{
    public class FrameCornerEffectIOS : PlatformEffect
    {

        protected override void OnAttached()
        {
            float radius = (float)FrameCornerEffect.GetRadius(Element);
            Container.Layer.CornerRadius = radius;
            Container.Layer.MasksToBounds = true;
        }

        protected override void OnDetached()
        {
        }
    }
}
