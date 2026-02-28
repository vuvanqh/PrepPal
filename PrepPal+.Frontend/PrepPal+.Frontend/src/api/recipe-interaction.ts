import type { meal } from "../types/RecipeTypes";
import { apiClient } from "./apiClient";

export type interactionType = {
    meal: meal,
    type: string,
    action: "add" | "remove" | "update"
}

const recipeInteractionUrl = "/recipe-interaction"

export const getLikedRecipes = async () => (await apiClient.get(recipeInteractionUrl + "/liked-recipes")).data;

export async function addInteraction(interaction: interactionType){
    console.log(`sending add interaction requests`);
    const interactionData = {
        type: interaction.type,
        externalRecipeId: interaction.meal.externalId,
        action: interaction.action
    }
    console.log(interactionData);
    return (await apiClient.post(recipeInteractionUrl + "/recipe-interaction", interactionData)).data;
}