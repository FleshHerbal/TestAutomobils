using Microsoft.EntityFrameworkCore;
using TestAssigment;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcCore();

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationContext>(option => option.UseInMemoryDatabase("temp_db"));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
