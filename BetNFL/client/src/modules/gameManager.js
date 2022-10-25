import { getToken } from "./authManager";

const apiUrl = "/api/game";

export const getGamesByWeek = (week) => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/weeklyGames/${week}`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            },
        }).then((res) => {
            return res.json();
        });
    });
}

export const getGameById = (id) => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/${id}`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            },
        }).then((res) => {
            return res.json();
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

export const setScore = (game) => {
    return getToken().then((token) => {
        return fetch(apiUrl, {
            method: "PUT",
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(game)
        })
    });
}

export const deleteGame = (id) => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/${id}`, {
            method: "DELETE",
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
    });
}