﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;

namespace FeedMe
{
    public partial class MainPage : ContentPage
    {
        List<string> eatebles = new List<string> {
            "tomato soup",
            "tomato",
            "tomato sace",
            "tomato puree",
            "Felix Tomato Ketchup",
            "Heinz Tomato Ketchup",
            "Heinz Organic Tomato Ketchup",
            "Sir Kensington's Classic Ketchup",
            "First Field Jersey Ketchup",
            "Catskill Provisions All Natural Ketchup",
            "Victoria Amory Champagne Ketchup",
            "Whataburger Fancy Ketchup",
            "Muir Glen Organic Tomato Ketchup",
            "Sosu Srirachachup",
            "Stonewall Kitchen Country Ketchup",
            "True Made Ketchup",
            "Hunt's 100% Natural Tomato Ketchup",
            "Sainsbury’s tomato ketchup",
            "Tesco tomato ketchup",
            "Co-op Loved by Us tomato ketchup",
            "Waitrose Stokes real tomato ketchup",
            "Morrisons squeezy tomato ketchup,",
            "Aldi Bramwells tomato ketchup",
            "Lidl Kania tomato ketchup",
            "Asda Chosen by You tomato ketchup",
            "French's Tomato Ketchup"};
        List<string> selectedEatebles = new List<string>();

        //the selectedEatebles List won't work without this. I don't know why
        private List<string> ConvertToNonCrashgingMagicalList(List<string> badList)
        {
            List<string> goodList = new List<string>();

            //Makes an copy of the old list that is magical and won't crash the program
            for (int i = 0; i < badList.Count(); i++)
            {
                goodList.Add(selectedEatebles[i]);
            }

            return goodList; //return the magic list
        }

        public MainPage()
        {
            InitializeComponent();

            ListView_selectedEatebles.ItemsSource = selectedEatebles; //update listview for selected items
            DisplayAlert("response", "test", "ok");
            //Add test spagetti to eatebles
            for (int i = 0; i < 1000; i++)
            {
                eatebles.Add("spagetti" + i);
            }
        }


        // Post
        async void Post(string str)
        {
            var client = new HttpClient();
            HttpContent content = new StringContent(str);
            HttpResponseMessage response = await client.PostAsync(Constants.server_adress, content);

            HttpContent newcontent = response.Content;
            string json = await newcontent.ReadAsStringAsync();
            await DisplayAlert("response", json, "ok");
        }


        //Sorts list of strings, shortes to longest
        private List<string> SortByLength(List<string> list)
        {
            List<string> sortedList = new List<string>();

            int length = list.Count();

            //loop through unsorted list
            for (int i = 0; i < length; i++)
            {
                int indexShortest = 0; //the index for the shortest string in the list

                //find shortes
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Length < list[indexShortest].Length)
                    {
                        indexShortest = j;
                    }
                }

                sortedList.Add(list[indexShortest]);  //add the shortes string to the new list
                list.RemoveAt(indexShortest);         //and remove it from the old one
            }

            return sortedList;
        }

        //Find string in list. Returns list index
        private int FindIn(string str, List<string> list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] == str)
                {
                    return i;
                }
            }
            return -1;
        }



        //Updates the ListView when user types in "SearchBar_eatebles"
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<string> filterdEatebles = new List<string>();
            string searchWord = SearchBar_eatebles.Text.ToLower();


            if (searchWord != "")
            {

                //Put all eatebles that contains the searchWord in a new List (filterdEatebles)
                for (int i = 0; i < eatebles.Count; i++)
                {
                    if (eatebles[i].ToLower().Contains(searchWord))
                    {
                        filterdEatebles.Add(eatebles[i]);
                    }
                }
            }


            //Update xaml ListView and sorts the list
            ListView_eatebles.ItemsSource = SortByLength(filterdEatebles);
        }

        //Add items to selectedEatebles
        private void ListView_eatebles_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string item = ListView_eatebles.SelectedItem.ToString();

            //Check if item allready exist
            bool copy = false;
            for (int i = 0; i < selectedEatebles.Count(); i++)
            {
                if (selectedEatebles[i] == item)
                {
                    copy = true;
                }
            }
            if (!copy)
            {
                selectedEatebles.Add(item);

                ListView_selectedEatebles.ItemsSource = ConvertToNonCrashgingMagicalList(selectedEatebles); //Update "ListView_selectedEatebles"
            }
        }

        //Remove from selectedEatebles
        private void ListView_selectedEatebles_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Get selected item name
            string item = ListView_selectedEatebles.SelectedItem.ToString();

            //Find the index of the item
            int listindex = FindIn(item, selectedEatebles);

            //Remove the item from the list
            selectedEatebles.RemoveAt(listindex);

            //Update the listView
            ListView_selectedEatebles.ItemsSource = ConvertToNonCrashgingMagicalList(selectedEatebles);
        }

        // Go to the MealsList page
        private async void Button_Clicked(object sender, EventArgs e)
        {
            string[] str = { "spagetti", "sås"};
            Meal[] meals = { new Meal("spagett", str, "koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka vvvkoka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka vkoka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka", "Länk") };
            await Navigation.PushAsync(new MealsListPage(meals) { Title = "Bon Appétit" });
        }
    }
}
