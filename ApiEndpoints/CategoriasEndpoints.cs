using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.ApiEndpoints
{
    public static class CategoriasEndpoints
    {
        public static void MapCategoriasEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Catálogo de produtos 2024").ExcludeFromDescription();

            app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias!.ToListAsync()).WithTags("Categorias").RequireAuthorization();

            app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
    {
        return await db.Categorias!.FindAsync(id) is Categoria categoria ? Results.Ok(categoria) : Results.NotFound();
    }).RequireAuthorization();


            app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
        {
            if (categoria is null)
            {
                return Results.BadRequest("Dados Inválidos.");
            }
            db.Categorias!.Add(categoria);
            await db.SaveChangesAsync();

            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        });


            //app.MapPost("/categorias", async ([FromBody] Categoria categoria, [FromServices] AppDbContext db) =>
            //{
            //    if (categoria is null)
            //    {
            //        return Results.BadRequest("Dados Inválidos.");
            //    }
            //    db.Categorias!.Add(categoria);
            //    await db.SaveChangesAsync();

            //    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
            //}).Accepts<Categoria>("application/json").Produces<Categoria>(StatusCodes.Status201Created).WithName("CriarNovaCategoria").WithTags("Setter");


            app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
            {
                if (categoria.CategoriaId != id)
                {
                    return Results.BadRequest();
                }
                var categoriaDB = await db.Categorias!.FindAsync(id);

                if (categoriaDB is null) return Results.NotFound();

                categoriaDB.Nome = categoria.Nome;
                categoriaDB.Descricao = categoria.Descricao;

                await db.SaveChangesAsync();

                return Results.Ok(categoriaDB);

            });

            app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
            {
                var categoria = await db.Categorias!.FindAsync(id);

                if (categoria is null) return Results.NotFound();
                db.Categorias.Remove(categoria);

                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
