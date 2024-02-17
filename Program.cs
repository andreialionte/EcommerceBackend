using Ecommerce2.Data;
using Ecommerce2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DataContextEf>();
/*builder.Services.AddScoped<OrderService>();*/
builder.Services.AddLogging(logging => logging.AddConsole());
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", options =>
    {
        options.AllowAnyHeader();
        options.AllowAnyMethod();
        options.AllowCredentials();
        options.WithOrigins("http://localhost:3000");
    });
    options.AddPolicy("ProdCors", options =>
    {
        options.AllowAnyHeader();
        options.AllowAnyMethod();
        options.WithOrigins("https://yourdomain.com");
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:PasswordKey").Value)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddTransient<IStorageService, StorageService>();
builder.Services.AddAzureClients(options =>
{
    options.AddBlobServiceClient(builder.Configuration.GetSection("Storage:ConnectionString").Value);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCors");
}
else
{
    app.UseHttpsRedirection();
    app.UseCors("ProdCors");
}



app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
