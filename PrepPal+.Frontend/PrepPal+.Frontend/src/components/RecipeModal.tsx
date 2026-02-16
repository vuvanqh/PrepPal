import Modal from "./Modal";
import type { meal } from "../types/RecipeTypes";
import { useEffect, useState } from "react";
import {useMutation} from '@tanstack/react-query';
import { addInteraction, type interactionType } from "../api/accountApi";
import { toastError, toastSuccess } from "../toastConfig";

type RecipeModalProps = {
    meal: meal,
    open: boolean,
    onClose: () =>void
}


export default function RecipeModal({meal, open ,onClose}: RecipeModalProps){
    const [imageLoaded, setLoad] = useState(true);
    const {mutate} = useMutation({
        mutationFn: (interaction: interactionType) => addInteraction(interaction),
        onError: (error) => {
            toastError(error.message);
            console.log(error.message);
        },
        onSuccess: () => toastSuccess("yaaay!")
    });

    useEffect(()=>{
        setLoad(true);
        //console.log("lol");
    },[]);

    function likeRecipe(){
        const interaction:interactionType = {
            externalRecipeId: meal.externalId,
            type: "like"
        }
        mutate(interaction);
    }

    return <Modal open={open && !imageLoaded} onClose={onClose}>
        <form className="recipe-modal">
           
            <img  className="recipe-modal-image" src={`${meal.imageUrl}`} alt={meal.name} onLoad={()=>setLoad(false)}/>

            <div className="recipe-modal-body">
                <div className="recipe-modal-header">
                    <h2>{meal.name}</h2>
                    <p className="recipe-meta">{meal.category} · {meal.area}</p>
                </div>

                <div className="recipe-actions">
                    <button className="primary" type="button" onClick={likeRecipe}>❤️ Like</button>
                    <button className="secondary">🛒 Add to cart</button>
                </div>

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