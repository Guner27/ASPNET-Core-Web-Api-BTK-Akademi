using Microsoft.AspNetCore.Mvc;
using NLog;
using Presentation.ActionFilters;
using Services;
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
            .AddXmlDataContractSerializerFormatters()
            .AddCustomCsvFormatter()            
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
            //.AddNewtonsoftJson();

        

        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

        builder.Services.Configure<ApiBehaviorOptions>(
            options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureSqlContext(builder.Configuration);
        builder.Services.ConfigureRepositoryManager();
        builder.Services.ConfigureServiceManager();
        builder.Services.ConfigureLoggerService();
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.ConfigureActionFilter();
        builder.Services.ConfigureCors();
        builder.Services.ConfigureDataShaper();
        builder.Services.AddCustomMediaTypes();
        builder.Services.AddScoped<IBookLinks, BookLinks>();


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

        app.UseCors("CorsPolicy");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}