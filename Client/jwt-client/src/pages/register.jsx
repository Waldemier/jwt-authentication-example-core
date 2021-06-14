import React from 'react';
import axios from 'axios';
import {Redirect} from 'react-router-dom';

function Register() {
    const [name, setName] = React.useState('');
    const [email, setEmail] = React.useState('');
    const [password, setPassword] = React.useState('');
    const [redirect, setRedirect] = React.useState(false);

    const onSubmitHandler = async e => {
        e.preventDefault();
        const body = {
            name, email, password
        }
        await axios.post("https://localhost:5001/api/register", body);
        setRedirect(true);
    }

    if(redirect) {
        return <Redirect to="/login"/>
    }

    return (
        <form onSubmit={onSubmitHandler}>
          <h1 className="h3 mb-3 fw-normal">Register</h1>
          <div className="form-floating" style={{marginBottom: "10px"}}>
            <input type="name" className="form-control" placeholder="Name" value={name} onChange={e => setName(e.target.value)} />
          </div>
          <div className="form-floating" style={{marginBottom: "10px"}}>
            <input type="email" className="form-control" placeholder="name@example.com" value={email} onChange={e => setEmail(e.target.value)} />
          </div>
          <div className="form-floating">
            <input type="password" className="form-control" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} />
          </div>
          <button className="w-100 btn btn-lg btn-primary" type="submit">Submit</button>
        </form>
    )
}

export default Register
