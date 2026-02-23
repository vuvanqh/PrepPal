import {HubConnection} from "@microsoft/signalr";
import { useEffect, useRef } from "react";
import { toastSuccess, toastFriendRequest } from "../toastConfig";
import { queryClient } from "../api/authentication";
import { notificationConnection } from "../hubConnections";

export default function useNotificationHub( ){
    const connectionRef = useRef<HubConnection | null>(notificationConnection);

    useEffect(()=>{        
        notificationConnection.on("ReceiveConnectionRequestNotification",onReceiveConnectionRequest);
        notificationConnection.on("NotifyConnectionAccepted",onConnectionAccepted);
        
        return () => {
            notificationConnection.off("ReceiveConnectionRequestNotification",onReceiveConnectionRequest);
            notificationConnection.off("NotifyConnectionAccepted",onConnectionAccepted);
        }
    }, []);

    return connectionRef.current;
}

const onConnectionAccepted = (username: string) => {
    toastFriendRequest(`${username} has sent you a conneciton request`);
    queryClient.invalidateQueries({queryKey:["connections"]});
}

const onReceiveConnectionRequest = (username: string) => {
    toastSuccess(`${username} has accepted your conneciton request`);
}