import { Routes, Route, Navigate } from "react-router-dom";
import Login from "./Login";
import Register from "./Register";

export default function ApplicationViews({isLoggedIn}) {
    return (
        <Routes>
            <Route path="/">
                {console.log(isLoggedIn)}
                <Route
                    index
                    element={isLoggedIn ? <div>Games</div> : <Navigate to="/login" />}
                />
                <Route path="login" element={<Login />} />
                <Route path="register" element={<Register />} />

                {/* Admin Routes */}
                <Route 
                    path="addGame" 
                    element={isLoggedIn ? <div>Add Game</div> : <Navigate to="/login" />} 
                />
                <Route 
                    path="setWeek" 
                    element={isLoggedIn ? <div>Set Current Week</div> : <Navigate to="/login" />}
                />
                <Route 
                    path="processBets" 
                    element={isLoggedIn ? <div>Process Bets</div> : <Navigate to="/login" />}
                />

                {/* Non-Admin Routes */}
                <Route 
                    path="openBets" 
                    element={isLoggedIn ? <div>Open Bets</div> : <Navigate to="/login" />} 
                />
                <Route 
                    path="profile" 
                    element={isLoggedIn ? <div>Profile</div> : <Navigate to="/login" />} 
                />
            </Route>
        </Routes>
    )
}
