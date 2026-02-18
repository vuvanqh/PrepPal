import { apiFetch, url, HttpError } from "./util";

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
    const searchUrl = recipeUrl + `/search?name=${recipeName}`;
    console.log(searchUrl)
    const response = await apiFetch(searchUrl, {
        method: "GET"
    });

    if(!response.ok)
    {
        const error =new HttpError(`Cannot perform the operation - ${(await response.json()).message}. Try again later`, response.status);
        error.code = response.status;
        console.log(error, searchUrl)
        throw error;
    }   
    var data = await response.json();
    console.log(data, searchUrl)
    return data;
}