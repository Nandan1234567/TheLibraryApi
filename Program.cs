using Microsoft.EntityFrameworkCore;
using TheLibraryApi.Data;
using TheLibraryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

//db
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));


// it shows error if we not use inheritnece between class

// it means whenever u call interface in controller , ger property of service
// eg i book service than get book service

// Add Services with Interfaces (Dependency Injection)

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IMemberService, MemberService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
