import React from "react";
import { Route, BrowserRouter, Routes } from "react-router-dom";

import Home from "../components/Home";
import Sobre from "../components/Sobre";

const RoutesApp = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home />}  exact />
                <Route path="/sobre" element={<Sobre />} />
            </Routes>
        </BrowserRouter>
    )
}

export default RoutesApp;