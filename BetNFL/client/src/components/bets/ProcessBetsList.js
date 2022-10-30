import { useState, useEffect } from "react";
import { getGamesWithOpenBets } from "../../modules/gameManager";
import ProcessBetsCard from "./ProcessBetsCard";

export default function ProcessBetsList() {
    const [gamesWithOpenBets, setGamesWithOpenBets] = useState();

    useEffect(() => {
        getGamesWithOpenBets().then((games) => {
            setGamesWithOpenBets(games);
        });
    }, []);

    return (
        <div className="open-bets-list">
            {
                gamesWithOpenBets?.map((game) => {
                    return (
                        <ProcessBetsCard 
                            key={game.id}
                            game={game}
                        />
                    )
                })
            }    
        </div>
    )
}