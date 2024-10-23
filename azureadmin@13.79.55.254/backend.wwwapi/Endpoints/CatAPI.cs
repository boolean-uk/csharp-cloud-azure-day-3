using backend.wwwapi.Models;
using backend.wwwapi.Repository;

namespace backend.wwwapi.Endpoints
{
    public static class CatAPI
    {
        public static void ConfigureCatAPI(this WebApplication app)
        {
            app.MapGet("/", GetIndex);
            var cats = app.MapGroup("cats");
            cats.MapGet("/", GetCats);
            cats.MapPost("/", AddCat);
            cats.MapPut("/{id}", UpdateCat);
            cats.MapDelete("/{id}", DeleteCat);
        }

        private static async Task<IResult> GetIndex(IDatabaseRepository<Cat> repository)
        {
            try
            {
                return Results.Ok("Working");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> GetCats(IDatabaseRepository<Cat> repository)
        {
            try
            {
                return Results.Ok(repository.GetAll());
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> AddCat(Cat cat, IDatabaseRepository<Cat> repository)
        {
            try
            {
                if (cat == null)
                {
                    return Results.BadRequest("Invalid Cat.");
                }

                repository.Insert(cat);
                return Results.Created($"/cats/{cat.Id}", cat);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> UpdateCat(int id, Cat updatedCat, IDatabaseRepository<Cat> repository)
        {
            try
            {
                // Find the existing Cat
                var existingCat = repository.GetById(id);
                if (existingCat == null)
                {
                    return Results.NotFound($"Cat with Id {id} not found.");
                }

                // Update the fields
                existingCat.Rating = updatedCat.Rating;

                // Save changes
                repository.Update(existingCat);

                return Results.Ok(existingCat);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> DeleteCat(int id, IDatabaseRepository<Cat> repository)
        {
            try
            {
                // Find the Cat by Id
                var existingCat = repository.GetById(id);
                if (existingCat == null)
                {
                    return Results.NotFound($"Cat with Id {id} not found.");
                }

                // Delete the Cat
                repository.Delete(id);

                return Results.Ok(existingCat);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
