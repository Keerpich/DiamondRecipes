using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.ObjectModel;

namespace DiamondRecipes
{
    class Utilities
    {
        private Utilities() { }
        private static Utilities instance;
        public static Utilities Instance
        {
            get
            {
                if (instance == null)
                    instance = new Utilities();
                return instance;
            }
        }

        #region Utilities
        public static System.Collections.ObjectModel.ObservableCollection<Recipe> getRecipes(string path)
        {
            System.Collections.ObjectModel.ObservableCollection<Recipe> allRecipes = new System.Collections.ObjectModel.ObservableCollection<Recipe>();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(path);
            System.Xml.XmlNode RecipeList = doc.SelectSingleNode("RecipeBook");
            System.Xml.XmlNodeList allRecipiesXML = RecipeList.SelectNodes("Recipe");
            foreach (System.Xml.XmlNode nowRecipe in allRecipiesXML)
            {
                //string category_name = Convert.ToString(rec.Attributes.GetNamedItem("name").Value);
                //System.Xml.XmlNodeList currentRecipes = category.SelectNodes("Recipe");
                //allRecipes.Add(new Tuple<string, List<Recipe>>(category_name, new List<Recipe>()));
                //foreach(System.Xml.XmlNode nowRecipe in currentRecipes)
                //{
                Recipe recipe = new Recipe();
                recipe.Category = nowRecipe.SelectSingleNode("Category").InnerText;
                recipe.Title = nowRecipe.SelectSingleNode("Title").InnerText;
                recipe.Ingredients = nowRecipe.SelectSingleNode("Ingredients").InnerText;
                recipe.WayToCook = nowRecipe.SelectSingleNode("WayToCook").InnerText;
                System.Xml.XmlNode ttc = nowRecipe.SelectSingleNode("TimeToCook");
                if (ttc != null)
                    recipe.TimeToCook = ttc.InnerText;
                System.Xml.XmlNode aut = nowRecipe.SelectSingleNode("Author");
                if (aut != null)
                    recipe.Author = aut.InnerText;

                allRecipes.Add(recipe);
                //}
            }

            return allRecipes;

        }
        //public static void saveRecipes(string path, List<Tuple<string, List<Recipe>>> allRecipes)
        public static void saveRecipes(string path, System.Collections.ObjectModel.ObservableCollection<Recipe> allRecipes)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.Encoding = Encoding.UTF8;
            XmlWriter writer = XmlWriter.Create(path, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("RecipeBook");

            foreach(Recipe recipe in allRecipes)
            {
                writer.WriteStartElement("Recipe");

                writer.WriteStartElement("Category");
                writer.WriteString(recipe.Category);
                writer.WriteEndElement();
                
                writer.WriteStartElement("Title");
                writer.WriteString(recipe.Title);
                writer.WriteEndElement();

                writer.WriteStartElement("Ingredients");
                writer.WriteString(recipe.Ingredients);
                writer.WriteEndElement();

                writer.WriteStartElement("WayToCook");
                writer.WriteString(recipe.WayToCook);
                writer.WriteEndElement();

                if (recipe.TimeToCook != null)
                {
                    writer.WriteStartElement("TimeToCook");
                    writer.WriteString(recipe.TimeToCook);
                    writer.WriteEndElement();
                }

                if(recipe.Author != null)
                {
                    writer.WriteStartElement("Author");
                    writer.WriteString(recipe.Author);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
        }
        public static ObservableCollection<Recipe> concatRecipeLists(ObservableCollection<Recipe> l1, ObservableCollection<Recipe> l2)
        {
            ObservableCollection<Recipe> result = new ObservableCollection<Recipe>();

            foreach (Recipe r in l1)
                result.Add(r);
            foreach (Recipe r in l2)
                result.Add(r);

            return result;
        }
        public static ObservableCollection<Recipe> searchForTitle(ObservableCollection<Recipe> list, string searchString)
        {

            ObservableCollection<Recipe> returnList = new ObservableCollection<Recipe>();

            foreach(Recipe r in list)
            {
                if (r.Title.ToLowerInvariant().Contains(searchString.ToLowerInvariant()))
                {
                    returnList.Add(r);
                }
            }

            return returnList;
        }
        #endregion
    }
}
