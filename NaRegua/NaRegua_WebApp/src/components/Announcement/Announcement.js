/* eslint-disable jsx-a11y/anchor-is-valid */
import React from 'react';
import './Announcement.css'

const Announcement = (props) => {
    return (
        <section className={props.bgColor + ' ' + props.textColor}>
            <div className="container pt-3">
                <div className="row">
                    <div className="col-md-6 d-flex">
                        <div className="align-self-center announcement">
                            <h1 className="display-4 announcement-title">Seu atendimento, descomplicado</h1>
                            <p>
                                Usado por milhares de pessoas, o
                                NaRegua é uma ferramenta online que vai facilitar a sua experiência com barbearias.
                            </p>

                            <p>Disponivel para
                                <a href="" className="btn btn-outline-light ml-1">
                                    <i className="fab fa-android fa-lg"></i>
                                </a>
                                <a href="" className="btn btn-outline-light">
                                    <i className="fab fa-apple"></i>
                                </a>
                            </p>

                        </div>
                    </div>
                    <div className="col-md-6 d-flex">
                        <div className="align-self-center announcement">
                            <h1 className="display-4 announcement-title">Tem uma barbearia?</h1>
                            <p>
                                Esse portal é exclusivo para clientes,
                                caso queira entender como podemos ajudar à alavancar o seu negócio.
                            </p>

                            <form className="mt-4 mb-4">
                                <div className="input-group input-group-lg">
                                    <input type="text" placeholder="Seu e-mail" className="form-control"></input>
                                    <div className="input-group-append">
                                        <button type="button" className="btn btn-primary">Cadastre-se</button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}

export default Announcement;