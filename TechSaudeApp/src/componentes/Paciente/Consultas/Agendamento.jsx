import React, { useState, useEffect } from 'react';
import { Container, Form, Dropdown, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Sidebar from '../Sidebar';
import Header from '../Header';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Agendamento.css';

const Agendamento = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    especialidade: '',
    localidade: '',
    medico: '',
    dataHora: ''
  });

  const apiUrl = import.meta.env.VITE_API_URL;

  const [especialidades, setEspecialidades] = useState([]);
  const [localidades, setLocalidades] = useState([]);
  const [medicos, setMedicos] = useState([]);

  useEffect(() => {
    const fetchEspecialidade = async () => {
      try {
        const especialidadeResponse = await axios.get(`${apiUrl}/api/Medicos/especialidades`);
        setEspecialidades(especialidadeResponse.data.data.$values);
        // console.log(especialidadeResponse.data);
      } catch (error) {
        console.error('Erro ao buscar dados', error);
      }
    };

    fetchEspecialidade();
  }, []);


  useEffect(() => {
    const fetchLocalidade = async () => {
      if (formData.especialidade) { 
        try {
          const localidadeResponse = await axios.get(`${apiUrl}/api/Medicos/localidades`, {
            params: { especialidade: formData.especialidade }
          });

          if (localidadeResponse.data && localidadeResponse.data.data) {
              setLocalidades(localidadeResponse.data.data.$values);
          } else {
              console.error('Formato de resposta inesperado', localidadeResponse.data);
          }    
          // console.log(localidadeResponse.data);
        } catch (error) {
          console.error('Erro ao buscar dados', error.response ? error.response.data : error.message);
        }
      }
    };

    fetchLocalidade();
  }, [formData.especialidade]); 


  useEffect(() => {
    const fetchMedicos = async () => {
      if (formData.especialidade && formData.localidade) {
        try {
          const medicoResponse = await axios.get(`${apiUrl}/api/Medicos/filtrar`, {
            params: {
              especialidade: formData.especialidade,
              localidade: formData.localidade
            }
          });
          setMedicos(medicoResponse.data.data.$values);
          // console.log(medicoResponse.data.data.$values)
        } catch (error) {
          console.error('Erro ao buscar médicos', error);
        }
      }
    };

    fetchMedicos();
  }, [formData.especialidade, formData.localidade]);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleClear = () => {
    setFormData({
      especialidade: '',
      localidade: '',
      medico: '',
      dataHora: ''
    });
    setMedicos([]);
  };

  const handleFilter = () => {
    navigate('/agendadas', { state: { filteredData: formData } });
  };

  const handleSchedule = async () => {
    try {
      const idUsuario = JSON.parse(localStorage.getItem('idUsuario'));

      const agendamento = {
        DataHora: formData.dataHora,
        PacienteId: idUsuario, // Substitua com o ID do paciente real
        MedicoId: parseInt(formData.medico), // Aqui está o ID do médico selecionado
        Local: formData.localidade
      };
      // console.log(agendamento);
      const response = await axios.post(`${apiUrl}/api/Consultas/agendamento/${idUsuario}`, agendamento);
      // console.log('Server Response:', response.data);
     // alert(response.data.message);
      var sucesso = response.data.sucesso;
      if(sucesso){
        alert("Consulta agendada com sucesso!");
        //navigate('')
      }
      else{
        alert(response.data.message);
      }
      // Navegar para a página de consultas agendadas ou mostrar uma mensagem de sucesso
      // navigate('/agendadas');
    } catch (error) {
      console.error('Erro ao agendar consulta', error);
    }
  };

  const handleAction = (action) => {
    if (action === 'clear') {
      handleClear();
    } else if (action === 'filter') {
      handleFilter();
    } else if (action === 'schedule') {
      handleSchedule();
    }
  };

  return (
    <div className="agendamento-container">
      <Sidebar />
      <div className="agendamento-main-content">
        <Header />
        <div className="agendamento-content">
          <div className="agendamento-buttons">
            <button onClick={() => navigate('/agendamento')} className="nav-button">Agendamento</button>
            <button onClick={() => navigate('/agendadas')} className="nav-button">Consultas Confirmadas</button>
            <button onClick={() => navigate('/historico')} className="nav-button">Histórico de Consultas</button>
          </div>
          <Container fluid className="main-content p-4" style={{ backgroundColor: 'var(--background-color)' }}>
            <div className="d-flex align-items-center justify-content-between mb-4">
              <h2 className="mb-0">Agendamento de Consulta</h2>
              {/* <Dropdown>
                <Dropdown.Toggle variant="primary" id="dropdown-basic">
                  Alterar
                </Dropdown.Toggle>

                <Dropdown.Menu>
                  <Dropdown.Item onClick={() => handleAction('clear')}>Limpar Filtro</Dropdown.Item>
                  <Dropdown.Item onClick={() => handleAction('filter')}>Filtrar</Dropdown.Item>
                  <Dropdown.Item onClick={() => handleAction('schedule')}>Agendar Consulta</Dropdown.Item>
                </Dropdown.Menu>
              </Dropdown> */}
            </div>
            <Form>
              <Form.Group controlId="especialidade">
                <Form.Label>Especialidade</Form.Label>
                <Form.Control
                  as="select"
                  name="especialidade"
                  value={formData.especialidade}
                  onChange={handleChange}
                  required
                >
                  <option value="">Selecione a especialidade</option>
                  {especialidades.map((esp, index) => (
                    <option key={index} value={esp.id}>{esp.nome}</option>
                  ))}
                </Form.Control>
              </Form.Group>

              <Form.Group controlId="localidade">
                <Form.Label>Localidade</Form.Label>
                <Form.Control
                  as="select"
                  name="localidade"
                  value={formData.localidade}
                  onChange={handleChange}
                  required
                >
                  <option value="">Selecione a localidade</option>
                  {localidades.map((loc, index) => (
                    <option key={index} value={loc.id}>{loc.nome}</option>
                  ))}
                </Form.Control>
              </Form.Group>

              <Form.Group controlId="medico">
                <Form.Label>Médico(a)</Form.Label>
                <Form.Control
                  as="select"
                  name="medico"
                  value={formData.medico}
                  onChange={handleChange}
                  required
                >
                  <option value="">Selecione o médico</option>
                  {medicos.map((medico, index) => (
                    <option key={index} value={medico.id}> {`Dr(a). ${medico.nome}`} </option>
                  ))}
                </Form.Control>
              </Form.Group>

              <Form.Group controlId="dataHora">
                <Form.Label>Data e Hora</Form.Label>
                <Form.Control
                  type="datetime-local"
                  name="dataHora"
                  value={formData.dataHora}
                  onChange={handleChange}
                  required
                />
              </Form.Group>
              <Button variant="primary" onClick={() => handleAction('schedule')}>Agendar Consulta</Button>
            </Form>
          </Container>
        </div>
      </div>
    </div>
  );
};

export default Agendamento;
