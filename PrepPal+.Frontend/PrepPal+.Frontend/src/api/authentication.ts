import {QueryClient} from "@tanstack/react-query";
import { apiFetch, HttpError, url } from "./util";
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


const authUrl= url + "/auth"


export async function register(registerData: registerDTO){
    const registerUrl = authUrl+"/register"
    const response = await apiFetch(registerUrl,{
        method: "POST",
        body: JSON.stringify(registerData)
    })

    if(!response.ok)
    {
        const error = new HttpError('An error occured while registering.', response.status);
        error.code = response.status;
        error.info = await response.json();
        throw error;
    }

    return await response.json();
}

export async function login(loginData: loginDTO){
    const loginUrl = authUrl + '/login';
    const response = await apiFetch(loginUrl, {
        method: "POST",
        body: JSON.stringify(loginData)
    })

    if(!response.ok)
    {
        const error =new HttpError('An error occured while logging in.', response.status);
        error.code = response.status;
        error.info = await response.json();
        localStorage.clear();
        throw error;
    }   

    const data = await response.json();
    return data;
}