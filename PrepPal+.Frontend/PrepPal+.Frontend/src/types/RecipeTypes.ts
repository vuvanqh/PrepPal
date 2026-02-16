export type ingredient = {
    ingredientName: string,
    ingredientMeasure: string
}

export type meal = {
    externalId: number,
    area: string,
    category: string,
    imageUrl: string,
    ingredients: ingredient[],
    instructions: string,
    name: string
}

