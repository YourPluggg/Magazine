using Magazine.Core;
using Magazine.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//������� ������������� 
builder.Services.AddTransient<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//����� �������� �� ID
app.MapGet("/products/{id}", (int id, IProductService service) =>
{
    var product = service.Search(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

//������� ��� ��������
app.MapGet("/products", (IProductService service) =>
{
    var allProducts = service.GetAll();
    return Results.Ok(allProducts);
});

//��������� ������� � ������
//�������� �������� �� ID
app.MapPost("/products", (Product product, IProductService service) => 
{
    var newProduct = service.Add(product);
    return Results.Created($"/products/{newProduct.Id}" ,newProduct);
});

//�������� ��������
app.MapDelete("/products/{id}", (int id, IProductService service) =>
{
    var removed = service.Remove(id);
    return removed is not null ? Results.Ok(removed) : Results.NotFound();
});

//���������������
app.MapPut("/products/{id}", (int id, Product product, IProductService service) =>
{
    if (id != product.Id) return Results.BadRequest("ID mismatch");
    var edited = service.Edit(product);
    return edited is not null ? Results.Ok(edited) : Results.NotFound();
});

//������ �������
app.Run();

