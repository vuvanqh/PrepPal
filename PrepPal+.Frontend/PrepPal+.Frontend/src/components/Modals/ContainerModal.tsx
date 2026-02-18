import Modal from "./Modal"
import type { ReactNode } from "react"

type ContainerModalProps = {
    label: string,
    close?: () => void,
    open: boolean,
    children: ReactNode
}

export default function ContainerModal({label, close, open, children}: ContainerModalProps){
    return <Modal open={open} onClose={close}>
        <div className="container-modal">
            <div className="container-modal-header">
                <h3 className="container-modal-title">
                    {label}
                </h3>
                <hr/>
            </div>
            <div className="container-modal-body">
                {children}
            </div>
        </div>
    </Modal>
}