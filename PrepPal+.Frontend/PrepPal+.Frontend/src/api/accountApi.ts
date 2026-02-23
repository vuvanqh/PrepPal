import { apiClient } from "./apiClient";


const account = "/account"


export const getPersonalDetails = async () => (await apiClient.get(account + "/my-info")).data;

