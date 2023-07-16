import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { AuthContext } from '../common/authentication/AuthProvider';

const Sobre = () => {
    const { token } = useContext(AuthContext);
    console.log(token)

    return (
        <div>
            <h1>Sobre</h1>
            <Link to="/">{token}</Link>
        </div>
    );
}

export default Sobre;