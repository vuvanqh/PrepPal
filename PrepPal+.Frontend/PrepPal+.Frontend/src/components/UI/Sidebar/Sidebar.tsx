import {useState, useRef, type SubmitEvent} from 'react';
import Profile from "../../../assets/account.png";
import useAuth from '../../../hooks/useAuth';
import FriendItem from './FriendItem';
import { useNavigate } from "react-router-dom";
import FriendRequestItem from './FriendRequestItem';
import { useConnections } from '../../../hooks/useConnecitons';
import { CartInvitationList } from './CartInvitationList';
import { CartItemList } from './CartItemList';


export default function Sidebar({isOpen, onClose}: {isOpen:boolean, onClose:()=>void}){
    const {userData, logout} = useAuth();
    const [isFriends, setFriends] = useState(true);
    const [viewRequests, setViewRequests] = useState(false);
    const [cartFilter, setSearch] = useState("");
    const {connections} = useConnections();
    const navigate = useNavigate();
    const searchInput = useRef<HTMLInputElement>(null);
    
    function searchSubmit(e: SubmitEvent<HTMLFormElement>){
        e.preventDefault();

        const searchValue = searchInput.current?.value.trim();
        if(!searchValue) return;

        navigate(`/main/connection-search/${searchInput.current?.value.toLowerCase()}`);
        e.currentTarget.reset(); 
    }

    function toggleView(state:boolean){
        setFriends(state);
        setSearch("");
    }

    const FriendComp = viewRequests? FriendRequestItem: FriendItem;
    const CartComp = viewRequests? CartInvitationList: CartItemList; 
    console.log(connections);
    const conn = viewRequests? connections?.filter(c=>c.status=="Pending" &&c.requestedByUsername!=userData?.userName):connections?.filter(c=>c.status=="Accepted");
    return  <>
        <div className={`sidebar-backdrop ${isOpen ? "visible" : ""}`} onClick={onClose} />

        <aside className={`sidebar ${isOpen ? "open" : ""}`}>
            <div className="sidebar-profile">
                <img src={Profile} alt="Profile"/>
                <p>{userData?.firstName} {userData?.lastName}</p> 
            </div>
        
            <div className="sidebar-toggle">
                <button onClick={()=>toggleView(true)} className={isFriends ? "active" : ""}>Friends</button>
                <button onClick={()=>toggleView(false)} className={!isFriends ? "active" : ""}>Carts</button>
            </div>
            <hr/>

            <div> 
                <button className={`view-requests-btn ${viewRequests ? "active" : ""}`} onClick={()=>setViewRequests(!viewRequests)}>{isFriends?"Friend Requests": "Cart Invitations"}</button>
            </div>

            
            <div className="sidebar-content">
                <form onSubmit={searchSubmit}>
                    <input placeholder={isFriends?"Find Friends...":"Filter Carts..."} ref={searchInput} onChange={(e) => setSearch(e.target.value.toLowerCase())}/>
                </form>
                
                    <div className="sidebar-items">
                        <ul>
                            
                        {isFriends?
                            conn && conn.length==0?
                            <p>{viewRequests?"No requests":"No connections yet"}</p>:
                            conn?.map(connection => (<FriendComp connection={connection} key={connection.userName}/>))
                            :
                            <CartComp filter={cartFilter}/>}
                        </ul>
                    </div>
                
            </div>

            <button className="logout-btn" onClick={logout}>Log Out</button> 
        </aside>
    </>
}


{/* <div>  -style this - when clicked some small window as if you clickthe right mouse button opens with the list of cart invitations and friend requests
        <button>{isFriends?"Friend Requests": "Cart Invitations"}</button>
</div> */}
