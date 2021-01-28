using LoginAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginAuthentication.Controllers
{
    public class RecepiesController : Controller
    {
        // Context to access the DB
        private RecepieContext recepiesContext = new RecepieContext();

        // Serves index page
        public ActionResult Index()
        {
            // Gets all recipes from DB as a list
            var recipies = recepiesContext.Recepies.ToList();

            // Returns all the recipes in DB in the Index view: Views\Recepies\Index
            return View(recipies);
        }

        // Creates new recipe with the data that comes from the view
        public ActionResult Save(string name, string description, int duration, int difficulty, string category, Ingredient[] ingredientsArray)
        {
            string result = "Error saving request";

            // Creates new recipe with the data
            Recepie recepie = new Recepie();
            recepie.Name = name;
            recepie.Description = description;
            recepie.Duration = duration;
            recepie.Difficulty = difficulty;
            recepie.Category = category;

            var ingredientsList = new List<Ingredient>();
            int ingredientLength = 0;

            if (ingredientsArray != null)
            {
                ingredientLength = ingredientsArray.Length;
            }

            // Saves all ingredients to the DB and adds them to the recipe
            for (int i = 0; i < ingredientLength; i++)
            {
                var ingredient = ingredientsArray[i];
                ingredientsList.Add(ingredient);

                // Insert ingredient into the DB
                recepiesContext.Ingredients.Add(ingredient);

                recepiesContext.SaveChanges();
            }

            recepie.Ingredients = ingredientsList;
            recepie.Discriminator = "";

            // Insert recipe into the DB
            recepiesContext.Recepies.Add(recepie);

            recepiesContext.SaveChanges();

            result = "Success";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Fetches details from one recipe from DB
        public ActionResult Details(int id)
        {
            // Gets the first element that matches the corresponding id in the DB
            var value = recepiesContext.Recepies.FirstOrDefault(x => x.Id == id);
            return View(value);
        }

        // Fetches details from one recipe from DB to be used in the edit page
        public ActionResult Edit(int id)
        {
            // Gets the first element that matches the corresponding id in the DB
            var value = recepiesContext.Recepies.FirstOrDefault(x => x.Id == id);
            return View(value);
        }

        // Saves the changes to a recipe that come from an edit operation
        public ActionResult EditRecipe(int id, string name, string description, int duration, int difficulty, string category)
        {
            // Gets the first element that matches the corresponding id in the DB
            var value = recepiesContext.Recepies.FirstOrDefault(x => x.Id == id);
            value.Name = name;
            value.Description = description;
            value.Duration = duration;
            value.Difficulty = difficulty;
            value.Category = category;

            // Saves the updated changes
            recepiesContext.SaveChanges();

            // Returns a message that will appear on front-end
            return Json("Saved with success!", JsonRequestBehavior.AllowGet);
        }

        // Deletes the element with corresponding id
        public ActionResult Delete(int id)
        {
            var element = recepiesContext.Recepies.FirstOrDefault(x => x.Id == id);

            // First removes ingredients, since they need to be deleted BEFORE the recipe
            recepiesContext.Ingredients.RemoveRange(element.Ingredients);
            recepiesContext.Recepies.Remove(element);
            recepiesContext.SaveChanges();

            return RedirectToAction("Index");
        }
           
        // Controllers of the Comment Session
        public ActionResult DeleteComment(int id, int ratingId)
        {
            var value = recepiesContext.Recepies.FirstOrDefault(x => x.Id == id);
            value.RecepieInfoes = value.RecepieInfoes.Where(x => x.Id != ratingId).ToList();

            recepiesContext.SaveChanges();

            return RedirectToAction("Details/" + id);
        }

        // Save the comment plus rating of a recipe
        public ActionResult SaveComment(int recipeId, string comment, int rating)
        {
            var value = recepiesContext.Recepies.FirstOrDefault(x => x.Id == recipeId);
            var recipeInfo = new RecepieInfo() { Comment = comment, Rating = rating};

            value.RecepieInfoes.Add(recipeInfo);
            recepiesContext.SaveChanges();

            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}