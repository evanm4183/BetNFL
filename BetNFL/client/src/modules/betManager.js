import { getToken } from "./authManager";

const apiUrl = "/api/bet";

export const postBet = (bet) => {
    return getToken().then((token) => {
        return fetch(apiUrl, {
            method: "POST",
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(bet)
        })
    });
}