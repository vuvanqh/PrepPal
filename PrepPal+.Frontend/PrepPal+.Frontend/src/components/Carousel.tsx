import { useState } from "react";
import type {meal} from "../types/RecipeTypes";
import RecipePreviewCard from "./RecipePreviewCard";

type CarouselProps = {
  items: meal[];
  itemsPerView?: number;
  label?: string
};
    

export default function Carousel({items, label="Our Favourite Recipes", itemsPerView=6}:CarouselProps){
    const [index,setIndex] = useState(0);
    const maxIndex = items.length - itemsPerView;

    return <div className="carousel-block" >
    <h2 className="carousel-label">{label}</h2>
    <div className="carousel">
        <button onClick={()=>setIndex(prev => Math.max(prev-1,0))} disabled={index===0}>
            {"<"}
        </button>

        <div className="carousel-viewport" >
            <ul className="carousel-track" style={{ "--index": index } as React.CSSProperties}>
                {items.map((meal)=>(
                     <li key={meal.externalId} className="carousel-item">
                        <RecipePreviewCard meal={meal} />
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