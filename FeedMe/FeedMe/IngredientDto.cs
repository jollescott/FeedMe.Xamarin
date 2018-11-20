using System;
using System.Collections.Generic;
using System.Text;

namespace FeedMe
{
    public class IngredientDto
    {
        public string Name { get; set; }
        public List<RecipePartDto> RecipeParts { get; set; }
    }
}
