import { useCartInvitationActions } from "../../../hooks/useCartSocials"
import type { cartInvitationResponse } from "../../../types/CartTypes"


export default function CartInvitation({invitation}: {invitation:cartInvitationResponse}){
    const {accept, decline} = useCartInvitationActions(invitation.cartId,invitation.invitationId);
    return <li className="sidebar-item">
        <div className="item-row">
            <p className="item-main">{invitation.ownerUserName}'s Cart</p>
             <button onClick={decline} className="item-action">❌</button>
            <button onClick={accept} className="item-action">+</button>
        </div>
    </li>
}
