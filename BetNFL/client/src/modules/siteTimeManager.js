import { getToken } from "./authManager";

const apiUrl = "/api/siteTime";

export const getSiteTime = () => {
    return getToken()?.then((token) => {
        return fetch(apiUrl, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            }
        }).then((res) => {
            return res.json();
        });
    });
}