using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureDb(builder);
builder.Services.AddControllers().AddJsonOptions(b => { b.JsonSerializerOptions.ConfigureJsonOptions(); });
;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDomainServices();
builder.Services.AddFluentValidation();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureSwagger();
app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();