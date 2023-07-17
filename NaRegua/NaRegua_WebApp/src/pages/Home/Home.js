/* eslint-disable no-unused-vars */
/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import '../../styles/bootstrap.min.css';
import { AuthContext } from '../../common/authentication/AuthProvider'
import Header from "../../components/Header/Header";
import Footer from "../../components/Footer/Footer";

const Home = () => {
    const navigate = useNavigate();
    const { setToken } = useContext(AuthContext);
    async function handleButtonClick() {

        setToken("xxx");
        navigate('/sobre');
    }

    const links = ['Home', 'Cadastrar','Entrar'];

    return (
        <div>
            <Header links={links}/>
            <div className='container'>
                <div className="card card-container">
                    {/*<img id="profile-img" className="profile-img-card" src="//ssl.gstatic.com/accounts/ui/avatar_2x.png" />*/}
                    <p id="profile-name" className="profile-name-card"></p>
                    <form className="form-signin">
                        <span id="reauth-email" className="reauth-email"></span>
                        <input type="text" id="nome" className="form-control" placeholder="Nome" required autoFocus></input>
                        <input type="password" id="senha" className="form-control" placeholder="Senha" required></input>
                        <div id="remember" className="checkbox">
                            {/* <label>
                    <input type="checkbox" value="remember-me"> Lembre-se de mim </input>
                </label> */}
                        </div>
                        <button className="btn btn-lg btn-primary btn-block btn-signin" type="submit" onClick={handleButtonClick}>Entrar</button>
                    </form>
                    <a href="#" className="forgot-password">
                        Esqueceu a senha?
                    </a>
                </div>
            </div>
            <Footer />
        </div>
    );
}

export default Home;