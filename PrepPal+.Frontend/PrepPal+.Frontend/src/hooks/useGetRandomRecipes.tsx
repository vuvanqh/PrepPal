import { useQuery } from "@tanstack/react-query";
import { getRandomRecipes } from "../api/recipeApi";
import { getRecommendaitons } from "../api/cart-recipe";
import type { meal } from "../types/RecipeTypes";
import useAuth from "./useAuth";

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

export function useGetRecommendedRecipes(){
    const {isAuthenticated} = useAuth();
    const {data, isPending} = useQuery<meal[]>({
        queryKey: ["recipe", "recommendations"],
        queryFn: getRecommendaitons,
        enabled: isAuthenticated
    });

    return {
        recommended: data??[],
        pending: isPending
    };
}