import { getToken } from "./authManager";

const apiUrl = "api/game";

export const getGamesByWeek = (week) => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/${week}`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            },
        }).then((res) => {
            return res.json()
        });
    });
}

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