using MidWareService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string RBCors = "DEFAULT_ALLOWED_ORIGINS";
builder.Services.AddCors(options =>
{
    options.AddPolicy(RBCors, 
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .WithMethods(new[] { "GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS", "TRACE" })
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()
                .WithOrigins("*")
                //.SetIsOriginAllowed()
                .WithExposedHeaders("Access-Control-Allow-Origin");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseMiddleware<RequestDetectionMiddleware>();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(RBCors);


app.UseAuthorization();

app.MapControllers();

app.Run();
