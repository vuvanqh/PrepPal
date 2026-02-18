import { useQuery } from "@tanstack/react-query"
import { getPersonalDetails } from "../api/accountApi"
import { queryClient } from "../api/authentication";
import { login as loginApi, type loginDTO } from "../api/authentication";
import { useNavigate } from "react-router-dom";


export default function useAuth(){
    const navigate = useNavigate();
    const {data: userData = [], isPending} = useQuery({
        queryFn: getPersonalDetails,
        queryKey: ["personal-details"],
        enabled: !!localStorage.getItem("token"),
        staleTime: 5*60*1000
    });

    const login = async(data: loginDTO) => {
        const resp = await loginApi(data);
        localStorage.setItem("token",resp.token);
        queryClient.invalidateQueries({queryKey: ["personal-details"]});
        return resp;
    }

    const logout = () => {
        navigate("/");
        queryClient.removeQueries(),
        localStorage.clear()
    }

    return {
        userData,
        isAuthenticated: !!localStorage.getItem("token"),
        logout,
        login,
        isPending
    }
}