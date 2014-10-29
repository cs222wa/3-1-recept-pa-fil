using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiledRecipes.Domain
{
    /// <summary>
    /// Holder for recipes.
    /// </summary>
    public class RecipeRepository : IRecipeRepository
    {
        /// <summary>
        /// Represents the recipe section.
        /// </summary>
        private const string SectionRecipe = "[Recept]";

        /// <summary>
        /// Represents the ingredients section.
        /// </summary>
        private const string SectionIngredients = "[Ingredienser]";

        /// <summary>
        /// Represents the instructions section.
        /// </summary>
        private const string SectionInstructions = "[Instruktioner]";

        /// <summary>
        /// Occurs after changes to the underlying collection of recipes.
        /// </summary>
        public event EventHandler RecipesChangedEvent;

        /// <summary>
        /// Specifies how the next line read from the file will be interpreted.
        /// </summary>
        private enum RecipeReadStatus { Indefinite, New, Ingredient, Instruction };

        /// <summary>
        /// Collection of recipes.
        /// </summary>
        private List<IRecipe> _recipes;

        /// <summary>
        /// The fully qualified path and name of the file with recipes.
        /// </summary>
        private string _path;

        /// <summary>
        /// Indicates whether the collection of recipes has been modified since it was last saved.
        /// </summary>
        public bool IsModified { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the RecipeRepository class.
        /// </summary>
        /// <param name="path">The path and name of the file with recipes.</param>
        public RecipeRepository(string path)
        {
            // Throws an exception if the path is invalid.
            _path = Path.GetFullPath(path);

            _recipes = new List<IRecipe>();
        }

        /// <summary>
        /// Returns a collection of recipes.
        /// </summary>
        /// <returns>A IEnumerable&lt;Recipe&gt; containing all the recipes.</returns>
        public virtual IEnumerable<IRecipe> GetAll()
        {
            // Deep copy the objects to avoid privacy leaks.
            return _recipes.Select(r => (IRecipe)r.Clone());
        }

        /// <summary>
        /// Returns a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to get.</param>
        /// <returns>The recipe at the specified index.</returns>
        public virtual IRecipe GetAt(int index)
        {
            // Deep copy the object to avoid privacy leak.
            return (IRecipe)_recipes[index].Clone();
        }

        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="recipe">The recipe to delete. The value can be null.</param>
        public virtual void Delete(IRecipe recipe)
        {
            // If it's a copy of a recipe...
            if (!_recipes.Contains(recipe))
            {
                // ...try to find the original!
                recipe = _recipes.Find(r => r.Equals(recipe));
            }
            _recipes.Remove(recipe);
            IsModified = true;
            OnRecipesChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to delete.</param>
        public virtual void Delete(int index)
        {
            Delete(_recipes[index]);
        }

        /// <summary>
        /// Raises the RecipesChanged event.
        /// </summary>
        /// <param name="e">The EventArgs that contains the event data.</param>
        protected virtual void OnRecipesChanged(EventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler handler = RecipesChangedEvent;

            // Event will be null if there are no subscribers. 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

        /// <summary>
        /// //Hämta recept
        ///Recept ska läsas in från textfilen recipes.txt. Väljer användaren menyalternativet ’1.Öppna’ ska 
        ///applikationen öppna textfilen, läsa och tolka den rad för rad för att skapa en lista med recept som 
        ///användaren sedan ska kunna välja att via menykommandon hantera på olika sätt.

        /// </summary>
        public virtual void Load()
        {
            //RecipeRepository decides which part is what and writes it out.

            RecipeReadStatus status = RecipeReadStatus.Indefinite; 

            List<IRecipe> recipeList = new List<IRecipe>();

            using (StreamReader reader = new StreamReader(_path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "")
                    {
                        continue;
                    }
                    if (line == SectionRecipe)
                    {
                        status = RecipeReadStatus.New;
                    }
                    else if (line == SectionIngredients)
                    {
                        status = RecipeReadStatus.Ingredient;
                    }
                    else if (line == SectionInstructions)
                    {
                        status = RecipeReadStatus.Instruction;
                    }
                    else
                    {
                        if (status == RecipeReadStatus.New)
                        {
                            recipeList.Add(new Recipe(line));

                            //new recipe object is added to the list of recipes.

                        }
                        else if (status == RecipeReadStatus.Ingredient)
                        {

                            string[] parts = line.Split(new char[] { ';' });
                            //Divide the line into 3 sections by using the method Split() in the String class.

                            if (parts.Length != 3)
                            {
                                throw new FileFormatException();
                            }
                            //If the number of sections are not three, throw new FileFormatException.

                            Ingredient newIngredient = new Ingredient();
                            newIngredient.Amount = parts[0];
                            newIngredient.Measure = parts[1];
                            newIngredient.Name = parts[2];
                            //Create an ingredient object and initiate it with the three sections for Measure, Amount and Name.

                            recipeList.Last().Add(newIngredient);
                            //Add the ingredient to the recipes' list of ingredients.

                        }
                        else if (status == RecipeReadStatus.Instruction)
                        {
                            recipeList.Last().Add(line);
                            //Add the line to the recipes' list of instructions.
                        }
                        else
                        {
                            throw new FileFormatException();
                            //If else - something is wrong and a new exceptoin of the type FileFormatException will be thrown.
                        }
                    } 
                }
                recipeList.TrimExcess();
                recipeList.Sort();
                //Sort the list with recipes according to the Names of the recipes.

                _recipes = recipeList;
                //Assign the field _recipes, in the class, a refrence to the list.

                IsModified = false;
                //Assign the property IsModified, in the class, a value which indicates that the list of recipes is unchanged.

                OnRecipesChanged(EventArgs.Empty);
                //Advertise that the recipes has been loaded by calling the method OnRecipesChanged and send it the parameter EventArgs.Empty.
            }
        }

        /// <summary>
        ///Spara recept
        ///Recept ska sparas permanent i textfilen recipes.txt. Väljer användaren menyalternativet 
        ///’2. Spara’ ska applikationen öppna textfilen och skriva recepten rad för rad till textfilen. Finns redan 
        ///textfilen ska den skrivas över.
        /// </summary>
        public virtual void Save()
        {
            // Öppna filen för att skriva. StreamWriter är det du kan använda för detta (titta på parametern append, den gör något som du ska göra enligt specen)

            //Append
            //Determines whether data is to be appended to the file. 
            //If the file exists and append is false, the file is overwritten. 
            //If the file exists and append is true, the data is appended to the file. 
            //Otherwise, a new file is created.

            //I dokumentet så står det att varje gång man kör Save, så ska filen bli överskrivit
             using (StreamWriter writer = new StreamWriter(_path, false))
             {
                 foreach (var recipe in _recipes)   //loopa listan med recept(_recipes?) för att hämta ut varje recept
                 { 
                     Console.WriteLine(SectionRecipe);  //Skriv varje receptes namn (header)

                     Console.WriteLine(String.Format("{;}", SectionIngredients)); 
                     //Skriv varje recepts ingredienser (header)
                     //Använd string.Format för att skriva ut och dela upp de olika ingredienserna vid varje ";".

                     Console.WriteLine(SectionInstructions); //Skriv varje recepts instruktioner (header)
                      
                     Console.WriteLine(recipe.Instructions);  //Skriv ut de rader för instruktioner som finns lagrade i listan.
                 }
             }
        }
    }
}
