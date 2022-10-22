import { getToken } from "./authManager";

const apiUrl = "api/game";

export const postGame = (game) => {
    return getToken().then((token) => {
        return fetch(apiUrl, {
            method: "POST",
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(game)
        })
    });
}