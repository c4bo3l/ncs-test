using AppServer.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Database;
using Infrastructure.Service;
using Infrastructure.Service.FileServices.CafeLogoServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

    builder.Register(context =>
    {
        var options = context.Resolve<DbContextOptions<AppDbContext>>();
        return new PooledDbContextFactory<AppDbContext>(options);
    }).As<IDbContextFactory<AppDbContext>>().InstancePerLifetimeScope();

    builder
        .RegisterAssemblyTypes(typeof(RegisterHelper).Assembly)
        .AsClosedTypesOf(typeof(IRequestHandler<,>))
        .InstancePerDependency();
    
    builder
        .RegisterAssemblyTypes(typeof(RegisterHelper).Assembly)
        .AsClosedTypesOf(typeof(IRequestHandler<>))
        .InstancePerDependency();

    builder.RegisterType<CafeLogoService>()
        .As<ICafeLogoService>()
        .InstancePerLifetimeScope();
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseHttpsRedirection();

app.MapControllers();

await using var serviceScope = app.Services.CreateAsyncScope();
await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.MigrateAsync();

app.Run();
