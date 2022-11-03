import React, { useState } from 'react';
import { NavLink as RRNavLink } from "react-router-dom";
import {
  Collapse,
  Navbar,
  NavbarToggler,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink
} from 'reactstrap';
import { logout } from '../modules/authManager';

export default function Header({ isLoggedIn, isAdmin }) {
  const [isOpen, setIsOpen] = useState(false);
  const toggle = () => setIsOpen(!isOpen);

  return (
    <div>
      <Navbar className="navbar navbar-dark" expand="md" style={{backgroundColor: "#000066"}}>
        <img 
          src="https://loodibee.com/wp-content/uploads/nfl-league-logo.png"
          alt="NFL Logo"
          height={60}
        />
        <NavbarBrand tag={RRNavLink} to="/">BetNFL</NavbarBrand>
        <NavbarToggler onClick={toggle} />
        <Collapse isOpen={isOpen} navbar>
          <Nav className="mr-auto" navbar>
            {isLoggedIn &&
            <>
              <NavItem>
                <NavLink tag={RRNavLink} to="/">Games</NavLink>
              </NavItem>

              {/* Admin navbar tabs*/}
              {
                isAdmin &&
                <>
                  <NavItem>
                    <NavLink tag={RRNavLink} to="/addGame">Add Game</NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={RRNavLink} to="/setTime">Set Site Time</NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={RRNavLink} to="/processBets">Process Bets</NavLink>
                  </NavItem>
                </>
              }

              {/* Non-admin navbar tabs */}
              {
                !isAdmin && 
                <>
                  <NavItem>
                    <NavLink tag={RRNavLink} to="/openBets">Open Bets</NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={RRNavLink} to="/profile">Profile</NavLink>
                  </NavItem>
                </>
              }
            </>
            }
          </Nav>
          <Nav navbar>
            {isLoggedIn &&
              <>
                <NavItem>
                  <a aria-current="page" className="nav-link"
                    style={{ cursor: "pointer" }} onClick={logout}>Logout</a>
                </NavItem>
              </>
            }
            {!isLoggedIn &&
              <>
                <NavItem>
                  <NavLink tag={RRNavLink} to="/login">Login</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={RRNavLink} to="/register">Register</NavLink>
                </NavItem>

              </>
            }
          </Nav>
        </Collapse>
      </Navbar>
    </div>
  );
}
