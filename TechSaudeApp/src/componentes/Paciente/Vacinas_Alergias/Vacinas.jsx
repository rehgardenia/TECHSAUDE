import React, { useState, useEffect, useRef } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit } from "@fortawesome/free-solid-svg-icons";
import axios from "axios";
import Sidebar from '../Sidebar';
import Header from '../Header';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Vacinas_Alergias.css';

const Vacinas = () => {
  const [vacinas, setVacinas] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [errors, setErrors] = useState({});

  const [formData, setFormData] = useState({
    id: null,
    nome: "",
    lote: "",
    unidadeSaude: "",
    data: ""
  });

  const apiUrl = import.meta.env.VITE_API_URL;
  const idUsuario = JSON.parse(localStorage.getItem('idUsuario'));
  const modalRef = useRef();
  
  useEffect(() => {
    const fetchVacinas = async () => {
      try {
        const response = await axios.get(`${apiUrl}/api/Vacinas/${idUsuario}`);
        const resposta = response.data.data.$values || [];
        setVacinas(resposta);
      } catch (error) {
        console.error('Erro ao buscar vacinas:', error);
        setErrors({ fetch: 'Erro ao buscar vacinas' });
      }
    };
  
    fetchVacinas();
  }, [idUsuario]);

  const openModal = (vacina = null) => {
    if (vacina) {
      setFormData({
        id: vacina.vacinaId,
        nome: vacina.nome,
        lote: vacina.lote,
        unidadeSaude: vacina.unidadeSaude,
        data: vacina.data ? vacina.data.split('T')[0] : ''
      });
      setIsEdit(true);
    } else {
      setFormData({
        id: null,
        nome: "",
        lote: "",
        unidadeSaude: "",
        data: ""
      });
      setIsEdit(false);
    }
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setErrors({});
  };

  const handleInputChange = (e) => {
    const { id, value } = e.target;
    setFormData((prevFormData) => ({ ...prevFormData, [id]: value }));
  };

  const validateForm = (data) => {
    const errors = {};
    if (!data.nome) {
      errors.nome = 'O campo "Vacina" é obrigatório';
    }
    if (!data.lote) {
      errors.lote = 'O campo "Lote" é obrigatório';
    }
    if (!data.unidadeSaude) {
      errors.unidadeSaude = 'O campo "Unidade de Saúde" é obrigatório';
    }
    if (!data.data) {
      errors.data = 'O campo "Data" é obrigatório';
    }
    return Object.keys(errors).length > 0 ? errors : null;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const validationErrors = validateForm(formData);

    if (validationErrors) {
      setErrors(validationErrors);
      alert('Por favor, corrija os erros abaixo:');
      return;
    }

    try {
      if (formData.id) {
        const response = await axios.put(`${apiUrl}/api/Vacinas/${formData.id}`, {
          nome: formData.nome,
          lote: formData.lote,
          unidadeSaude: formData.unidadeSaude,
          data: formData.data
        });
        setVacinas(prevVacinas => prevVacinas.map(v => (v.vacinaId === formData.id ? response.data.data : v)));
      } else {
        const response = await axios.post(`${apiUrl}/api/Vacinas/${idUsuario}`, {
          nome: formData.nome,
          lote: formData.lote,
          unidadeSaude: formData.unidadeSaude,
          data: formData.data,
        });
        setVacinas(prevVacinas => [...prevVacinas, response.data.data]);
      }
      closeModal();
    } catch (error) {
      console.error('Erro ao enviar dados:', error);
    }
  };

  const ErrorMessages = ({ errors }) => {
    if (!errors || Object.keys(errors).length === 0) return null;
    return (
      <div className="error-messages">
        {Object.keys(errors).map((key) => (
          <p key={key}>{errors[key]}</p>
        ))}
      </div>
    );
  };

  const handleOutsideClick = (e) => {
    if (modalRef.current && !modalRef.current.contains(e.target)) {
      closeModal();
    }
  };

  useEffect(() => {
    if (isModalOpen) {
      document.addEventListener("mousedown", handleOutsideClick);
    } else {
      document.removeEventListener("mousedown", handleOutsideClick);
    }

    return () => {
      document.removeEventListener("mousedown", handleOutsideClick);
    };
  }, [isModalOpen]);

  const formatDate = (dateString) => {
    if (!dateString) return '';
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
  };

  return (
    <div>
      <Sidebar />
      <div className="alergias-vacinas-container">
        <Header />
        <main className="vacinas-main">
          <div className="vacinas-header">
            <h2 className="vacinas-page-title">Gerenciamento de Vacinas</h2>
            <button className="vacinas-botao-adicionar" onClick={() => openModal()}>
              Adicionar
            </button>
          </div>
          <table className="vacinas-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Vacina</th>
                <th>Lote</th>
                <th>Unidade de Saúde</th>
                <th>Data</th>
                <th className="vacinas-acao">Alterações</th>
              </tr>
            </thead>
            <tbody>
              {vacinas.length === 0 ? (
                <tr>
                  <td colSpan="6" style={{ textAlign: 'center' }}>Nenhuma Vacina Encontrada</td>
                </tr>
              ) : (
                vacinas.map((vacina) => (
                  <tr key={vacina.vacinaId}>
                    <td>{vacina.vacinaId}</td>
                    <td>{vacina.nome}</td>
                    <td>{vacina.lote}</td>
                    <td>{vacina.unidadeSaude}</td>
                    <td>{formatDate(vacina.data)}</td>
                    <td className="vacinas-acao">
                      <button className="alergias-td-button" onClick={() => openModal(vacina)}>
                        <FontAwesomeIcon icon={faEdit} />
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </main>

        {isModalOpen && (
          <div className="vacinas-modal-container">
            <div className="vacinas-modal" ref={modalRef}>
              <form onSubmit={handleSubmit}>
                <div className="form-group">
                  <label htmlFor="nome">Vacina</label>
                  <input
                    type="text"
                    id="nome"
                    value={formData.nome}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="lote">Lote</label>
                  <input
                    type="text"
                    id="lote"
                    value={formData.lote}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="unidadeSaude">Unidade de Saúde</label>
                  <input
                    type="text"
                    id="unidadeSaude"
                    value={formData.unidadeSaude}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="data">Data</label>
                  <input
                    type="date"
                    id="data"
                    value={formData.data}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <ErrorMessages errors={errors} />
                <button type="submit" className="vacinas-botao-adicionar">
                  {isEdit ? "Salvar Alterações" : "Adicionar Vacina"}
                </button>
              </form>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Vacinas;
