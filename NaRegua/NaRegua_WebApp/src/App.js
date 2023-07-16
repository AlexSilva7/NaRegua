import React from 'react';
import RoutesApp from "./routes/Routes";
import { AuthProvider } from './common/authentication/AuthProvider';
import Footer from './components/Footer/Footer'

export default function App() {
    return (
        <AuthProvider>
            <RoutesApp />
            <Footer />
        </AuthProvider>
    );
}