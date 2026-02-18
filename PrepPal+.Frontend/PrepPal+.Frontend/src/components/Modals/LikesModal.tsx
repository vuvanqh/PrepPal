import ContainerModal from "./ContainerModal"
import LikedContent from "../RecipeUI/LikedContent"

type LikesProps = {
    close: () => void,
}

export default function LikesModal({ close}: LikesProps){
    return <ContainerModal open label="Likes" close={close}>
            <LikedContent/>
    </ContainerModal>
}