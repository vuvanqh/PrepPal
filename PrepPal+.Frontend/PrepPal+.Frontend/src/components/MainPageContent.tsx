
import useGetRandomRecipe from "../hooks/useGetRandomRecipes";
import Carousel from "./Carousel";

export default function MainPageContent(){
    const {recipes,isPending} = useGetRandomRecipe();

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

    let content = isPending? <p className="text-stone-950 text-5xl">Loading...</p>:<Carousel items={recipes} isPending={isPending}/>;

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