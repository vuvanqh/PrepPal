import Navbar from "../components/UI/Navbar";
import { Outlet } from "react-router-dom";
import { QueryClientProvider, useQuery } from "@tanstack/react-query";
import {queryClient} from "../api/authentication";
import { useNavigate } from "react-router-dom";
import Welcome from "../assets/welcome.png"
import { getRandomRecipes } from "../api/recipeApi";
import { toastSuccess, toastError } from "../toastConfig";
import { useEffect } from "react";

const test = false;

export default function MainPage(){
    const navigate = useNavigate();

    const {data, isError, isPending, refetch, isSuccess} = useQuery({
        queryFn: getRandomRecipes,
        queryKey: ["random-recipes"]
    });

    console.log(data, isError, isPending);

    function onLogout(){
        navigate("/");
        localStorage.clear();
    }

    useEffect(() => {
    if (isSuccess) {
        toastSuccess('worked!!');
    }
    }, [isSuccess]);

    useEffect(() => {
    if (isError) {
        toastError('failed :c');
    }
    }, [isError]);

    let content = undefined;
    if(test)
        content =  <button onClick={async()=>await refetch()} disabled={isPending}>Trial</button>

    return <QueryClientProvider client={queryClient}>
        <Navbar>
             <div>
                <h1>Hey, {localStorage.getItem("username")!}</h1>
                <img src={Welcome}/>
                <span className="ml-7 mr-2">Search</span>
                <input className="bg-stone-200 rounded-2xl px-2 text-stone-500" placeholder={"Find Recipes..."}/>
            </div>
            <div>
                <button onClick={onLogout}>Logout</button>
                {content}
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