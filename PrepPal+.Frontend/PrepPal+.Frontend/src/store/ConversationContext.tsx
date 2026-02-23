import { createContext, useContext, useState, useMemo, type ReactNode } from "react";

export type  activeChatType = {
    connectionId: string,
    username: string
} | null;

export type chatContextType = {
    activeChat: activeChatType,
    openChat: (c:activeChatType) => void,
    closeChat: () => void
}

export const ChatContext = createContext<chatContextType | null>(null);

export function ChatContextProvider({children}: {children:ReactNode}){
    const [activeChat, setActiveChat] = useState<activeChatType>(null);
    const ctxValue: chatContextType = useMemo(() => ({
        activeChat,
        openChat: (c:activeChatType) => setActiveChat(c),
        closeChat: () => setActiveChat(null)
    }),[activeChat]);

    return <ChatContext value={ctxValue}>
        {children}
    </ChatContext>
}

export function useChat(){
    const ctx = useContext(ChatContext);
    if(!ctx) throw new Error();

    return ctx;
}