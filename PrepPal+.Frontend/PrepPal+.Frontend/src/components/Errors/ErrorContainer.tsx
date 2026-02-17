import type { ReactNode } from "react";



export default function ErrorContainer({children}:{children: ReactNode}){
    return <p>
        <ul>
        {children}
    </ul>
    </p>
}