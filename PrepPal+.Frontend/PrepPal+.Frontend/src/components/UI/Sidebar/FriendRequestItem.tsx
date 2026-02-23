import type { connectionResponse } from "../../../types/SocialTypes";
import { useConnections } from "../../../hooks/useConnecitons";

export default function FriendRequestItem({connection}: {connection:connectionResponse}){
    const {acceptConnection, rejectConneciton} = useConnections();
    return <li className="sidebar-item">
        <div className="item-row">
            <button className="item-main">
                {connection.lastName} {connection.firstName}<p className="username">@{connection.userName}</p>
            </button>
            <button onClick={()=>rejectConneciton(connection.connectionId)} className="item-action">❌</button>
            <button onClick={()=>acceptConnection(connection.connectionId)} className="item-action">+</button>
        </div>
    </li>
}
