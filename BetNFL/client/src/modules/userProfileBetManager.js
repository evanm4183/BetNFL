import { getToken } from "./authManager";

const apiUrl = "/api/userProfileBet";

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