import type { ReactNode } from "react";



export default function ErrorContainer({children}:{children: ReactNode}){
    return <ul>
        {children}
    </ul>
}