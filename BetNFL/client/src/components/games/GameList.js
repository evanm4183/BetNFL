import { useState, useEffect } from "react";
import GameCard from "./GameCard";
import "../../styles/game-styles.css";
import { getSiteTime } from "../../modules/siteTimeManager";
import { getGamesByWeek } from "../../modules/gameManager";

export default function GameList() {
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
        <>
            <h4 className="game-list-header">Week {currWeek}</h4>
            <div className="game-list-container">
                {
                    games.map((game) => <GameCard key={game.id} game={game} />)
                }
            </div>
        </>
    );
}