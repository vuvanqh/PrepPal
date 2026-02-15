using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrepPal_.Core.ClientContracts;
using PrepPal_.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Application.Services;

public class CategoryBootstrapService: IHostedService
{
    private readonly IMealDbClient _mealDbClient;
    private readonly IServiceScopeFactory _scopedServices;

    public CategoryBootstrapService(IMealDbClient mealDbClient, IServiceScopeFactory scopedServices)
    {
        _mealDbClient = mealDbClient;
        _scopedServices = scopedServices;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopedServices.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IRecipeCategoryRepository>();

        if (await repo.AnyAsync()) return;

        var categories = await _mealDbClient.GetRecipeCategories();
        await repo.AddRangeAsync(categories);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
