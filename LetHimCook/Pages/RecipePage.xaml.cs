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
    /// Логика взаимодействия для RecipePage.xaml
    /// </summary>
    public partial class RecipePage : Page
    {
        private List<RecipeTags> rt = Core.Context.RecipeTags.ToList();
        private List<Tags> tags = Core.Context.Tags.ToList();
        private List<Comments> comments = Core.Context.Comments.ToList();
        private List<Users> users = Core.Context.Users.ToList();

        Recipes recipe;
        
        public RecipePage(Recipes _recipe)
        {
            InitializeComponent();
            recipe = _recipe;
            DataContext = recipe;

            if(Core.user != null && Core.user.RoleID == 2)
            {
                AuthComplBTN.Visibility = Visibility.Collapsed;
                RecipeComplBTN.Visibility = Visibility.Collapsed;
                AuthDelBTN.Visibility = Visibility.Visible;
                RecipeDelBTN.Visibility = Visibility.Visible;
            }

            ReviewList.ItemsSource = null;
            ReviewList.ItemsSource = comments.Where(c => c.RecipeID == recipe.ID).ToList();
            int count = recipe.Comments.Count;
            CountTB.Text = "Кол-во отзывов: " + count;
            List<RecipeTags> recipeTags = rt.Where(c => c.RecipeID == recipe.ID).ToList();

            TagTB.Text = "Тэги: ";
            foreach (var tag in recipeTags)
            {
                TagTB.Text += tags.FirstOrDefault(c => c.ID == tag.TagID).Name.ToString();
                TagTB.Text += " ";
            }
        }


        private void ReviewBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            if(NavigationService.CanGoBack) NavigationService.GoBack();
        }

        private void ReadBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InstructionPage(recipe));
        }

        private void RecipeComplBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AuthComplBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CommentComplBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecipeDelBTN_Click(object sender, RoutedEventArgs e)
        {
            Users author = users.FirstOrDefault(p => p.ID == recipe.AuthorID);
            Judgment.Delete(recipe, author);
            if(NavigationService.CanGoBack) { NavigationService.GoBack(); }
        }

        private void AuthDelBTN_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
