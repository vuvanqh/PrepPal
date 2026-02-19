import { apiFetch, HttpError, url } from "./util";
import type { meal } from "../types/RecipeTypes";

export type interactionType = {
    meal: meal,
    type: string,
    action: "add" | "remove" | "update"
}

const recipeInteractionUrl = url+"/recipe-interaction"

export async function getLikedRecipes(){
    var response = await apiFetch(recipeInteractionUrl + "/liked-recipes");

    if(!response.ok)
    {
        const error = new HttpError((await response.json()).message || "Recipes not found" , response.status);
        error.code = response.status;
        throw error;
    }

    var data = await response.json();
    console.log(data);
    console.log("sending liked-recipes requests");
    return data;
}


export async function addInteraction(interaction: interactionType){
    const interactionData = {
        type: interaction.type,
        externalRecipeId: interaction.meal.externalId
    }
    var response = await apiFetch(recipeInteractionUrl + "/recipe-interaction", {
        method: "POST",
        body: JSON.stringify(interactionData)
    })

    if(!response.ok)
    {
        const error =new HttpError(`Cannot perform the operation - ${(await response.json()).message}. Try again later`, response.status);
        error.code = response.status;
        throw error;
    }   
}