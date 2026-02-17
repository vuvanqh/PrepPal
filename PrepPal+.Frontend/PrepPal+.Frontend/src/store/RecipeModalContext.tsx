import { createContext, useState ,type ReactNode } from "react";
import type { meal } from "../types/RecipeTypes";
import RecipeModal from "../components/Modals/RecipeModal";

type recipeContext = {
    openModal: (meal:meal) => void,
    closeModal: () => void
}

export const RecipeModalContext = createContext<recipeContext>({
    openModal: ()=> {},
    closeModal: () => {}
})


export default function RecipeModalProvider({children}: {children: ReactNode}){
    const [meal, setMeal] = useState<meal|null>(null);
    
    const openModal = (m:meal) => setMeal(m)
    const closeModal = () => setMeal(null);

    const ctxValue:recipeContext = {
        openModal,
        closeModal
    }

    return <RecipeModalContext value={ctxValue}>
        {children}
        {meal && <RecipeModal meal={meal} open={meal!=null} onClose={closeModal}/>}
    </RecipeModalContext>
}

