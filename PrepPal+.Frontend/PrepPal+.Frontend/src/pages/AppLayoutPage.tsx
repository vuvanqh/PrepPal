import { Outlet } from "react-router-dom";
import { ToastContainer, Bounce } from 'react-toastify';
import { QueryClientProvider } from "@tanstack/react-query";
import {queryClient} from "../api/authentication";
import RecipeModalProvider from "../store/RecipeModalContext";

export default function AppLayoutPage(){
    return <QueryClientProvider client={queryClient}>
        <RecipeModalProvider>
            <ToastContainer
                position="top-right"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
                theme="colored"
                transition={Bounce}
                />
            <Outlet/>
        </RecipeModalProvider>
    </QueryClientProvider>
}