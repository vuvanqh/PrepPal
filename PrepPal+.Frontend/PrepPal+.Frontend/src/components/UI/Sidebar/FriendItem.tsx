import { useChat } from "../../../store/ConversationContext";
import type { connectionResponse } from "../../../types/SocialTypes";
import { useState, useEffect, useRef } from "react";
import { useConnections } from "../../../hooks/useConnecitons";
import { createPortal } from "react-dom";
import { queryClient } from "../../../api/authentication";

export default function FriendItem({connection}: {connection:connectionResponse}){
    const {openChat} = useChat();
    const [menu, setMenu] = useState<{x: number; y: number;} | null>(null);
    const {removeConnection} = useConnections();
    const menuRef = useRef<HTMLDivElement>(null);

    function removeFriend(){
        setMenu(null);
        removeConnection(connection.connectionId);
        queryClient.invalidateQueries({queryKey:["connections"]});
    }

    useEffect(() => {
    if (!menu) return;

    function handleClick(e: MouseEvent) {
      if (!menuRef.current?.contains(e.target as Node)) {
        setMenu(null);
      }
    }

    function handleEsc(e: KeyboardEvent) {
      if (e.key === "Escape") setMenu(null);
    }

    document.addEventListener("mousedown", handleClick);
    document.addEventListener("keydown", handleEsc);

    return () => {
      document.removeEventListener("mousedown", handleClick);
      document.removeEventListener("keydown", handleEsc);
    };
  }, [menu]);
    return <li className="sidebar-item">
        <div className="item-row">
            <div className="item-main" onContextMenu={(e) => {
                e.preventDefault();
                setMenu({x: e.clientX, y: e.clientY});
            }}>
                {connection.lastName} {connection.firstName}<p className="username">@{connection.userName}</p>
            </div>
            <button className="item-action" onClick={() => openChat({connectionId: connection.connectionId, username: connection.userName})}>✉︎</button>

            {menu && createPortal(<div ref={menuRef} className="context-menu danger" style={{ top: menu.y, left: menu.x }}>
                <button onClick={removeFriend}>Remove Connection</button>
                </div>,document.body)}
        </div>
    </li>
}

