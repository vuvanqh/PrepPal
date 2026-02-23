type cartData = {
    ownerUser: string,
    cartId: string,
}
export default function CartItem({cart}: {cart:cartData}){
    return <li className="sidebar-item">
        <div className="item-row">
            <p className="item-main">{cart.ownerUser}'s Cart</p>
            <button className="item-action">🛒</button>
        </div>
    </li>
}

{/* <li role="button">
    <div> - display flex the content below is on the same line
        <p>{cart.ownerUser}'s Cart</p> -left aligned
        <button>🛒(5)</button> - right aliged 
    </div>
</li> */}
