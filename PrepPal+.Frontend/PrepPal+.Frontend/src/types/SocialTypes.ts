export type connectionResponse = {
    connectionId: string;
    firstName: string;
    lastName: string;
    userName: string;
    requestedByUsername: string;
    status: "Pending" | "Accepted"
};

export type userResponse = {
    firstName: string,
    lastName: string,
    userName: string
}
