import CartRecipePreviewCard from "./CartRecipePreviewCard";
import type { cartRecipe } from "../../types/CartTypes";

export default function CartRecipeContent({cartRecipes, cartId}:{cartRecipes:cartRecipe[], cartId:string}){
    if(cartRecipes.length==0) return <p>Empty Cart...</p>;
    
    return  (
    
        <div className="container-grid">
            {cartRecipes.map(cartRecipe => (
                <CartRecipePreviewCard cartId={cartId} key={cartRecipe.recipe.externalId} meal={cartRecipe.recipe} quantity={cartRecipe.quantity}
                     className="recipe-preview-grid"/>
            ))}
        </div>
    );
}