import { createPortal } from "react-dom";

type Props = {
  menu: { x: number; y: number } | null;
  menuRef: React.RefObject<HTMLDivElement>;
  carts: { cartId: string; ownerUserName: string }[];
  onSelect: (cartId: string) => void;
};

export default function CartSelectorPortal({ menu, menuRef, carts, onSelect}: Props) {
  const dialog = document.querySelector("dialog");
  if (!menu || !dialog) return null;

  return createPortal(
    <div
      ref={menuRef}
      style={{ top: menu.y, left: menu.x }}
      className="context-menu"
      onClick={(e)=>e.stopPropagation()}
    >
      <ul>
        {carts.map((c) => (
          <li key={c.cartId}>
            <button type="button" onClick={() => onSelect(c.cartId)}>
              {c.ownerUserName}'s cart
            </button>
          </li>
        ))}
      </ul>
    </div>,
    dialog
  );
}