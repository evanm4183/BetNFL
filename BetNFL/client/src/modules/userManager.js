import { getToken } from "./authManager"

const apiUrl = "/api/user"

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