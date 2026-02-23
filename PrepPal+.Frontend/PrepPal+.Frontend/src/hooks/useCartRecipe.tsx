import { useQuery, useMutation } from "@tanstack/react-query";
import { addToCart, getCartContents, getOwnedCarts, removeFromCart } from "../api/cart-recipe";
import { queryClient } from "../api/authentication";
import type { cartResponse } from "../types/CartTypes";
import { toastError, toastSuccess } from "../toastConfig";
import type { meal } from "../types/RecipeTypes";
import useAuth from "./useAuth";

export function useOwnedCarts(){
    const {isAuthenticated} = useAuth();
    const {data, isPending} = useQuery({
        queryKey: ["owned-carts"],
        queryFn: getOwnedCarts,
        enabled: isAuthenticated
    })
    return{
        ownedCarts: data?.cartIdList??[],
        isPending,
    }
}

export function useCartContent(cartId: string){
    const {isAuthenticated} = useAuth();
    const {data, isPending} = useQuery<cartResponse>({
        queryKey: ["cart-content", cartId],
        queryFn: () => getCartContents(cartId),
        enabled: isAuthenticated
    })

    return {
        cartOwner: data?.ownerUserName,
        cartId: data?.cartId,
        members: data?.members??[],
        cartRecipes: data?.cartRecipes??[],
        cartContentPending: isPending,
    }
}


export function useCartContentMutations(cartId?:string){
    if(!cartId){
        return{
            addRecipe: () => {},
            removeRecipe: () => {}
        }
    }
    const queryKey = ["cart-content", cartId];

    const optimisticUpdate = async (updater: (prev: cartResponse) => cartResponse) => {
        await queryClient.cancelQueries({ queryKey });

        const prevCart =
        queryClient.getQueryData<cartResponse>(queryKey);

        if (!prevCart) return { prevCart: undefined };

        const updated = updater(prevCart);

        queryClient.setQueryData(queryKey, updated);

        return { prevCart };
    };

    const rollback = (context?: { prevCart?: cartResponse }) => {
        if (context?.prevCart) 
            queryClient.setQueryData(queryKey, context.prevCart);

        toastError("meh from useCart");
    };
    
    const {mutate: addRecipe, isPending:addRecipePending} = useMutation({
        mutationFn: (recipe:meal) => addToCart(cartId,recipe.externalId),
        onMutate: async (recipe:meal) => optimisticUpdate((prevCart) => {
            const recipeIndex = prevCart.cartRecipes.findIndex(r => r.recipe.externalId === recipe.externalId);
            let updatedRecipe;
            let updatedRecipes = [...prevCart.cartRecipes];

            if(recipeIndex!=-1){
                updatedRecipe = {
                    ...prevCart.cartRecipes[recipeIndex],
                    quantity: prevCart.cartRecipes[recipeIndex].quantity+1};
                updatedRecipes[recipeIndex] = updatedRecipe;
            }else {
                updatedRecipes.push({
                    recipe,
                    quantity: 1
                });
            }

            return {
                ...prevCart,
                cartRecipes: updatedRecipes
            };
        }),
        onError: (_err, _vars, context) => rollback(context),
        onSettled: () => queryClient.invalidateQueries({queryKey:["cart-content",cartId]}),
        onSuccess: () => toastSuccess("yay from useCart")
    })

    const {mutate: removeRecipe, isPending:removeRecipePending} = useMutation({
        mutationFn: (recipe:meal) => removeFromCart(cartId,recipe.externalId),
        onMutate: async (recipe:meal) => optimisticUpdate((prevCart) => {
            const recipeIndex = prevCart.cartRecipes.findIndex(r => r.recipe.externalId === recipe.externalId);
            let updatedRecipes = [...prevCart.cartRecipes];

            if(recipeIndex!=-1){
                let recipe = {
                    ...prevCart.cartRecipes[recipeIndex],
                    quantity: prevCart.cartRecipes[recipeIndex].quantity-1};

                if(recipe.quantity==0)
                    updatedRecipes = updatedRecipes.filter((_, i) => i !== recipeIndex);
                else
                    updatedRecipes[recipeIndex] = recipe;
            }

            return {
                ...prevCart,
                cartRecipes: updatedRecipes
            };
        }),
        onError: (_err, _vars, context) => rollback(context),
        onSettled: () => queryClient.invalidateQueries({queryKey:["cart-content",cartId]}),
        onSuccess: () => toastSuccess("yay from useCart")
    })


    return {
        addRecipe,
        addRecipePending,
        removeRecipe,
        removeRecipePending
    }
}