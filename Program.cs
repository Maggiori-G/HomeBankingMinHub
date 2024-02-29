using HomeBankingMindHub.Repositories;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext to the container
builder.Services.AddDbContext<HomeBankingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingDAO")));

//builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(x =>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
builder.Services.AddControllers().AddJsonOptions(x =>
x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<HomeBankingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingConexion")));

builder.Services.AddScoped<HomeBankingMinHub.Repositories.IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IClientLoanRepository, ClientLoanRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ILoansService, LoanService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClientService, ClientService>();

//autentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.LoginPath = new PathString("/index.html");
});

//autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
   //Aqui obtenemos todos los services registrados en la App
    var services = scope.ServiceProvider;
    try
    {
       // En este paso buscamos un service que este con la clase HomeBankingContext
        var context = services.GetRequiredService<HomeBankingContext>();
        DBInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ha ocurrido un error al enviar la información a la base de datos!");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.UseAuthorization();

app.UseAuthentication();

app.MapRazorPages();

app.Run();
