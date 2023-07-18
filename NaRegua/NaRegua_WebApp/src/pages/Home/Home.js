/* eslint-disable no-unused-vars */
/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import '../../styles/bootstrap.min.css';
import './Home.css';
import { AuthContext } from '../../common/authentication/AuthProvider'
import Header from "../../components/Header/Header";
import Footer from "../../components/Footer/Footer";
import logoBarbearia from '../../assets/Logobarbearia.jpg'
import logoFacil from '../../assets/facil.png'
import logoEconomize from '../../assets/economize.png'
import logoSuport from '../../assets/suporte.png'

const Home = () => {
    const links = ['Home', 'Cadastrar','Entrar'];
    return (
        <div>
            <Header links={links}/>
            <section className="bg-warning text-white">
                <div className="container">
                    <div className="row">
                        <div className="col-md-6 d-flex">
                            <div className="align-self-center">
                                <h1 className="display-4">Seu atendimento, descomplicado</h1>
                                <p>
                                    Usado por mais de 1 milh�o de pessoas, o
                                    NaRegua � uma ferramenta online que vai facilitar a sua experi�ncia com barbearias.
                                </p>

                                <form className="mt-4 mb-4">
                                    <div className="input-group input-group-lg">
                                        <input type="text" placeholder="Seu e-mail" className="form-control"></input>
                                            <div className="input-group-append">
                                                <button type="button" className="btn btn-primary">Cadastre-se</button>
                                            </div>
                                    </div>
                                </form>

                                <p>Disponivel para
                                    <a href="" className="btn btn-outline-light">
                                        <i className="fab fa-android fa-lg"></i>
                                    </a>
                                    <a href="" className="btn btn-outline-light">
                                        <i className="fab fa-apple"></i>
                                    </a>
                                </p>

                            </div>
                        </div>
                        {/*<div className="col-md-6 d-none d-md-block">*/}
                        {/*    <img src={ logoBarbearia } width="120%"/>*/}
                        {/*</div>*/}
                    </div>
                </div>
            </section>

            <section className="caixa mb-5">
                <div className="container">
                    <div className="row">
                        <div className="col-md-4">
                            <img src={logoFacil} className="img-fluid" />
                                <h4>F�cil de usar</h4>
                                <p>
                                    O Finans vai al�m do b�sico e permite que voc� fa�a controles incr�veis, essenciais para suas finan�as. Simples como tem que ser!
                                </p>
                        </div>
                        <div className="col-md-4">
                            <img src={logoEconomize} className="img-fluid" />
                                <h4>Economize seu tempo</h4>
                                <p>
                                    Tempo � dinheiro! Em segundos, voc� tem tudo sob controle e aproveite seu tempo com o que realmente importa pra voc�!
                                </p>
                        </div>
                        <div className="col-md-4">
                            <img src={logoSuport} className="img-fluid" />
                                <h4>Suporte amigo</h4>
                                <p>
                                    D�vidas? Perguntas? Nosso suporte super legal ajuda voc�! A gente t� aqui pra resolver seus problemas e deixar sua vida bem mais f�cil!
                                </p>
                        </div>
                    </div>
                </div>
            </section>
            <Footer />
        </div>
    );
}

export default Home;