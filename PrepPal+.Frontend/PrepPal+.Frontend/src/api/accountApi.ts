import { apiFetch, url, HttpError } from "./util";



const account = url + "/account"

export type interactionType = {
    externalRecipeId: number,
    type: string
}

export async function addInteraction(interaction: interactionType){
    var response = await apiFetch(account + "/recipe-interaction", {
        method: "POST",
        body: JSON.stringify(interaction)
    })

    if(!response.ok)
    {
        const error =new HttpError(`Cannot perform the operation - ${(await response.json()).message}. Try again later`, response.status);
        error.code = response.status;
        throw error;
    }   
}