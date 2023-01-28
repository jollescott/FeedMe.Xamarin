using Plugin.Iconize;

namespace FeedMe;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<FormsApp>()
            .UseIconize();

        return builder.Build();
    }
}