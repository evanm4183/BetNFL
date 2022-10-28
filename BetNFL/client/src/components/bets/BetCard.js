import { useNavigate } from "react-router-dom";
import "../../styles/game-styles.css";
import "../../styles/bet-styles.css";

export default function BetCard({game, bet}) {
    const navigate = useNavigate();
    
    const formatOdds = (odds) => {
        if (odds > 0) {
            return "+" + odds;
        }
        return odds;
    }

    const handleClick = () => {
        navigate(`/placeBet/${bet.id}`);
    }

    return (
        <div className="game-card" onClick={handleClick}>
            <div className="bet-header">{bet.userProfile.username} - {bet.betType.name.toUpperCase()}</div>
            <div className="bet-header"></div>
            <div className="team-row">
                <img 
                    src={game.awayTeam.logoUrl}
                    alt="Away Team Logo"
                    height={50}
                />
                <div className="game-text">{game.awayTeam.teamName}</div>
                <div className="odds-text">{formatOdds(bet.awayTeamOdds)}</div>
            </div>
            <div className="team-row">
                <img 
                    src={game.homeTeam.logoUrl}
                    alt="Home Team Logo"
                    height={50}
                />
                <div className="game-text">{game.homeTeam.teamName}</div>
                <div className="odds-text">{formatOdds(bet.homeTeamOdds)}</div>
            </div>
        </div>
    );
}