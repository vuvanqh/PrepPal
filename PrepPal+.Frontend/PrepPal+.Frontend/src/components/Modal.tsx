import { useRef, useEffect } from "react";
import { createPortal } from "react-dom";

type ModalProps = {
  children: React.ReactNode;
  open: boolean;
  className?: string;
  onClose?: ()=>void;
};

export default function Modal({children, open, onClose, className=""}:ModalProps){
    const dialog = useRef<HTMLDialogElement|null>(null);

    useEffect(()=>{
        const current = dialog.current;
        if(open)
            current?.showModal();
        else if(!open && current?.open)
            current?.close();

    }, [open]);

    return createPortal(
    <dialog ref={dialog} onClose={onClose} className="modal">
        {children}
    </dialog>, document.getElementById("modal")!)
}
