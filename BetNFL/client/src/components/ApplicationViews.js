import { Routes, Route, Navigate } from "react-router-dom";
import Login from "./auth/Login";
import Register from "./auth/Register";
import GameForm from "./games/GameForm";
import SetTimeForm from "./siteTime/SetTimeForm";
import GameList from "./games/GameList";
import GameEditForm from "./games/GameEditForm";
import BetPropertiesForm from "./bets/BetPropertiesForm";
import BetList from "./bets/BetList";
import PlaceBetForm from "./bets/PlaceBetForm";
import OpenBetsList from "./bets/OpenBetsList";
import ProfilePage from "./userProfile/ProfilePage";
import ProcessBetsList from "./bets/ProcessBetsList";
import SettledBetsList from "./bets/SettledBetsList";
import "../App.css";

export default function ApplicationViews({isLoggedIn, isAdmin, isSportsbook}) {
    return (
        <Routes>
            <Route path="/">
                <Route
                    index
                    element={isLoggedIn 
                                ? <GameList isAdmin={isAdmin} isSportsbook={isSportsbook} /> 
                                : <Navigate to="/login" />
                            }
                />
                <Route path="login" element={<Login />} />
                <Route path="register" element={<Register />} />

                {/* Admin Routes */}
                {
                    isAdmin &&
                    <>
                        <Route 
                            path="addGame" 
                            element={isLoggedIn ? <GameForm /> : <Navigate to="/login" />} 
                        />
                        <Route 
                            path="setTime" 
                            element={isLoggedIn ? <SetTimeForm /> : <Navigate to="/login" />}
                        />
                        <Route 
                            path="processBets" 
                            element={isLoggedIn ? <ProcessBetsList /> : <Navigate to="/login" />}
                        /> 
                        <Route
                            path="editGame/:gameId"
                            element={isLoggedIn ? <GameEditForm /> : <Navigate to="/login" />}
                        />
                    </>
                }

                {
                    isSportsbook && 
                        <Route
                            path="createBet/:gameId"
                            element={isLoggedIn ? <BetPropertiesForm /> : <Navigate to="/login" />}
                        />
                }

                {/* Non-Admin Routes */}
                <Route 
                    path="openBets" 
                    element={isLoggedIn ? <OpenBetsList isSportsbook={isSportsbook}/> : <Navigate to="/login" />} 
                />
                <Route 
                    path="settledBets" 
                    element={isLoggedIn ? <SettledBetsList /> : <Navigate to="/login" />} 
                />
                <Route 
                    path="profile" 
                    element={isLoggedIn ? <ProfilePage /> : <Navigate to="/login" />} 
                />
                <Route 
                    path="viewBets/:gameId" 
                    element={isLoggedIn ? <BetList /> : <Navigate to="/login" />} 
                />
                <Route 
                    path="placeBet/:betId" 
                    element={isLoggedIn ? <PlaceBetForm /> : <Navigate to="/login" />} 
                />

                <Route path="*" element={<div>Nothing found...</div>} />
            </Route>
        </Routes>
    )
}
