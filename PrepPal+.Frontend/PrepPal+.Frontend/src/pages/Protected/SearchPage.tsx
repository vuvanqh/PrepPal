import useRecipe from "../../hooks/useSearchRecipe"
import { useParams } from "react-router-dom";
import Carousel from "../../components/Carousel";
import useGetRandomRecipe from "../../hooks/useGetRandomRecipes";
import RecipePreviewCard from "../../components/RecipePreviewCard";
import FoodImg from "../../assets/food.jpg"


export default function SearchPage(){
    const {name} = useParams();
    const {recipes, isPending} = useRecipe(name);
    const {recipes: randomRecipes, isPending: isRandomPending} = useGetRandomRecipe();
    if(isPending)
        return <p>Loading</p>
    return <>
        <div className="search-banner">
            <img src={FoodImg}/>
        </div>
        <section className="search-page">
            <div className="search-inner">
                <header className="search-meta">
                    <h2>
                        Search Results for: {name}
                    </h2>
                </header>

                {recipes.length==0 ? 
                    <div className="empty-state-box">
                        <p>No recipes found for {name} </p>
                        <span>Try a different keyword or check the recommendations below 👇</span>
                    </div>
                    :
                    <div className="search-recipe-grid">
                        {recipes.map(r=> (
                            <RecipePreviewCard key={r.externalId} meal={r} showActions={true}/>
                        ))}
                    </div>
                }
            </div>
            
        </section>
        <section className="recommendations">
            <Carousel isPending={isRandomPending} items={randomRecipes} label="Recommended for You :D"/>
        </section>
    </>
}