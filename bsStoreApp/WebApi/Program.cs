using NLog;
using Services.Contracts;
using WebApi.Extensions;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true; //Ýçerik pazarlýðýna uygulama açýldý.
            config.ReturnHttpNotAcceptable = true;
        })
            .AddCustomCsvFormatter()
            .AddXmlDataContractSerializerFormatters()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
            .AddNewtonsoftJson();

        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureSqlContext(builder.Configuration);
        builder.Services.ConfigureRepositoryManager();
        builder.Services.ConfigureServiceManager();
        builder.Services.ConfigureLoggerService();
        builder.Services.AddAutoMapper(typeof(Program));


        var app = builder.Build();

        var logger = app.Services.GetRequiredService<ILoggerService>();
        app.ConfigExceptionHandler(logger);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}