import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import BetCard from "./BetCard";
import "../../styles/game-styles.css";
import { getGameWithLiveBets } from "../../modules/gameManager";

export default function BetList() {
    const [game, setGame] = useState([]);
    const { gameId } = useParams();

    useEffect(() => {
        getGameWithLiveBets(gameId).then((game) => {
            setGame(game);
        })
    }, []);
    
    return (
        <>
            <h4 className="game-list-header">{game.awayTeam?.fullName} @ {game.homeTeam?.fullName}, Week {game.week}</h4>
            <div className="game-list-container">
                {
                    game?.liveBets?.map((bet) => 
                        <BetCard 
                            key={bet.id} 
                            game={game} 
                            bet={bet}
                        />
                    )
                }
            </div>
        </>
    );
}