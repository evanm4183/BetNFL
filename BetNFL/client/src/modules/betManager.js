import { getToken } from "./authManager";

const apiUrl = "/api/bet";

export const getLiveBetForGame = (gameId) => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/${gameId}`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
            }
        }).then((res) => {
            if (res.status === 200) {
                return res.json();
            } else if (res.status === 204) {
                return null;
            }
        });
    });
}

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