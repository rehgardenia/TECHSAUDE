import React, { useState, useEffect } from 'react';
import { Container, Table } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Sidebar from '../Sidebar';
import Header from '../Header';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Historico.css';



const Historico = () => {
    const navigate = useNavigate();
    const idUsuario = JSON.parse(localStorage.getItem('idUsuario'));
    // console.log(idUsuario);
    const [historico, setHistorico] = useState([]);

    const apiUrl = import.meta.env.VITE_API_URL;
    useEffect(() => {
        const fetchHistorico = async () => {
            try {
                // console.log(idUsuario);
                const response = await axios.get(`${apiUrl}/api/Consultas/paciente/${idUsuario}`);
                // console.log(response.data);
                if (response.data.sucesso) {
                    setHistorico(response.data.data.$values);
                    // console.log(response.data.data.$values);
                } else {
                    console.error('Erro ao buscar histórico:', response.data.message);
                }
            } catch (error) {
                console.error('Erro ao buscar histórico:', error);
            }
        };

        if (idUsuario) {
            fetchHistorico();
        }
    }, [idUsuario]);

    return (
        <div className="historico-consultas-container">
            <Sidebar />
            <div className="historico-consultas-main-content">
                <Header />
                <div className="historico-consultas-content">
                    <div className="historico-consultas-buttons">
                        <button onClick={() => navigate('/agendamento')} className="nav-button">Agendamento</button>
                        <button onClick={() => navigate('/agendadas')} className="nav-button">Consultas Confirmadas</button>
                        <button onClick={() => navigate('/historico')} className="nav-button">Histórico de Consultas</button>
                    </div>
                    <Container fluid className="historico-consultas-table-container p-4" style={{ backgroundColor: 'var(--background-color)' }}>
                        <h2>Histórico de Consultas</h2>
                        <Table striped bordered hover responsive>
                            <thead>
                                <tr>
                                    <th>Data/Hora</th>
                                    <th>Médico(a)</th>
                                    <th>Especialidade</th>
                                    <th>Localidade</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                 {historico.length > 0 ? (
                                    historico.map((consulta) => (
                
                                            <tr key={consulta.id} >
                                                <td>{new Date(consulta.dataHora).toLocaleString()}</td>
                                                <td>{consulta.medico.nome}</td>
                                                <td>{consulta.medico.especialidade}</td>
                                                <td>{consulta.local}</td>
                                                <td>{consulta.status}</td>
                                            </tr>
                                    ))
                                ) : (
                                    <tr>
                                        <td colSpan="6">Nenhuma consulta encontrada.</td>
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

export default Historico;
