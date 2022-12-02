import { useState, useEffect } from "react";

export default function SettledBetCard() {
    return(
        <div className="settled-bet-card">
            <div className="settled-bet-section" style={{width: "9%"}}>
                <strong>Game: </strong>
                    <div>
                        ARI @ SEA
                    </div>
            </div>
            <div className="settled-bet-section" style={{backgroundColor: "gainsboro", width: "8%"}}>
                <strong>Yield: </strong>
                    <div>
                        +$250
                    </div>
            </div>
            <div className="settled-bet-section">
                <strong>Bet Amount: </strong>
                    <div>
                        $100
                    </div>
            </div>
            <div className="settled-bet-section" style={{backgroundColor: "gainsboro", width: "9%"}}>
                <strong>Side: </strong>
                    <div>
                        SEA +250
                    </div>
            </div>
            <div className="settled-bet-section" style={{width: "9%"}}>
                <strong>Score: </strong>
                    <div>
                        17 - 41
                    </div>
            </div>
            <div className="settled-bet-section" style={{backgroundColor: "gainsboro", width: "11%"}}>
                <strong>When: </strong>
                    <div>
                        Week 10 - 2022
                    </div>
            </div>
            <div className="settled-bet-section" style={{width: "12%"}}>
                <strong>Sportsbook: </strong>
                    <div>
                        DraftKings
                    </div>
            </div>
            <div className="settled-bet-section" style={{backgroundColor: "gainsboro", width: "18%"}}>
                <strong>Date Placed: </strong>
                    <div>
                        11/12/2022, 8:05:55 PM
                    </div>
            </div>
            <div className="settled-bet-section" style={{width: "18%"}}>
                <strong>Date Processed: </strong>
                    <div>
                        11/21/2022, 10:05:55 PM
                    </div>
            </div>
        </div>
    )
}