//DEPRECATED

export const url="https://localhost:7101/api"

let refreshPromise: Promise<void> | null = null;

export async function apiFetch(input: RequestInfo, init: RequestInit = {}) {
    const response = await myFetch(input, init);

    if(response.status != 401 || !localStorage.getItem("token"))
        return response

    await refreshToken();

    return await myFetch(input, init);
}

function myFetch(input: RequestInfo, init: RequestInit = {}){
    const token = localStorage.getItem("token");
    return fetch(input, {
        ...init,
        headers: {
            ...init.headers,
            ...(init.body && {"Content-Type": "application/json"}),
            ...(token && { Authorization: `Bearer ${token}` }),
        },
        credentials: "include"
    });
}

async function refreshToken(){

    if(!refreshPromise){
        refreshPromise = ( async () => {
            const refreshResponse = await fetch(url + "/auth/refreshToken",{
            method: "POST",
            credentials: "include"
            });

            if(!refreshResponse.ok){
                console.log(`refresh token err - ${(await refreshResponse.json()).message}`)
                localStorage.clear();
                window.location.href = "/login";
                throw new Error("Session Expired");
            }

            const data = await refreshResponse.json();
            localStorage.setItem("token", data);
        })().finally(() => refreshPromise=null);
    }

    return refreshPromise;
}


export class HttpError extends Error {
  code: number;
  info?: any;

  constructor(message: string, code: number, info?: any) {
    super(message);
    this.code = code;
    this.info = info;
  }
}

type MakeRequestOptions = {
  urlSuffix: string;
  body?: object | null;
  requestMethod?: string;
  errMessage?: string;
};

export async function makeRequest({urlSuffix ,body = null, requestMethod = "GET", errMessage = "Cannot perform the operation"}: MakeRequestOptions ){
    var response = await apiFetch(url+urlSuffix,{
        method: requestMethod,
        ...(body !== null && { body: JSON.stringify(body) })
    } );

    if(!response.ok)
    {
        const error =new HttpError(`${errMessage} - ${(await response.json()).message}. Try again later`, response.status);
        error.code = response.status;
        console.log(error);
        throw error;
    } 
    if(requestMethod=="POST") return;
    
    return await response.json();
}