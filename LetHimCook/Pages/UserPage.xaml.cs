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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        Users user;
        private static List<Recipes> recipes = Core.Context.Recipes.ToList();
        public UserPage(Users _user)
        {
            InitializeComponent();
            user = _user;
            DataContext = user;
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            HistoryList.ItemsSource = user.Recipes?.OrderByDescending(r => r.CreatedAt).ToList();
        }

        private void ChangePassBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PassChangePage());
        }

        private void ChangeAvatarBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RedactBioBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelBTN_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryList.SelectedItem is Recipes selectedRecipe)
            {
                var result = MessageBox.Show(
                    $"Удалить рецепт «{selectedRecipe.Title}»?\nВсе комментарии к нему тоже будут удалены.",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                    return;

                Judgment.Delete(selectedRecipe, user);

                LoadRecipes();
                DelBTN.IsEnabled = false;
                MessageBox.Show("Рецепт и связанные комментарии успешно удалены.",
                                "Готово", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DelBTN.IsEnabled = HistoryList.SelectedItem != null;
        }

        private void LeaveBTN_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                    $"Выйти из аккаунта «{user.Name}»?\n",
                    "Подтверждение выхода",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;
            Core.user = null;

            //var mainWindow = Window.GetWindow(this) as MainWindow;
            //if (mainWindow != null)
            //{
            //    mainWindow.ModButt.Visibility = Visibility.Collapsed;
            //}

            NavigationService.Navigate(new MainPage());
        }
    }
}
