import Navbar from "../../components/UI/Navbar";
import { Outlet } from "react-router-dom";
import Logo from '../../assets/logo.png';
import { useNavigate  } from 'react-router-dom';
import MainPageContent from "../../components/MainPageContent";

export default function IntroLayout(){
    const navigate = useNavigate();

    return <>
        <Navbar>
            <div>
                <div>
                    <h1>PrepPal+</h1>
                    <img src={Logo}/>
                </div>
                <span className="ml-7 mr-2">Search</span>
                <input className="bg-stone-200 rounded-2xl px-2 text-stone-500" placeholder={"Find Recipes..."}/>
            </div>
            <div>
                <button onClick={()=>navigate("/register")}>Register</button>
                <button onClick={()=>navigate("/login")}>Login</button>
            </div>
        </Navbar>
        <MainPageContent/>
        <Outlet/>   
    </>
}