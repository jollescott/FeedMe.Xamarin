namespace FeedMe.Effects;

public class FrameCornerEffect : RoutingEffect
{
    public static readonly BindableProperty RadiusProperty =
        BindableProperty.CreateAttached("Radius", typeof(double), typeof(FrameCornerEffect), 0.0);

    public FrameCornerEffect() : base("FeedMe.FrameCornerEffect")
    {
    }

    public static void SetRadius(BindableObject view, double radius)
    {
        view.SetValue(RadiusProperty, radius);
    }

    public static double GetRadius(BindableObject view)
    {
        return (double)view.GetValue(RadiusProperty);
    }
}