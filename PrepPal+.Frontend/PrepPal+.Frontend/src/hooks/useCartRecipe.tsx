import { useQuery, useMutation } from "@tanstack/react-query";
import { addToCart, getCartContents, getOwnedCarts, removeFromCart } from "../api/cart-recipe";
import { queryClient } from "../api/authentication";
import type { accessibleCarts, cartResponse } from "../types/CartTypes";
import { toastError, toastSuccess } from "../toastConfig";
import type { meal } from "../types/RecipeTypes";
import useAuth from "./useAuth";
import { clearCart as clearCartContents } from "../api/cart-recipe";
import { getAccessibleCarts } from "../api/cart-recipe";

type cartIdResponse = {
    cartIdList:string[]
}
export function useOwnedCarts(){
    const {isAuthenticated} = useAuth();
    const {data, isPending} = useQuery<cartIdResponse>({
        queryKey: ["owned-carts"],
        queryFn: getOwnedCarts,
        enabled: isAuthenticated
    })
    return{
        ownedCarts: data?.cartIdList ?? [],
        isPending,
    }
}

export function useAccessibleCarts(){
    const {data} = useQuery<accessibleCarts>({
        queryKey: ["cart", "accessible"],
        queryFn: getAccessibleCarts
    })

    return {
        accessibleCarts: data?.carts?? []
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
        cart: data,
        cartOwner: data?.ownerUserName,
        cartId: data?.cartId,
        members: data?.members??[],
        cartRecipes: data?.cartRecipes??[],
        cartContentPending: isPending,
    }
}


export function useCartContentMutations(){
    const optimisticUpdate = async (cartId: string, updater: (prev: cartResponse) => cartResponse) => {
        const queryKey = ["cart-content", cartId];
        await queryClient.cancelQueries({ queryKey });

        const prevCart =
        queryClient.getQueryData<cartResponse>(queryKey);

        if (!prevCart) return { prevCart: undefined };

        const updated = updater(prevCart);

        queryClient.setQueryData(queryKey, updated);

        return { prevCart };
    };

    const rollback = (cartId: string, context?: { prevCart?: cartResponse }) => {
        if (context?.prevCart) 
            queryClient.setQueryData(["cart-content", cartId], context.prevCart);

        toastError("meh from useCart");
    };
    
    const {mutate: addRecipe, isPending:addRecipePending} = useMutation({
        mutationFn: ({cartId, recipe}:{cartId:string, recipe:meal}) => addToCart(cartId,recipe.externalId),
        onMutate: async ({cartId, recipe}:{cartId:string, recipe:meal}) => optimisticUpdate(cartId, (prevCart) => {
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
        onError: (_err, vars, context) => rollback( vars.cartId, context),
        onSettled: (_data, _err ,vars) => queryClient.invalidateQueries({queryKey:["cart-content",vars.cartId]}),
        onSuccess: () => toastSuccess("yay from useCart")
    })

    const {mutate: clearCart} = useMutation({
        mutationFn: (cartId: string) => clearCartContents(cartId),
        onMutate: async (cartId: string) => optimisticUpdate(cartId, (prevCart) => {
            return {...prevCart, cartRecipes:[]};
        }),
        onError: (_err, vars, context) => rollback(vars,context),
        onSettled: (_data, _err ,vars) => queryClient.invalidateQueries({queryKey:["cart-content",vars]}),
    })

    const {mutate: removeRecipe, isPending:removeRecipePending} = useMutation({
        mutationFn: ({cartId, recipe}:{cartId:string, recipe:meal}) => removeFromCart(cartId,recipe.externalId),
        onMutate: async ({cartId, recipe}:{cartId:string, recipe:meal}) => optimisticUpdate(cartId, (prevCart) => {
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
        onError: (_err, vars, context) => rollback(vars.cartId, context),
        onSettled: (_data, _err ,vars) => queryClient.invalidateQueries({queryKey:["cart-content",vars]}),
        onSuccess: () => toastSuccess("yay from useCart")
    })


    return {
        addRecipe,
        addRecipePending,
        removeRecipe,
        removeRecipePending,
        clearCart
    }
}