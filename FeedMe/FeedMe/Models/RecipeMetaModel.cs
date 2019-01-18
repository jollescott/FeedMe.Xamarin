using Ramsey.Shared.Dto;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FeedMe.Models
{
    public class RecipeMetaModel 
    {
        public string RecipeID { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public RecipeProvider Owner { get; set; }
        public string OwnerLogo { get; set; }
        public string Image { get; set; }

        public string CoverageMessage { get; set; }
        public double LogoRadius { get; set; }
        public double LogoDiameter { get { return 2 * LogoRadius; } set { LogoRadius = value / 2; } }

        public bool IsAd { get; set; }
    }
}
