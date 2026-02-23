import type { cartRecipe } from "../../types/CartTypes";

type ingredientDTO = {
    name: string,
    measure: string,
    quantity: number
}
// const dummyIngredients:ingredientDTO[] = [{
//     name: "egg",
//     measure: "egg",
//     quantity:10
// },{
//     name: "salt",
//     measure: "pinch",
//     quantity:9
// },{
//     name: "flour",
//     measure: "400 gram",
//     quantity: 2
// }];

//&& i.measure==ingredient.ingredientMeasure
function aggregateIngredients(recipes:cartRecipe[]){
    let ingredients:ingredientDTO[] = []
    console.log(recipes)
    recipes?.forEach((recipe) => {
        recipe.recipe.ingredients?.forEach((ingredient) => {
            var idx = ingredients?.findIndex(i=>i.name == ingredient.ingredientName)
            if(idx!=-1)
                ingredients[idx].quantity++;
            else 
                ingredients.push({
                    name: ingredient.ingredientName,
                    measure: ingredient.ingredientMeasure,
                    quantity: 1
                })
        })
    })
    ingredients.sort((a,b) => {
        for(var i=0;i<a.name.length;i++){
            if(a.name[i] < b.name[i])
                return 0;
        }
        return b.name.length>a.name.length?0:1
    })
    return ingredients;
}

export default function Ingredients({cartRecipes}:{cartRecipes: cartRecipe[]}){
    const ingredients = aggregateIngredients(cartRecipes);
    return <div>
        <ul>
            {ingredients.map(i => (
                <IngredientItem ingredient={i} key={i.name}/>
            ))}
        </ul>
    </div>
}
{/* <div> - one after another each item should kinda darken a bit on hover
        <ul>
            {dummyIngredients.map(i => (
                <IngredientItem ingredient={i} key={i.name}/>
            ))}
        </ul>
    </div> */}

export function IngredientItem({ingredient}:{ingredient:ingredientDTO}){
    return <li className="ingredient-item">
        <p className="ingredient-name">{ingredient.name}</p>

        <div className="ingredient-right">
            <span className="ingredient-quantity">{ingredient.quantity} x {ingredient.measure}</span>

        </div>
    </li>
}


{/* <div> - flex 
    <p>{ingredient.name}</p> right left clour - grayish textstone +-500
    <div> right aligned
        <p>{ingredient.quantity} x {ingredient.measure}</p> same colour as ingredient name
        <button>+</button> -buttons of text col black with a bit darker background (not black -darker than white) and round
        <button>-</button>
    </div>
</div> */}

