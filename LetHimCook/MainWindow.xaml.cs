using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Path = System.IO.Path;

namespace LetHimCook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            MainFrame.NavigationService.Navigate(new Pages.MainPage());

        }

        private bool IsDarkTheme = false;


        private void StyleButton_Click(object sender, RoutedEventArgs e)
        {
            IsDarkTheme = !IsDarkTheme;

            string theme = "";
            if (IsDarkTheme)
            {
                theme = "Dark.xaml";
                SetImage(HomeImage, "home.png");
                SetImage(EditImage, "edit.png");
                SetImage(AccImage, "account.png");
                //SetImage(ModImage, "admin.png");
                SetImage(ThemeImage, "settings-sliders.png");
            }
            else { 
                theme = "Light.xaml";
                SetImage(HomeImage, "Dark-home.png");
                SetImage(EditImage, "Dark-edit.png");
                SetImage(AccImage, "Dark-user.png");
                //SetImage(ModImage, "Dark-admin.png");
                SetImage(ThemeImage, "Dark-settings-sliders.png");  // именно sliders (с s)
            }

            var dictionary = new ResourceDictionary
            {
                Source = new Uri($"/Styles/{theme}", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictionary);


        }

        private void SetImage(Image image, string fileName)
        {
            image.Source = new BitmapImage(
                new Uri($"pack://application:,,,/res/{fileName}", UriKind.Absolute));
        }

        private void MainButt_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                if (button.Tag != null)
                {

                    if (button.Tag != null)
                    {
                        switch (button.Tag)
                        {
                            case "1":
                                MainFrame.Navigate(new Pages.MainPage());
                                break;
                            case "2":
                                if (Core.user == null)
                                {
                                    MessageBox.Show("Войдите в аккаунт для возможности выкладывать рецепты");
                                    MainFrame.Navigate(new Pages.AuthPage());
                                }
                                else MainFrame.Navigate(new Pages.CreatePage());
                                break;
                            case "3":
                                if(Core.user == null) MainFrame.Navigate(new Pages.AuthPage());
                                else MainFrame.Navigate(new Pages.UserPage(Core.user));
                                break;
                            case "4":
                                MainFrame.Navigate(new Pages.ModerationPage());
                                break;
                        }

                    }
                }

            }
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = MessageBox.Show("Вы действительно хотите закрыть приложение?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes;
        }

    }
}
