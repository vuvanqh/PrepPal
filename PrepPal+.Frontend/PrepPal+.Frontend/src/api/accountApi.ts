import { apiFetch, url, HttpError } from "./util";



const account = url + "/account"


export async function getPersonalDetails(){
    var response = await apiFetch(account + "/my-info");
    
    if(!response.ok)
    {
        const error = new HttpError((await response.json()).message || "User not found" , response.status);
        error.code = response.status;
        throw error;
    }
    var data = await response.json();
    //console.log(data, "hey");
    return data;
}
