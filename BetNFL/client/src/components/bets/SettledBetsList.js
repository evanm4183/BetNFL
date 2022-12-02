import { useState, useEffect } from "react"
import SettledBetCard from "./SettledBetCard"

export default function SettledBetsList() {
    return (
        <div className="settled-bet-container">
            <div className="settled-bets-list">
                <SettledBetCard />
            </div>
        </div>
    )
}