import React from 'react';
import axios from 'axios';
import {Redirect} from 'react-router-dom';

function Login({history}) {
  const [email, setEmail] = React.useState('');
  const [password, setPassword] = React.useState('');
  const [succesfull, setSuccesfull] = React.useState(false);
 
  const onSubmitHandler = async e => {
      e.preventDefault();
      const body = {
        email, password
      };
      const response = await axios.post("https://localhost:5001/api/login", body, { withCredentials: true }); // withCredentials: true - gets a cookie from the server
      console.log(response);
      setSuccesfull(true);
  }

  if(succesfull) {
    history.push("/");
    document.location.reload();
  }

  return (
      <form onSubmit={onSubmitHandler}>
        <h1 className="h3 mb-3 fw-normal">Sign in</h1>
        <div className="form-floating" style={{marginBottom: "10px"}}>
          <input type="email" className="form-control" placeholder="name@example.com" value={email} onChange={e => setEmail(e.target.value)}/>
        </div>
        <div className="form-floating">
          <input type="password" className="form-control" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)}/>
        </div>
        <button className="w-100 btn btn-lg btn-primary" type="submit">Sign in</button>
      </form>
  )
}

export default Login
