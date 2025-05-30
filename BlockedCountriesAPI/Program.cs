using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using System.Reflection;

using DomainLayer.Contracts;
using Service;
using Presentation.Controllers;
using DomainLayer.Interfaces;
using Persistence;

namespace BlockedCountriesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddApplicationPart(typeof(CountriesController).Assembly);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(Persistence.Assembly).Assembly);

            builder.Services.AddSingleton<IBlockedCountryRepository, InMemoryBlockedCountryRepository>();
            builder.Services.AddSingleton<ILogService, InMemoryLogService>();

            builder.Services.AddSingleton<IBlockedCountryService, BlockedCountryService>();
            builder.Services.AddSingleton<IBlockedAttemptLogService, BlockedAttemptLogService>();

            builder.Services.AddScoped<IGeoLocationService, GeoLocationService>();

            builder.Services.AddHttpClient<IGeoLookupService, IPGeolocationService>();

            builder.Services.AddHostedService<Services.ExpiredBlocksCleanupService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();            // We don't need this timer anymore since we're using the ExpiredBlocksCleanupService hosted service
            // which handles cleanup automatically in the background

            app.Run();
        }
    }
}
