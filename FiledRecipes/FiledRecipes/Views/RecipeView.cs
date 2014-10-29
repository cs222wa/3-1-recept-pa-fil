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
        public void Show(IRecipe recipe)
        {
            foreach (var header in recipe.Name)
            {
                Header = recipe.Name;
                ShowHeaderPanel();
                //// Retrieve Recipe name
            }
            Console.WriteLine();
            Console.WriteLine("Ingredienser");
            Console.WriteLine("_______________________________");
            foreach (var ingredients in recipe.Ingredients)
            {
                Console.WriteLine(ingredients);
            }

            Console.WriteLine();
            Console.WriteLine("Instruktioner:");
            Console.WriteLine("_______________________________");
            foreach (var instructions in recipe.Instructions)
            {
                Console.WriteLine(instructions);
            }

            //Dis plays one recipe 
        }

        public void Show(IEnumerable<IRecipe> recipes)
        {
        //     
        //        //show one recipe at a time.
        //        foreach (var recipeToShow in recipes)
        //        {
        //            Show(recipeToShow);
        //            ContinueOnKeyPressed();
        //        }
        //
            
            //Displays the list of recipes
        }
    }
}


//Visa recept
//Då användaren väljer menykommandot ’4. Visa recept.’ ska en lista med samtliga recepts namn 
//presenteras varefter användaren väljer det recept som ska visas

//Visa alla recept
//Då användaren väljer menykommandot ’5. Visa alla recept.’ ska alla recept visas sorterade 
//efter receptens namn.
//Bara ett recept åt gången ska visas och användaren ska trycka på en tangent för att visa nästa recept. 
//Efter att recepten visats ska användaren kunna trycka på en tangent för att återvända till menyn.



