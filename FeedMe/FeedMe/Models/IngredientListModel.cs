using Ramsey.Shared.Dto.V2;
using System.Drawing;

namespace FeedMe.Models
{
    public class IngredientListModel
    {
        public IngredientDtoV2 Ingredient { get; set; }
        public Color Color { get; set; }
        public bool IsAdded { get; set; }
        public string AddedIcon { get; set; } = Constants.DeleteIngredientCheckIcon;
        public string NotAddedIcon { get; set; } = Constants.AddIngredientCheckIcon;
        public string DefultIcon { get; set; } = Constants.DeleteIngredientIcon;

        public string IngredientName { get { return Ingredient.IngredientName; } }
        public string Icon { get { return (IsAdded) ? AddedIcon : NotAddedIcon; } }
    }
}
