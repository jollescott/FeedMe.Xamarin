using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMe
{

    public class FDMasterPageMenuItem
    {
        public FDMasterPageMenuItem()
        {
            TargetType = typeof(FDMasterPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}