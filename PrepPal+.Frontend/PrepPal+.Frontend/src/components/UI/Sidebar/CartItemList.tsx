import { useAccessibleCarts } from '../../../hooks/useCartRecipe';
import CartItem from './CartItem';

export function CartItemList({filter=""}:{filter?:string}){
    const {accessibleCarts} = useAccessibleCarts();
    const filteredCarts = accessibleCarts?.filter(c=> c.ownerUserName.toLocaleLowerCase().includes(filter));
    return <>
    {!!filteredCarts && filteredCarts.length>0? filteredCarts.map(c => <CartItem key={c.cartId} cart={c}/>):
        <p>No accessible carts yet</p>
    }
    </>
}