/* eslint-disable jsx-a11y/anchor-is-valid */
/* eslint-disable jsx-a11y/alt-text */
import React from 'react';

const Header = () => {
    return (
        <header>
            <nav className="navbar navbar-expand-sm navbar-light bg-warning">
                <div className="container">

                    <a href="#" className="navbar-brand">
                        <img src="img/logo.png" width="142" />
                    </a>

                    <button className="navbar-toggler" data-toggle="collapse" data-target="#nav-principal">
                        <span className="navbar-toggler-icon"></span>
                    </button>

                    <div className="collapse navbar-collapse" id="nav-principal">
                        <ul className="navbar-nav ml-auto">
                            <li className="nav-item">
                                <a href="" className="nav-link">Home</a>
                            </li>
                            <li className="nav-item">
                                <a href="" className="nav-link">Recursos</a>
                            </li>
                            <li className="nav-item">
                                <a href="" className="nav-link">Benefícios</a>
                            </li>
                            <li className="nav-item">
                                <a href="" className="nav-link">Preços</a>
                            </li>
                            <li className="nav-item">
                                <a href="" className="btn btn-outline-light ml-4">Entrar</a>
                            </li>
                        </ul>
                    </div>

                </div>
            </nav>
        </header>
    );
}

export default Header;