using Microsoft.EntityFrameworkCore;
using Notes.WebAPI.Data;
using Notes.WebAPI.Mapping;
using Notes.WebAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NotesDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NotesConnectionString"));
});

builder.Services.AddScoped<INoteRepository, SQLNoteRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddCors(policy => policy.AddPolicy("corsPolicy",build =>
{
    build
    .WithOrigins("http://localhost:5173")
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
