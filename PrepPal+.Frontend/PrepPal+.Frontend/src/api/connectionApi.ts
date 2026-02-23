import { apiClient } from "./apiClient";

export type modifyConnectinoRequest = {
    connectionId: string,
    action: "Accept" | "Reject" | "Remove" | "Cancel"
}

export type addConnectionRequest = {
    userName: string
}



const connectionUrl = "/connection"


export const addConnection = async (request: addConnectionRequest) => (await apiClient.post(connectionUrl+ "/request", request)).data;
export const modifyConnection = async (request: modifyConnectinoRequest) => (await apiClient.patch(connectionUrl + "/modify-connection", request)).data;
export const getConnections = async () => (await apiClient.get(connectionUrl+"/get-all")).data;
export const searchConnections = async (search: string) => (await apiClient.get(connectionUrl + `/search/${search}`)).data;