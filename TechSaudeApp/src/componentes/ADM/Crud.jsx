import React, { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash, faTimes } from "@fortawesome/free-solid-svg-icons";
import "./Crud.css";

const Crud = () => {
  const [medicos, setMedicos] = useState([
    { id: 1, nome: "Dr. João", crm: "123456", especialidade: "Cardiologia" },
    { id: 2, nome: "Dra. Maria", crm: "654321", especialidade: "Dermatologia" },
    { id: 3, nome: "Dr. Pedro", crm: "112233", especialidade: "Pediatria" },
  ]);

  const [formData, setFormData] = useState({
    id: null,
    nome: "",
    crm: "",
    especialidade: "",
  });

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEdit, setIsEdit] = useState(false);

  const openModal = (medico = null) => {
    if (medico) {
      setFormData(medico);
      setIsEdit(true);
    } else {
      setFormData({ id: null, nome: "", crm: "", especialidade: "" });
      setIsEdit(false);
    }
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
  };

  const handleInputChange = (e) => {
    const { id, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [id]: value,
    }));
  };

  const handleSubmit = () => {
    if (isEdit) {
      setMedicos(
        medicos.map((medico) =>
          medico.id === formData.id ? formData : medico
        )
      );
    } else {
      setMedicos([...medicos, { id: medicos.length + 1, ...formData }]);
    }
    closeModal();
  };

  const handleDelete = (id) => {
    setMedicos(medicos.filter((medico) => medico.id !== id));
  };

  return (
    <div className="crud-container">
      <header className="crud-header">
        <h1>Gerenciamento dos Usuários - Médico</h1>
      </header>

      <main className="crud-main">
        <div className="crud-topo-tabela">
          <button className="crud-botao-adicionar" onClick={() => openModal()}>
            Adicionar
          </button>
        </div>

        <table className="crud-table">
          <thead className="crud-thead">
            <tr>
              <th>ID DO MÉDICO</th>
              <th>NOME</th>
              <th>CRM</th>
              <th>ESPECIALIDADE</th>
              <th className="crud-acao">Alterações</th>
            </tr>
          </thead>
          <tbody className="crud-tbody">
            {medicos.map((medico) => (
              <tr key={medico.id}>
                <td>{medico.id}</td>
                <td>{medico.nome}</td>
                <td>{medico.crm}</td>
                <td>{medico.especialidade}</td>
                <td className="crud-acao">
                  <button className="crud-td-button" onClick={() => openModal(medico)}>
                    <FontAwesomeIcon icon={faEdit} className="crud-icon edit-icon" />
                  </button>
                  <button className="crud-td-button" onClick={() => handleDelete(medico.id)}>
                    <FontAwesomeIcon icon={faTrash} className="crud-icon delete-icon" />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </main>

      {/* <footer className="crud-footer">
        <button>&lt;</button>
        <button>1</button>
        <button>2</button>
        <button>3</button>
        <button>&gt;</button>
      </footer> */}

      {isModalOpen && (
        <div className="crud-modal-container crud-active">
          <div className="crud-modal">
            <button className="crud-close-modal" onClick={closeModal}>
              <FontAwesomeIcon icon={faTimes} />
            </button>

            <form>
              <div className="form-group">
                <label htmlFor="nome">Nome</label>
                <input
                  type="text"
                  id="nome"
                  value={formData.nome}
                  onChange={handleInputChange}
                  required
                />
              </div>

              <div className="form-group">
                <label htmlFor="crm">CRM</label>
                <input
                  type="text"
                  id="crm"
                  value={formData.crm}
                  onChange={handleInputChange}
                  maxLength={6}
                  required
                />
              </div>

              <div className="form-group">
                <label htmlFor="especialidade">Especialidade</label>
                <input
                  type="text"
                  id="especialidade"
                  value={formData.especialidade}
                  onChange={handleInputChange}
                  required
                />
              </div>

              <button type="button" className="crud-btnAdicionar" onClick={handleSubmit}>
                {isEdit ? "Atualizar" : "Enviar"}
              </button>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Crud;
