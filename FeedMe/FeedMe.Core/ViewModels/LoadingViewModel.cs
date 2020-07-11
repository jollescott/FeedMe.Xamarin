using FeedMe.Core.Interfaces;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FeedMe.Core.ViewModels
{
    public class LoadingViewModel : MvxViewModel
    {
        private bool _connectionFailed;
        private readonly IRamseyService _ramseyService;

        public bool ConnectionFailed
        {
            get => _connectionFailed;
            set => SetProperty(ref _connectionFailed, value);
        }

        public LoadingViewModel(IRamseyService ramseyService)
        {
            _ramseyService = ramseyService;
        }

        public override async Task Initialize()
        {
            await TestConnectionAsync();
        }

        private async Task TestConnectionAsync()
        {
            ConnectionFailed = false;

            var online = await _ramseyService.TestConnectionAsync();

            if (online)
            {
                //TODO: Navigation
            }
            else
            {
                ConnectionFailed = true;
            }
        }
    }
}
