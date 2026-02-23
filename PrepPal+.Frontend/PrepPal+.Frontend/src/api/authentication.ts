import {QueryClient} from "@tanstack/react-query";
import { apiClient } from "./apiClient";
export const queryClient = new QueryClient();

export type registerDTO = {
    FirstName: string,
    LastName: string,
    UserName: string,
    Email: string,
    PhoneNumber: string,
    Password: string,
}

export type loginDTO = {
    Email: string,
    Password: string
}


const authUrl= "/auth"


export const register = async (registerData: registerDTO) => (await apiClient.post(authUrl+"/register",registerData)).data;

export const login = async (loginData: loginDTO) => (await apiClient.post(authUrl + '/login', loginData)).data;