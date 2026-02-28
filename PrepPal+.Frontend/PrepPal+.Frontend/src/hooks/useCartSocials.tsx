import {useQuery, useMutation} from "@tanstack/react-query";
import { getPendingInvitations, modifyInvitaiton, sendCartInvitation } from "../api/cart-social";
import type { accessType, actionType, cartInvitationResponse } from "../types/CartTypes";
import { queryClient } from "../api/authentication";
import { useOwnedCarts } from "./useCartRecipe";
import { joinCart } from "../hubConnections";

export function useCartInvitation(){
    const {data: invitations} = useQuery<cartInvitationResponse[]>({
        queryKey: ["cart", "invitations"],
        queryFn: getPendingInvitations,
    });

    return {
        invitations,
    }
}

export function useCartInvitationActions(cartId: string, invitationId: string){

    const {mutate} = useMutation({
        mutationFn: async (action: actionType) => {
            await modifyInvitaiton({
                cartId,
                invitationId,
                action
            });
            queryClient.invalidateQueries({queryKey: ["cart","invitations"]});
        }
    })

    return {
        decline: () => mutate("reject"),
        accept: () =>{  
            mutate("accept");
            joinCart(cartId);
        },
    }
}


export function useCartOwnerActions(){
    const {ownedCarts} = useOwnedCarts();
    const myCart = ownedCarts[0] as string;

    const {mutate: inviteAction} = useMutation({
        mutationFn: ({userName, access}:{userName: string, access: accessType}) => sendCartInvitation({
            cartId: myCart!,
            userName,
            access
        })
    })

    return {
        invite: (userName:string, access:accessType) => inviteAction({userName, access})
    }
}