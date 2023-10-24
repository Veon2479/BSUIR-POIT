import React, { useState } from "react";
import axios from "axios";
import {Link, Navigate} from "react-router-dom";
import {isLoggedStore, loginStore, Server} from "./utils";

const AuthForm =  () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isLogged, setIsLogged] = useState(false);

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
        try {
            await axios.post(`http://localhost:3001/registration`,
                {username: username, hash: hash},
                {
                    headers: {
                        'content-type': 'multipart/form-data'
                    },
                    withCredentials: true
                }
            ).catch(() => {
            });
        } catch (error) {
        }
    };
    const handleSignIn = async (event) => {
        event.preventDefault();
        const hash = getHash(password);
        const config = {
            withCredentials: true,
        }
        await axios.post(`http://localhost:3001/login`, {username: username, hash: hash}, config)
            .then((response) => {
                isLoggedStore.set("true");
                loginStore.set(username);
                setIsLogged(true);
            })
            .catch((err) => {alert(err)});

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
