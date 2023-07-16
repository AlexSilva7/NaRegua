/* eslint-disable jsx-a11y/anchor-is-valid */
/* eslint-disable jsx-a11y/alt-text */
import '../styles/bootstrap.min.css';
import React from 'react';

const Footer = () => {
    return (
        <footer className="mt-4 mb-4">
            <div className="container">
                <div className="row">
                    <div className="col-md-8">
                        <p>
                            <a href="">Home</a>
                            <a href="">Recursos</a>
                            <a href="">Benefícios</a>
                        </p>
                    </div>
                    <div className="col-md-4 d-flex justify-content-end">
                        <a href="" className="btn btn-outline-dark">
                            <i className="fab fa-facebook"></i>
                        </a>
                        <a href="" className="btn btn-outline-dark ml-2">
                            <i className="fab fa-linkedin"></i>
                        </a>
                        <a href="" className="btn btn-outline-dark ml-2">
                            <i className="fab fa-instagram"></i>
                        </a>
                        <a href="" className="btn btn-outline-dark ml-2">
                            <i className="fab fa-youtube"></i>
                        </a>
                    </div>
                </div>
            </div>
        </footer>
    );
}

export default Footer;