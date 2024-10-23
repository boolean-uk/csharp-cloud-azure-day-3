using backend.wwwapi.Repository;
using backend.wwwapi.Models;

namespace backend.wwwapi.EndPoints
{
    public static class DogAPI
    {
        public static void ConfigureTodoAPI(this WebApplication app)
        {
            app.MapGet("/", GetIndex);
            app.MapGet("/dogs", GetTodos);
            app.MapPost("/dogs", CreateTodo);
            app.MapPut("/dogs/{id:int}", UpdateTodo);
            app.MapDelete("/dogs/{id:int}", DeleteTodo);
        }

        private static async Task<IResult> GetIndex(IDatabaseRepository<Dog> repository)
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

        private static async Task<IResult> GetTodos(IDatabaseRepository<Dog> repository)
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

        private static async Task<IResult> CreateTodo(Dog dog, IDatabaseRepository<Dog> repository)
        {
            try
            {
                if (dog == null || string.IsNullOrWhiteSpace(dog.Name))
                {
                    return Results.BadRequest("Invalid Todo item.");
                }

                repository.Insert(dog); // Assuming repository has an AddAsync method
                return Results.Created($"/dogs/{dog.Id}", dog); // Return the created todo with its location
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> UpdateTodo(int id, Dog updatedTodo, IDatabaseRepository<Dog> repository)
        {
            try
            {
                // Find the existing Todo
                var existingTodo = repository.GetById(id);
                if (existingTodo == null)
                {
                    return Results.NotFound($"Dog with Id {id} not found.");
                }

                // Update the fields
                if (updatedTodo.Name != null)
                    existingTodo.Name = updatedTodo.Name;
                if (updatedTodo.Completed != null)
                    existingTodo.Completed = updatedTodo.Completed;

                // Save changes
                repository.Update(existingTodo);

                return Results.Ok(existingTodo);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> DeleteTodo(int id, IDatabaseRepository<Dog> repository)
        {
            try
            {
                // Find the Todo by Id
                var existingTodo = repository.GetById(id);
                if (existingTodo == null)
                {
                    return Results.NotFound($"Todo with Id {id} not found.");
                }

                // Delete the Todo
                repository.Delete(id);

                return Results.Ok(existingTodo);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}