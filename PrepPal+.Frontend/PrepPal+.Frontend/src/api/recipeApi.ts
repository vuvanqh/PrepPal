import { apiFetch, url } from "./util";

export const headers = {
    "Authorization": "Bearer " + localStorage.getItem("token")
}


const recipeUrl = url + "/recipe"


export async function getRandomRecipes(){
    const randomRecipeUrl = recipeUrl+"/random";
    const response = await apiFetch(randomRecipeUrl,{
        method: "GET"
    });

    return await response.json();
}

export async function getSearchedRecipes(recipeName: string){
    const searchUrl = url + `/search?name=${recipeName}`;
    const response = await apiFetch(searchUrl, {
        method: "GET"
    });

    return await response.json();
}