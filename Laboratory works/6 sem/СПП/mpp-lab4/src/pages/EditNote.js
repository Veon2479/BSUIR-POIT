import React, {useContext, useEffect, useState} from 'react';
import {Link, Navigate, useParams} from 'react-router-dom';
import {isUserSignedIn} from "./utils";
import UserContext from "../contexts/UserContext";
import {useCookies} from "react-cookie";

export function withRouter(Children){
    return(props)=>{
        const match  = {params: useParams()};
        return <Children {...props}  match = {match}/>
    }
}

//3 lab - useContext can be used
function EditNote(props) {

    const [title, setTitle] = useState('');
    const [body, setBody] = useState('');
    const [goBack, setGoBack] = useState(false);
    const [goAuth, setGoAuth] = useState(false);

    const {socket} = useContext(UserContext);
    const [cookies] = useCookies(['JWT']);

    useEffect(() => {

        if (!isUserSignedIn()) {
            setGoAuth(true);
            console.log("Unauthorized")
        }
        else {
            const id = props.match.params.id;
            let req = {};
            req.id = id;
            req.cookies = cookies;
            socket.emit('getNote', req);

            socket.on('getNote', (note) => {
                setTitle(note.title);
                setBody(note.body);
            });

        }

    }, []);

    const handleDelete = async (e) => {
        e.preventDefault();
        if (!isUserSignedIn()) {
            setGoAuth(true);
            console.log("Unauthorized")
        } else {
            const id = props.match.params.id;
            let req = {};
            req.id = id;
            req.cookies = cookies;
            socket.emit('deleteNote', req);
            setGoBack(true);
        }
    }

    const handleCancel = (e) => {
        e.preventDefault();
        setGoBack(true);
    }

    const handleSave = async (e) => {
        e.preventDefault();
        if (!isUserSignedIn()) {
            setGoAuth(true);
            console.log("Unauthorized")
        } else {
            const id = props.match.params.id;
            let req = {};
            req.id = id;
            req.title = title;
            req.body = body;
            req.cookies = cookies;
            socket.emit('patchNote', req);
            setGoBack(true);
        }
    }

    const handleTitleChange = (e) => {
        setTitle(e.target.value);
    }

    const handleBodyChange = (e) => {
        setBody(e.target.value);
    }

    if (goBack) {
        return <Navigate to="/"></Navigate>
    } else
    if (goAuth || !isUserSignedIn())
        return <Navigate to="/auth"></Navigate>
    else
    return (
        <>
            <header className="bg-gray-900 shadow h-12 w-full mb-4 flex justify-center items-center">
                <Link
                    to="/"
                    className="text-gray-50 font-bold text-lg uppercase tracking-wide"
                >
                    Notes
                </Link>
            </header>
            <div className="px-4 mb-4">
                <form className="px-4 py-6 rounded-lg shadow-lg">
                    <label htmlFor="title" className="text-white text-sm ml-1">
                        Note Title
                    </label>
                    <input
                        value={title}
                        onChange={handleTitleChange.bind(this)}
                        type="text"
                        className="w-full p-4 rounded-lg outline-none text-gray-300 bg-gray-800 block mb-4 "
                        id="title"
                        name="title"
                    />
                    <label htmlFor="body" className="text-white text-sm ml-1">
                        Note Body
                    </label>
                    <textarea
                        value={body}
                        onChange={handleBodyChange.bind(this)}
                        className="w-full h-36 p-4 rounded-lg text-gray-300 bg-gray-800 outline-none bg-transparent mb-4"
                        id="body"
                        name="title"
                    />
                    <div className="flex justify-between items-center">
                        <div>
                            <button onClick={handleDelete.bind(this)} className="bg-red-500 text-white px-6 py-3 inline-block mb-6 shadow-lg rounded-lg hover:shadow flex items-center">
                                <svg
                                    className="h-5 w-5 text-lg"
                                    xmlns="http://www.w3.org/2000/svg"
                                    viewBox="0 0 20 20"
                                    fill="currentColor"
                                >
                                    <path
                                        fillRule="evenodd"
                                        d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
                                        clipRule="evenodd"
                                    />
                                </svg>
                                <span className="hidden ml-2 md:inline">Delete Note</span>
                            </button>
                        </div>
                        <div className="flex">
                            <Link
                                to={"/"}
                                onClick={handleSave.bind(this)}
                                className="bg-blue-500 text-white px-6 py-3 inline-block mb-6 shadow-lg rounded-lg hover:shadow flex items-center"
                            >
                                <svg
                                    className="h-5 w-5 text-lg"
                                    xmlns="http://www.w3.org/2000/svg"
                                    viewBox="0 0 20 20"
                                    fill="currentColor"
                                >
                                    <path
                                        fillRule="evenodd"
                                        d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                                        clipRule="evenodd"
                                    />
                                </svg>
                                <span className="hidden ml-2 md:inline">Save</span>
                            </Link>
                            <Link
                                to={"/"}
                                onClick={handleCancel.bind(this)}
                                className="bg-transparent text-gray-800 px-6 py-3 inline-block mb-6 rounded-lg hover:shadow flex items-center"
                            >
                                <svg
                                    className="h-5 w-5 text-lg"
                                    xmlns="http://www.w3.org/2000/svg"
                                    viewBox="0 0 20 20"
                                    fill="currentColor"
                                >
                                    <path
                                        fillRule="evenodd"
                                        d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                                        clipRule="evenodd"
                                    />
                                </svg>
                                <span className="hidden ml-2 md:inline">Go back</span>
                            </Link>
                        </div>
                    </div>
                </form>
            </div>
        </>

    );
}

// export default EditNote;

export default withRouter(EditNote);

// function withMatch(Component) {
//     return props => <Component {...props} match={useMatch(undefined)} />;
// }
// export default withMatch(EditNote);