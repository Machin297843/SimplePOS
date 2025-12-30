using SimplePOS.Application.Products;
using SimplePOS.Application.ProductGroups;
using SimplePOS.Infrastructure;
using SimplePOS.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductGroupService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();