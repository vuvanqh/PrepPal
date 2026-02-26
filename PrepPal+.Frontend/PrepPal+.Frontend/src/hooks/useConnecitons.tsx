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
        mutationFn: async (request: modifyConnectinoRequest) => {
            await modifyConnection(request);
        },
        onMutate: async (request: modifyConnectinoRequest) => {
                    await queryClient.cancelQueries({queryKey: ["connections"]});
        
                    const prevLiked = queryClient.getQueryData<connectionResponse[]>([connections]);
        
                    queryClient.setQueryData<connectionResponse[]>([connections], (old = []) => {
        
                        if(request.action==="Reject")
                            return old.filter(c=> c.connectionId!=request.connectionId && c.status=="Pending");
                        if(request.action==="Remove")
                            return old.filter(c=> c.connectionId!=request.connectionId && c.status=="Accepted");
                        if(request.action==="Accept")
                        return old;
                    })
                    
                    return {prevLiked}
                },
                onError: (_err, _vars, context) =>{
                    if(context?.prevLiked){
                        queryClient.setQueryData<connectionResponse[]>([connections],context.prevLiked);
                    }
                },
                onSettled: () => queryClient.invalidateQueries({queryKey:["connections"]}),
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