import type { meal } from "../../types/RecipeTypes";
import { useContext } from "react";
import { ModalContext } from "../../store/ModalContext";
import useAuth from "../../hooks/useAuth";
import useLikes from "../../hooks/useLikes";
import { useCartSelector } from "../../hooks/useCartSelector";
import CartSelectorPortal from "../UI/CartSelectorPortal";

type Variant = "carousel" | "compact";

type RecipePreviewCardProps = {
  meal: meal;
  variant?: Variant;
  showActions?: boolean;
  className?: string;
  cartId?: string;
};

export default function RecipePreviewCard({meal, variant="carousel",showActions=false, className="",cartId, ...props}: RecipePreviewCardProps)
{
    const {open} = useContext(ModalContext);
    const {isAuthenticated} = useAuth();
    const {likedRecipes, toggleLike:useToggleLike} = useLikes();
    const liked = likedRecipes.some(r=>r.externalId===meal.externalId);
    const { menu, openMenu, addToCart, allCarts, menuRef } = useCartSelector(meal);

    function openRecipe(){
        open({type: "recipe", meal});
    }

    function toggleLike(e:React.MouseEvent<HTMLButtonElement>){
        e.stopPropagation();
        useToggleLike({meal, type: "like",action:liked?"remove":"add"});
        console.log({meal, type: "like",action:liked?"remove":"add"});
    }

    return <article className={`recipe-preview recipe-preview-${variant} ${className}`} role="button" onClick={openRecipe} {...props}>        
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

        {showActions && isAuthenticated && (
            <div className="recipe-preview-actions">
            <button className="primary" onClick={(e)=>toggleLike(e)}>❤️</button>
            <button className="secondary" onClick={(e)=>{e.stopPropagation(); openMenu(e)}}>🛒</button>
            </div>
        )}
         {menu && <CartSelectorPortal menu={menu} menuRef={menuRef} carts={allCarts.carts} onSelect={addToCart}/>}
    </article>
}