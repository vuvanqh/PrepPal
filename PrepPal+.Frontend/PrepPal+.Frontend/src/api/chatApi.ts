import { apiClient } from "./apiClient";

export type sendMessageDto = {
    connectionId: string,
    message: string
}

const chatUrl = "/chat/"

export const sendMessage = async (messageDto: sendMessageDto) => {
    console.log(`sending ${messageDto.message}`);
    return (await apiClient.post(chatUrl + `send/${messageDto.connectionId}`,{message: messageDto.message})).data;
}

export const getConversation = async (connectionId: string) => (await apiClient.get(chatUrl + `get-conversation/${connectionId}`)).data;