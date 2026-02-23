import useConversation from "../../../hooks/useConversation"
import { type SubmitEvent, useState, useRef, useEffect } from "react";
import MessageItem from "./MessageItem";
import { useConnections } from "../../../hooks/useConnecitons";

export default function Conversation({connectionId, username, className=""}:{connectionId: string, username:string, className?:string}){
    const {sendMessage,conversation} = useConversation(connectionId);
    const {connections} = useConnections();
    const [messageInput, setInput] = useState<string>("");
    const [minimized, setMinimized] = useState(false);

    const messageBody = useRef<HTMLDivElement>(null);
    const messages = conversation?.messages;
    const sorted = messages? [...messages].sort((a,b) => new Date(a.timeStamp).getDate()-new Date(b.timeStamp).getDate()):[];
    useEffect(() => {
        const el = messageBody.current;
        if (!el) return;

        el.scrollTo({
            top: 0,
            behavior: "smooth",
        });
    }, [conversation?.messages?.length]);

    function onSubmit(e: SubmitEvent<HTMLFormElement>){
        e.preventDefault();
        
        if(messageInput.length==0) return;

        sendMessage(messageInput)
        setInput("");
    }
    return <div className={`chat-modal open ${minimized? "minimized":""} ${className}`}>
        <div>
            <div className="username-bubble" onClick={() => {if(minimized) setMinimized(false)}}>
                <span>{username}</span>

                <button onClick={()=>setMinimized(!minimized)} className="minimize">{minimized?"+":"-"}</button>
            </div>
            <hr/>
            {!minimized && <>
            {sorted.length>0? 
            <>
                <div className="conversation-body" ref={messageBody}>
                    {sorted.reverse().map(m => (
                        <MessageItem key={m.messageId} message={m} className={m.senderUsername==username?"receive-bubble":"sender-bubble"}/>
                    ))}
                </div>

                
            </>:
            <div className="conversation-body empty">
                <p className="content-center">No messages yet</p>
            </div>}
            <form className="input-box" onSubmit={onSubmit}>
                    <input placeholder="message" onChange={(e)=>setInput(e.target.value)} value={messageInput} disabled={connections && !connections.find(c=>c.connectionId==connectionId)}/>
            </form>
            </>
            }
        </div>
    </div>
}
