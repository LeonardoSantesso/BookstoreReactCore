import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import Login from "./pages/Login";
import Books from "./pages/Books";
import NewBook from "./pages/NewBook";

export default function AllRoutes() {
    return (
        <Router>            
            <Routes>
                <Route exact path="/" element={<Login/>}/>
                <Route exact path="/books" element={<Books/>}/>
                <Route exact path="/book/new/:bookId" element={<NewBook/>}/>
            </Routes>           
        </Router>
    );
}