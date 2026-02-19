import type { meal } from "./RecipeTypes"


export type cartResponse = {
    cartId: string,
    ownerUserName: string,
    members: CartMembers[],
    recipeResponses: meal[]
}

export type CartMembers = {
    userName: string,
    accessType: "owner" | "editor" | "viewer"
}