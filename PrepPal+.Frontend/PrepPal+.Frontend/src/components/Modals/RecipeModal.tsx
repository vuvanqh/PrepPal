import Modal from "./Modal";
import type { meal } from "../../types/RecipeTypes";
import { useState } from "react";
import useLikes from "../../hooks/useLikes";
import useAuth from "../../hooks/useAuth";
import CartSelectorPortal from "../UI/CartSelectorPortal";
import { useCartSelector } from "../../hooks/useCartSelector";
type RecipeModalProps = {
    meal: meal,
    open: boolean,
    onClose: () =>void
}


export default function RecipeModal({meal, open ,onClose}: RecipeModalProps){
    const [imageLoaded, setLoad] = useState(true);
    const {likedRecipes, toggleLike, isPending} = useLikes();
    const {isAuthenticated} = useAuth();
    const { menu, openMenu, addToCart, allCarts, menuRef } = useCartSelector(meal);

    const liked = likedRecipes?.some(r => r.externalId === meal.externalId);
    console.log(meal.ingredients)
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
                <button className="secondary" type="button" onClick={(e)=>{e.stopPropagation(); openMenu(e)}}>🛒 Add to cart</button>
            </div>}

            <div className="recipe-modal-body">

                <section className="recipe-section">
                    <h3>Ingredients</h3>

                    <ul className="ingredients-list">
                        {meal.ingredients.map((i,idx) => (
                        <li key={`${i.ingredientName} - ${idx} - ${i.ingredientMeasure}`}>
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
            {menu && <CartSelectorPortal menu={menu} menuRef={menuRef} carts={allCarts.carts} onSelect={addToCart}/>}
            
        </form>
    </Modal>
}