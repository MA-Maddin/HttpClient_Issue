namespace WebApi_Net80
{
    public class ProgramWeb80
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();

            app.MapGet("/health", () =>
            {
                return Results.Ok(true);
            })
            .WithName("GetHealth")
            .WithOpenApi();

            app.MapPost("/upload", (IFormFile file) =>
            {
                return Results.NotFound();
            })
            .WithName("Upload")
            .WithOpenApi()
            .DisableAntiforgery();

            app.Run();
        }
    }
}
