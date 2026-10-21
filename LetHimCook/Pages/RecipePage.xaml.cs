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
using Word = Microsoft.Office.Interop.Word;

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

        private void ExportWordBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var application = new Word.Application();
                Word.Document document = application.Documents.Add();

                Word.Paragraph titleParagraph = document.Paragraphs.Add();
                Word.Range titleRange = titleParagraph.Range;
                titleRange.Text = recipe.Title;
                try { titleParagraph.set_Style("Заголовок 1"); } catch { titleParagraph.set_Style("Heading 1"); }
                titleRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                titleRange.Font.Bold = 1;
                titleRange.Font.Size = 18;
                titleRange.InsertParagraphAfter();
                document.Paragraphs.Add(); 

                Word.Paragraph authorParagraph = document.Paragraphs.Add();
                Word.Range authorRange = authorParagraph.Range;
                string authorName = users.FirstOrDefault(u => u.ID == recipe.AuthorID)?.Name ?? "Неизвестен";
                authorRange.Text = $"Автор: {authorName}";
                authorRange.Font.Size = 12;
                authorRange.Font.Italic = 1;
                authorRange.InsertParagraphAfter();
                document.Paragraphs.Add();

                Word.Paragraph infoParagraph = document.Paragraphs.Add();
                Word.Range infoRange = infoParagraph.Range;

                Word.Table infoTable = document.Tables.Add(infoRange, 5, 2);
                infoTable.Borders.InsideLineStyle = infoTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                infoTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                infoTable.Cell(1, 1).Range.Text = "Параметр";
                infoTable.Cell(1, 2).Range.Text = "Значение";
                infoTable.Rows[1].Range.Font.Bold = 1;
                infoTable.Rows[1].Range.Font.Size = 12;
                infoTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                infoTable.Cell(2, 1).Range.Text = "Время приготовления";
                infoTable.Cell(2, 2).Range.Text = $"{recipe.CookingTime} мин.";

                infoTable.Cell(3, 1).Range.Text = "Количество порций";
                infoTable.Cell(3, 2).Range.Text = recipe.Servings.ToString();

                infoTable.Cell(4, 1).Range.Text = "Оценка";
                infoTable.Cell(4, 2).Range.Text = recipe.Rating?.ToString("0.0") ?? "—";

                infoTable.Cell(5, 1).Range.Text = "Количество просмотров";
                infoTable.Cell(5, 2).Range.Text = recipe.ViewsCount.ToString();

                for (int i = 2; i <= 5; i++)
                {
                    infoTable.Cell(i, 1).Range.Font.Size = 11;
                    infoTable.Cell(i, 2).Range.Font.Size = 11;
                }

                document.Paragraphs.Add();

                Word.Paragraph tagsParagraph = document.Paragraphs.Add();
                Word.Range tagsRange = tagsParagraph.Range;
                tagsRange.Text = TagTB.Text;
                tagsRange.Font.Size = 11;
                tagsRange.InsertParagraphAfter();
                document.Paragraphs.Add();

                Word.Paragraph descTitle = document.Paragraphs.Add();
                Word.Range descTitleRange = descTitle.Range;
                descTitleRange.Text = "Описание";
                try { descTitle.set_Style("Заголовок 2"); } catch { descTitle.set_Style("Heading 2"); }
                descTitleRange.InsertParagraphAfter();

                Word.Paragraph descParagraph = document.Paragraphs.Add();
                Word.Range descRange = descParagraph.Range;
                descRange.Text = recipe.Description ?? "Описание отсутствует";
                descRange.Font.Size = 11;
                descRange.InsertParagraphAfter();
                document.Paragraphs.Add();


                Word.Paragraph instrTitle = document.Paragraphs.Add();
                Word.Range instrTitleRange = instrTitle.Range;
                instrTitleRange.Text = "Инструкция по приготовлению";
                try { instrTitle.set_Style("Заголовок 2"); } catch { instrTitle.set_Style("Heading 2"); }
                instrTitleRange.InsertParagraphAfter();

                Word.Paragraph instrParagraph = document.Paragraphs.Add();
                Word.Range instrRange = instrParagraph.Range;
                instrRange.Text = recipe.Instruction ?? "Инструкция отсутствует";
                instrRange.Font.Size = 11;
                instrRange.InsertParagraphAfter();
                document.Paragraphs.Add();


                var recipeComments = comments.Where(c => c.RecipeID == recipe.ID).ToList();
                if (recipeComments.Any())
                {
                    Word.Paragraph reviewsTitle = document.Paragraphs.Add();
                    Word.Range reviewsTitleRange = reviewsTitle.Range;
                    reviewsTitleRange.Text = $"Отзывы ({recipeComments.Count})";
                    try { reviewsTitle.set_Style("Заголовок 2"); } catch { reviewsTitle.set_Style("Heading 2"); }
                    reviewsTitleRange.InsertParagraphAfter();

                    foreach (var comment in recipeComments)
                    {
                        string commentAuthor = users.FirstOrDefault(u => u.ID == comment.UserID)?.Name ?? "Аноним";

                        Word.Paragraph reviewParagraph = document.Paragraphs.Add();
                        Word.Range reviewRange = reviewParagraph.Range;
                        reviewRange.Text = $"{commentAuthor}: {comment.Text}";
                        reviewRange.Font.Size = 10;
                        reviewRange.Font.Italic = 1;
                        reviewRange.InsertParagraphAfter();
                    }
                }

                foreach (Word.Section section in document.Sections)
                {
                    Word.HeaderFooter footer = section.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                    footer.PageNumbers.Add(Word.WdPageNumberAlignment.wdAlignPageNumberCenter);
                }

                foreach (Word.Section section in document.Sections)
                {
                    Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Text = DateTime.Now.ToString("dd.MM.yyyy");
                    headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.Size = 10;
                }

                string safeTitle = string.Join("_", recipe.Title.Split(System.IO.Path.GetInvalidFileNameChars()));
                string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                          $"Recipe_{safeTitle}.docx");

                document.SaveAs2(path);

                application.Visible = true;

                MessageBox.Show($"Рецепт успешно экспортирован!\nФайл сохранён на рабочий стол:\n{path}",
                                "Экспорт в Word", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Word:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
