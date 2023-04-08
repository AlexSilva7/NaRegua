import React, { useState } from 'react';
import { useLocation } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import '../styles/App.css';
import '../styles/bootstrap.min.css';

type TelaProps = {
  token: string;
};

function Tela2() {
  const location = useLocation();
  const params = new URLSearchParams(location.search);
  const token = params.get('token');
  // const { id } = useParams<{ id: string }>();
  return (
    <div>
      <h1>{token}</h1>
    </div>
  );
}

export default Tela2;
