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
    /// Логика взаимодействия для InstructionPage.xaml
    /// </summary>
    public partial class InstructionPage : Page
    {
        Recipes recipe;
        public InstructionPage(Recipes _recipe)
        {
            InitializeComponent();
            recipe = _recipe;
            DataContext = recipe;
            RecipeTB.Text = recipe.Instruction;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(NavigationService.CanGoBack) NavigationService.GoBack();
        }
    }
}
