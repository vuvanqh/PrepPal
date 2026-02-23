import axios from "axios";
import { stopConnections } from "../hubConnections";

export let refreshPromise: Promise<string> | null = null;

export const apiClient = axios.create({
    baseURL: "https://localhost:7101/api",
    withCredentials: true
});

const refreshClient = axios.create({
    baseURL: "https://localhost:7101/api",
    withCredentials: true
});

apiClient.interceptors.request.use(config => {
    const token = localStorage.getItem("token");
    if(token)
        config.headers.Authorization = `Bearer ${token}`;

    return config;
})

apiClient.interceptors.response.use(response => response,
    async error => {
        const request = error.config;

        if(error.response?.status !=401 || request._retry)
            return Promise.reject(error);

        request._retry = true;

        try {
            if(!refreshPromise){
                refreshPromise = refreshClient.post("/auth/refreshToken")
                .then(res => {
                    const newToken = res.data;
                    localStorage.setItem("token", newToken);
                    return newToken;
                }).finally(() => refreshPromise=null);
            }
            const newToken = await refreshPromise;

            request.headers.Authorization = `Bearer ${newToken}`;
            return apiClient(request);
        }
        catch(err){
            stopConnections();
            localStorage.clear();
            window.location.href = "/";
            return Promise.reject(err);
        }
    }
)