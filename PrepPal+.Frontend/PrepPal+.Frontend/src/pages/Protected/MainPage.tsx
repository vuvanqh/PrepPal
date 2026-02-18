import Navbar from "../../components/UI/Navbar";
import { Outlet } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import Welcome from "../../assets/welcome.png"
import useAuth from "../../hooks/useAuth";
import { ModalContext } from "../../store/ModalContext";
import { useContext, useRef, useEffect, type SubmitEvent } from "react";
import useLikes from "../../hooks/useLikes";

export default function MainPage(){
    const navigate = useNavigate();
    const {userData, logout, isPending, isAuthenticated} = useAuth();
    const {open} = useContext(ModalContext);
    const {likedRecipes} = useLikes();
    const searchInput = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if(!isAuthenticated && !isPending){
            logout();
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
                <h1>Hey, {!isPending && userData["userName"]}</h1>
                <img src={Welcome}/>
                <span className="ml-7 mr-2">Search</span>
                <form onSubmit={(e) => searchSubmit(e)}>
                    <input ref={searchInput} className="bg-stone-200 rounded-2xl px-2 text-stone-500" placeholder={"Find Recipes..."}/>
                </form>
            </div>
            
            <div>
                <button id="navbar-likes-button" type="button" onClick={openLikes}>❤️({likedRecipes.length??0})</button>
                <button id="navbar-likes-button" type="button" onClick={openCart}>🛒</button>
                <button onClick={logout}>Logout</button>
            </div>
        </Navbar>
        <Outlet/>  
    </>
}