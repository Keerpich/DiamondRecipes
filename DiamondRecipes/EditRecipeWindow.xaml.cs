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
using System.Windows.Shapes;

namespace DiamondRecipes
{
    /// <summary>
    /// Interaction logic for EditRecipeWindow.xaml
    /// </summary>
    public partial class EditRecipeWindow : Window
    {
        public MainWindow parentWindow = null;
        public bool isNew = false;
        public Recipe selectedRecipe = null;

        public EditRecipeWindow()
        {
            InitializeComponent();
        }

        private void button_Initialized(object sender, EventArgs e)
        {
            (sender as Button).Content = LocalizationManager.Instance.getStringForKey((sender as Button).Content.ToString());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (isNew)
            {
                AddNewRecipe();
            }
            else
                EditThisRecipe();

            parentWindow.RefreshListBox();
            Close();
        }

        private void AddNewRecipe()
        {
            Recipe newRep = new Recipe();

            newRep.Title = (FindName("titleBox") as TextBox).Text;
            newRep.Category = (FindName("categoryBox") as TextBox).Text;
            newRep.Author = (FindName("authorBox") as TextBox).Text;
            newRep.TimeToCook = (FindName("timeToCookBox") as TextBox).Text;
            newRep.WayToCook = (FindName("wayToCookBox") as TextBox).Text;
            newRep.Ingredients = (FindName("ingredientsBox") as TextBox).Text;

            parentWindow.AddNewRecipe(newRep);
        }

        private void EditThisRecipe()
        {
            selectedRecipe.Title = (FindName("titleBox") as TextBox).Text;
            selectedRecipe.Category = (FindName("categoryBox") as TextBox).Text;
            selectedRecipe.Author = (FindName("authorBox") as TextBox).Text;
            selectedRecipe.TimeToCook = (FindName("timeToCookBox") as TextBox).Text;
            selectedRecipe.WayToCook = (FindName("wayToCookBox") as TextBox).Text;
            selectedRecipe.Ingredients = (FindName("ingredientsBox") as TextBox).Text;
        }
    }
}
