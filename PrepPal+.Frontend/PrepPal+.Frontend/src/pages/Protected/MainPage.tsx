import Navbar from "../../components/UI/Navbar";
import { Outlet } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import Welcome from "../../assets/welcome.png"
import useAuth from "../../hooks/useAuth";
import { ModalContext } from "../../store/ModalContext";
import { useContext, useState, useRef, useEffect, type SubmitEvent } from "react";
import useLikes from "../../hooks/useLikes";
import Sidebar from "../../components/UI/Sidebar/Sidebar";
import { useCartContent, useOwnedCarts } from "../../hooks/useCartRecipe";
import { useSignalR } from "../../hooks/useSignalR";
import { startConnections } from "../../hubConnections";
import ConversationHost from "../../components/ConversationHost";
import { NavLink } from "react-router-dom";

export default function MainPage(){
    const searchInput = useRef<HTMLInputElement>(null);
    const [isOpen, setIsOpen] = useState(false);
    const {ownedCarts} = useOwnedCarts();
    const {cartRecipes} = useCartContent(ownedCarts[0]);
    
    useSignalR();
    const navigate = useNavigate();
    const {userData, isPending, isAuthenticated} = useAuth();
    const {open} = useContext(ModalContext);
    const {likedRecipes} = useLikes();

    useEffect(() => {
        function handleEsc(e: KeyboardEvent) {
            if (e.key === "Escape") {
            setIsOpen(false);
            }
        }
        document.addEventListener("keydown", handleEsc);
        return () => document.removeEventListener("keydown", handleEsc);
    }, []);

    useEffect(() => {
        if (isAuthenticated) {
            startConnections();
        }
    }, [isAuthenticated, isPending])

    function openLikes(){
        open({type: "likes"})
    }
    function openCart(){
        open({type: "cart"})
    }
    function searchSubmit(e: SubmitEvent<HTMLFormElement>){
        e.preventDefault();

        const searchValue = searchInput.current?.value.trim();
        if(!searchValue) return;

        navigate(`/main/search/${searchInput.current?.value}`);
        e.currentTarget.reset(); 
    }
    return <>
        <Navbar>
             <div>
                <h1>Hey, {userData?.userName}</h1>
                <img src={Welcome}/>
                <span className="ml-7 mr-2">Search</span>
                <form onSubmit={(e) => searchSubmit(e)}>
                    <input ref={searchInput} className="bg-stone-200 rounded-2xl px-2 text-stone-500" placeholder={"Find Recipes..."}/>
                </form>
            </div>
            
            <div>
                <button id="navbar-likes-button" type="button" onClick={openLikes}>❤️({likedRecipes.length??0})</button>
                <button id="navbar-likes-button" type="button" onClick={openCart}>🛒({cartRecipes.length??0})</button>
                <button className="" onClick={() => setIsOpen(true)}>☰</button>
            </div>
        </Navbar>

        
        <Sidebar isOpen={isOpen} onClose={() => setIsOpen(false)}/>
        <ConversationHost/>
       
        <Outlet/>  
    </>
}