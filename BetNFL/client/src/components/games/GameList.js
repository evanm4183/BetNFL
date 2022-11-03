import { useState, useEffect } from "react";
import GameCard from "./GameCard";
import "../../styles/game-styles.css";
import { getSiteTime } from "../../modules/siteTimeManager";
import { getGamesByWeek } from "../../modules/gameManager";

export default function GameList({isAdmin, isSportsbook}) {
    const [games, setGames] = useState([]);
    const [currWeek, setCurrWeek] = useState();

    useEffect(() => {
        getSiteTime().then((siteTime) => {
            setCurrWeek(siteTime.currentWeek);

            getGamesByWeek(siteTime.currentWeek).then((games) => {
                setGames(games);
            });
        })
    }, []);
    
    return (
        <div className="main-game-container">
            <div className="center-container">
                <h2 className="game-list-header">Week {currWeek}</h2>
                <div className="game-list-container">
                    {
                        games.map((game) => 
                            <GameCard 
                                key={game.id} 
                                game={game} 
                                isAdmin={isAdmin}
                                isSportsbook={isSportsbook}
                            />
                        )
                    }
                </div>
            </div>
        </div>
    );
}