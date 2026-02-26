import ContainerModal from "./ContainerModal";
import CartRecipeContent from "../Cart/CartRecipeContent";
import Ingredients from "../Cart/Ingredients";
import { useState } from "react";
import { useCartContent } from "../../hooks/useCartRecipe";
import { useCartContentMutations } from "../../hooks/useCartRecipe";

type CartModalProps = {
    cartId: string
    close: ()=>void
}

export default function CartModal({cartId ,close}: CartModalProps ){
    const [ingredientsState, setState] = useState(true);
    const {cartRecipes} = useCartContent(cartId);
    const {clearCart} = useCartContentMutations();
    return <ContainerModal open label="Cart" close={close}>
        <div className="cart-actions">
            <div className="cart-toggle">
                <button onClick={()=>setState(false)} className={!ingredientsState ? "active" : ""} disabled={!ingredientsState}>Recipes</button>
                <button onClick={()=>setState(true)} className={ingredientsState ? "active" : ""} disabled={ingredientsState}>Ingredients</button>
            </div>
            <div className="cart-danger">
                <button onClick={()=>clearCart(cartId)}>Clear Cart 🗑️</button>
            </div>
        </div>
            {ingredientsState?<Ingredients cartRecipes={cartRecipes}/>:<CartRecipeContent cartRecipes={cartRecipes} cartId={cartId}/>}
    </ContainerModal>
}