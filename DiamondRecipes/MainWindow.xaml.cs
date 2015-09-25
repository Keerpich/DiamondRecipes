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
        //private List<Tuple<string, List<Recipe>>> allRecipes = null;
        ObservableCollection<Recipe> allRecipes = null;
        public MainWindow()
        {
            InitializeComponent();
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

        private void PopulateContentList()
        {
            ListBox lbRecipeList = this.FindName("RecipeList") as ListBox;
            string localizedCategory = "Category";//LocalizationManager.Instance.getStringForKey("CATEGORY");
            ICollectionView view = CollectionViewSource.GetDefaultView(allRecipes);
            view.GroupDescriptions.Add(new PropertyGroupDescription(localizedCategory));
            view.SortDescriptions.Add(new SortDescription(localizedCategory, ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            lbRecipeList.ItemsSource = view;
            //lbRecipeList.ItemSource = view;
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
                    //if(filename.EndsWith(".doc") || filename.EndsWith(".docx"))
                      //Parse doc file and get everything you can from it with a ???Python??? script
                    //else if(filename.EndsWith(".xml")
                        //see if it is a RecipeBook xml and then parse and merge it 
                }

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
                //allRecipes = Utilities.Instance.getRecipes(current_path);
                allRecipes = Utilities.getRecipes(current_path);

                PopulateContentList();
            }
        }
        #endregion
    }
}
