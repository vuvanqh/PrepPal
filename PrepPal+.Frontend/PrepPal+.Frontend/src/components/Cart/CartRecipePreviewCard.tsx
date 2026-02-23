import type { meal } from "../../types/RecipeTypes";
import { useContext } from "react";
import { ModalContext } from "../../store/ModalContext";
import { useCartContentMutations } from "../../hooks/useCartRecipe";

type RecipePreviewCardProps = {
    cartId: string;
    quantity: number;
    meal: meal;
    className?: string;
};


export default function CartRecipePreviewCard({cartId,quantity, meal, className="", ...props}: RecipePreviewCardProps)
{
    const {open} = useContext(ModalContext);
    const {addRecipe:add, removeRecipe:remove} = useCartContentMutations(cartId);
    function openRecipe(){
        open({type: "recipe", meal});
    }

    function removeRecipe(e:React.MouseEvent<HTMLButtonElement>){
        e.stopPropagation();
        remove(meal);
    }
    function addRecipe(e:React.MouseEvent<HTMLButtonElement>){
        e.stopPropagation();
        add(meal);
    }

    return <article className={`recipe-preview recipe-preview-compact ${className}`} role="button" onClick={openRecipe} {...props}>        
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

       
        <div className="recipe-preview-actions">
            <button onClick={(e)=>removeRecipe(e)}>-</button>
            <span className="justify-center align-middle py-2 px-1 text-stone-950">{quantity}</span>
            <button onClick={(e)=>addRecipe(e)}>+</button>
        </div>
        
    </article>
}