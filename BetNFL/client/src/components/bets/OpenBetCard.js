export default function OpenBetCard({openBet, isSportsbook}) {
    const formatOdds = (odds) => {
        if (odds > 0) {
            return "+" + odds;
        }
        return odds;
    }

    return (
        <div className="open-bet-card">
            <div>
                <div>
                    {
                        isSportsbook 
                        ? 
                            <>
                                <strong>Username: </strong>
                                {openBet.userProfile.username}
                            </>
                        :
                            <>
                                <strong>Sportsbook: </strong>
                                {openBet.bet.userProfile.username}
                            </>
                    }
                </div>
            </div>
            <div>
                <div>
                    <strong>Game: </strong>
                    {openBet.bet.game.awayTeam.abbreviation} @ {openBet.bet.game.homeTeam.abbreviation}
                </div>
            </div>
            <div>
                <div>
                    <strong>When: </strong>
                    Week {openBet.bet.game.week} - {openBet.bet.game.year}
                </div>
            </div>
            <div>
                <div>
                    <strong>Side: </strong>
                    {
                        openBet.side === 1 
                        ? `${openBet.bet.game.awayTeam.abbreviation} ${formatOdds(openBet.bet.awayTeamOdds)}`
                        : `${openBet.bet.game.homeTeam.abbreviation} ${formatOdds(openBet.bet.homeTeamOdds)}`
                    }
                </div>
            </div>
            <div>
                <div>
                    <strong>Bet Amount: </strong>
                    ${openBet.betAmount}
                </div>
            </div>
            <div>
                <div>
                    <strong>Date Placed: </strong>
                    {new Date(openBet.createDateTime).toLocaleString()}
                </div>
            </div>
        </div>
    )
}