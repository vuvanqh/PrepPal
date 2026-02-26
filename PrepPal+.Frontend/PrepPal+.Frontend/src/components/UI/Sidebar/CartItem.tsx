import type { accessibleCart } from "../../../types/CartTypes"
import { ModalContext } from "../../../store/ModalContext"
import { useContext } from "react";

export default function CartItem({cart}: {cart:accessibleCart}){
    const {open} = useContext(ModalContext);

    function openCart() {
        open({type: "cart", cartId: cart.cartId})
    }

    return <li className="sidebar-item">
        <div className="item-row">
            <p className="item-main">{cart.ownerUserName}'s Cart</p>
            <button className="item-action" onClick={openCart}>🛒</button>
        </div>
    </li>
}

