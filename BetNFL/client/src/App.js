import React, { useEffect, useState } from 'react';
import { BrowserRouter as Router } from "react-router-dom";
import { Spinner } from 'reactstrap';
import { onLoginStatusChange} from "./modules/authManager";
import { getUserType } from './modules/userProfileManager';
import "firebase/auth";
import ApplicationViews from './components/ApplicationViews';
import Header from './components/Header';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(null);
  const [isAdmin, setIsAdmin] = useState(false);
  const [isSportsbook, setIsSportsbook] = useState(false);

  useEffect(() => {
    onLoginStatusChange(setIsLoggedIn);
  }, []);
  
  useEffect(() => {
    if (isLoggedIn) {
      getUserType()?.then((userType) => {
        if (userType === "admin") {
          setIsAdmin(true);
          setIsSportsbook(false);
        } else if (userType === "sportsbook") {
          setIsSportsbook(true);
          setIsAdmin(false);
        } else {
          setIsAdmin(false);
          setIsSportsbook(false);
        }
      });
      
    }
  }, [isLoggedIn]);
  
  if (isLoggedIn === null) {
    return <Spinner className="app-spinner dark" />;
  }

  return (
    <Router>
      <Header isLoggedIn={isLoggedIn} isAdmin={isAdmin} />
      <div className="main-container">
        <ApplicationViews isLoggedIn={isLoggedIn} isAdmin={isAdmin} isSportsbook={isSportsbook} />
      </div>
    </Router>
  );
}

export default App;
