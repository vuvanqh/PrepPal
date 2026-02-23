import { useQuery } from "@tanstack/react-query"
import { getPersonalDetails } from "../api/accountApi"
import { queryClient } from "../api/authentication";
import { login as loginApi, type loginDTO } from "../api/authentication";
import { useNavigate } from "react-router-dom";
import { stopConnections } from "../hubConnections";


type personalDetails = {
    firstName: string,
    lastName: string,
    userName: string,
    email: string,
    phoneNumber: string
} | undefined

export default function useAuth(){
    const navigate = useNavigate();
    const {data, isPending} = useQuery<personalDetails>({
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
        stopConnections();
    }

    return {
        userData: data,
        isAuthenticated: !!localStorage.getItem("token"),
        logout,
        login,
        isPending
    }
}