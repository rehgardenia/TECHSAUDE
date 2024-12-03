import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

import 'bootstrap/dist/css/bootstrap.min.css';
import '@fortawesome/fontawesome-free/css/all.min.css';
import './Login.css';

const Login = () => {
  const [usuario, setUsuario] = useState('');
  const [senha, setSenha] = useState('');
  const [erros, setErros] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const navigate = useNavigate();
  const apiUrl = import.meta.env.VITE_API_URL;

  const validarUsuario = (usuario) => {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(usuario);
  };

  const validarSenha = (senha) => {
    const regex = /^(?=.*[A-Z])(?=.*[!@#$%^&*(),.?":{}|<>])(?=.*[0-9]).{6,}$/;
    return regex.test(senha);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (isSubmitting) return;

    const errosValidacao = [];

    if (!validarUsuario(usuario)) {
      errosValidacao.push('O email fornecido é inválido.');
    }
    if (!validarSenha(senha)) {
      errosValidacao.push('A senha deve ter pelo menos 6 caracteres, uma letra maiúscula, um símbolo e um dígito.');
    }

    setErros(errosValidacao);

    if (errosValidacao.length === 0) {
      setLoading(true);
      setIsSubmitting(true);
      setErros([]); // Limpa erros ao tentar logar

      try {
        const response = await axios.post(`${apiUrl}/api/Auth/login`, { email: usuario, senha: senha }, {
          headers: {
            'Content-Type': 'application/json'
          }
        });

        console.log(response.data);
        const sucesso = response.data.sucesso;
        const message = response.data.message;
      
       
        if (sucesso) {
          const idUsuario = response.data.data.id;
          console.log("Usuario:", idUsuario);
          const token = response.data.token;
          
          var user = localStorage.setItem('idUsuario', JSON.stringify(idUsuario));
          localStorage.setItem('token', JSON.stringify(token));
          navigate("/perfil");

        } else {
          setErros([message || 'Erro desconhecido.']); // Mensagem de erro do servidor
        }
      } catch (error) {
        console.error('Erro ao fazer login:', error);
        setErros(['Erro ao fazer login. Verifique suas credenciais e tente novamente.']);
      } finally {
        setLoading(false); // Para o loading mesmo se ocorrer erro
        setIsSubmitting(false); // Permite novas tentativas
      }
    }
  };

  return (
    <main className="login-container">
      <section className="login-card">
        <div className="left-section">
          <div className="logo-container">
            <img src="/logotech.png" alt="Logo" className="logo" />
            <h2 className="site-name">TechSaúde</h2>
          </div>
        </div>
        <div className="form-container">
          <header className="text-center mb-4">
            <h1>Bem-vindo!</h1>
          </header>
          <form onSubmit={handleSubmit}>
            {erros.length > 0 && (
              <div className="alert alert-danger">
                {erros.map((erro, index) => (
                  <p key={index}>{erro}</p>
                ))}
              </div>
            )}
            <article className="mb-3">
              <label htmlFor="usuario" className="form-label">Usuário</label>
              <input
                type="text"
                id="usuario"
                placeholder="Digite seu e-mail"
                className="form-control"
                value={usuario}
                onChange={(e) => {
                  setUsuario(e.target.value);
                  setErros([]); // Limpa os erros quando o usuário começa a digitar
                }}
                required
              />
            </article>
            <article className="mb-3">
              <label htmlFor="senha" className="form-label">Senha</label>
              <input
                type="password"
                id="senha"
                placeholder="Digite sua senha"
                className="form-control"
                value={senha}
                onChange={(e) => {
                  setSenha(e.target.value);
                  setErros([]); // Limpa os erros quando o usuário começa a digitar
                }}
                required
              />
            </article>
            <footer className="text-center mt-3">
              <button type="submit" className="btn btn-primary" disabled={loading || isSubmitting}>
                {loading ? 'Entrando..' : 'Entrar'}
              </button>
              <div className="mt-3">
                <a href="/cadastro" style={{ color: 'var(--secondary-color)' }}>Cadastre-se</a>
              </div>
            </footer>
          </form>
        </div>
      </section>
    </main>
  );
};

export default Login;
