import Modal from "./Modals/Modal";
import type { meal } from "../types/RecipeTypes";
import {useState } from "react";
import useLikes from "../customHooks/useLikes";

type RecipeModalProps = {
    meal: meal,
    open: boolean,
    onClose: () =>void
}


export default function RecipeModal({meal, open ,onClose}: RecipeModalProps){
    const [imageLoaded, setLoad] = useState(true);
    const {likedRecipes, toggleLike, isPending} = useLikes();
    const liked = likedRecipes?.some(r => r.externalId === meal.externalId);

    return <Modal open={open && !imageLoaded} onClose={onClose}>
        <form className="recipe-modal">
           
            <div className="recipe-modal-image-wrapper">
                <img  className="recipe-modal-image" src={`${meal.imageUrl}`} alt={meal.name} onLoad={()=>setLoad(false)}/>
            </div>
            <div className="recipe-modal-header">
                <h2>{meal.name}</h2>
                <p className="recipe-meta">{meal.category} · {meal.area}</p>
            </div>

            <div className="recipe-actions">
                <button className="primary" type="button" onClick={()=>toggleLike({meal,type: liked?"unlike":"like"})} disabled={isPending}>❤️ {!liked?"Like":"Unlike"}</button>
                <button className="secondary">🛒 Add to cart</button>
            </div>

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
        </form>
    </Modal>
}