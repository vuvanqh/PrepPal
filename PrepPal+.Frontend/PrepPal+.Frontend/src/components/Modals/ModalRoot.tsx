import RecipeModal from "../RecipeModal";
import ContainerModal from "./ContainerModal";
import { useContext } from "react";
import { ModalContext } from "../../store/ModalContext";
import useLikes from "../../customHooks/useLikes";
// const MODAL_REGISTRY = {
//   recipe: RecipeModal,
//   likes: ContainerModal,
//   cart: CartModal,
//   settings: SettingsModal,
//   confirmDelete: ConfirmDeleteModal,
// };


export function ModalRoot() {
  const { state, close } = useContext(ModalContext);
  const {getPending, likedRecipes} = useLikes();
 // console.log(getPending, likedRecipes);
  const current = state.stack.at(-1);
  if(!current) return null;

  switch(current.type){
    case "likes": return <ContainerModal likedRecipes={likedRecipes} open={!getPending} label="Likes" close={close} />
    case "recipe": return <RecipeModal open meal={current.meal} onClose={close}/>
    case "cart": return <ContainerModal likedRecipes={likedRecipes} open label="Likes" close={close} />
  };
}