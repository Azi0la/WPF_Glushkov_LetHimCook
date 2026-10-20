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
    /// Логика взаимодействия для PassChangePage.xaml
    /// </summary>
    public partial class PassChangePage : Page
    {
        private static readonly Regex PassRegex = new Regex(
        @"^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^*]{8,}$",
        RegexOptions.Compiled);
        public PassChangePage()
        {
            InitializeComponent();
        }

        private void ChangePassButt_Click(object sender, RoutedEventArgs e)
        {
            string OldPass = PassMaker.GetHash(OldPassTB.Password);
            string NewPass = PassMaker.GetHash(NewPassTB.Password);
            string NewPassDub = PassMaker.GetHash(NewPassDubTB.Password);

            if (string.IsNullOrEmpty(OldPassTB.Password) || string.IsNullOrEmpty(NewPassTB.Password) || string.IsNullOrEmpty(NewPassDubTB.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            if(OldPass != Core.user.Password)
            {
                MessageBox.Show("Неверный старый пароль!");
                return;
            }
            if (NewPass != NewPassDub)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            if (PassRegex.IsMatch(NewPassTB.Password))
            {
                Core.user.Password = NewPass;
                Core.Context.SaveChanges();
                MessageBox.Show("Смена пароля завершена!");
                NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show("Ваш новый пароль должен быть не менее 8 символов, а также иметь как минимум: \n 1 строчную латинскую букву \n 1 заглавную латинскую букву \n 1 цифру \n 1 спецсимвол из списка (!@#$%^&*)");
            }
        }

        private void OldPassTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            OldPassPlaceholder.Visibility = string.IsNullOrEmpty(OldPassTB.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OldPassTB_GotFocus(object sender, RoutedEventArgs e)
        {
            OldPassPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void OldPassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OldPassTB.Password)) OldPassPlaceholder.Visibility = Visibility.Visible;
        }

        private void NewPassTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            NewPassPlaceholder.Visibility = string.IsNullOrEmpty(NewPassTB.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NewPassTB_GotFocus(object sender, RoutedEventArgs e)
        {
            NewPassPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void NewPassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewPassTB.Password)) NewPassPlaceholder.Visibility = Visibility.Visible;
        }

        private void NewPassDubTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            NewPassDubPlaceholder.Visibility = string.IsNullOrEmpty(NewPassDubTB.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NewPassDubTB_GotFocus(object sender, RoutedEventArgs e)
        {
            NewPassDubPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void NewPassDubTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewPassDubTB.Password)) NewPassDubPlaceholder.Visibility = Visibility.Visible;

        }


    }
}
