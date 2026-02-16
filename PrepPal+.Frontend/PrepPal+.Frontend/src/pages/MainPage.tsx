import Navbar from "../components/UI/Navbar";
import { Outlet } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import Welcome from "../assets/welcome.png"


export default function MainPage(){
    const navigate = useNavigate();

    function onLogout(){
        navigate("/");
        localStorage.clear();
    }

    return <>
        <Navbar>
             <div>
                <h1>Hey, {localStorage.getItem("username")!}</h1>
                <img src={Welcome}/>
                <span className="ml-7 mr-2">Search</span>
                <input className="bg-stone-200 rounded-2xl px-2 text-stone-500" placeholder={"Find Recipes..."}/>
            </div>
            <div>
                <button onClick={onLogout}>Logout</button>
            </div>
        </Navbar>
        
        <Outlet/>  
    </>
}