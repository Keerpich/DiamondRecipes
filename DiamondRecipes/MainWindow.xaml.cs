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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DiamondRecipes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string current_path = "";
        ObservableCollection<Recipe> allRecipes = null;
        public Recipe selectedRecipe = null;

        public MainWindow()
        {
            InitializeComponent();

            allRecipes = new ObservableCollection<Recipe>();
        }

        #region EventHandlers
        private void LocalizeText(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                mi.Header = LocalizationManager.Instance.getStringForKey(mi.Header.ToString());
                return;
            }

            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = LocalizationManager.Instance.getStringForKey(tb.Text);
                return;
            }

            Button b = sender as Button;
            if (b != null)
            {
                b.Content = LocalizationManager.Instance.getStringForKey(b.Content.ToString());
                return;
            }


            

        }

        public void RefreshListBox()
        {
            ListBox lbRecipeList = this.FindName("RecipeList") as ListBox;
            lbRecipeList.BeginInit();
            lbRecipeList.EndInit();
            lbRecipeList.InvalidateVisual();

            TextBox tb = this.FindName("RecipeDescriptionTextBox") as TextBox;
            tb.Clear();

        }

        private void PopulateContentList()
        {
            ListBox lbRecipeList = this.FindName("RecipeList") as ListBox;
            


            ICollectionView view = CollectionViewSource.GetDefaultView(allRecipes);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            view.SortDescriptions.Add(new SortDescription("Category", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            lbRecipeList.ItemsSource = view;
        }

        private void RecipeSelectedEventHandler(object sender, RoutedEventArgs e)
        {
            ListBoxItem lib = sender as ListBoxItem;
            Recipe rep = lib.Content as Recipe;
            TextBox tb = this.FindName("RecipeDescriptionTextBox") as TextBox;
            tb.Text = rep.Title;

            tb.Text += " " + LocalizationManager.Instance.getStringForKey("BY") + " " + rep.Author;

            tb.Text += System.Environment.NewLine;
            tb.Text += System.Environment.NewLine;

            tb.Text += LocalizationManager.Instance.getStringForKey("TIME_TO_COOK") + ": ";
            tb.Text += rep.TimeToCook;

            tb.Text += System.Environment.NewLine;
            tb.Text += System.Environment.NewLine;

            tb.Text += LocalizationManager.Instance.getStringForKey("INGREDIENTS") + ":" + System.Environment.NewLine;
            tb.Text += rep.Ingredients;

            tb.Text += System.Environment.NewLine;
            tb.Text += System.Environment.NewLine;
            tb.Text += LocalizationManager.Instance.getStringForKey("WAY_TO_COOK") + ":" + Environment.NewLine;
            tb.Text += rep.WayToCook;

            //activate edit and delete buttons
            MenuItem emi = this.FindName("EditRecipeButton") as MenuItem;
            MenuItem dmi = this.FindName("DeleteRecipeButton") as MenuItem;

            emi.IsEnabled = true;
            dmi.IsEnabled = true;

            selectedRecipe = rep;
        }

        private void SaveToClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog browser = new Microsoft.Win32.SaveFileDialog();
            browser.DefaultExt = ".xml";
            browser.Filter = "XML File (.xml)|*.xml";

            Nullable<bool> result = browser.ShowDialog();

            if (result == true)
            {
                string filename = browser.FileName;

                //save all info in this file
                Utilities.saveRecipes(filename, allRecipes);
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.saveRecipes(current_path, allRecipes);
        }

        private void SearchBoxGotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        private void AddRecipeClick(object sender, RoutedEventArgs e)
        {
            EditRecipeWindow editWin = new EditRecipeWindow();
            (editWin.FindName("titleBox") as TextBox).Text = "";
            (editWin.FindName("authorBox") as TextBox).Text = "";
            (editWin.FindName("categoryBox") as TextBox).Text = "";
            (editWin.FindName("timeToCookBox") as TextBox).Text = "";
            (editWin.FindName("ingredientsBox") as TextBox).Text = "";
            (editWin.FindName("wayToCookBox") as TextBox).Text = "";

            editWin.parentWindow = this;
            editWin.isNew = true;
            editWin.selectedRecipe = null;//selectedRecipe;

            editWin.Show();

        }

        private void EditRecipeClick(object sender, RoutedEventArgs e)
        {
            EditRecipeWindow editWin = new EditRecipeWindow();
            (editWin.FindName("titleBox") as TextBox).Text = selectedRecipe.Title;
            (editWin.FindName("authorBox") as TextBox).Text = selectedRecipe.Author;
            (editWin.FindName("categoryBox") as TextBox).Text = selectedRecipe.Category;
            (editWin.FindName("timeToCookBox") as TextBox).Text = selectedRecipe.TimeToCook;
            (editWin.FindName("ingredientsBox") as TextBox).Text = selectedRecipe.Ingredients;
            (editWin.FindName("wayToCookBox") as TextBox).Text = selectedRecipe.WayToCook;

            editWin.parentWindow = this;
            editWin.isNew = false;
            editWin.selectedRecipe = selectedRecipe;

            editWin.Show();
        }

        private void ImportRecipeClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog browser = new Microsoft.Win32.OpenFileDialog();
            browser.DefaultExt = ".xml";
            browser.Filter = "XML File|*.xml|Microsoft Word|*.doc;*.docx";
            browser.FilterIndex = 1;
            browser.Multiselect = true;

            Nullable<bool> result = browser.ShowDialog();

            if (result == true)
            {
                string[] filenames = browser.FileNames;
                foreach (string filename in filenames)
                {
                    if(filename.EndsWith(".xml"))
                    {
                        var importList = Utilities.getRecipes(filename);
                        //allRecipes = Utilities.concatRecipeLists(allRecipes, importList);
                        foreach (Recipe r in importList)
                            allRecipes.Add(r);
                    }
                    //if(filename.EndsWith(".doc") || filename.EndsWith(".docx"))
                      //Parse doc file and get everything you can from it with a ???Python??? script
                    //else if(filename.EndsWith(".xml")
                        //see if it is a RecipeBook xml and then parse and merge it 
                }


                RefreshListBox();

            }
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog browser = new Microsoft.Win32.OpenFileDialog();
            browser.DefaultExt = ".xml";
            browser.Filter = "XML File|*.xml";
            browser.FilterIndex = 1;
            browser.Multiselect = false;

            Nullable<bool> result = browser.ShowDialog();

            if(result == true)
            {
                current_path = browser.FileName;
                //parse xml file
                allRecipes = Utilities.getRecipes(current_path);

                PopulateContentList();

                //set save to enabled
                (this.FindName("SaveButton") as MenuItem).IsEnabled = true;
            }
        }

        private void DeleteRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            allRecipes.Remove(selectedRecipe);
            RefreshListBox();
        }

        private void NewRecipeClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog browser = new Microsoft.Win32.SaveFileDialog();
            browser.DefaultExt = ".xml";
            browser.Filter = "XML File (.xml)|*.xml";

            Nullable<bool> result = browser.ShowDialog();

            if (result == true)
            {
                string filename = browser.FileName;

                //save all info in this file
                Utilities.saveRecipes(filename, new ObservableCollection<Recipe>());

                allRecipes = Utilities.getRecipes(filename);

                PopulateContentList();

                //set save to enabled
                (this.FindName("SaveButton") as MenuItem).IsEnabled = true;
            }
        }
        #endregion

        public void AddNewRecipe(Recipe recipe)
        {
            allRecipes.Add(recipe);
        }
        
    }
}
