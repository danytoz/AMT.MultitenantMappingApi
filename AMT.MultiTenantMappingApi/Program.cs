using AMT.FluentMigrator;
using AMT.GenericRepository;
using AMT.GenericRepository.EfCore;
using AMT.Services.MappedObjects;
using AMT.Services;
using AMT.UserRepository;
using AMT.UserRepository.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWorkUser, UnitOfWorkUser>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
builder.Services.AddServices();
AutoMapperConfiguration.Configure(builder.Services);
builder.Services.AddUserDbContext(options =>
{
    options.ServerAddress = builder.Configuration["Connections:UserDataBase:Server"];
    options.DatabaseName = builder.Configuration["Connections:UserDataBase:Database"];
    options.UserName = builder.Configuration["Connections:UserDataBase:User Id"];
    options.Password = builder.Configuration["Connections:UserDataBase:Password"];
    options.TrustServerCertificate = Convert.ToBoolean(builder.Configuration["Connections:UserDataBase:TrustServerCertificate"]);
});

// Migration this set of credential should be different from the one used for the application, and probably should not eb run
// from this application. This is just for demo purposes.
var migrator = new InitializeMigration(
        builder.Configuration["Connections:UserDataBase:Server"],
        builder.Configuration["Connections:UserDataBase:Database"],
        builder.Configuration["Connections:UserDataBase:User Id"],
        builder.Configuration["Connections:UserDataBase:Password"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
