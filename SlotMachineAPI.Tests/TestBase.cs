using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SlotMachineAPI.Data;
using SlotMachineAPI.Services;

namespace SlotMachineAPI.Tests;

public abstract class TestBase : IDisposable
{
    protected readonly IServiceProvider ServiceProvider;
    protected readonly SlotMachineDbContext DbContext;
    private bool _disposed = false;

    protected TestBase()
    {
        var services = new ServiceCollection();

        // Add in-memory database
        services.AddDbContext<SlotMachineDbContext>(options =>
            options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));

        // Add services
        services.AddScoped<IGameLogicService, GameLogicService>();

        ServiceProvider = services.BuildServiceProvider();
        DbContext = ServiceProvider.GetRequiredService<SlotMachineDbContext>();
        
        // Ensure database is created
        DbContext.Database.EnsureCreated();
        
        // Seed test data
        SeedTestData();
    }

    protected virtual void SeedTestData()
    {
        // Add default symbols if they don't exist
        if (!DbContext.Symbols.Any())
        {
            var defaultSymbols = SlotMachineAPI.Models.Symbol.GetDefaultSymbols();
            DbContext.Symbols.AddRange(defaultSymbols);
            DbContext.SaveChanges();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                DbContext?.Dispose();
                (ServiceProvider as IDisposable)?.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
} 