import React, { useState } from 'react';
import { NavLink , useNavigate} from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faHistory, faCalendarAlt, faSyringe, faAllergies, faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import './Sidebar.css';

const Sidebar = () => {
  const [collapsed, setCollapsed] = useState(true);

  const navigate = useNavigate();

  const handleToggleSidebar = () => {
    setCollapsed(!collapsed);
  };

  const logout = () => {
    // remover o token e o id do usuario
    localStorage.removeItem("idUsuario");
    localStorage.removeItem("token");
    navigate("/")
  }
  return (
    <aside className={`sidebar ${collapsed ? 'collapsed' : 'expanded'}`}>
      <div className="logo" onClick={handleToggleSidebar}>
        <img src="/logotech.png" alt="Logo" />
      </div>
      <nav>
        <ul>
          <li>
            <NavLink to="/perfil">
              <FontAwesomeIcon icon={faUser} className="fa-icon" /> {!collapsed && 'Perfil'}
            </NavLink>
          </li>
        
          <li>
            <NavLink to="/agendamento">
              <FontAwesomeIcon icon={faCalendarAlt} className="fa-icon" /> {!collapsed && 'Agendamento'}
            </NavLink>
          </li>
          <li>
            <NavLink to="/vacinas">
              <FontAwesomeIcon icon={faSyringe} className="fa-icon" /> {!collapsed && 'Vacinas'}
            </NavLink>
          </li>
          <li>
            <NavLink to="/alergias">
              <FontAwesomeIcon icon={faAllergies} className="fa-icon" /> {!collapsed && 'Alergias'}
            </NavLink>
          </li>
          <li>
            <NavLink to="/exames">
              <FontAwesomeIcon icon={faHistory} className="fa-icon" /> {!collapsed && 'Meu Histórico'}
            </NavLink>
          </li>
        </ul>
      </nav>
  
      <div className="toggle-sidebar">
        <a onClick={logout}> {!collapsed && 'Sair da Conta'}</a>
        <a onClick={handleToggleSidebar}>
          <FontAwesomeIcon icon={collapsed ? faArrowRight : faArrowLeft} className="fa-icon" />
        </a>
      </div>
    </aside>
  );
};

export default Sidebar;
