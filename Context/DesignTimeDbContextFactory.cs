using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OrtSurvey.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OrtSurveyDataBase>
    {
        public OrtSurveyDataBase CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=.;Database=OrtSurveyDb;Trusted_Connection=True;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder<OrtSurveyDataBase>();
            optionsBuilder.UseSqlServer(connectionString);

            return new OrtSurveyDataBase(optionsBuilder.Options);
        }
    }
}
