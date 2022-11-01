import { getToken } from "./authManager";

const apiUrl = "/api/userProfileBet";

export const getMyOpenBets = () => {
    return getToken().then((token) => {
        return fetch(apiUrl, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            },
        }).then((res) => res.json());
    });
}

export const postUserProfileBet = (upBet) => {
    return getToken().then((token) => {
        return fetch(apiUrl, {
            method: "POST",
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(upBet)
        })
    });
}

export const settleOpenBetsByGame = (gameId) => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/${gameId}`, {
            method: "PUT",
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
    });
}