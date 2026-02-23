import type { messageResponse } from "../../../types/ChatTypes";

type messageProps ={
    message: messageResponse,
    className?: string,
}
export default function MessageItem({message, className="", ...props}:messageProps){
    const time = new Date(message.timeStamp).toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
    })
    return <div className="message-bubble" {...props}>
        <div className="message-meta">{message.senderUsername} · {time}</div>
        <div className={className}>{message.message}</div>
    </div>  
}

