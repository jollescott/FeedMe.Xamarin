﻿ using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FeedMe
{
    public class Meal
    {
        public readonly string name;
        public readonly string[] ingredients;
        public readonly string recipe;
        public readonly int rating = 4;
        public readonly int ratings = 27;
        public readonly int time = 10;
        public readonly int yield= 1; 
        public readonly string imageLink;

        public Meal() { }

        public Meal(string name, string[] ingredients, string recipe, string imageLink = "img_1.png")
        {
            this.name = name;
            this.ingredients = ingredients;
            this.recipe = recipe;
            this.imageLink = imageLink;
        }
    }
}
