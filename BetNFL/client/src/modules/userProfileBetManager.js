import { getToken } from "./authManager";

const apiUrl = "/api/userProfileBet";

export const getBettorOpenBets = () => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/BettorOpenBets`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            },
        }).then((res) => res.json());
    });
}

export const getSportsbookOpenBets = () => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/SportsbookOpenBets`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            },
        }).then((res) => res.json());
    });
}

export const getBettorSettledBets = () => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/BettorSettledBets`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            },
        }).then((res) => res.json());
    });
}

export const getSportsbookSettledBets = () => {
    return getToken().then((token) => {
        return fetch(`${apiUrl}/SportsbookSettledBets`, {
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