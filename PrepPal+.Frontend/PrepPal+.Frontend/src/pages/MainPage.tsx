import Navbar from "../components/UI/Navbar";
import { Outlet } from "react-router-dom";
import { QueryClientProvider } from "@tanstack/react-query";
import {queryClient} from "../api/authentication";
import { useNavigate } from "react-router-dom";
import Welcome from "../assets/welcome.png"

export default function MainPage(){
    const navigate = useNavigate();

    function onLogout(){
        navigate("/");
        localStorage.clear();
    }

    return <QueryClientProvider client={queryClient}>
        <Navbar>
             <div>
                <h1>Hey, {localStorage.getItem("username")!}</h1>
                <img src={Welcome}/>
            </div>
            <div>
                <button onClick={onLogout}>Logout</button>
            </div>
        </Navbar>
        <header className="hero">
            <div className="hero-content">
                <h1>PrepPal+</h1>
                <p>Prepare Shopping Lists with Ease</p>
            </div>
        </header>
        <Outlet/>   
    </QueryClientProvider>
}