using BusinessLogic;
using Data.IUnitOfWork.UnitOfWork;
using Data.IUnitOfWork;
using Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/**********************************************
   * HABILITAR LOGGER - SERILOG
   ***********************************************/
//serilog
IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
Log.Information("La aplicacion esta corriendo");
builder.Host.UseSerilog();


//agregamos los servicios para que funcione el serilog por cada capa
//inyeccion de dependencias
builder.Services.AddScoped<Conexion>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<StudentBL>();
builder.Services.AddScoped<ProfessorBL>();
builder.Services.AddScoped<SubjectBL>();
builder.Services.AddScoped<EnrollmentBL>();
builder.Services.AddHttpClient();

/***********************************************
 * FIN HABILITAR LOGGER - SERILOG
 ***********************************************/



builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
