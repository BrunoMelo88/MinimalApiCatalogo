using ApiCatalogo.ApiEndpoints;
using ApiCatalogo.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAuthenticationJwt();

var app = builder.Build();

//definir os endpoints
app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();


// Configure the HTTP request pipeline.
var environment = app.Environment;

app.UseExceptionHandling(environment).UseSwaggerMiddleware().UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

