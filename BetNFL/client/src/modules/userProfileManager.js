import { getToken } from "./authManager"

const apiUrl = "/api/userProfile"

export const getAdminStatus = () => {
    return getToken()?.then((token) => {
        return fetch(`${apiUrl}/IsAdmin`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            }
        }).then((res) => {
            return res.json();
        });
    });
}

export const getUserType = () => {
    return getToken()?.then((token) => {
        return fetch(`${apiUrl}/UserType`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            }
        }).then((res) => {
            return res.text();
        });
    });
}