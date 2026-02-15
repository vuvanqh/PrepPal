import type { ReactNode } from 'react';

type NavbarProps = {
    children: ReactNode
}

export default function Navbar({children}: NavbarProps) {
    return <nav className="navbar">
        {children}
    </nav>
}

