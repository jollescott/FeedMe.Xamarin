using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMe.Pages.MasterDetail
{

    public class FDMasterDetailPageMenuItem
    {
        public FDMasterDetailPageMenuItem()
        {
            TargetType = typeof(FDMasterDetailPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}