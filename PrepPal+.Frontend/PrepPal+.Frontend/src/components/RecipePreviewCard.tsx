import type { meal } from "../types/RecipeTypes";
import { useContext } from "react";
import { ModalContext } from "../store/ModalContext";

type Variant = "carousel" | "compact";

type RecipePreviewCardProps = {
  meal: meal;
  variant?: Variant;
  showActions?: boolean;
  className?: string;
};

export default function RecipePreviewCard({meal, variant="carousel",showActions=false, className="", ...props}: RecipePreviewCardProps)
{
    const {open} = useContext(ModalContext);

    function openRecipe(){
        open({type: "recipe", meal});
    }

    return <article className={`recipe-preview-${variant} ${className}`} role="button" onClick={openRecipe} {...props}>        
        <div className="recipe-preview-image">
            <img src={`${meal.imageUrl}/preview`} alt={meal.name}/>
        </div>
        <div className="recipe-preview-content">
            <h4 className="recipe-preview-name">
                {meal.name}
            </h4>

            <p className="recipe-preview-category">
                {meal.category}
            </p>
        </div>

        {showActions && (
            <div className="recipe-preview-actions">
            <button className="primary">❤️</button>
            <button className="secondary">🛒</button>
            </div>
        )}
    </article>
}