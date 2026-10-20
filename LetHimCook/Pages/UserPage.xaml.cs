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
        public UserPage(Users _user)
        {
            InitializeComponent();
            user = _user;
            DataContext = user;
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
    }
}
