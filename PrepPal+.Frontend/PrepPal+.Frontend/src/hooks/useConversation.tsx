import { useMutation, useQuery } from "@tanstack/react-query";
import { getConversation, sendMessage } from "../api/chatApi";
import type { conversationResponse } from "../types/ChatTypes";


export default function useConversation(connectionId:string){
    const {data: conversation} = useQuery<conversationResponse>({
        queryKey: ["conversation", connectionId],
        queryFn: () => getConversation(connectionId)
    })

    const {mutate} = useMutation({
        mutationFn: (message: string) => sendMessage({
            connectionId,
            message
        })
    })

    return {
        sendMessage: mutate,
        conversation
    }
}