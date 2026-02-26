import { useCartInvitation } from '../../../hooks/useCartSocials';
import CartInvitation from './CartInvitation';

export function CartInvitationList({filter=""}:{filter?:string}){
    const {invitations} = useCartInvitation();
    const filteredInvitations = invitations?.filter(c=> c.ownerUserName.toLocaleLowerCase().includes(filter));
    return <>
    {!!filteredInvitations? filteredInvitations.map(i => <CartInvitation key={i.invitationId} invitation={i}/>):
        <p>No invitations yet</p>
    }
    </>
}