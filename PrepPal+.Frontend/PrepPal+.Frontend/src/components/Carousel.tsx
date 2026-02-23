import { useState } from "react";
import type {meal} from "../types/RecipeTypes";
import RecipePreviewCard from "./RecipeUI/RecipePreviewCard";

type CarouselProps = {
  items: meal[];
  itemsPerView?: number;
  label?: string;
  isPending: boolean
};
    

export default function Carousel({items = [], label="Our Favourite Recipes", itemsPerView=6, isPending}:CarouselProps){
    const [index,setIndex] = useState(0);
    const maxIndex = items.length - itemsPerView;

    if(isPending)
        return <p className="text-stone-950 text-5xl">Loading...</p>

    return <div className="carousel-block" >
    <h2 className="carousel-label">{label}</h2>
    <div className="carousel">
        <button onClick={()=>setIndex(prev => Math.max(prev-1,0))} disabled={index===0}>
            {"<"}
        </button>

        <div className="carousel-viewport" >
            <ul className="carousel-track" style={{ "--index": index } as React.CSSProperties}>
                {items.map((meal)=>(
                     <li key={meal.externalId} className="carousel-slot">
                        <RecipePreviewCard meal={meal} className="recipe-preview-carousel" />
                    </li>
                ))}
            </ul>
        </div>

        <button onClick={()=>setIndex(prev => Math.min(prev+1,maxIndex))} disabled={index===maxIndex}>
            {">"}
        </button>
    </div>
    </div>
}