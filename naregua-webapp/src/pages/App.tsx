import React, { useState } from 'react';
import '../styles/App.css';
import '../styles/bootstrap.min.css';
import Tela2 from './teste';
import { createBrowserHistory } from 'history';

function App() {
  const history = createBrowserHistory();
  async function handleButtonClick() {
    try{
    //   let payload = {
    //     username: "leskfp",
    //     password: "123456"
    //   }

    //   const response = await fetch("https://localhost:3000/v1/auth/sign", {
    //     method: 'POST',
    //     body: JSON.stringify(payload),
    //     headers:{
    //       'Content-Type':'application/json'
    //     }
    //   })

    //   const data = await response.json();

      // History.prototype.pushState('sa')
      // useHistory.push('/outra-tela');
      const token = 'alex';

      history.push({
        pathname: '/tela2',
        search: `?token=${token}`
      });
      // history.push(Tela2({token: '123'}));
    
      // console.log(data);

    }catch{
      alert("Não foi possível processar sua solicitação, tenta mais tarde!");
    }
  }

  return (
    <div className='container'>
      <div className="card card-container">
        <img id="profile-img" className="profile-img-card" src="//ssl.gstatic.com/accounts/ui/avatar_2x.png" />
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
  );
}

export default App;
