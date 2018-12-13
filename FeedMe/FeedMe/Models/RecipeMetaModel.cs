using Ramsey.Shared.Dto;
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

        public bool IsAd { get; set; }
    }
}
