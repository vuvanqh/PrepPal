import { apiClient } from "./apiClient";

const recipeUrl = "/recipe"


export const getRandomRecipes = async () =>  (await apiClient.get(recipeUrl+"/random")).data;
export const getSearchedRecipes = async (recipeName: string) => (await apiClient.get(recipeUrl + `/search?name=${recipeName}`)).data;
