import { Button } from "reactstrap";
import { settleOpenBetsByGame } from "../../modules/userProfileBetManager";

export default function ProcessBetsCard({game, getAndSetGamesWithOpenBets}) {
    return (
        <div className="open-bet-card">
            <div>
                <div>
                    <strong>Game: </strong>
                    {game.awayTeam.fullName} @ {game.homeTeam.fullName}
                </div>
            </div>
            <div>
                <div>
                    <strong>{game.awayTeam.abbreviation} Pts: </strong>
                    {game.awayTeamScore}
                </div>
            </div>
            <div>
                <div>
                    <strong>{game.homeTeam.abbreviation} Pts: </strong>
                    {game.homeTeamScore}
                </div>
            </div>
            <div>
                <div>
                    <strong>When: </strong>
                    Week {game.week} - {game.year}
                </div>
            </div>
            <div>
                <div>
                    <strong>Kickoff Time: </strong>
                    {new Date(game.kickoffTime).toLocaleTimeString()}
                </div>
            </div>
            <Button
                onClick={() => {
                    settleOpenBetsByGame(game.id).then(getAndSetGamesWithOpenBets);
                }}
            >
                Settle Open Bets
            </Button>
        </div>
    )
}