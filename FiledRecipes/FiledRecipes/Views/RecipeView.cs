using FiledRecipes.Domain;
using FiledRecipes.App.Mvp;
using FiledRecipes.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiledRecipes.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class RecipeView : ViewBase, IRecipeView
    {
        /// <summary>
        /// Show recipe
        /// When the user selects 4 in the menu ("Show recipe"), a list of all recipe names will
        /// be presented. The user will then select which recipe to view.
        /// </summary>
        /// <param name="recipe"></param>
        public void Show(IRecipe recipe)
        {
            Header = recipe.Name;  // Retrieve Recipe name
            ShowHeaderPanel();     //Display Recipe name in panel
            Console.WriteLine();
            Console.WriteLine("Du behöver följande:");
            Console.WriteLine("_______________________________");
            foreach (var ingredientRows in recipe.Ingredients)
            {
                Console.WriteLine(ingredientRows);      //Display list of Ingredients.
            }
            Console.WriteLine();
            Console.WriteLine("Så här gör du:");
            Console.WriteLine("_______________________________");
            foreach (var instructionRows in recipe.Instructions)
            {
                Console.WriteLine(instructionRows);     //Display list of instructions.
            }
            //Displays one recipe 
        }

        /// <summary>
        /// Show all recipes.
        /// When the user selects 5 in the menu ("Show all recipes"), all recipes will be displays
        /// sorted by the recipe names.
        /// Only one recipe at a time will be displayed and the user presses a key to show the following one.
        /// After the recipes has been shown, the user should be able to press a key to return to the menu.
        /// </summary>
        /// <param name="recipes"></param>
        public void Show(IEnumerable<IRecipe> recipes)
        {
            foreach (var showRecipe in recipes)  //Loops through all recipes.
            {
                Show(showRecipe);                //Displays one recipe.
                ContinueOnKeyPressed();
            }
            //Displays full recipes, one at a time.
        }
    }
}
