import { useQuery } from "@tanstack/react-query";
import { getSearchedRecipes } from "../api/recipeApi";
import { type meal } from "../types/RecipeTypes";

export default function useRecipe(name?:string){

    const {data = [], isPending} = useQuery({
        queryFn: async ():Promise<meal[]> => (await getSearchedRecipes(name as string)).recipes,
        queryKey: ["recipes", "search", name],
        enabled: !!name
    });

    return {
        recipes: data,
        isPending
    }
}