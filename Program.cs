using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersAndAuth.Data;
using UsersAndAuth.Models;
using UsersAndAuth.Services;

var builder = WebApplication.CreateBuilder(args);

string dbConnection;

if (builder.Environment.IsDevelopment())
{
	// У випадку запуску проєкту в середовищі розробки використовується підключення через localhost
	dbConnection = builder.Configuration.GetConnectionString("PostgresLocalhostConnection")!;
	// When running the project in a development environment, connection via localhost are used
}
else
{
	// В інших випадках використовується змінна середовища
	dbConnection = Environment.GetEnvironmentVariable("USER_DB_CONNECTION")!;
	// In other cases, environment variable are used
}

// В проєкті використовується PostgreSQL та "ліниве завантаження"
builder.Services.AddDbContext<UserDbContext>(options =>
	options.UseLazyLoadingProxies().UseNpgsql(dbConnection));
// The project uses PostgreSQL and lazy loading

builder.Services.AddIdentity<User, IdentityRole>()
	.AddEntityFrameworkStores<UserDbContext>()
	.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.HttpOnly = true;
	options.LoginPath = "/api/auth/login";
	options.AccessDeniedPath = "/api/auth/access-denied";
});

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();