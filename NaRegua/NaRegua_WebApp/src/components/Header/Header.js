/* eslint-disable jsx-a11y/anchor-is-valid */
/* eslint-disable jsx-a11y/alt-text */
import React from 'react';
import logo from '../../assets/maquina-de-cortar-cabelo.png'

const Header = (props) => {
    return (
        <header>
            <nav className="navbar navbar-expand navbar-light bg-warning">
                <a href="#" className="navbar-brand">
                    <img src={logo} width="45" />
                </a>
                <p className="pt-3">NaRegua</p>
                <div className="container">
                    <button className="navbar-toggler" data-toggle="collapse" data-target="#nav-principal">
                        <span className="navbar-toggler-icon"></span>
                    </button>

                    <div className="collapse navbar-collapse" id="nav-principal">
                        <ul className="navbar-nav ml-auto">
                            <li className="nav-item">
                                <a href="" className="nav-link">{props.links[0]}</a>
                            </li>
                            <li className="nav-item">
                                <a href="" className="nav-link">{props.links[1]}</a>
                            </li>
                            { props.links[2] != null ?
                                <li className="nav-item">
                                    <a href="" className="btn btn-outline-light ml-4">{props.links[2]}</a>
                                </li>
                            : null} 
                        </ul>
                    </div>

                </div>
            </nav>
        </header>
    );
}

export default Header;