import { useState, useEffect } from "react"
import SettledBetCard from "./SettledBetCard"
import { getBettorSettledBets } from "../../modules/userProfileBetManager"

export default function SettledBetsList() {
    const [settledBets, setSettledBets] = useState();
    
    useEffect(() => {
        getBettorSettledBets().then((settledBets) => {
            setSettledBets(settledBets);
        });
    }, []);

    return (
        <div className="settled-bet-container">
            <div className="settled-bets-list">
                {
                    settledBets?.map((bet) => <SettledBetCard key={bet.id} settledBet={bet}/>)
                }
            </div>
        </div>
    )
}