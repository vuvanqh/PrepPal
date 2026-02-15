import {toast, Bounce, type ToastOptions } from 'react-toastify';

export const toastConfig = { //remove later
    position: "top-right",
    autoClose: 3000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: false,
    draggable: true,
    progress: undefined,
    theme: "colored",
    transition: Bounce,
} as ToastOptions;

export function toastSuccess(message:string){
    toast.success(message,toastConfig);
}

export function toastError(message:string){
    toast.error(message,toastConfig);
}