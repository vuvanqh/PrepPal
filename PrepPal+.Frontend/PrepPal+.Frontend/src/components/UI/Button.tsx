type ButtonProps = {
    text: string,
    className?: string
} & React.ButtonHTMLAttributes<HTMLButtonElement>; 


export default function Button({text, className="", ...props}:ButtonProps){
    return <button className={className} {...props}>
        {text}
    </button>
}