import React from "react";
import {
    BrowserRouter as Router,
    Routes,
    Route,
} from "react-router-dom"
import Notes from "./pages/Notes";
import EditNote from "./pages/EditNote";
import CreateNote from "./pages/CreateNote";
import AuthForm from "./pages/AuthForm";
import {Logout} from "./pages/Logout";

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
                    <Route exact path="/:id" element={<EditNote />} />
                    <Route exact path="/create" element={<CreateNote />} />
                    <Route exact path="/auth" element={<AuthForm />} />
                    <Route exact path="/logout" element={<Logout />} />

                </Routes>
            </>
        </Router>
    );
}