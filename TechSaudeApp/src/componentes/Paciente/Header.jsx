import React, { useState, useEffect } from "react"; 
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBell, faHome } from '@fortawesome/free-solid-svg-icons';
import axios from "axios"; // Certifique-se de importar o axios
import './Header.css';

const Header = () => {
  const idUsuario = JSON.parse(localStorage.getItem('idUsuario')); // Obtendo o ID do usuário do localStorage
  const [nomePaciente, setNomePaciente] = useState('Nome Paciente'); // Estado para armazenar o nome do paciente

  useEffect(() => {
    const fetchPacienteNome = async () => {
      try {
        const apiUrl = import.meta.env.VITE_API_URL; // URL da API (variável de ambiente)
        const response = await axios.get(`${apiUrl}/api/Pacientes/${idUsuario}`); // Requisição à API
        const nome = response.data.data.nomeCompleto; // Ajuste conforme o campo correto na resposta da API
        // console.log(nome);
        setNomePaciente(nome || 'Nome Paciente'); // Atualiza o estado com o nome ou usa valor padrão
      } catch (error) {
        console.error('Erro ao buscar o nome do paciente:', error);
      }
    };

    if (idUsuario) {
      fetchPacienteNome(); // Chama a função se o idUsuario estiver disponível
    }
  }, [idUsuario]); // O useEffect será executado sempre que o idUsuario mudar

  return (
    <header className="app-header">
      <div className="header-content">
        <div className="logo">
          <FontAwesomeIcon icon={faHome} />
          <span>Home</span>
        </div>
        <div className="header-right">
          <FontAwesomeIcon icon={faBell} />
          <div className="user-info">
            <span className="user-department">{nomePaciente}</span> {/* Nome dinâmico do paciente */}
          </div>
          <div className="user-avatar">
            <span>NU</span>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;