using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FeedMe.Core.Interfaces
{
    public interface IRamseyService
    {
        Task<bool> TestConnectionAsync();
    }
}
