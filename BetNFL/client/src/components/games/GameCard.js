import "../../styles/game-styles.css";

export default function GameCard({game}) {
    
    const getTimeDiv = () => {
        if (game.awayTeamScore && game.homeTeamScore) {
            return <div className="time-text" style={{fontWeight: "bold"}}>Final</div>;
        }

        const date = new Date(game.kickoffTime);
        return <div className="time-text">
            {date.toLocaleTimeString("en-US", {hour: "numeric", minute: "numeric"})}
        </div>
    }

    const handleClick = () => {
        console.log("clicked");
    }

    return (
        <div className="game-card" onClick={handleClick}>
            {getTimeDiv()}
            <div className="team-row">
                <img 
                    src={game.awayTeam.logoUrl}
                    alt="Away Team Logo"
                    height={50}
                />
                <div className="game-text">{game.awayTeam.teamName}</div>
                <div className="score-text">{game.awayTeamScore ? game.awayTeamScore : ""}</div>
            </div>
            <div className="team-row">
                <img 
                    src={game.homeTeam.logoUrl}
                    alt="Home Team Logo"
                    height={50}
                />
                <div className="game-text">{game.homeTeam.teamName}</div>
                <div className="score-text">{game.homeTeamScore ? game.homeTeamScore : ""}</div>
            </div>
        </div>
    );
}