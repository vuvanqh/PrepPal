import { url, makeRequest } from "./util";


const cartUrl = url + "/cart";


export async function getCartContents(cartId:string){
    return await makeRequest({urlSuffix:`/get-content/${cartId}`})
}

export async function addToCart(cartId: string, externalId:number){
    return await makeRequest({
        urlSuffix: cartUrl + "/add-recipe",
        body: {cartId, externalId},
        requestMethod:"POST"
    }  );
}

export async function removeFromCart(cartId: string, externalId:number){
    return await makeRequest({
        urlSuffix: cartUrl + "/remove-recipe",
        body: {cartId, externalId},
        requestMethod:"POST"
    }  );
}

export async function getOwnedCarts(){
    return await makeRequest({urlSuffix: `/get-owned`})
}

export async function getAccessibleCarts(){
    return await makeRequest({urlSuffix: `/get-accessible`})
}

export async function getCart(cartId:string) {
    return await makeRequest({urlSuffix: `/get-cart/${cartId}`})
}

export async function createCart(){
    return await makeRequest({urlSuffix: `/create-cart`})
}

export async function deleteCart(cartId:string){
    return await makeRequest({urlSuffix: `/delete-cart/${cartId}`})
}

