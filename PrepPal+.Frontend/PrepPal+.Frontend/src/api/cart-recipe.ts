import { apiClient } from "./apiClient";

const cartUrl = "/cart";


export const getCartContents = async(cartId:string) => (await apiClient.get(cartUrl + `/get-content/${cartId}`)).data;
export const addToCart = async (cartId: string, externalId:number) => (await apiClient.post( cartUrl + "/add-recipe", {cartId, externalId})).data;

export const removeFromCart = async (cartId: string, externalId:number) => {
    console.log("removing recipe")
    return (await apiClient.post( cartUrl + "/remove-recipe", {cartId, externalId})).data;
}

export const getOwnedCarts = async () => (await apiClient.get( cartUrl +`/get-owned`)).data;
export const getAccessibleCarts = async () => (await apiClient.get(cartUrl +`/get-accessible`)).data;
export const getCart = async (cartId:string) => (await apiClient.get(cartUrl + `/get-cart/${cartId}`)).data;
export const createCart = async () => (await apiClient.post(cartUrl+ `/create-cart`)).data;
export const deleteCart = async (cartId:string) => (await apiClient.delete(cartUrl+ `/delete-cart/${cartId}`)).data


