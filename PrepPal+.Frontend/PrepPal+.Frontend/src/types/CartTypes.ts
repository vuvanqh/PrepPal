import type { meal } from "./RecipeTypes"

export type cartRecipe = {
    recipe: meal,
    quantity: number
}
export type cartResponse = {
    cartId: string,
    ownerUserName: string,
    members: CartMembers[],
    cartRecipes: cartRecipe[]
}

export type CartMembers = {
    userName: string,
    accessType: "Owner" | "Editor" | "Viewer"
}