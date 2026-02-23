import { Navigate } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import { ChatContextProvider } from "../store/ConversationContext";

export default function ProtectedRoute({children}: {children: React.ReactNode}){
    const {isAuthenticated} = useAuth();
    return isAuthenticated?<ChatContextProvider>{children}</ChatContextProvider>:<Navigate to="/login" replace/>;
}