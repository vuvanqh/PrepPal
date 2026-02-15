using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrepPal_.Core.Application.DTO.Recipes;

public class CategoryResponse
{
    [Required] public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

    public List<RecipeCategory> ToRecipeCategories()
    {
        return Categories.Select(c => c.ToRecipeCategory()).ToList();
    }
}

public class CategoryDTO
{
    public int idCategory;
    public string strCategory = null!;

    public RecipeCategory ToRecipeCategory()
    {
        return new RecipeCategory()
        {
            Id = Guid.NewGuid(),
            ExternalId = idCategory,
            CategoryName = strCategory,
        };
    }
}