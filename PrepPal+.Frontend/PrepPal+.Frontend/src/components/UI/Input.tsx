import type { ReactHTMLElement } from "react"

type InputProps = {
    label: string,
    id: string,
    className?: string
} & React.InputHTMLAttributes<HTMLInputElement>; 

const classes = "w-full p-2 border-2 rounded-sm border-stone-300 bg-stone-200 text-stone-600 focus:outline-none focus:border-stone-500"
export default function Input({label, id, className = '', ...props}:InputProps){
    return <p className="flex flex-col gap-1 my-4 w-full">
        <label htmlFor={id} className="text-sm font-bold uppercase text-stone-400 text-left ">{label} </label>
        <input name={id} {...props} className={classes} required/>
    </p>
}