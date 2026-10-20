using System;
using System.Collections.Generic;
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

namespace LetHimCook.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        private static readonly Regex PassRegex = new Regex(
        @"^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^*]{8,}$",
        RegexOptions.Compiled);
        private List<Users> users = Core.Context.Users.ToList();


        public RegPage()
        {
            InitializeComponent();
            

        }


        

        private void RegButt_Click(object sender, RoutedEventArgs e)
        {
            string Name = LoginTB.Text.Trim();
            string Email = EmailTB.Text.Trim();
            string Pass = PassMaker.GetHash(PassTB.Password);
            string PassDub = PassMaker.GetHash(PassDubTB.Password);

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(EmailTB.Text) || string.IsNullOrEmpty(PassTB.Password) || string.IsNullOrEmpty(PassDubTB.Password)) 
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            
            if (Pass != PassDub) 
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            if (PassRegex.IsMatch(PassTB.Password))
            {
                if(Core.Context.Users.Any(p => p.Name == Name))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }
                Core.user = new Users
                {
                    Name = Name,
                    Email = Email,
                    Password = Pass,
                    AvatarUrl = "monke.jpg",
                    Bio = "",
                    RoleID = 1,
                    CreatedAt = DateTime.Now
                };
                Core.Context.Users.Add(Core.user);
                Core.Context.SaveChanges();
                MessageBox.Show("Регистрация завершена!");
                NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show("Ваш пароль должен быть не менее 8 символов, а также иметь как минимум: \n 1 строчную латинскую букву \n 1 заглавную латинскую букву \n 1 цифру \n 1 спецсимвол из списка (!@#$%^&*)");
            }

        }


        private void LoginTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginPlaceholder.Visibility = string.IsNullOrEmpty(LoginTB.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PassTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PassPlaceholder.Visibility = string.IsNullOrEmpty(PassTB.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PassDubTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PassDubPlaceholder.Visibility = string.IsNullOrEmpty(PassDubTB.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoginTB_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void LoginTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(LoginTB.Text)) LoginPlaceholder.Visibility = Visibility.Visible;
        }

        private void PassTB_GotFocus(object sender, RoutedEventArgs e)
        {
            PassPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void PassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PassTB.Password)) PassPlaceholder.Visibility = Visibility.Visible;
        }

        private void PassDubTB_GotFocus(object sender, RoutedEventArgs e)
        {
            PassDubPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void PassDubTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PassDubTB.Password)) PassDubPlaceholder.Visibility = Visibility.Visible;
        }

        private void EmailTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EmailPlaceholder.Visibility = string.IsNullOrEmpty(EmailTB.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void EmailTB_GotFocus(object sender, RoutedEventArgs e)
        {
            EmailPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void EmailTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailTB.Text)) LoginPlaceholder.Visibility = Visibility.Visible;
        }
    }
}
