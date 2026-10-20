using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LetHimCook
{
    internal class Judgment
    {
        public static void Delete(Recipes recipe, Users user)
        {

            var comments = Core.Context.Comments.Where(c => c.RecipeID == recipe.ID).ToList();

            foreach (var comment in comments)
            {
                Core.Context.Comments.Remove(comment);
            }


            var likes = Core.Context.Likes
        .Where(l => l.RecipeID == recipe.ID)
        .ToList();
            foreach (var lik in likes)
            {
                Core.Context.Likes.Remove(lik);
            }


            var favorites = Core.Context.Favorites
                .Where(f => f.RecipeID == recipe.ID)
                .ToList();
            foreach (var favorite in favorites)
            {
                Core.Context.Favorites.Remove(favorite);
            }

            var ingRecs = Core.Context.IngRec
                .Where(ir => ir.RecipeID == recipe.ID)
                .ToList();
            foreach (var ingrecs in ingRecs)
            {
                Core.Context.IngRec.Remove(ingrecs);
            }

            var recipeTags = Core.Context.RecipeTags
                .Where(rt => rt.RecipeID == recipe.ID)
                .ToList();
            foreach (var recipeTag in recipeTags)
            {
                Core.Context.RecipeTags.Remove(recipeTag);
            }

            var collectionRecipes = Core.Context.CollectionRecipe
                .Where(cr => cr.RecipeID == recipe.ID)
                .ToList();
            foreach (var collectionRecipe in collectionRecipes)
            {
                Core.Context.CollectionRecipe.Remove(collectionRecipe);
            }

            var reports = Core.Context.Reports
                .Where(r => r.RecipeID == recipe.ID)
                .ToList();
            foreach (var report in reports)
            {
                Core.Context.Reports.Remove(report);
            }


            var recipeToDelete = Core.Context.Recipes.Find(recipe.ID);
            if (recipeToDelete != null)
            {
                Core.Context.Recipes.Remove(recipeToDelete);
            }

            Core.Context.SaveChanges();
            user.Recipes.Remove(recipe);

            return;
        }
    }
}
