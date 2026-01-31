using Microsoft.EntityFrameworkCore;
using SampleRestAPI.Interfaces;
using SampleRestAPI.Models;
using SampleRestAPI.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<SampleRestAPIContext>(opt =>
    opt.UseInMemoryDatabase("SampleRestAPI"));

builder.Services.AddMemoryCache();

// Repository registration
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //Swagger UI:  https://localhost:7221/swagger/index.html
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
