import { useState, useEffect } from "react";
import { getMyOpenBets } from "../../modules/userProfileBetManager";
import OpenBetCard from "./OpenBetCard";

export default function OpenBetsList() {
    const [openBets, setOpenBets] = useState();

    useEffect(() => {
        getMyOpenBets().then((openBets) => {
            setOpenBets(openBets);
        });
    }, [])

    return (
        <div className="open-bets-list">
            {
                openBets?.map((bet) => <OpenBetCard key={bet.id} openBet={bet} />)
            }
        </div>
    );
}