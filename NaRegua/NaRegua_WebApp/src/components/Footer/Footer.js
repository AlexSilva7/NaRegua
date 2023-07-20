/* eslint-disable jsx-a11y/anchor-is-valid */
/* eslint-disable jsx-a11y/alt-text */
//import '../../styles/bootstrap.min.css';
import React from 'react';
import './Footer.css'

const Footer = () => {
    return (
        <footer className="footer">
            <div className="container">
                <div className="row">
                    <div className="col-sm-10">
                        <p className="text-dark display-4 footer-paragraph">Todos os direitos reservados Alex Silva</p>
                    </div>
                    <div className="col-sm-2 mb-2">
                        <a href="https://github.com/AlexSilva7" className="btn btn-outline-dark">
                            <i className="fab fa-github"></i>
                        </a>
                        <a href="https://www.linkedin.com/in/alexs-araujo/" className="btn btn-outline-dark ml-2">
                            <i className="fab fa-linkedin"></i>
                        </a>
                    </div>
                </div>
            </div>
        </footer>
    );
}

export default Footer;