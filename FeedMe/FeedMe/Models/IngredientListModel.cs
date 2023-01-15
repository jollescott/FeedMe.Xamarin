using Ramsey.Shared.Dto.V2;
using System.Drawing;
using FeedMe.Classes;
using Color = Microsoft.Maui.Graphics.Color;

namespace FeedMe.Models
{
    public class IngredientListModel
    {
        public IngredientDtoV2 Ingredient { get; set; }
        public bool IsAdded { get; set; }

        public Color AddedColor { get; set; } = Constants.AppColor.Green;
        public Color NotAddedColor { get; set; } = Constants.AppColor.TextDefault;
        public Color DefultColor { get; set; } = Constants.AppColor.TextDefault;

        public string AddedIcon { get; set; } = Constants.DeleteIngredientCheckIcon;
        public string NotAddedIcon { get; set; } = Constants.AddIngredientCheckIcon;
        public string DefultIcon { get; set; } = Constants.DeleteIngredientIcon;

        public string IngredientName => Ingredient.IngredientName;
        public string Icon => (IsAdded) ? AddedIcon : NotAddedIcon;
        public Color Color => (IsAdded) ? AddedColor : NotAddedColor;
    }
}
