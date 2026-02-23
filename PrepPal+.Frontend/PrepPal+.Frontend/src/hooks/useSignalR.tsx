import useChatHub from "./useChatHub";
import { queryClient } from "../api/authentication";
import type { conversationResponse } from "../types/ChatTypes";
import useNotificationHub from "./useNotificationHub";
import { toastMessage } from "../toastConfig";
import { useChat } from "../store/ConversationContext";

export function useSignalR(){
    const {activeChat} = useChat();

    useChatHub((messageResponse, connectionId)=>{
        queryClient.setQueryData(["conversation",connectionId], (prev: conversationResponse | undefined) => {
            if(connectionId==activeChat?.connectionId)
                toastMessage(messageResponse.senderUsername,messageResponse.message);
            if(!prev)
                return {
                    connectionId: connectionId,
                    messages: [messageResponse]
                };
            else
                return {...prev, messages:[...prev.messages, messageResponse]};
        })
    });

    useNotificationHub();
}