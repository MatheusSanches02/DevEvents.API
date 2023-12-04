
using AwesomeDevEvents.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var conn = builder.Configuration.GetConnectionString("DefaultConnection");

            //builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseInMemoryDatabase("DevEvents"));

            builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseSqlServer(conn));

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
        }
    }
}