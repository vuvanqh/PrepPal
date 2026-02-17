
import { getRandomRecipes } from "../api/recipeApi";
import { useQuery } from "@tanstack/react-query";
import Carousel from "./Carousel";

export default function MainPageContent(){
     const {data, isPending} = useQuery({
        queryFn: getRandomRecipes,
        queryKey: ["random-recipes"],
        staleTime: 50000,
    });

    //console.log(data, isError, isPending, error);

    // useEffect(() => {
    // if (isSuccess) {
    //     toastSuccess('worked!!');
    // }
    // }, [isSuccess]);

    // useEffect(() => {
    // if (isError) {
    //     toastError('failed :c');
    // }
    // }, [isError]);

    let content = isPending? <p className="text-stone-950 text-5xl">Loading...</p>:<Carousel items={data}/>;

    return <>
        <header className="hero">
            <div className="hero-content">
                <h1>PrepPal+</h1>
                <p>Prepare Shopping Lists with Ease</p>
                <a href="#learn-more">Learn More</a>
            </div>
        </header>

        <section id="learn-more">
            {content}
        </section>
    </>
}