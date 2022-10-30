import { Button } from "reactstrap";

export default function ProcessBetsCard({game}) {
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
                    
                }}
            >
                Settle Open Bets
            </Button>
        </div>
    )
}