using Ramsey.Shared.Dto.V2;
using System.Drawing;

namespace FeedMe.Models
{
    public class IngredientListModel
    {
        public IngredientDtoV2 Ingredient { get; set; }
        public bool IsAdded { get; set; }

        public Color AddedColor { get; set; } = Constants.AppColor.green;
        public Color NotAddedColor { get; set; } = Constants.AppColor.text_defult;
        public Color DefultColor { get; set; } = Constants.AppColor.text_defult;

        public string AddedIcon { get; set; } = Constants.DeleteIngredientCheckIcon;
        public string NotAddedIcon { get; set; } = Constants.AddIngredientCheckIcon;
        public string DefultIcon { get; set; } = Constants.DeleteIngredientIcon;

        public string IngredientName => Ingredient.IngredientName;
        public string Icon => (IsAdded) ? AddedIcon : NotAddedIcon;
        public Color Color => (IsAdded) ? AddedColor : NotAddedColor;
    }
}
