import ContainerModal from "./ContainerModal";
import LikedContent from "../RecipeUI/LikedContent";
import Ingredients from "./Ingredients";
import { useState } from "react";

type CartModalProps = {
    close: ()=>void
}

export default function CartModal({ close}: CartModalProps ){
    const [ingredientsState, setState] = useState(true);

    return <ContainerModal open label="Cart" close={close}>
            <div className="cart-actions justify-center gap-6">
                <button onClick={()=>setState(false)} disabled={!ingredientsState}>Recipes</button>
                <button onClick={()=>setState(true)} disabled={ingredientsState}>Ingredients</button>
            </div>
            {ingredientsState?<Ingredients/>:<LikedContent/>}
    </ContainerModal>
}