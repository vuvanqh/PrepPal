import {HubConnection} from "@microsoft/signalr";
import { useEffect, useRef } from "react";
import { toastSuccess, toastFriendRequest, toastMessage } from "../toastConfig";
import { queryClient } from "../api/authentication";
import { notificationConnection } from "../hubConnections";

export default function useNotificationHub( ){
    const connectionRef = useRef<HubConnection | null>(notificationConnection);
   
    useEffect(()=>{        
        //connection
        notificationConnection.on("ReceiveConnectionRequestNotification",onReceiveConnectionRequest);
        notificationConnection.on("NotifyConnectionAccepted",onConnectionAccepted);
        notificationConnection.on("UpdateConnections", onUpdateConnections);

        //cart
        notificationConnection.on("ReceiveCartInvitationNotification",onReceiveCartInvitationNotification);
        notificationConnection.on("NotifyCartInvitationAccepted",onNotifyCartInvitationAccepted);
        notificationConnection.on("RemoveFromCart",onRemoveFromCart);
        notificationConnection.on("UpdateCart", onUpdateCart);


        return () => {
            notificationConnection.off("ReceiveConnectionRequestNotification",onReceiveConnectionRequest);
            notificationConnection.off("NotifyConnectionAccepted",onConnectionAccepted);
            notificationConnection.off("ReceiveCartInvitationNotification",onReceiveCartInvitationNotification);
            notificationConnection.off("NotifyCartInvitationAccepted",onNotifyCartInvitationAccepted);
            notificationConnection.off("RemoveFromCart",onRemoveFromCart);
            notificationConnection.off("UpdateCart", onUpdateCart);
            notificationConnection.off("UpdateConnections", onUpdateConnections);
        }
    }, []);

    return connectionRef.current;
}

const onReceiveConnectionRequest = (username: string) => {
    toastMessage(username,`${username} has sent you a conneciton request`);
    queryClient.invalidateQueries({queryKey:["connections"]});
}

const onConnectionAccepted= (username: string) => {
    toastSuccess(`${username} has accepted your conneciton request`);
    queryClient.invalidateQueries({queryKey:["connections"]});
}

const onRemoveFromCart = (cartId: string) => {
    notificationConnection.invoke("LeaveCart", cartId);
}

const onReceiveCartInvitationNotification = (username: string) => {
    toastFriendRequest(`${username} has invited you to join their cart!`);
    queryClient.invalidateQueries({queryKey:["cart", "invitations"]});
}

const onNotifyCartInvitationAccepted = (username: string) => {
    toastMessage(username, `${username} has joined the cart`);
}

const onUpdateCart = (cartId: string) => {
    queryClient.invalidateQueries({queryKey: ["cart-content", cartId]});
}

const onUpdateConnections = () => {
    queryClient.invalidateQueries({queryKey:["connections"]});
}