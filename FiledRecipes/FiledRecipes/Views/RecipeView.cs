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
            //Anropa metoder som redan finns?
            //Visar upp hela receptet
        }

        public void Show(IEnumerable<IRecipe> recipes)
        {
            //Anropa metoder som redan finns?
        }
    }
}


//jag har gjort två olika Shows i RecipeView
//[15:39:26] Maria Sjöberg: den ena ska vara fulla receptet
//[15:39:30] Maria Sjöberg: den andra är sjkälva menyn
//[15:39:37] Maria Sjöberg: listan med recept