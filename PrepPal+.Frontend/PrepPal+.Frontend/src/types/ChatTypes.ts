export type messageResponse = {
    messageId: string,
    senderUsername: string,
    timeStamp: Date,
    message: string
}

export type conversationResponse = {
    connectionId: string,
    messages: messageResponse[]
}