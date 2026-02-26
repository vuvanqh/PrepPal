import { createContext, useReducer, type ReactNode } from "react";
import type { meal } from "../types/RecipeTypes";
export type ModalType = 
    | { type: "likes" }
    | { type: "cart", cartId: string }
    | { type: "recipe"; meal: meal };

type ModalState = {
    stack: ModalType[]
};

type Action = 
    | { type: "open", modal:ModalType}
    | { type: "close" };

type ContextType = {
    state: ModalState,
    open: (modal: ModalType) => void
    close: () => void
}


export const ModalContext = createContext<ContextType>(null!);

function reducer(prev: ModalState, action: Action): ModalState{
    switch(action.type){
        case "open": return {stack: [...prev.stack,action.modal]};
        case "close": return {stack: prev.stack.slice(0,-1)}; 
        default: return prev;
    }
}

const initialState = {
    stack: []
};

export default function ModalContextProvider({children}: {children:ReactNode}){
    const [state, dispatch] = useReducer(reducer, initialState);

    const ctxValue: ContextType = {
        state,
        open: (modal) => dispatch({type:"open", modal}),
        close: () => dispatch({type: "close"})
    };

    return <ModalContext value={ctxValue}>
        {children}
    </ModalContext>
}