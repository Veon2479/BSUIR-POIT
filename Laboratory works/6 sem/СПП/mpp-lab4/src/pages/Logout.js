import React, {useContext, useEffect} from "react";
import {isLoggedStore, isUserSignedIn} from "./utils";
import axios from "axios";
import {Navigate} from 'react-router-dom';
import UserContext from "../contexts/UserContext";


export function Logout() {

    const {socket} = useContext(UserContext);

    useEffect( () => {
        let req = {};
        req.cookies = undefined;
        socket.emit('logout', req);
    }, []);

    return <Navigate to="/auth"></Navigate>;

}