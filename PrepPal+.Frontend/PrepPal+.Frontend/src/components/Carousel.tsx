import { useState } from "react";
import RecipeItem from "./RecipeItem";
import type {meal} from "../types/RecipeTypes";

type CarouselProps = {
  items: meal[];
  itemsPerView?: number;
  label?: string
};

const SLIDE_WIDTH = 240;

export default function Carousel({items, label="Our Favourite Recipes", itemsPerView=6}:CarouselProps){
    const [index,setIndex] = useState(0);
    const maxIndex = items.length - itemsPerView;

    return <div className="carousel-block" >
    <h2 className="carousel-label">{label}</h2>
    <div className="carousel" style={{ width: `${itemsPerView * SLIDE_WIDTH}px` }}>
        <button onClick={()=>setIndex(prev => Math.max(prev-1,0))} disabled={index===0}>
            {"<"}
        </button>

        <div className="carousel-viewport" style={{ "width": `${itemsPerView * 220}px` }}>
            <ul className="carousel-track" style={{ "--index": index } as React.CSSProperties}>
                {items.map((item)=>(
                    <RecipeItem key={item.externalId} meal={item} className="carousel-item"/>
                ))}
            </ul>
        </div>

        <button onClick={()=>setIndex(prev => Math.min(prev+1,maxIndex))} disabled={index===maxIndex}>
            {">"}
        </button>
    </div>
    </div>
}