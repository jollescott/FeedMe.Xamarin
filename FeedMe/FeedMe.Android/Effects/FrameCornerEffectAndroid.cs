using Android.Graphics;
using Android.Views;
using FeedMe.Android.Effects;
using FeedMe.Effects;
using Microsoft.Maui.Controls.Platform;
using View = Android.Views.View;

[assembly: ResolutionGroupName("FeedMe")]
[assembly: ExportEffect(typeof(FrameCornerEffectAndroid), "FrameCornerEffect")]

namespace FeedMe.Android.Effects;

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

internal class RoundedOutlineProvider : ViewOutlineProvider
{
    private readonly double _radius;

    public RoundedOutlineProvider(double radius)
    {
        _radius = radius;
    }

    public override void GetOutline(View view, Outline outline)
    {
        outline?.SetRoundRect(0, 0, view.Width, view.Height, (float)_radius);
    }
}