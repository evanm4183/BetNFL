const apiUrl = "/api/UserType";

export const getPublicUserTypes = () => {
    return fetch(apiUrl).then((res) => {
        return res.json();
    });
}