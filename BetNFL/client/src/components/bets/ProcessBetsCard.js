import { Button } from "reactstrap";
import { settleOpenBetsByGame } from "../../modules/userProfileBetManager";
import "../../styles/bet-styles.css";

export default function ProcessBetsCard({game, getAndSetGamesWithOpenBets}) {
    return (
        <div className="open-bet-card">
            <div className="process-bet-section" style={{width: "27rem"}}>
                <div>
                    <strong>Game: </strong>
                    <div>
                        {game.awayTeam.fullName} @ {game.homeTeam.fullName}
                    </div>
                </div>
            </div>
            <div className="process-bet-section" style={{width: "7rem", backgroundColor: "gainsboro"}}>
                <div>
                    <strong>{game.awayTeam.abbreviation} Score: </strong>
                    <div>
                        {game.awayTeamScore}
                    </div>
                </div>
            </div>
            <div className="process-bet-section" style={{width: "7rem"}}>
                <div>
                    <strong>{game.homeTeam.abbreviation} Score: </strong>
                    <div>
                        {game.homeTeamScore}
                    </div>
                </div>
            </div>
            <div className="process-bet-section" style={{backgroundColor: "gainsboro"}}>
                <div>
                    <strong>When: </strong>
                    <div>
                        Week {game.week} - {game.year}
                    </div>
                </div>
            </div>
            <div className="process-bet-section">
                <div>
                    <strong>Kickoff Time: </strong>
                    <div>
                        {new Date(game.kickoffTime).toLocaleTimeString()}
                    </div>
                </div>
            </div>
            <Button
                onClick={() => {
                    if (game.awayTeamScore === null || game.homeTeamScore === null) {
                        window.alert("Error: cannot process bets for a game without a final score");
                        return
                    }
                    settleOpenBetsByGame(game.id).then(getAndSetGamesWithOpenBets);
                }}
            >
                Settle Open Bets
            </Button>
        </div>
    )
}