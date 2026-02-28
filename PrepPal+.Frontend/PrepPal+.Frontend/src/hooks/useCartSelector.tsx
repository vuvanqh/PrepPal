import { useState, useRef, useEffect } from "react";
import { useOwnedCarts, useCartContentMutations, useAccessibleCarts } from "./useCartRecipe";
import type { meal } from "../types/RecipeTypes";
import type { accessibleCarts } from "../types/CartTypes";

export function useCartSelector(meal: meal) {

    const { ownedCarts } = useOwnedCarts();
    const { accessibleCarts } = useAccessibleCarts();
    const { addRecipe } = useCartContentMutations();

    const [menu, setMenu] = useState<{ x: number; y: number } | null>(null);
    const menuRef = useRef<HTMLDivElement>(null!);
        const allCarts:accessibleCarts = {carts: [...(ownedCarts?[{cartId: ownedCarts[0], ownerUserName:"My Cart"}]:[]), ...accessibleCarts]}

        function openMenu(e: React.MouseEvent) {
    setMenu({ x: e.clientX, y: e.clientY });
    }

    function addToCart(cartId: string) {
    addRecipe({ cartId, recipe: meal });
    setMenu(null);
    }

    useEffect(() => {
    if (!menu) return;

    function handleClick(e: MouseEvent) {
        if (!menuRef.current?.contains(e.target as Node)) {
        setMenu(null);
        }
    }

    function handleEsc(e: KeyboardEvent) {
        if (e.key === "Escape") setMenu(null);
    }

    document.addEventListener("mousedown", handleClick);
    document.addEventListener("keydown", handleEsc);

    return () => {
        document.removeEventListener("mousedown", handleClick);
        document.removeEventListener("keydown", handleEsc);
    };
    }, [menu]);

    return { menu, openMenu, addToCart, allCarts, menuRef };
}