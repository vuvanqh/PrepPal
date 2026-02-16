import type { meal } from "../types/RecipeTypes"
import type React from "react"

type RecipeProps = {
    meal: meal,
    className: string
} & React.LiHTMLAttributes<HTMLLIElement>

export default function RecipeItem({meal, className ="", ...props}: RecipeProps){
    return <li {...props} className={className}>
        <img src={`${meal.imageUrl}\\preview`}/>
        <p className="recipe-preview-name">{meal.name}</p>
        <p className="recipe-preview-category">{meal.category}</p>
    </li>
}