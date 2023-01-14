using FeedMe.Core.ViewModels;
using FeedMe.Pages.MasterDetail;
using MvvmCross.Forms.Views;
using Ramsey.Shared.Misc;
using System;
using System.Net.Http;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : MvxContentPage<LoadingViewModel>
    {
        public LoadingPage()
        {
            InitializeComponent();
        }
    }
}