import React, { useState, useEffect } from 'react';
import { Container, Table, Dropdown } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Sidebar from '../Sidebar';
import Header from '../Header';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Agendadas.css';

const Agendadas = () => {
    const navigate = useNavigate();
    const apiUrl = import.meta.env.VITE_API_URL;
    const idUsuario = JSON.parse(localStorage.getItem('idUsuario'));
    const [agendadas, setAgendadas] = useState([]);

    // Função para buscar as consultas confirmadas
    const fetchConsultas = async () => {
        try {
            const response = await axios.get(`${apiUrl}/api/Consultas/confirmadas`,{
                params : {
                    pacienteId: idUsuario
                },
            } );
            const result = response.data;
            // console.log(result);
            if (result.sucesso) {
                setAgendadas(result.data.$values || []);
            } else {
                console.error(result.message);
            }
        } catch (error) {
            console.error("Erro ao buscar consultas confirmadas", error);
        }
    };

    useEffect(() => {
        if (idUsuario) {
            fetchConsultas();
        }
    }, [idUsuario]);



    return (
        <div className="consultas-agendadas-container">
            <Sidebar />
            <div className="consultas-agendadas-main-content">
                <Header />
                <div className="consultas-agendadas-content">
                    <div className="consultas-agendadas-buttons">
                        <button onClick={() => navigate('/agendamento')} className="nav-button">Agendamento</button>
                        <button onClick={() => navigate('/agendadas')} className="nav-button">Consultas Confirmadas</button>
                        <button onClick={() => navigate('/historico')} className="nav-button">Histórico de Consultas</button>
                    </div>
                    <Container fluid className="consultas-agendadas-table-container p-4" style={{ backgroundColor: 'var(--background-color)' }}>
                        <h2>Consultas Agendadas</h2>
                        <Table striped bordered hover responsive>
                            <thead>
                                <tr>
                                    <th>Data/Hora</th>
                                    <th>Médico(a)</th>
                                    <th>Especialidade</th>
                                    <th>Localidade</th>
                                    {/* <th>Status</th> */}
                                    {/* <th>Alterações</th> */}
                                </tr>
                            </thead>
                            <tbody>
                                {agendadas.length > 0 ? (
                                    agendadas.map((consulta) => (
                                        <tr key={consulta.id}>
                                            <td>{new Date(consulta.dataHora).toLocaleString()}</td>
                                            <td>{consulta.medico.nome}</td>
                                            <td>{consulta.medico.especialidade}</td>
                                            <td>{consulta.local}</td>
                                        
                                        </tr>
                                    ))
                                ) : (
                                    <tr>
                                        <td colSpan="6">Nenhuma consulta agendada encontrada.</td>
                                    </tr>
                                )}
                            </tbody>
                        </Table>
                    </Container>
                </div>
            </div>
        </div>
    );
};

export default Agendadas;
