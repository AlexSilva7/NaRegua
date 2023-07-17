/* eslint-disable jsx-a11y/anchor-is-valid */
/* eslint-disable jsx-a11y/alt-text */
//import '../../styles/bootstrap.min.css';
import React from 'react';
import './Footer.css'

const Footer = () => {
    return (
        <footer className="mt-4 w-100 bg-dark footer">
            <div className="container pt-2">
                <div className="row">
                    <div className="col-md-8">
                        <p>
                            <a className="pr-3" href="">Home</a>
                            <a href="">Cadastrar</a>
                        </p>
                    </div>
                    <div className="col-md-4 d-flex justify-content-end">
                        <a href="https://github.com/AlexSilva7" className="btn btn-outline-light">
                            <i className="fab fa-facebook"></i>
                        </a>
                        <a href="" className="btn btn-outline-light ml-2">
                            <i className="fab fa-linkedin"></i>
                        </a>
                        {/*<a href="" className="btn btn-outline-light ml-2">*/}
                        {/*    <i className="fab fa-instagram"></i>*/}
                        {/*</a>*/}
                        {/*<a href="" className="btn btn-outline-light ml-2">*/}
                        {/*    <i className="fab fa-youtube"></i>*/}
                        {/*</a>*/}
                    </div>
                </div>
                <div className="row">
                    <p className="ml-2 text-white">Todos os direitos reservados à Alex Silva</p>
                </div>
            </div>
        </footer>
    );
}

export default Footer;