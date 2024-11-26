

using Amazon.S3;
using app.enchantedair.extensions;
using app.enchantedair.persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using persistence.enchantedair.app;

namespace app.enchantedair.api
{   public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.AddConfigBasedCors();
            builder.Services.AddHttpLogging(o => {  });
            builder.Services.AddSingleton<IAmazonS3>(sp =>
            {
                var config = builder.Configuration;

                var s3Config = new AmazonS3Config
                {
                    ServiceURL = config["S3Upload:ServiceURL"] ?? Environment.GetEnvironmentVariable("AWS_SERVICE_URL"),
                    ForcePathStyle = true // Required for S3-compatible storage
                };

                var accessKey = config["S3Upload:AccessKey"] ?? Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
                var secretKey = config["S3Upload:SecretKey"] ?? Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

                return new AmazonS3Client(accessKey, secretKey, s3Config);
            });
            //Migrated This to DBExtensions for Cleaning up code while migrating to DockerFile
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEnchantedAirDB();
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://dev-0revwfmqbhtkb7m4.us.auth0.com/";
                options.Audience = "app.enchantedair.api";
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            var app = builder.Build();
            app.UseHttpLogging();
            app.Services.EnsureDatabaseCreated<EnchantedAirDB>();
            // Configure the HTTP request pipeline.
            app.UseCors("AllowReactApp");


            app.MapControllers();


            app.Run();
        }
    }
}
