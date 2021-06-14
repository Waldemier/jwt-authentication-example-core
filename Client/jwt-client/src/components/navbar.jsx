import React from 'react';
import {Link} from 'react-router-dom';

function Navbar({user, onLogoutHandler}) {
  
  return (
      <nav className="navbar navbar-expand-md navbar-dark bg-dark mb-4">
      <div className="container-fluid">
        <Link className="navbar-brand" to="#">Home</Link>
        <div>
          {!user ? 
            <ul className="navbar-nav me-auto mb-2 mb-md-0">
              <li className="nav-item">
                <Link className="nav-link active" to="/login">Login</Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link active" to="/register">Register</Link>
              </li>
            </ul>
            :
            <ul className="navbar-nav me-auto mb-2 mb-md-0">
              <li className="nav-item">
                <button className="w-100 btn btn-lg btn-primary" onClick={onLogoutHandler}>Logout</button>
              </li>
            </ul>
          }
        </div>
      </div>
    </nav>
  )
}

export default Navbar;
