import Navbar from "../../components/UI/Navbar";
import { Outlet } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import Welcome from "../../assets/welcome.png"
import useAuth from "../../customHooks/useAuth";
import { ModalContext } from "../../store/ModalContext";
import { useContext } from "react";

export default function MainPage(){
    const navigate = useNavigate();
    const {userData, logout, isPending} = useAuth();
    const {open} = useContext(ModalContext);
    

    function onLogout(){
        navigate("/");
        logout();
    }
    function openLikes(){
        open({type: "likes"})
    }
    function openCart(){
        open({type: "cart"})
    }
    return <>
        <Navbar>
             <div>
                <h1>Hey, {!isPending && userData["userName"]}</h1>
                <img src={Welcome}/>
                <span className="ml-7 mr-2">Search</span>
                <input className="bg-stone-200 rounded-2xl px-2 text-stone-500" placeholder={"Find Recipes..."}/>
            </div>
            
            <div>
                <button id="navbar-likes-button" type="button" onClick={openLikes}>❤️</button>
                <button id="navbar-likes-button" type="button" onClick={openCart}>🛒</button>
                <button onClick={onLogout}>Logout</button>
            </div>
        </Navbar>
        <Outlet/>  
    </>
}