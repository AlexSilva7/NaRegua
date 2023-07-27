/* eslint-disable react/jsx-no-undef */
import React, { useContext } from 'react';
import '../../styles/bootstrap.min.css';
import Header from "../../components/Header/Header";
import Footer from "../../components/Footer/Footer";
import RegistrationForm from "../../components/RegistrationForm/RegistrationForm";

const Register = () => {
    return (
        <div>
            <Header links={['Home', 'Cadastrar', 'Entrar']} />
            <RegistrationForm />
            <Footer />
        </div>
    );
}

export default Register;