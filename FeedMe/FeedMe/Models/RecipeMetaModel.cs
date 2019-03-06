using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Enums;

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

        public int FrameHeight { get; set; }

        public string CoverageMessage { get; set; }
        public bool ShowCoverageMessage { get; set; }
        public double LogoRadius { get; set; }
        public double LogoDiameter { get { return 2 * LogoRadius; } set { LogoRadius = value / 2; } }

        public RecipeDtoV2 Recipe { get; set; }

        public bool IsAd { get; set; }
    }
}
