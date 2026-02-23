import { useQuery, useMutation } from "@tanstack/react-query";
import { addConnection, getConnections, type modifyConnectinoRequest, modifyConnection } from "../api/connectionApi";
import type { connectionResponse } from "../types/SocialTypes";
import { queryClient } from "../api/authentication";
export function useConnections(){
    const {data: connections} = useQuery<connectionResponse[]>({
        queryKey: ["connections"],
        queryFn: getConnections
    });

    const {mutate} = useMutation({
        mutationFn: (request: modifyConnectinoRequest) => modifyConnection(request)
    });

    const {mutate:invite} = useMutation({
        mutationFn: async (userName: string) => {
            await addConnection({userName});
            queryClient.invalidateQueries({queryKey: ["connections"]});
        }
    });

    return{
        connections,
        acceptConnection: (connectionId: string) => mutate({
            connectionId,
            action: "Accept"
        }),
        rejectConneciton: (connectionId: string) => mutate({
            connectionId,
            action: "Reject"
        }),
        cancelConnection: (connectionId: string) => mutate({
            connectionId,
            action: "Cancel"
        }),
        removeConnection: (connectionId: string) => mutate({
            connectionId,
            action: "Remove"
        }),
        invite
    }
}