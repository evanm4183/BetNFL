import React, { useEffect, useState } from 'react';
import { BrowserRouter as Router } from "react-router-dom";
import { Spinner } from 'reactstrap';
import { onLoginStatusChange} from "./modules/authManager";
import { getAdminStatus } from './modules/userProfileManager';
import "firebase/auth";
import ApplicationViews from './components/ApplicationViews';
import Header from './components/Header';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(null);
  const [isAdmin, setIsAdmin] = useState(null);

  useEffect(() => {
    onLoginStatusChange(setIsLoggedIn);
  }, []);

  useEffect(() => {
    getAdminStatus()?.then((adminStatus) => {
      setIsAdmin(adminStatus);
    });
  }, [isLoggedIn]);

  if (isLoggedIn === null) {
    return <Spinner className="app-spinner dark" />;
  }

  return (
    <Router>
      <Header isLoggedIn={isLoggedIn} isAdmin={isAdmin} />
      <ApplicationViews isLoggedIn={isLoggedIn} />
    </Router>
  );
}

export default App;
