import Modal from "./Modal";
import type { meal } from "../../types/RecipeTypes";
import {useRef, useState, useEffect } from "react";
import useLikes from "../../hooks/useLikes";
import useAuth from "../../hooks/useAuth";
import { useOwnedCarts, useCartContentMutations, useAccessibleCarts } from "../../hooks/useCartRecipe";
import { createPortal } from "react-dom";
import type { accessibleCarts } from "../../types/CartTypes";
type RecipeModalProps = {
    meal: meal,
    open: boolean,
    onClose: () =>void
}


export default function RecipeModal({meal, open ,onClose}: RecipeModalProps){
    const [imageLoaded, setLoad] = useState(true);
    const {likedRecipes, toggleLike, isPending} = useLikes();
    const {isAuthenticated} = useAuth();
    const {ownedCarts} = useOwnedCarts();
    console.log(ownedCarts);
    const {addRecipe} = useCartContentMutations();
    const [menu, setMenu] = useState<{x: number; y: number;} | null>(null)
    const menuRef = useRef<HTMLDivElement | null>(null);
    const {accessibleCarts} = useAccessibleCarts();

    const allCarts:accessibleCarts = {carts: [...(ownedCarts?[{cartId: ownedCarts[0], ownerUserName:"My Cart"}]:[]), ...accessibleCarts]}
    const liked = likedRecipes?.some(r => r.externalId === meal.externalId);
    const dialog = document.querySelector("dialog")
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

    function addOnClick(cartId: string)
    {
        addRecipe({cartId: cartId, recipe: meal});
        setMenu(null);
    }

    return <Modal open={open && !imageLoaded} onClose={onClose}>
        <form className="recipe-modal">
           
            <div className="recipe-modal-image-wrapper">
                <img  className="recipe-modal-image" src={`${meal.imageUrl}`} alt={meal.name} onLoad={()=>setLoad(false)}/>
            </div>
            <div className="recipe-modal-header">
                <h2>{meal.name}</h2>
                <p className="recipe-meta">{meal.category} · {meal.area}</p>
            </div>

            {isAuthenticated &&
            <div className="recipe-actions">
                <button className="primary" type="button" onClick={()=>toggleLike({meal,type: "like",action:liked?"remove":"add"})} disabled={isPending}>❤️ {!liked?"Like":"Unlike"}</button>
                <button className="secondary" type="button" onClick={(e)=>setMenu({x: e.clientX, y: e.clientY})}>🛒 Add to cart</button>
            </div>}

            <div className="recipe-modal-body">

                <section className="recipe-section">
                    <h3>Ingredients</h3>

                    <ul className="ingredients-list">
                        {meal.ingredients.map((i,idx) => (
                        <li key={`${i.ingredientName} - ${idx}`}>
                            <span>{i.ingredientName}</span>
                            <em>{i.ingredientMeasure}</em>
                        </li>
                        ))}
                    </ul>
                </section>
                
                <section className="recipe-section">
                    <h3>Instructions</h3>
                    <p className="instructions">{meal.instructions}</p>
                </section>
            </div>
            {menu && dialog && createPortal(<>
                <div ref={menuRef} style={{ top: menu.y, left: menu.x }} className="context-menu"> 
                    <ul>
                        {allCarts.carts.map(c => (
                        <li key={c.cartId} className="cart-context">
                            <button type="button" onClick={() => addOnClick(c.cartId)}>{c.ownerUserName}'s cart</button>
                        </li>))}
                    </ul>           
                </div>
            </>, dialog)}
            
        </form>
    </Modal>
}