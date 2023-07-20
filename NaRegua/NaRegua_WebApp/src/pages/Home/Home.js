/* eslint-disable jsx-a11y/alt-text */
/* eslint-disable no-unused-vars */
/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useContext } from 'react';
import '../../styles/bootstrap.min.css';
import Header from "../../components/Header/Header";
import Footer from "../../components/Footer/Footer";
import Announcement from '../../components/Announcement/Announcement';
import Benefits from '../../components/Benefits/Benefits';
import logoFacil from '../../assets/facil.png'
import logoEconomize from '../../assets/economize.png'
import logoSuport from '../../assets/suporte.png'

const Home = () => {
    return (
        <div>
            <Header links = {['Home', 'Cadastrar', 'Entrar']}/>
            <Announcement bgColor = {'bg-warning'} textColor={'text-white'} />
            <Benefits propertiesBenefits = {[
                {
                    image: logoFacil,
                    title: 'F�cil de usar',
                    paragraph: `O NaRegua vai al�m do b�sico e permite que voc� encontre 
                        os profissionais mais bem avaliados direto do seu app. 
                        Simples como tem que ser!`
                },
                {
                    image: logoEconomize,
                    title: 'Economize seu tempo',
                    paragraph: `Tempo � dinheiro! Em segundos, voc� tem tudo sob controle, 
                        realiza seu agendamento e aproveite seu 
                        tempo com o que realmente importa pra voc�!`
                },
                {
                    image: logoSuport,
                    title: 'Suporte amigo',
                    paragraph: `D�vidas? Perguntas? Nosso suporte super 
                        legal ajuda voc�! A gente t� aqui pra resolver 
                        seus problemas e deixar sua vida bem mais f�cil!`
                }
                ]} />
            <Footer />
        </div>
    );
}

export default Home;