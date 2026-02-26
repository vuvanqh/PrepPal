import { apiClient } from "./apiClient";
import type { cartInvitationRequest, modifyCartAccess, modifyCartInvitation } from "../types/CartTypes";

const cartUrl = "/cart";

export const sendCartInvitation = async (invitation: cartInvitationRequest) => (await apiClient.post(cartUrl + "/invite-to-cart",invitation)).data;
export const modifyInvitaiton = async (invitation: modifyCartInvitation) => (await apiClient.put(cartUrl + "/modify-invitation", invitation)).data;
export const modifyAccess = async (accessRequest: modifyCartAccess) => (await apiClient.put(cartUrl + "/modify-access",accessRequest));
export const getPendingInvitations = async () => (await apiClient.get(cartUrl + "/get-pending-invitations")).data;