import { BrowserRouter, Routes, Route } from 'react-router-dom';
import App from '../pages/App';
import Tela2 from '../pages/teste';

function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<App />} />
        <Route path="/tela2" element={<Tela2 />} />
      </Routes>
    </BrowserRouter>
  );
}

export default AppRoutes;