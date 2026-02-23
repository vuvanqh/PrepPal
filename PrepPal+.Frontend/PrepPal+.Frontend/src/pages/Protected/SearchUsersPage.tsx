import { useParams } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { searchConnections } from "../../api/connectionApi";
import FoodImg from "../../assets/food.jpg";
import UserPreviewCard  from "../../components/UI/UserPreviewCard";

import type { userResponse } from "../../types/SocialTypes";

export default function SearchUserPage(){
    const {username} = useParams();

    const {data, isLoading} = useQuery<userResponse[]>({
        queryKey: ["searchUsers"],
        queryFn: ()=>searchConnections(username!)
    });

    if(isLoading) return <p>Loading...</p>

    return <>
        <div className="search-banner">
            <img src={FoodImg}/>
        </div>
        <section className="search-page">
            <div className="search-inner">
                <header className="search-meta">
                    <h2>
                        Search Results for: {username}
                    </h2>
                </header>

                {data?.length==0 ? 
                    <div className="empty-state-box">
                        <p>No users found with username: {username} </p>
                        <span>Try a different username</span>
                    </div>
                    :
                    <div className="search-recipe-grid">
                        {data?.map( u => (
                            <UserPreviewCard key={u.userName} userData={u}/>
                        ))}
                    </div>
                }
            </div>
            
        </section>
    </>
}