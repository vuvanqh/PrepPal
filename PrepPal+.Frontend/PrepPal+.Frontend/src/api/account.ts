import {QueryClient} from "@tanstack/react-query";

export const queryClient = new QueryClient();

export type registerDTO = {
    FirstName: string,
    LastName: string,
    UserName: string,
    Email: string,
    PhoneNumber: string,
    Password: string,
}

class HttpError extends Error {
  code: number;
  info?: any;

  constructor(message: string, code: number, info?: any) {
    super(message);
    this.code = code;
    this.info = info;
  }
}

const url="http://localhost:5054/api/Account"

export async function register(registerData: registerDTO){
    const registerUrl = url+"/register"
    const response = await fetch(registerUrl,{
        method: "POST",
        headers: {"Content-Type": "application/json"},
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