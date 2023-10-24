import React, {useContext, useEffect, useState} from "react";
import axios from "axios";
import {Link, Navigate} from "react-router-dom";
import {isLoggedStore, loginStore} from "./utils";
import UserContext from "../contexts/UserContext";

const AuthForm =  () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isLogged, setIsLogged] = useState(false);

    const {socket} = useContext(UserContext);

    useEffect(()=>{
        socket.on("set-cookie", (cookie) => {
            isLoggedStore.set("true");
            setIsLogged(true);
        });
    },[]);

    const handleUsernameChange = (event) => {
        setUsername(event.target.value);
    };

    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
    };

    const getHash = (password) => {
        return password;
    }
    const handleSignUp = async (event) => {
        const hash = getHash(password);
        event.preventDefault();
        let req = {};
        req.username = username;
        req.hash = hash;
        socket.emit('registration', req);
    };
    const handleSignIn = async (event) => {
        event.preventDefault();
        const hash = getHash(password);
        let req = {};
        req.username = username;
        req.hash = hash;
        socket.emit('login', req);
    }

    if (isLogged) {
        return <Navigate to="/"></Navigate>
    }
    return (
        <div>
            <div className="text-gray-400">
                <h2>Sign Up</h2>
                <label>
                    Username:
                    <input type="text" value={username} onChange={handleUsernameChange}/>
                </label>
                <br/>
                <label>
                    Password:
                    <input type="password" value={password} onChange={handlePasswordChange}/>
                </label>
                <br/>
                <div>
                    <Link to={"/"}
                          onClick={handleSignUp}>
                        <span>
                            Sign Up
                        </span>
                    </Link>
                </div>
            </div>

            <div  className="text-gray-400">
                <h2>Sign In</h2>
                <label>
                    Username:
                    <input type="text" value={username} onChange={handleUsernameChange}/>
                </label>
                <br/>
                <label>
                    Password:
                    <input type="password" value={password} onChange={handlePasswordChange}/>
                </label>
                <br/>
                <div>
                    <Link to={"/"}
                          onClick={handleSignIn}>
                        <span>
                            Sign In
                        </span>
                    </Link>
                </div>
            </div>
        </div>
    );
};

export default AuthForm;
