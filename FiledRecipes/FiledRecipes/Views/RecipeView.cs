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
        /// //Visa recept
        ///Då användaren väljer menykommandot ’4. Visa recept.’ ska en lista med samtliga recepts namn 
        ///presenteras varefter användaren väljer det recept som ska visas
        /// </summary>
        /// <param name="recipe"></param>
        public void Show(IRecipe recipe)
        {
            Header = recipe.Name;
            ShowHeaderPanel();
            //// Retrieve Recipe name

            //List<IIngredient> ingredients = new List<IIngredient>(recipe.Ingredients);
            //List<string> instructions = new List<string>(recipe.Instructions);
            //Create new local lists with references to the saved list
            //containing ingredients and instruktions.

            Console.WriteLine();
            Console.WriteLine("Du behöver följande:");
            Console.WriteLine("_______________________________");
            foreach (var ingredientRows in recipe.Ingredients)
            {
                Console.WriteLine(ingredientRows);
                //Display list of Ingredients.
            }

            Console.WriteLine();
            Console.WriteLine("Så här gör du:");
            Console.WriteLine("_______________________________");
            foreach (var instructionRows in recipe.Instructions)
            {
                Console.WriteLine(instructionRows);
                //Display list of instructions.
            }
            //Displays one recipe 
        }

        /// <summary>
        /// 
        ///Visa alla recept
        ///Då användaren väljer menykommandot ’5. Visa alla recept.’ ska alla recept visas sorterade 
        ///efter receptens namn.
        ///Bara ett recept åt gången ska visas och användaren ska trycka på en tangent för att visa nästa recept. 
        ///Efter att recepten visats ska användaren kunna trycka på en tangent för att återvända till menyn.
        /// </summary>
        /// <param name="recipes"></param>
        public void Show(IEnumerable<IRecipe> recipes)
        {
            foreach (var recipeToShow in recipes)
            {
                Show(recipeToShow);
                ContinueOnKeyPressed();
            }
            //Displays full recipes, one at a time.
        }
    }
}
