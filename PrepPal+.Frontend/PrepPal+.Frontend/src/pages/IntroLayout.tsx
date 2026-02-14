import Navbar from "../components/UI/Navbar";
import { Outlet } from "react-router-dom";
import { ToastContainer, Bounce } from 'react-toastify';
import { QueryClientProvider } from "@tanstack/react-query";
import {queryClient} from "../api/account";

export default function IntroLayout(){

    return <QueryClientProvider client={queryClient}>
        <Navbar/>
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
    </QueryClientProvider>
}