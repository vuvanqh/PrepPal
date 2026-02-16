import type { meal } from "../types/RecipeTypes"
import type React from "react"
import { useContext } from "react";
import { RecipeModalContext } from "../store/RecipeModalContext";

type RecipeProps = {
    meal: meal,
    className: string
} & React.LiHTMLAttributes<HTMLLIElement>

export default function RecipeItem({meal, className ="", ...props}: RecipeProps){
    const {openModal} = useContext(RecipeModalContext);

    return <li {...props} className={className} role="button" onClick={()=>openModal(meal)}>
        <img src={`${meal.imageUrl}\\preview`}/>
        <p className="recipe-preview-name">{meal.name}</p>
        <p className="recipe-preview-category">{meal.category}</p>
    </li>
}