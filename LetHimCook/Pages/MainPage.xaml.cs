using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LetHimCook.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private List<Recipes> recipes = Core.Context.Recipes.ToList();
        private List<RecipeTags> rt = Core.Context.RecipeTags.ToList();
        private List<Tags> t = Core.Context.Tags.ToList();

        private List<Tags> tags = Core.Context.Tags.ToList();
        private List<Tags> selectedTags = new List<Tags>();

        public MainPage()
        {
            InitializeComponent();
            List<String> sorting = new List<String> { "По Названию", "По Оценке" };
            RecipeList.ItemsSource = recipes;
            FilterBox.ItemsSource = sorting;
            TagList.ItemsSource = tags;

        }

        private void Search()
        {
            var FilterRecipes = Core.Context.Recipes.ToList();

            if (SearchBar == null && FilterBox == null && TagList == null) return;

            if (!string.IsNullOrWhiteSpace(SearchBar.Text))
            {
                FilterRecipes = FilterRecipes.Where(p => p.Title.ToLower().Contains(SearchBar.Text.ToLower())
                || p.Users.Name.ToLower().Contains(SearchBar.Text.ToLower())).ToList();
            }

            if (FilterBox.SelectedIndex == 0)
            {
                FilterRecipes = FilterRecipes.OrderBy(p => p.Title).ToList();
            }
            else if (FilterBox.SelectedIndex == 1)
            {
                FilterRecipes = FilterRecipes.OrderByDescending(p => p.Rating).ToList();
            }


            FilterRecipes = FilterRecipes.Where(fb => selectedTags.All(sg => fb.RecipeTags.Any(bg => bg.TagID == sg.ID))).ToList();


            RecipeList.ItemsSource = FilterRecipes;
        }

        private void TagBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            selectedTags.Add(t.FirstOrDefault(tags => tags.Name == checkBox.Content.ToString()));
        }

        private void TagBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            selectedTags.Remove(t.FirstOrDefault(tags => tags.Name == checkBox.Content.ToString()));
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search();
        }

        private void RecipeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RecipePage(RecipeList.SelectedItem as Recipes));
        }

        private void TagBox_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
