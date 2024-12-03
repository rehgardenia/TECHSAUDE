import React, { useState, useEffect } from 'react';
import { Button, Form, Container, Dropdown, Modal } from 'react-bootstrap';
import axios from 'axios';
import Sidebar from '../Sidebar';
import Header from '../Header';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Perfil.css';

const Perfil = () => {
  const [isEditing, setIsEditing] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    nomeCompleto: '',
    dataNascimento: '',
    sexo: '',
    endereco: '',
    telefone: '',
    email: '',
    cns: '',
    convenio: '',
  });

  const idUsuario = JSON.parse(localStorage.getItem('idUsuario'));

  // Função para converter DateTime para dd/mm/aaaa
  const formatDate = (dateString) => {
    if (!dateString) return '';
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Mês começa do 0
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
  };

  // Função para converter dd/mm/aaaa para DateTime (ISO)
  const formatToISO = (dateString) => {
    if (!dateString) return '';
    const [day, month, year] = dateString.split('/');
    return new Date(`${year}-${month}-${day}`).toISOString();
  };

  // Busca os dados do paciente ao montar o componente
  useEffect(() => {
    const fetchPacienteData = async () => {
      try {
        const apiUrl = import.meta.env.VITE_API_URL;  // URL da API
        const response = await axios.get(`${apiUrl}/api/Pacientes/${idUsuario}`);
        const data = response.data.data;
       
        // Converte a data de nascimento para o formato dd/mm/aaaa
        setFormData({
          nomeCompleto: data.nomeCompleto || '',
          dataNascimento: formatDate(data.dataNascimento),
          sexo: data.sexo || '',
          endereco: data.endereco || '',
          telefone: data.telefone || '',
          email: data.email || '',
          cns: data.cns || '',
          convenio: data.convenio || '',
        });

        // console.log('Dados recebidos:', data);
        // console.log('FormData atual:', formData);

      } catch (error) {
        console.error('Erro ao buscar dados do paciente:', error);
      }
    };

    if (idUsuario) {
      fetchPacienteData();
    }
  }, [idUsuario]);

  const handleEdit = () => {
    setIsEditing(true);
  };

  const handleSave = async () => {
    try {
      const apiUrl = import.meta.env.VITE_API_URL;  // URL da API

      // Converte a data de nascimento de dd/mm/aaaa para ISO antes de enviar
      const updatedData = {
        ...formData,
        dataNascimento: formatToISO(formData.dataNascimento),
      };

      await axios.put(`${apiUrl}/api/Pacientes/${idUsuario}`, updatedData);
      setIsEditing(false);
      setShowModal(true);
    } catch (error) {
      console.error('Erro ao atualizar dados do paciente:', error);
    }
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.id]: e.target.value,
    });
  };

  return (
    <div className="d-flex">
      <Sidebar />
      <div className="perfil-content">
        <Header />
        <Container fluid className="perfil-content-container">
          <div className="dropdown-container">
            <h2 className="perfil-page-title">Informações Gerais</h2>
            <Dropdown className="dropdown">
              <Dropdown.Toggle variant="primary" id="dropdown-basic">
                {isEditing ? 'Salvando' : 'Alterar'}
              </Dropdown.Toggle>

              <Dropdown.Menu>
                {isEditing ? (
                  <Dropdown.Item as="button" onClick={handleSave}>
                    Salvar
                  </Dropdown.Item>
                ) : (
                  <Dropdown.Item as="button" onClick={handleEdit}>
                    Editar
                  </Dropdown.Item>
                )}
              </Dropdown.Menu>
            </Dropdown>
          </div>
          <Form>
            <Form.Group controlId="nomeCompleto">
              <Form.Label className="form-label">Nome completo:</Form.Label>
              <Form.Control
                type="text"
                value={formData.nomeCompleto}
                onChange={handleChange}
                disabled={!isEditing}
              />
            </Form.Group>

            <Form.Group controlId="dataNascimento">
              <Form.Label className="form-label">Data de nascimento:</Form.Label>
              <Form.Control
                type="text"  // Usa tipo text para aceitar dd/mm/aaaa
                value={formData.dataNascimento}
                onChange={handleChange}
                disabled={!isEditing}
                placeholder="dd/mm/aaaa"
              />
            </Form.Group>

            <Form.Group controlId="email">
              <Form.Label className="form-label">E-mail:</Form.Label>
              <Form.Control
                type="email"
                value={formData.email}
                onChange={handleChange}
                disabled={!isEditing}
              />
            </Form.Group>
            <Form.Group controlId="cns">
              <Form.Label className="form-label">CNS:</Form.Label>
              <Form.Control
                type="text"
                value={formData.cns}
                onChange={handleChange}
                disabled={!isEditing}
              />
            </Form.Group>
            <Form.Group controlId="sexo">
              <Form.Label className="form-label">Sexo:</Form.Label>
              <Form.Control
                as="select"
                value={formData.sexo}
                onChange={handleChange}
                disabled={!isEditing}
              >
                <option value="">{formData.sexo}</option>
                <option value="feminino">Feminino</option>
                <option value="masculino">Masculino</option>
                <option value="outro">Outro</option>
              </Form.Control>
            </Form.Group>

            <Form.Group controlId="convenio">
              <Form.Label className="form-label">Convênio:</Form.Label>
              <Form.Control
                type="text"
                value={formData.convenio}
                onChange={handleChange}
                disabled={!isEditing}
              />
            </Form.Group>

            <Form.Group controlId="endereco">
              <Form.Label className="form-label">Endereço:</Form.Label>
              <Form.Control
                type="text"
                value={formData.endereco}
                onChange={handleChange}
                disabled={!isEditing}
              />
            </Form.Group>

            <Form.Group controlId="telefone">
              <Form.Label className="form-label">Telefone:</Form.Label>
              <Form.Control
                type="text"
                value={formData.telefone}
                onChange={handleChange}
                disabled={!isEditing}
              />
            </Form.Group>
          </Form>

          {/* Modal de confirmação */}
          <Modal show={showModal} onHide={handleCloseModal}>
            <Modal.Header closeButton>
              <Modal.Title>Atualização Concluída</Modal.Title>
            </Modal.Header>
            <Modal.Body>Os dados foram atualizados com sucesso!</Modal.Body>
            <Modal.Footer>
              <Button
                style={{ backgroundColor: 'var(--primary-color)', borderColor: 'var(--primary-color)' }}
                onClick={handleCloseModal}
              >
                Fechar
              </Button>
            </Modal.Footer>
          </Modal>
        </Container>
      </div>
    </div>
  );
};

export default Perfil;
