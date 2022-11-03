import { useState, useEffect } from "react";
import { getGamesWithOpenBets } from "../../modules/gameManager";
import ProcessBetsCard from "./ProcessBetsCard";

export default function ProcessBetsList() {
    const [gamesWithOpenBets, setGamesWithOpenBets] = useState();

    const getAndSetGamesWithOpenBets = () => {
        getGamesWithOpenBets().then((games) => {
            setGamesWithOpenBets(games);
        });
    }

    useEffect(() => {
        getAndSetGamesWithOpenBets();
    }, []);

    return (
        <div className="open-bet-container">
            <div className="open-bets-list">
                {
                    gamesWithOpenBets?.map((game) => {
                        return (
                            <ProcessBetsCard 
                                key={game.id}
                                game={game}
                                getAndSetGamesWithOpenBets={getAndSetGamesWithOpenBets}
                            />
                        )
                    })
                }    
            </div>
        </div>
    )
}