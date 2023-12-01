using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using fs_12_team_1_BE.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<MsCourseData>();
builder.Services.AddScoped<MsCategoryData>();
builder.Services.AddScoped<MsUserData>();
builder.Services.AddScoped<MsPaymentMethodData>();
builder.Services.AddScoped<EmailService>();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173", //url client
                                              "https://localhost:7201") //optional
                                             .AllowAnyHeader()
                                             .AllowAnyMethod()
                                             .AllowCredentials();
                      });
});

var JwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
    builder.Configuration.GetSection("JwtConfig:Key").Value));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
        (options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = JwtKey,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
        });


builder.Services.AddScoped<TsOrderData>();
builder.Services.AddScoped<TsOrderDetailData>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
