import { apiFetch, HttpError, url } from "./util";

const headers = {
    "Authorization": "Bearer " + localStorage.getItem("token")
}


const recipeUrl = url + "/recipe"


export async function getRandomRecipes(){
    const randomRecipeUrl = recipeUrl+"/random"
        const response = await apiFetch(randomRecipeUrl,{
            method: "GET"
        });

    return await response.json();
}