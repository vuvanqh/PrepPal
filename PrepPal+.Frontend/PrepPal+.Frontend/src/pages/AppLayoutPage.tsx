import { Outlet } from "react-router-dom";
import { ToastContainer, Bounce } from 'react-toastify';
import { QueryClientProvider } from "@tanstack/react-query";
import {queryClient} from "../api/authentication";

export default function AppLayoutPage(){
    return <QueryClientProvider client={queryClient}>
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