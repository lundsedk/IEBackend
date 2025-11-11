using IEBackend.Shared;
using IEBackend.Catalog;

// TODO: cleanup!!!
//      add class and inject catalog via ctor

var builder = WebApplication.CreateBuilder(args);

// register Catalog for DI
builder.Services.AddSingleton<Catalog>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();





app.MapGet("/getProductDetails", (Catalog catalog, string id) =>
{
    var products = catalog.GetProducts();
    var product = products.FirstOrDefault(p => p.id == id);

    if (string.IsNullOrEmpty(product.id))
        return Results.NotFound();

    return Results.Ok(product);

})
.WithName("GetProductDetails")
.WithOpenApi();

app.MapGet("/getProductListing", (Catalog catalog, string category) =>
{
    var products = catalog.GetProducts();
    var list = products
        .Where(p => string.Equals(p.category, category, StringComparison.OrdinalIgnoreCase))
        .ToArray();

    return Results.Ok(list);
})
.WithName("GetProductListing")
.WithOpenApi();


app.Run();