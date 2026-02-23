import AnnonymousUser from "../../assets/annonymousUser.png";
import { useConnections } from "../../hooks/useConnecitons";
import type { userResponse } from "../../types/SocialTypes";

type UserPreviewCardProps ={
    userData: userResponse,
    className?: string
}

export default function UserPreviewCard({userData, className=""}:UserPreviewCardProps){
    const {connections, invite} = useConnections();
    console.log(connections);
    const isAdded = connections?.find(c=>c.status!="Accepted");


    return <article className={`recipe-preview recipe-preview-compact ${className}`}>        
        <div className="recipe-preview-image">
            <img src={AnnonymousUser} alt="annonymous user"/>
        </div>
        <div className="recipe-preview-content">
            <h4 className="recipe-preview-name">
                {userData.lastName} {userData.firstName} 
            </h4>

            <p className="recipe-preview-category">
                •{userData.userName}
            </p>
        </div>

       
        <div className="recipe-preview-actions">
            <button onClick={()=>invite(userData.userName)} disabled={!!isAdded}>
                {isAdded?"✔":"✚"}
            </button>
        </div>
        
    </article>
}
