import { useNavigate } from "react-router-dom";
import "../../styles/game-styles.css";

export default function GameCard({game, isAdmin, isSportsbook}) {
    const navigate = useNavigate();
    
    const getTimeDiv = () => {
        if (game.awayTeamScore !== null && game.homeTeamScore !== null) {
            return <div className="time-text" style={{fontWeight: "bold"}}>Final</div>;
        }

        const date = new Date(game.kickoffTime);

        return <div className="time-text">
            {date.toLocaleTimeString("en-US", {weekday: "short", hour: "numeric", minute: "numeric"})}
        </div>
    }

    const handleClick = () => {
        if (isAdmin) {
            navigate(`editGame/${game.id}`);
        } else if (isSportsbook && (!game.homeTeamScore && !game.awayTeamScore)) {
            navigate(`createBet/${game.id}`);
        } else if (!game.homeTeamScore && !game.awayTeamScore) {
            navigate(`viewBets/${game.id}`);
        }
    }

    return (
        <div className="game-card" onClick={handleClick}>
            {getTimeDiv()}
            <div className="team-row">
                <img 
                    src={game.awayTeam.logoUrl}
                    alt="Away Team Logo"
                    height={60}
                />
                <div className="game-text">{game.awayTeam.teamName}</div>
                <div className="score-text">{game.awayTeamScore !== null ? game.awayTeamScore : ""}</div>
            </div>
            <div className="team-row">
                <img 
                    src={game.homeTeam.logoUrl}
                    alt="Home Team Logo"
                    height={60}
                />
                <div className="game-text">{game.homeTeam.teamName}</div>
                <div className="score-text">{game.homeTeamScore !== null ? game.homeTeamScore : ""}</div>
            </div>
        </div>
    );
}