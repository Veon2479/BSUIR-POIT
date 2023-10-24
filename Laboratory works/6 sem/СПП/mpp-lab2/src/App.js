import React from "react";
import {
    BrowserRouter as Router,
    Routes,
    Route,
    Link, BrowserRouter
} from "react-router-dom"
import Notes from "./pages/Notes";
import EditNote from "./pages/EditNote";
import CreateNote from "./pages/CreateNote";

export default function BasicExample() {
    return (
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
                    <Route exact path="/notes/:id/edit" element={<EditNote />} />
                    <Route exact path="/notes/create" element={<CreateNote />} />

                </Routes>
            </>
        </Router>
    );
}

function Home() {
    return (
        <div>
            <h2>Home Page</h2>
        </div>
    );
}