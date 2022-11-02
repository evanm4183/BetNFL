import { useState, useEffect } from "react";
import { getBettorOpenBets, getSportsbookOpenBets } from "../../modules/userProfileBetManager";
import OpenBetCard from "./OpenBetCard";

export default function OpenBetsList({isSportsbook}) {
    const [openBets, setOpenBets] = useState();

    useEffect(() => {
        if (isSportsbook) {
            getSportsbookOpenBets().then((openBets) => {
                setOpenBets(openBets);
            });
        } else {
            getBettorOpenBets().then((openBets) => {
                setOpenBets(openBets);
            });
        }
    }, []);

    return (
        <div className="open-bets-list">
            {
                openBets?.map((bet) => <OpenBetCard key={bet.id} openBet={bet} isSportsbook={isSportsbook}/>)
            }
        </div>
    );
}