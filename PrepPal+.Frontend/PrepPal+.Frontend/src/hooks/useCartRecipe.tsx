import { useQuery, useMutation } from "@tanstack/react-query";
import { addToCart, getCartContents, getOwnedCarts } from "../api/cart-recipe";
import { queryClient } from "../api/authentication";
import type { cartResponse } from "../types/CartTypes";

export function useOwnedCarts(){
    const {data = [], isPending} = useQuery({
        queryKey: ["owned-carts"],
        queryFn: getOwnedCarts
    })

    return{
        ownedCarts: data,
        isPending,
    }
}

export function useCartContent<cartResponse, _Error>(cartId: string){
    const {data = [], isPending} = useQuery({
        queryKey: ["cart-content", cartId],
        queryFn: () => getCartContents(cartId)
    })

    const {mutate: addRecipe, isPending:addRecipePending} = useMutation({
        mutationFn: (externalId:number) => addToCart(cartId,externalId),
        onMutate: async (cartId: string) => {
            await queryClient.cancelQueries({queryKey: ["cart-content", cartId]});

            const prevCart = queryClient.getQueryData<cartResponse>(["cart-content", cartId]);

            queryClient.setQueryData(["cart-content", cartId], (old:cartResponse) => {
                const idx = old["recipeReponses"].indexOf()
                return {...prevCart, 
                    recipeReponses: [...old]}
            })
            
            return {prevCart}
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
        cartContent: data,
        cartContentPending: isPending,
        addToCart: addRecipe,
        addRecipePending
    }
}