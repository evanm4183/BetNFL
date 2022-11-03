import "../../styles/bet-styles.css";

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
                <div className="open-bet-section">
                    {
                        isSportsbook 
                        ? 
                            <>
                                <strong>Username: </strong>
                                <div>
                                    {openBet.userProfile.username}
                                </div>
                            </>
                        :
                            <>
                                <strong>Sportsbook: </strong>
                                <div>
                                    {openBet.bet.userProfile.username}
                                </div>
                            </>
                    }
                </div>
            </div>
            <div className="open-bet-section" style={{width: "9rem", backgroundColor: "gainsboro"}}>
                <div>
                    <strong>Game: </strong>
                    <div>
                        {openBet.bet.game.awayTeam.abbreviation} @ {openBet.bet.game.homeTeam.abbreviation}
                    </div>
                </div>
            </div>
            <div>
                <div className="open-bet-section" style={{width: "9rem"}}>
                    <strong>When: </strong>
                    <div>
                        Week {openBet.bet.game.week} - {openBet.bet.game.year}
                    </div>
                </div>
            </div>
            <div>
                <div className="open-bet-section" style={{width: "9rem", backgroundColor: "gainsboro"}}>
                    <strong>Side: </strong>
                    <div>
                        {
                            openBet.side === 1 
                            ? `${openBet.bet.game.awayTeam.abbreviation} ${formatOdds(openBet.bet.awayTeamOdds)}`
                            : `${openBet.bet.game.homeTeam.abbreviation} ${formatOdds(openBet.bet.homeTeamOdds)}`
                        }
                    </div>
                </div>
            </div>
            <div>
                <div className="open-bet-section" style={{width: "9rem"}}>
                    <strong>Bet Amount: </strong>
                    <div>
                        ${openBet.betAmount}
                    </div>
                </div>
            </div>
            <div>
                <div className="open-bet-section" style={{backgroundColor: "gainsboro"}}>
                    <strong>Date Placed: </strong>
                    <div>
                        {new Date(openBet.createDateTime).toLocaleString()}
                    </div>
                </div>
            </div>
        </div>
    )
}