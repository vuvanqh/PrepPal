import useLikes from "../../hooks/useLikes";
import RecipePreviewCard from "./RecipePreviewCard";

export default function LikedContent({}){
    const {getPending, likedRecipes} = useLikes();

    if(getPending) return <p>working on it...</p>;
    
    return  (
    <div className="container-modal-body">
        <div className="container-grid">
            {likedRecipes.map(recipe => (
                <RecipePreviewCard key={recipe.externalId} meal={recipe}
                    variant="compact" className="recipe-preview-grid" showActions/>
            ))}
        </div>
    </div>);
}