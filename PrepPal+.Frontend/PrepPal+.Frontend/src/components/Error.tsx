
type ErrorProps ={
    message: string,
    children?: React.ReactNode
}

export default function Error({message}: ErrorProps){
    return <li className="text-red-950 px-0.5">
        {message}
    </li>
}