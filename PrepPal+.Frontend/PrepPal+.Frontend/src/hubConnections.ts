import {HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import { refreshPromise } from "./api/apiClient";

export const notificationConnection = new HubConnectionBuilder()
            .withUrl("https://localhost:7101/notification",{accessTokenFactory: ()=> localStorage.getItem("token")!})
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: () => {
                    if (!refreshPromise) return null;
                    return 2000;
                }
            })
            .build();

export const chatConnection = new HubConnectionBuilder()
            .withUrl("https://localhost:7101/chat",{accessTokenFactory: ()=>localStorage.getItem("token")!})
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: () => {
                    if (!refreshPromise) return null;
                    return 2000;
                }
            })
            .build();


export async function startConnections(carts:string[]){
    if(notificationConnection.state==HubConnectionState.Disconnected) {
        notificationConnection.start().catch(err => console.error("SignalR error notification: ",err));
        await joinAllCartGroups(carts);
        notificationConnection.onreconnected(async()=> await joinAllCartGroups(carts));
    }
    if(chatConnection.state==HubConnectionState.Disconnected)
        chatConnection.start().catch(err => console.error("SignalR error chat: ",err));
}

export function stopConnections(){
    if(notificationConnection.state==HubConnectionState.Disconnected)
        notificationConnection.stop();
    if(chatConnection.state!=HubConnectionState.Disconnected)
        chatConnection.stop();
}


function joinAllCartGroups(carts: string[]){
    carts.forEach(async (c) => await notificationConnection.invoke("JoinCart",c));
}