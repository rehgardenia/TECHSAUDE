import React, { useState, useEffect, useRef } from "react";
import axios from "axios";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTimes } from "@fortawesome/free-solid-svg-icons";
import Sidebar from '../Sidebar';
import Header from '../Header';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Vacinas_Alergias.css';

const Alergias = () => {
  const [alergias, setAlergias] = useState([]);
  const [doencas, setDoencas] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [errors, setErrors] = useState({});

  const [formData, setFormData] = useState({
    id: null,
    tipo: "",
    descricao: "",
    medicamento: ""
  });

  const apiUrl = import.meta.env.VITE_API_URL;
  const idUsuario = JSON.parse(localStorage.getItem('idUsuario'));

  const modalRef = useRef();

  // Fetch Alergias
  useEffect(() => {
    const fetchAlergias = async () => {
      try {
        const response = await axios.get(`${apiUrl}/api/Alergias/${idUsuario}`);
        const resposta = response.data.data.$values || [];
        setAlergias(resposta);
      } catch (error) {
        console.error('Erro ao buscar alergias:', error);
        setErrors({ fetch: 'Erro ao buscar alergias' });
      }
    };

    fetchAlergias();
  }, [idUsuario]);

  // Fetch Doenças
  useEffect(() => {
    const fetchDoencas = async () => {
      try {
        const response = await axios.get(`${apiUrl}/api/Doencas/${idUsuario}`);
        const resposta = response.data.data.$values || [];
        setDoencas(resposta);
      } catch (error) {
        console.error('Erro ao buscar doenças:', error);
        setErrors({ fetch: 'Erro ao buscar doenças' });
      }
    };

    fetchDoencas();
  }, [idUsuario]);

  const openModal = (tipo = null, item = null) => {
    if (tipo === "Alergia") {
      if (item) {
        setFormData({
          id: item.alergiaId || item.doencaId,
          tipo: tipo,
          descricao: item.descricao,
          medicamento: item.medicamento,
        });
        setIsEdit(true);
      }
      else {
        setFormData({ id: null, tipo, descricao: "", medicamento: "" });
        setIsEdit(false);
      }
    }
    else if (tipo === "Doenca") {
      if (item) {
        setFormData({
          id: item.doencaId,
          tipo: tipo,
          descricao: item.descricao,
          medicamento: item.medicamento,
        });
        setIsEdit(true);
      }
      else {
        setFormData({ id: null, tipo, descricao: "", medicamento: "" });
        setIsEdit(false);
      }
    }
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setErrors({});
    setFormData({ id: null, tipo: "", descricao: "", medicamento: "" });
  };

  const handleInputChange = (e) => {
    const { id, value } = e.target;
    setFormData((prevFormData) => ({ ...prevFormData, [id]: value }));
  };


  const handleSubmit = async (e) => {
    e.preventDefault();

    // Validação da descrição
    if (!formData.descricao) {
      setErrors((prev) => ({ ...prev, descricao: 'Descrição é obrigatória' }));
      return;
    }

    try {
      if (formData.tipo === "Alergia") {
        if (formData.id) {
          const response = await axios.put(`${apiUrl}/api/Alergias/${formData.id}`, {
            descricao: formData.descricao,
            medicamento: formData.medicamento
          });
          console.log(response);
          setAlergias(alergias.map((item) => (item.alergiaId === response.data.data.alergiaId ? response.data.data : item)));
        } else {
          const response = await axios.post(`${apiUrl}/api/Alergias/${idUsuario}`, { ...formData, idUsuario });
          setAlergias([...alergias, response.data.data]);
        }
      } else if (formData.tipo === "Doenca") {
        if (formData.tipo === "Doenca") {
          if (formData.id) {
            console.log(formData.id);
            const response = await axios.put(`${apiUrl}/api/Doencas/${formData.id}`, {
              descricao: formData.descricao,
              medicamento: formData.medicamento
            });

            console.log('Resposta da atualização de doença:', response.data);

            if (response.data.data) {
              setDoencas(doencas.map((item) => (item && item.doencaId === response.data.data.doencaId ? response.data.data : item)));
            } else {
              console.error('Dados da doença não encontrados na resposta da API');
            }
          } else {
            const response = await axios.post(`${apiUrl}/api/Doencas/${idUsuario}`, { ...formData, idUsuario });
            console.log('Resposta da criação de doença:', response.data);
            setDoencas([...doencas, response.data.data]);
          }
        }
        closeModal();
      }
    }
    catch (error) {
      console.error('Erro ao enviar dados:', error.response ? error.response.data : error.message);
      setErrors({ submit: 'Erro ao enviar dados' });
    }
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

  return (
    <div>
      <Sidebar />
      <div className="alergias-vacinas-container">
        <Header />
        <main className="alergias-main">
          <div className="alergias-header">
            <h2 className="alergias-page-title">Gerenciamento de Alergias e Doenças</h2>
            <div>
              <button className="alergias-botao-adicionar espacamento-botao" onClick={() => openModal("Alergia")}>
                Adicionar Alergia
              </button>
              <button className="alergias-botao-adicionar" onClick={() => openModal("Doenca")}>
                Adicionar Doença
              </button>
            </div>
          </div>

          <table className="alergias-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Tipo</th>
                <th>Descrição</th>
                <th>Medicamento</th>
                <th className="alergias-acao">Alterações</th>
              </tr>
            </thead>
            <tbody>
              {alergias.length === 0 && doencas.length === 0 ? (
                <tr>
                  <td colSpan="5" style={{ textAlign: 'center' }}>Nenhuma Alergia e Doença Encontrada</td>
                </tr>
              ) : (
                <>
                  {alergias.map((alergia) => (
                    <tr key={alergia.alergiaId} className="table-row">
                      <td>{alergia.alergiaId}</td>
                      <td>Alergia</td>
                      <td>{alergia.descricao}</td>
                      <td>{alergia.medicamento}</td>
                      <td className="alergias-acao">
                        <button className="alergias-td-button" onClick={() => openModal("Alergia", alergia)}>
                          <FontAwesomeIcon icon={faEdit} className="alergias-icon" />
                        </button>
                      </td>
                    </tr>
                  ))}
                  {doencas.map((doenca) => (
                    <tr key={doenca.doencaId} className="table-row">
                      <td>{doenca.doencaId}</td>
                      <td>Doença</td>
                      <td>{doenca.descricao}</td>
                      <td>{doenca.medicamento}</td>
                      <td className="alergias-acao">
                        <button className="alergias-td-button" onClick={() => openModal("Doenca", doenca)}>
                          <FontAwesomeIcon icon={faEdit} className="alergias-icon" />
                        </button>
                      </td>
                    </tr>
                  ))}
                </>
              )}
            </tbody>
          </table>
        </main>

        {isModalOpen && (
          <div className="alergias-modal-container">
            <div className="alergias-modal" ref={modalRef}>

              <button className="alergias-close-modal" onClick={closeModal}>
                <FontAwesomeIcon icon={faTimes} />
              </button>
              <h4>{isEdit ? `Editar ${formData.tipo}` : `Adicionar ${formData.tipo}`}</h4> {/* Título do modal */}
              <form onSubmit={handleSubmit}>
                <div className="form-group">
                  <label htmlFor="descricao">Descrição</label>
                  <input
                    type="text"
                    id="descricao"
                    value={formData.descricao}
                    onChange={handleInputChange}
                    required
                  />
                  {errors.descricao && <span className="error-message">{errors.descricao}</span>}
                </div>

                <div className="form-group">
                  <label htmlFor="medicamento">Medicamento</label>
                  <input
                    type="text"
                    id="medicamento"
                    value={formData.medicamento}
                    onChange={handleInputChange}
                  />
                </div>

                <button type="submit" className="alergias-botao-adicionar" >
                  {isEdit ? "Atualizar" : "Adicionar"}
                </button>
              </form>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Alergias;
