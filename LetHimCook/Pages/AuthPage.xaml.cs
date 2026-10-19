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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private int FailAttempts = 0;
        private Random random = new Random();
        private List<Users> users = Core.Context.Users.ToList();

        public AuthPage()
        {
            InitializeComponent();
            
        }

        private void ForgotButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }

        private void AuthButt_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text.Trim();
            string pass = PassMaker.GetHash(PassTB.Password);

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(PassTB.Password)) 
            {
                MessageBox.Show("Заполните все поля!");
                LoginTB.Clear();
                PassTB.Clear();
                return;
            }
            if (users.FirstOrDefault(p => p.Name == login && p.Password == pass) != null) 
            {
                Core.user = users.FirstOrDefault(p => p.Name == login && p.Password == pass);
                MessageBox.Show("Вход выполнен!");
                NavigationService.Navigate(new MainPage());
                FailAttempts = 0;
            }
            else
            {
                FailAttempts++;
                LoginTB.Clear();
                PassTB.Clear();

                if(FailAttempts == 3)
                {
                    ShowCaptcha();
                    FailAttempts = 0;
                }

                MessageBox.Show("Неправильный логин или пароль!");
            }

        }



        private void CaptchaBTN_Click(object sender, RoutedEventArgs e)
        {
            if(CaptchaInput.Text.Trim() != Captcha.Text)
            {
                CaptchaInput.Clear();
                CaptchaChange();
                MessageBox.Show("Неверная капча. Попробуйте ещё раз.");
            }
            else
            {
                CaptchaInput.Clear();
                CaptchaGrid.Visibility = Visibility.Collapsed;
                LoginGrid.Visibility = Visibility.Visible;
                PassGrid.Visibility = Visibility.Visible;
                AuthButt.IsEnabled = true;
            }
        }

        private void ShowCaptcha()
        {
            CaptchaInput.Clear();
            CaptchaChange();
            CaptchaGrid.Visibility = Visibility.Visible;
            LoginGrid.Visibility = Visibility.Collapsed;
            PassGrid.Visibility = Visibility.Collapsed;
            AuthButt.IsEnabled = false;
        }

        private void CaptchaChange()
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] captcha = new char[6];

            for (int i = 0; i < captcha.Length; i++)
            {
                captcha[i] = characters[random.Next(characters.Length)];
            }

            Captcha.Text = new string(captcha);
        }

        private void LoginTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginPlaceholder.Visibility = string.IsNullOrEmpty(LoginTB.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoginTB_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void LoginTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTB.Text)) LoginPlaceholder.Visibility = Visibility.Visible;
        }

        private void PassTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PassPlaceholder.Visibility = string.IsNullOrEmpty(PassTB.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PassTB_GotFocus(object sender, RoutedEventArgs e)
        {
            PassPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void PassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PassTB.Password)) PassPlaceholder.Visibility = Visibility.Visible;
        }

        private void CaptchaInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            CaptchaPlaceholder.Visibility = string.IsNullOrEmpty(CaptchaInput.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CaptchaInput_GotFocus(object sender, RoutedEventArgs e)
        {
            CaptchaPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void CaptchaInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CaptchaInput.Text)) CaptchaPlaceholder.Visibility = Visibility.Visible;
        }
    }
}
