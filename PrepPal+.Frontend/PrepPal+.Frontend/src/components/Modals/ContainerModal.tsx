import Modal from "./Modal"
import RecipePreviewCard from "../RecipePreviewCard"
import type { meal } from "../../types/RecipeTypes"

type ContainerModalProps = {
    label: string,
    close?: () => void,
    open: boolean,
    likedRecipes: meal[]
}

export default function ContainerModal({label, close, open, likedRecipes}: ContainerModalProps){
    return <Modal open={open} onClose={close}>
        <div className="container-modal">
            <h3 className="container-modal-title">
                {label}
            </h3>
            <hr/>
            <div className="container-grid">
                {likedRecipes.map(recipe => (
                    <RecipePreviewCard key={recipe.externalId} meal={recipe}
                        variant="compact" showActions/>
                ))}
            </div>
        </div>
    </Modal>
}