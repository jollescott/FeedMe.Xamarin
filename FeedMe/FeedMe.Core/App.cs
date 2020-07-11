using FeedMe.Core.Interfaces;
using FeedMe.Core.Services;
using FeedMe.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace FeedMe.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<IRamseyService, RamseyService>();
            RegisterAppStart<LoadingViewModel>();
        }
    }
}
