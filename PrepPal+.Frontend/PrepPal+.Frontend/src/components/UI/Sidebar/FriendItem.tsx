import { useChat } from "../../../store/ConversationContext";
import type { connectionResponse } from "../../../types/SocialTypes";

export default function FriendItem({connection}: {connection:connectionResponse}){
    const {openChat} = useChat();

    return <li className="sidebar-item">
        <div className="item-row">
            <button className="item-main">
                {connection.lastName} {connection.firstName}<p className="username">@{connection.userName}</p>
            </button>
            <button className="item-action" onClick={() => openChat({connectionId: connection.connectionId, username: connection.userName})}>✉︎</button>
        </div>
    </li>
}

