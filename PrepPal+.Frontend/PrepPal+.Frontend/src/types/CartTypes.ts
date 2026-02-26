import type { meal } from "./RecipeTypes"

export type accessType = "Owner" | "Editor" | "Viewer"
export type actionType = "accept" | "reject" | "remove" | "block" | "edit"

export type cartRecipe = {
    recipe: meal,
    quantity: number
}
export type cartResponse = {
    cartId: string,
    ownerUserName: string,
    members: cartMembers[],
    cartRecipes: cartRecipe[]
}

export type cartMembers = {
    userName: string,
    accessType: accessType
}


export type cartInvitationRequest = {
    cartId: string,
    userName: string,
    access: accessType,
}

export type modifyCartInvitation = {
    cartId: string,
    invitationId: string,
    action: actionType
}

export type modifyCartAccess = {
    cartId: string,
    userName: string,
    access: accessType
}

export type cartInvitationResponse = {
    cartId: string,
    invitationId: string,
    ownerUserName: string
}

export type accessibleCarts = {
    carts: accessibleCart[]
}

export type accessibleCart = {
    cartId:string,
    ownerUserName: string
}