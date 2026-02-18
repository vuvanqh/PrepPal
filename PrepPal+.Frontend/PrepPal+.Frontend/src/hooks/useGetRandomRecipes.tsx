import { useQuery } from "@tanstack/react-query";
import { getRandomRecipes } from "../api/recipeApi";

export default function useGetRandomRecipe(){
    const {data, isPending} = useQuery({
            queryFn: getRandomRecipes,
            queryKey: ["random-recipes"],
            staleTime: 50000,
    });

    return {
        recipes: data,
        isPending
    }
    
}