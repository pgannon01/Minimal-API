using Microsoft.EntityFrameworkCore;
using MinimalShoppingListAPI;

var builder = WebApplication.CreateBuilder(args); // need to build a web app with builder

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add our DBcontext to our current app so we can make use of it
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ShoppingListApi"));

var app = builder.Build();

app.MapGet("/shoppinglist", async (ApiDbContext db) => 
        await db.Groceries.ToListAsync()
);

app.MapGet("/shoppinglist/{id}", async (int id, ApiDbContext db) =>
{
    var grocery = await db.Groceries.FindAsync(id);

    return grocery != null? Results.Ok(grocery) : Results.NotFound();
});

app.MapPost("/shoppinglist", async (Grocery grocery, ApiDbContext db) =>
{
    db.Groceries.Add(grocery);

    await db.SaveChangesAsync();

    return Results.Created($"/shoppinglist/{grocery.Id}", grocery);
});

app.MapPut("/shoppinglist/{id}", async (int id, Grocery grocery, ApiDbContext db) =>
{
    var updateGrocery = await db.Groceries.FindAsync(id);

    if (updateGrocery != null)
    {
        updateGrocery.Name = grocery.Name;
        updateGrocery.IsBought = grocery.IsBought;

        // db.Update(grocery); // Will replace the whole object, which we may not want to do, since it'll replace all values

        await db.SaveChangesAsync();
        return Results.Ok(updateGrocery);
    }

    return Results.NotFound();
});

app.MapDelete("/shoppinglist/{id}", async (int id, ApiDbContext db) =>
{
    var grocery = await db.Groceries.FindAsync(id);

    if (grocery != null)
    {
        db.Groceries.Remove(grocery);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});


if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();