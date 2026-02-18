import RecipeModal from "./RecipeModal";
import CartModal from "./CartModal";
import { useContext } from "react";
import { ModalContext } from "../../store/ModalContext";
import LikesModal from "./LikesModal";
// const MODAL_REGISTRY = {
//   recipe: RecipeModal,
//   likes: ContainerModal,
//   cart: CartModal,
//   settings: SettingsModal,
//   confirmDelete: ConfirmDeleteModal,
// };


export function ModalRoot() {
  const { state, close } = useContext(ModalContext);
 // console.log(getPending, likedRecipes);
  const current = state.stack.at(-1);
  if(!current) return null;

  console.log(state.stack.length);
  switch(current.type){
    case "likes": return <LikesModal close={close}/>
    case "recipe": return  <RecipeModal open meal={current.meal} onClose={close}/>
    case "cart": return  <CartModal close={close} />
  };
}