using Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using FluentResults;
using ALMS_UnitTests;
using Microsoft.Extensions.Configuration;
using Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using DomainDDD.Entities.SubProduct;
using DomainDDD.Entities.Product;

namespace ALMS_UnitTests;

public class DBTests {
    // this is NOT the same appsettings.json as the one in src
    // because for some reason, this project refuses to find it even with the correct path
    // this one is in tests\ALMS-UnitTests\bin\Debug\net8.0
    // if the original is changed and tests fail, this might be the reason
    public static string appsettingsPath = "appsettings.json";
    
    [Fact]
    public void TestDBWithInvalidConfigs() {
        
        // Actual good attempt
        {
            var builder = WebApplication.CreateBuilder();

            builder.Configuration.AddJsonFile(appsettingsPath);

            Assert.NotNull(builder.Configuration["ConnectionString"]);

            // Testing if connection string loads correctly
            var constr = builder.Configuration["ConnectionString"]!;
            Assert.Equal(constr, DbInit.GetConnectionString(builder.Configuration, true).Value);
            Assert.Equal(constr, DbInit.GetConnectionString().Value);

            FluentAssert.IsSuccess(builder.AddDatabase());
        }
        // Invalid connection string
        {
            var builder = WebApplication.CreateBuilder();

            builder.Configuration.AddJsonFile(appsettingsPath);
            var configs = builder.Configuration;
            
            configs["ConnectionString"] = null;

            FluentAssert.IsFailed(builder.AddDatabase());
        }
    }

    [Fact]
    public static void TestDBInit() {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddJsonFile(appsettingsPath);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Host.InitLogger(true);

        FluentAssert.IsSuccess(builder.AddDatabase());
        var app = builder.Build();
        FluentAssert.IsSuccess(app.InitializeDatabase());
    }

    [Fact]
    public static void TestDBPushData() {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddJsonFile(appsettingsPath);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Host.InitLogger(true);

        FluentAssert.IsSuccess(builder.AddDatabase());

        var app = builder.Build();
        FluentAssert.IsSuccess(app.InitializeDatabase());

        // Getting a hold of the db
        var ctx = app.Services.GetRequiredService<ALMSDbContext>();
        Assert.NotNull(ctx);

        // Testing if/how id-s and lists of ids get stored
        var sproducts = Enumerable.Range(1, 9)
            .Select(val => new SubProduct())
            .OrderBy(sprod => sprod.SubProductId)
            .ToList();

        ctx.SubProducts.AddRange(sproducts);

        var prod = new Product(sproducts.Select(sprod => sprod.SubProductId).ToList());

        ctx.Products.Add(prod);

        ctx.SaveChanges();

        Assert.Equal(ctx.Products.First(), prod);
        Assert.Equal(ctx.SubProducts.ToList(), sproducts.ToList());
    }
}
