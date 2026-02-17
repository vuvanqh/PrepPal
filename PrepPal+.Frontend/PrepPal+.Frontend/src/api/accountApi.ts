import type { meal } from "../types/RecipeTypes";
import { apiFetch, url, HttpError } from "./util";



const account = url + "/account"

export type interactionType = {
    meal: meal,
    type: string
}

export async function addInteraction(interaction: interactionType){
    const interactionData = {
        type: interaction.type,
        externalRecipeId: interaction.meal.externalId
    }
    var response = await apiFetch(account + "/recipe-interaction", {
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

export async function getPersonalDetails(){
    var response = await apiFetch(account + "/my-info");
    
    if(!response.ok)
    {
        const error = new HttpError((await response.json()).message || "User not found" , response.status);
        error.code = response.status;
        throw error;
    }
    var data = await response.json();
    //console.log(data, "hey");
    return data;
}

export async function getLikedRecipes(){
    var response = await apiFetch(account + "/liked-recipes");

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