import { useState, useEffect } from "react"
import SettledBetCard from "./SettledBetCard"
import { getBettorSettledBets, getSportsbookSettledBets } from "../../modules/userProfileBetManager"

export default function SettledBetsList({isSportsbook}) {
    const [settledBets, setSettledBets] = useState();
    
    useEffect(() => {
        if (isSportsbook) {
            getSportsbookSettledBets().then((settledBets) => {
                setSettledBets(settledBets);
            });
        } else {
            getBettorSettledBets().then((settledBets) => {
                setSettledBets(settledBets);
            });
        }
    }, []);

    return (
        <div className="settled-bet-container">
            <div className="settled-bets-list">
                {
                    settledBets?.map((bet) => <SettledBetCard key={bet.id} settledBet={bet} isSportsbook={isSportsbook}/>)
                }
            </div>
        </div>
    )
}