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
            <div className="container-modal-header">
                <h3 className="container-modal-title">
                    {label}
                </h3>
                <hr/>
            </div>
            <div className="container-modal-body">
                <div className="container-grid">
                    {likedRecipes.map(recipe => (
                        <RecipePreviewCard key={recipe.externalId} meal={recipe}
                            variant="compact" className="recipe-preview-grid" showActions/>
                    ))}
                </div>
            </div>
        </div>
    </Modal>
}