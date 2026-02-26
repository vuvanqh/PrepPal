import {toast, Bounce, Zoom, type ToastOptions } from 'react-toastify';

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
    limit: 3,
} as ToastOptions;

export function toastSuccess(message:string){
    toast.success(message,toastConfig);
}

export function toastError(message:string){
    toast.error(message,toastConfig);
}

export function toastMessage(username: string, message: string) {
  toast(
    <div className="chat-toast">
      <span className="chat-toast-user">@{username}</span>
      <span className="chat-toast-message">{message}</span>
    </div>,
    {
      position: "top-right",
      autoClose: 1800,
      hideProgressBar: true,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: false,
      theme: "dark",
      transition: Zoom,
      limit: 3,
    } as ToastOptions
  );
}


export function toastFriendRequest(username: string) {
  toast(
    <div className="chat-toast">
      <span className="chat-toast-user">@{username}</span>
      <span className="chat-toast-message">
        sent you a friend request
      </span>
    </div>,
    {
      position: "top-right",
      autoClose: 4000,
      hideProgressBar: true,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: false,
      theme: "colored",
      transition: Zoom,
      limit: 3,
    } as ToastOptions
  );
}