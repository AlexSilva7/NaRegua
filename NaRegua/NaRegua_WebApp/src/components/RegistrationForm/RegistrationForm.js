/* eslint-disable no-unused-vars */
/* eslint-disable jsx-a11y/anchor-is-valid */
/* eslint-disable jsx-a11y/alt-text */
//import '../../styles/bootstrap.min.css';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const RegistrationForm = () => {
    const [dados, setDados] = useState({});
    const navigate = useNavigate();

    const handleChange = (event) => {
        const { name, value } = event.target;
        setDados({ ...dados, [name]: value });
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        PostApiData(dados);
        // Chame a função de chamada à API aqui
        // Por exemplo: chamarApi(dados);
    };

    const PostApiData = (dados) => {
        fetch('https://localhost:5000/v1/user', {
            method: 'POST', // ou 'GET', 'PUT', 'DELETE', etc., dependendo da sua API
            headers: {
                'Content-Type': 'application/json', // Certifique-se de definir o tipo de conteúdo correto
            },
            body: JSON.stringify(dados),
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Erro ao fazer a chamada à API');
                    //throw new Error(response.body);
                }
                return response.json();
            })
            .then((data) => {
                console.log('Resposta da API:', data);
                // Faça alguma ação com a resposta da API
                alert('Cadastrado com Sucesso!')
                navigate('/');
                
            })
            .catch((error) => {
                alert(error)
                console.error('Erro na chamada da API:', error);
                // Lide com o erro da chamada à API
            });
    };

    return (
        <div className="bg-light p-2">
            <div className="container w-75 mt-2 mb-2">
                <div className="card p-5">
                    <form onSubmit={handleSubmit}>
                        <div className="form-group row">
                            <label for="inputEmail" className="col-sm-2 col-form-label">Nome:</label>
                            <div className="col-sm-10">
                                <input name="name" type="text" className="form-control" id="inputEmail" placeholder="Nome" onChange={handleChange} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label for="inputEmail" className="col-sm-2 col-form-label">CPF:</label>
                            <div className="col-sm-10">
                                <input name="document" type="text" className="form-control" id="inputEmail" placeholder="Documento" onChange={handleChange} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label for="inputEmail" className="col-sm-2 col-form-label">Email:</label>
                            <div className="col-sm-10">
                                <input name="email" type="text" className="form-control" id="inputEmail" placeholder="email@exemplo.com" onChange={handleChange} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label for="inputEmail" className="col-sm-2 col-form-label">Telefone:</label>
                            <div className="col-sm-10">
                                <input name="phone" type="text" className="form-control" id="inputEmail" placeholder="Telefone" onChange={handleChange} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label for="inputEmail" className="col-sm-2 col-form-label">Usuário:</label>
                            <div className="col-sm-10">
                                <input name="username" type="text" className="form-control" id="inputEmail" placeholder="Usuário" onChange={handleChange} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label for="inputPassword" className="col-sm-2 col-form-label">Senha:</label>
                            <div className="col-sm-10">
                                <input name="password" type="password" className="form-control" id="inputPassword" placeholder="Senha" onChange={handleChange} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <div className="col-sm-2"></div>
                            <div className="col-sm-10">
                                <button type="submit" className="btn btn-primary mt-2">Enviar</button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default RegistrationForm;