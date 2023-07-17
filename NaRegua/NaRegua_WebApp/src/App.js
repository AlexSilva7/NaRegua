import React from 'react';
import RoutesApp from "./routes/Routes";
import { AuthProvider } from './common/authentication/AuthProvider';

export default function App() {
    return (
        <AuthProvider>
            <RoutesApp />
        </AuthProvider>
    );
}