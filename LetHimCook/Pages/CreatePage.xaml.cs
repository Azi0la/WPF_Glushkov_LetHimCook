using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Path = System.IO.Path;

namespace LetHimCook.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreatePage.xaml
    /// </summary>
    public partial class CreatePage : Page
    {
        private string _selectedImagePath;
        private string _savedRelativePath;
        Recipes recipe;
        private List<Recipes> recipes = Core.Context.Recipes.ToList();

        public CreatePage()
        {
            InitializeComponent();
        }

        private void SelectBTN_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.webp",
                Title = "Выберите фото рецепта"
            };

            if (dialog.ShowDialog() == true)
            {
                _selectedImagePath = dialog.FileName;

                
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_selectedImagePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                PreviewImage.Source = bitmap;
            }
        }

        private string SaveImageToRecipesFolder()
        {
            if (string.IsNullOrEmpty(_selectedImagePath) || !File.Exists(_selectedImagePath))
                return null;

            string recipesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecipesImages");

            Directory.CreateDirectory(recipesFolder);

            string extension = Path.GetExtension(_selectedImagePath);
            string uniqueName = $"{Guid.NewGuid()}{extension}";
            string destinationPath = Path.Combine(recipesFolder, uniqueName);

            File.Copy(_selectedImagePath, destinationPath, overwrite: true);

            return Path.Combine("RecipesImages", uniqueName).Replace('\\', '/');
        }

        private void SaveRecipeBTN_Click(object sender, RoutedEventArgs e)
        {

            string imagePathforDb = SaveImageToRecipesFolder();
            int ChosenDif = DifficultyComboBox.SelectedIndex + 1;
            if (imagePathforDb == null || string.IsNullOrEmpty(TitleTB.Text) || string.IsNullOrEmpty(DescriptionTB.Text) || string.IsNullOrEmpty(InstructionTB.Text) || ChosenDif == null
                || string.IsNullOrEmpty(CookingTimeTextBox.Text) || string.IsNullOrEmpty(ServingsTextBox.Text)) 
            {
                MessageBox.Show("Не все поля заполнены!");
            }
            else
            {
                var recipe = new Recipes
                {
                    AuthorID = Core.user.ID,
                    Title = TitleTB.Text,
                    Description = DescriptionTB.Text,
                    Instruction = InstructionTB.Text,
                    CookingTime = Convert.ToInt32(CookingTimeTextBox.Text),
                    Servings = Convert.ToInt32(ServingsTextBox.Text),
                    Rating = 0,
                    DifficultyID = ChosenDif,
                    MainImage = imagePathforDb,
                    ViewsCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                Core.Context.Recipes.Add(recipe);
                Core.Context.SaveChanges();
                MessageBox.Show("Рецепт создан!");
                NavigationService.Navigate(new MainPage());

            }

        }

        private void ServingsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
