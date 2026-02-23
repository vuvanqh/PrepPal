import {HubConnection} from "@microsoft/signalr";
import { useEffect, useRef } from "react";
import type { messageResponse } from "../types/ChatTypes";
import { chatConnection } from "../hubConnections";

type receiveMessageHandler = (
    message: messageResponse,
    connectionId: string
) => void


export default function useChatHub(onReceivedMessage: receiveMessageHandler ){
    const connectionRef = useRef<HubConnection | null>(chatConnection);

    useEffect(()=>{        
        chatConnection.on("ReceiveMessage",onReceivedMessage);

        return () => {
            chatConnection.off("ReceiveMessage", onReceivedMessage);
        }
    }, [onReceivedMessage]);

    return connectionRef.current;
}


