import React, {useEffect, useState} from "react";
import {
    BrowserRouter as Router,
    Routes,
    Route, Navigate,
} from "react-router-dom"
import Notes from "./pages/Notes";
import EditNote from "./pages/EditNote";
import CreateNote from "./pages/CreateNote";
import AuthForm from "./pages/AuthForm";
import {Logout} from "./pages/Logout";
import io from 'socket.io-client';
import {useCookies} from 'react-cookie';
import UserContext from "./contexts/UserContext";
import {isLoggedStore} from "./pages/utils";



export default function App() {
    const [socket, setSocket] = useState(io('http://localhost:3001', {
        withCredentials: true
    }));
    const [cookies, setCookie, clearCookie] = useCookies(['JWT']);

    useEffect(() => {
        socket.on('set-cookie', (cookieHeader) => {
            setCookie(cookieHeader.name,cookieHeader.value,cookieHeader.options);
        });

        socket.on('Unauthorized', () => {
            isLoggedStore.set("false");
        })

    });


    return (
        <UserContext.Provider value={{ socket}}>
            <Router>
                <>
                    <Routes>
                        <Route index element={<Notes />}/>
                            {/*    <Route path="about" element={<About />} />*/}
                            {/*    <Route path="dashboard" element={<Dashboard />} />*/}

                            {/*    /!* Using path="*"" means "match anything", so this route*/}
                            {/*acts like a catch-all for URLs that we don't have explicit*/}
                            {/*routes for. *!/*/}
                            {/*    <Route path="*" element={<NoMatch />} />*/}
                        <Route exact path="/:id" element={<EditNote />} />
                        <Route exact path="/create" element={<CreateNote />} />
                        <Route exact path="/auth" element={<AuthForm />} />
                        <Route exact path="/logout" element={<Logout />} />
                        <Route exact path="*" element={<Navigate to={"/"} />} />
                    </Routes>
                </>
            </Router>
        </UserContext.Provider>
    );
}