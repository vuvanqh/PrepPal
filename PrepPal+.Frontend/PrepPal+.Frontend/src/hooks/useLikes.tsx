import { useMutation, useQuery } from "@tanstack/react-query";
import { queryClient } from "../api/authentication";
import { addInteraction, getLikedRecipes, type interactionType } from "../api/recipe-interaction";
import type { meal } from "../types/RecipeTypes";
import { toastSuccess, toastError } from "../toastConfig";
import useAuth from "./useAuth";

type likeRespType = {
    likedRecipes: meal[],
    toggleLike: (interaction:interactionType) => void,
    isPending: boolean,
    getPending: boolean
}


export default function useLikes(enabled=true): likeRespType{
    const { isAuthenticated } = useAuth();

    const {data: likedRecipes = [], isPending: getPending} = useQuery({
        queryFn: getLikedRecipes,
        queryKey: ["auth", "liked-recipes"],
        staleTime: 5*60*1000,
        enabled: enabled&&isAuthenticated
    })

    const {mutate, isPending} = useMutation({
        mutationFn: addInteraction,
        onMutate: async (interaction: interactionType) => {
            await queryClient.cancelQueries({queryKey: ["auth", "liked-recipes"]});

            const prevLiked = queryClient.getQueryData<meal[]>(["auth","liked-recipes"]);

            queryClient.setQueryData<meal[]>(["auth","liked-recipes"], (old = []) => {

                if(interaction.action==="add")
                    return [...old, interaction.meal];
                return old.filter(r => r.externalId!=interaction.meal.externalId);
            })
            
            return {prevLiked}
        },
        onError: (_err, _vars, context) =>{
            if(context?.prevLiked){
                queryClient.setQueryData<meal[]>(["auth","liked-recipes"],context.prevLiked);
            }
            toastError("meh from useLikes");
        },
        onSettled: () => queryClient.invalidateQueries({queryKey:["auth","liked-recipes"]}),
        onSuccess: () => toastSuccess("yay from useLikes")
    })

    return {
        likedRecipes,
        toggleLike: mutate,
        isPending,
        getPending
    }
}