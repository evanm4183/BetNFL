import { Routes, Route, Navigate } from "react-router-dom";
import Login from "./auth/Login";
import Register from "./auth/Register";
import SetTimeForm from "./setTime/SetTimeForm";

export default function ApplicationViews({isLoggedIn, isAdmin}) {
    return (
        <Routes>
            <Route path="/">
                <Route
                    index
                    element={isLoggedIn ? <div>Games</div> : <Navigate to="/login" />}
                />
                <Route path="login" element={<Login />} />
                <Route path="register" element={<Register />} />

                {/* Admin Routes */}
                {
                    isAdmin &&
                    <>
                        <Route 
                            path="addGame" 
                            element={isLoggedIn ? <div>Add Game</div> : <Navigate to="/login" />} 
                        />
                        <Route 
                            path="setTime" 
                            element={isLoggedIn ? <SetTimeForm /> : <Navigate to="/login" />}
                        />
                        <Route 
                            path="processBets" 
                            element={isLoggedIn ? <div>Process Bets</div> : <Navigate to="/login" />}
                        /> 
                    </>
                }

                {/* Non-Admin Routes */}
                <Route 
                    path="openBets" 
                    element={isLoggedIn ? <div>Open Bets</div> : <Navigate to="/login" />} 
                />
                <Route 
                    path="profile" 
                    element={isLoggedIn ? <div>Profile</div> : <Navigate to="/login" />} 
                />

                <Route path="*" element={<div>Nothing found...</div>} />
            </Route>
        </Routes>
    )
}
