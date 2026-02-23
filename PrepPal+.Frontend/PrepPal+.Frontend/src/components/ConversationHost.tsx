import { useChat } from "../store/ConversationContext";
import Conversation from "./UI/Message/Conversation";

export default function ConversationHost() {
  const { activeChat } = useChat();

  if (!activeChat) return null;

  return (
    <Conversation
      connectionId={activeChat.connectionId}
      username={activeChat.username}
    />
  );
}