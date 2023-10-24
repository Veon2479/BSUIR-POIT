import React, {Component} from 'react';
import {Navigate, useNavigation, useParams} from 'react-router-dom';
import axios from "axios";

class CreateNote extends Component {

    constructor(props) {
        super(props);
        this.state = {
            title: '',
            body: '',
        };
    }

    handleCancel(e) {
        e.preventDefault();
        this.setState({goBack: true});
    }

    handleSave(e) {
        e.preventDefault();
        axios.post(`http://localhost:8080/notes/`, {
            title: this.state.title,
            body: this.state.body
        })
            .then(response => {
                this.setState({goBack: true});
            });
    }

    handleTitleChange(e) {
        this.setState({title: e.target.value})
    }

    handleBodyChange(e) {
        this.setState({body: e.target.value})
    }
    render() {
        if (this.state.goBack) {
            return <Navigate to="/"></Navigate>
        }
        return (
            <>
                <header className="bg-gray-900 shadow h-12 w-full mb-4 flex justify-center items-center">
                    <a
                        href="/"
                        className="text-gray-50 font-bold text-lg uppercase tracking-wide"
                    >
                        Notes
                    </a>
                </header>
                <div className="px-4 mb-4">
                    <form className="px-4 py-6 rounded-lg shadow-lg">
                        <label htmlFor="title" className="text-white text-sm ml-1">
                            Note Title
                        </label>
                        <input
                            value={this.state.title}
                            onChange={this.handleTitleChange.bind(this)}
                            type="text"
                            className="w-full p-4 rounded-lg outline-none text-gray-300 bg-gray-800 block mb-4 "
                            id="title"
                            name="title"
                        />
                        <label htmlFor="body" className="text-white text-sm ml-1">
                            Note Body
                        </label>
                        <textarea
                            value={this.state.body}
                            onChange={this.handleBodyChange.bind(this)}
                            type="text"
                            className="w-full h-36 p-4 rounded-lg text-gray-300 bg-gray-800 outline-none bg-transparent mb-4"
                            id="body"
                            name="title"
                        />
                        <div className="flex justify-between items-center">
                            <div className="flex">
                                <a
                                    onClick={this.handleSave.bind(this)}
                                    href="notes.html"
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
                                </a>
                                <a
                                    onClick={this.handleCancel.bind(this)}
                                    href="notes.html"
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
                                </a>
                            </div>
                        </div>
                    </form>
                </div>
            </>

        );
    }
}

export default CreateNote;

// function withMatch(Component) {
//     return props => <Component {...props} match={useMatch(undefined)} />;
// }
// export default withMatch(EditNote);