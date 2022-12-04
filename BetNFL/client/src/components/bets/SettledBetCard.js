import { useState, useEffect } from "react";

export default function SettledBetCard({settledBet}) {
    const getOddsString = (num) => {
        if (num > 0) {
            return `+${num}`;
        } else {
            return num;
        }
    }

    const getSide = (settledBet) => {
        if (settledBet.side === 1) {
            const abv = settledBet.bet.game.awayTeam.abbreviation;
            const odds = getOddsString(settledBet.bet.awayTeamOdds);

            return `${abv} ${odds}`;
        } else {
            const abv = settledBet.bet.game.homeTeam.abbreviation;
            const odds = getOddsString(settledBet.bet.homeTeamOdds);

            return `${abv} ${odds}`;
        }
    }

    const calcWinnings = (betAmount, odds) => {
        if (odds > 0) {
            return Math.round(betAmount * odds) / 100;
        } else {
            return Math.round(betAmount * (100 / Math.abs(odds)) * 100) / 100;
        }
    }

    const getNetChange = (settledBet) => {
        if (settledBet.bet.game.awayTeamScore < settledBet.bet.game.homeTeamScore) {
            if (settledBet.side === 1) {
                return `- $${settledBet.betAmount}`;
            } else {
                return `+ $${calcWinnings(settledBet.betAmount, settledBet.bet.homeTeamOdds)}`
            }
        } else if (settledBet.bet.game.awayTeamScore > settledBet.bet.game.homeTeamScore) {
            if (settledBet.side === 1) {
                return `+ $${calcWinnings(settledBet.betAmount, settledBet.bet.awayTeamOdds)}`
            } else {
                return `- $${settledBet.betAmount}`;
            }
        } else {
            return `$0`;
        }
    }

    const getWinner = (settledBet) => {
        if (settledBet.bet.game.awayTeamScore < settledBet.bet.game.homeTeamScore) {
            return settledBet.bet.game.homeTeam.abbreviation;
        } else if (settledBet.bet.game.awayTeamScore > settledBet.bet.game.homeTeamScore) {
            return settledBet.bet.game.awayTeam.abbreviation;
        } else {
            return "-";
        }
    }

    return(
        <div className="settled-bet-card">
            <div className="settled-bet-section" style={{width: "9%"}}>
                <strong>Game: </strong>
                    <div>
                        {settledBet.bet.game.awayTeam.abbreviation} @ {settledBet.bet.game.homeTeam.abbreviation}
                    </div>
            </div>
            <div className="settled-bet-section" style={{backgroundColor: "gainsboro", width: "10%"}}>
                <strong>Net Change: </strong>
                    <div>
                        {
                            getNetChange(settledBet)
                        }
                    </div>
            </div>
            <div className="settled-bet-section">
                <strong>Bet Amount: </strong>
                    <div>
                        ${settledBet.betAmount}
                    </div>
            </div>
            <div className="settled-bet-section" style={{backgroundColor: "gainsboro", width: "9%"}}>
                <strong>Side: </strong>
                    <div>
                        {
                            getSide(settledBet)
                        }
                    </div>
            </div>
            <div className="settled-bet-section" style={{width: "9%"}}>
                <strong>Winner: </strong>
                    <div>
                        {
                            getWinner(settledBet)
                        }
                    </div>
            </div>
            <div className="settled-bet-section" style={{backgroundColor: "gainsboro", width: "9%"}}>
                <strong>Score: </strong>
                    <div>
                        {settledBet.bet.game.awayTeamScore} - {settledBet.bet.game.homeTeamScore}
                    </div>
            </div>
            <div className="settled-bet-section" style={{width: "11%"}}>
                <strong>When: </strong>
                    <div>
                        Week {settledBet.bet.game.week} - {settledBet.bet.game.year}
                    </div>
            </div>
            <div className="settled-bet-section" style={{backgroundColor: "gainsboro", width: "12%"}}>
                <strong>Sportsbook: </strong>
                    <div>
                        {settledBet.bet.userProfile.username}
                    </div>
            </div>
            <div className="settled-bet-section" style={{width: "18%"}}>
                <strong>Date Placed: </strong>
                    <div>
                        {new Date(settledBet.createDateTime).toLocaleString()}
                    </div>
            </div>
            <div className="settled-bet-section" style={{backgroundColor: "gainsboro", width: "18%"}}>
                <strong>Date Processed: </strong>
                    <div>
                        {new Date(settledBet.processedDateTime).toLocaleString()}
                    </div>
            </div>
        </div>
    )
}