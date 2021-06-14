import React from 'react';
import axios from 'axios';

import './App.css';

import {BrowserRouter, Route, Redirect} from 'react-router-dom';

import Login from './pages/login';
import Navbar from './components/navbar';
import Home from './pages/home';
import Register from './pages/register'
import { createBrowserHistory } from 'history';
let history = createBrowserHistory();

function App() {

  const [user, setUser] = React.useState(null);

  React.useEffect(() => {
    axios.get("https://localhost:5001/api/user", { withCredentials: true })
      .then(res => setUser(res.data))
      .catch(_ => setUser(null));
  }, []);

  const onLogoutHandler = () => {
    axios.post("https://localhost:5001/api/logout", null, { withCredentials: true })
      .then(_ => {
        setUser(null)
        history.push("/login");
      })
      .catch(err => console.error(err));
  }

  return (
    <div className="App">
        <BrowserRouter>
        <Navbar user={user} onLogoutHandler={onLogoutHandler} />
        <main className="form-signin">
            { user == null && <Route path="/login" component={Login} /> }
            { user && <Route path="/" exact  component={() => <Home user={user} />} /> }
            { user == null && <Route path="/register" component={Register} /> }
        </main>
      </BrowserRouter>
    </div>
  );
}

export default App;
