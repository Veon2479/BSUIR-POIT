import React, {useEffect} from "react";
import {isLoggedStore, isUserSignedIn} from "./utils";
import axios from "axios";
import {Navigate} from 'react-router-dom';


export function Logout() {


    useEffect( () => {
        axios.post(`http://localhost:3001/logout`)
            .then((response) => {})
            .catch(({response}) => {});
        isLoggedStore.set("false");
    }, []);

    return <Navigate to="/auth"></Navigate>;

}