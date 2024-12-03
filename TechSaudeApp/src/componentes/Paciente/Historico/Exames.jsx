import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Sidebar from '../Sidebar';
import Header from '../Header';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUpload, faDownload, faPlus } from '@fortawesome/free-solid-svg-icons';
import { Modal, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import axios from 'axios';
import './Historico.css';

const Exames = () => {
  const [exames, setExames] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    file: null,
    descricao: '',
    dataAssociada: '',
  });

  const [fileName, setFileName] = useState('');
  const navigate = useNavigate();
  const apiUrl = import.meta.env.VITE_API_URL;
  const idUsuario = JSON.parse(localStorage.getItem('idUsuario'));

  useEffect(() => {
    const fetchExames = async () => {
      try {
        const response = await axios.get(`${apiUrl}/api/Documento/exames`, {
          params: { pacienteId: idUsuario },
        });
  
        // Verifique se a resposta é válida e contém os dados esperados
        if (response.data && response.data.data && Array.isArray(response.data.data.$values)) {
          setExames(response.data.data.$values);
        } else {
          console.error('Dados inesperados recebidos:', response.data);
          setExames([]); // Define exames como vazio se a estrutura não estiver correta
        }
      } catch (error) {
        console.error('Erro ao buscar exames:', error);
        setExames([]); // Limpa a lista de exames em caso de erro
      }
    };
  
    fetchExames();
  }, [idUsuario]);

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    setFormData((prevState) => ({
      ...prevState,
      file: file,
    }));
    setFileName(file ? file.name : '');
  };

  const handleAddExam = async () => {
    if (!formData.file || !formData.descricao || !formData.dataAssociada) {
      alert('Por favor, preencha todos os campos!');
      return;
    }

    const uploadFormData = new FormData();
    uploadFormData.append('file', formData.file);
    uploadFormData.append('categoria', 'Exames');
    uploadFormData.append('pacienteId', idUsuario);
    uploadFormData.append('descricao', formData.descricao);
    uploadFormData.append('dataAssociada', formData.dataAssociada);

    try {
      const response = await axios.post(`${apiUrl}/api/Documento/upload`, uploadFormData, {
        headers: {
          'accept': 'text/plain',
        },
      });

      if (!response.data.sucesso) {
        alert('Erro ao enviar arquivo: ' + response.data.message);
      } else {
        setExames((prevExames) => [...prevExames, response.data.data]);
        setFormData({ file: null, descricao: '', dataAssociada: '' });
        setFileName('');
        setShowModal(false);
      }
    } catch (error) {
      console.error('Erro ao enviar o arquivo:', error);
      alert('Erro ao enviar o arquivo.');
    }
  };

  const handleDownload = async (exame) => {
    try {
      const response = await axios.get(`${apiUrl}/api/Documento/download/${exame.documentoID}`, {
        responseType: 'blob',
      });

      const url = window.URL.createObjectURL(new Blob([response.data]));
      const contentType = response.headers['content-type'];
      let extension = '';

      if (contentType.includes('pdf')) {
        extension = '.pdf';
      } else if (contentType.includes('image/jpeg')) {
        extension = '.jpg';
      } else if (contentType.includes('image/png')) {
        extension = '.png';
      } else if (contentType.includes('image/gif')) {
        extension = '.gif';
      }

      const fileName = exame.fileName ? `${exame.fileName}${extension}` : `arquivo${extension}`;
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', fileName);
      document.body.appendChild(link);
      link.click();
      link.remove();
    } catch (error) {
      console.error('Erro ao baixar o arquivo:', error);
      alert('Erro ao baixar o arquivo.');
    }
  };

  const formatData = (dateString) => {
    if (!dateString) return '';
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
  };

  return (
    <div className="historico-container">
      <Sidebar />
      <div className="historico-main-content">
        <Header />
        <div className="historico-content">
          <div className="historico-buttons">
            <button onClick={() => navigate('/exames')} className="custom-file-upload">Exames</button>
            <button onClick={() => navigate('/receituario')} className="custom-file-upload">Receituários</button>
            <button onClick={() => navigate('/documentos')} className="custom-file-upload">Documentos</button>
            <button
              className="upload-btn"
              style={{ marginLeft: 'auto' }}
              onClick={() => setShowModal(true)}
            >
              <FontAwesomeIcon icon={faPlus} /> Adicionar Exame
            </button>
          </div>
          <table className="historico-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Descrição</th>
                <th>Data</th>
                <th className="historico-acao">Opções</th>
              </tr>
            </thead>
            <tbody>
              {exames.length === 0 ? (
                <tr>
                  <td colSpan="4" style={{ textAlign: 'center' }}>Nenhum Exame Encontrado</td>
                </tr>
              ) : (
                exames.map((exame) => (
                  <tr key={exame?.documentoID || 'default-key'}>
                    <td>{exame?.documentoID || 'N/A'}</td>
                    <td>{exame?.descricao || 'N/A'}</td>
                    <td>{formatData(exame?.dataAssociada)}</td>
                    <td className="historico-acao">
                      <button onClick={() => handleDownload(exame)} className="historico-botao-download">
                        <FontAwesomeIcon icon={faDownload} />
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>

          {/* Modal para Adicionar Exame */}
          <Modal show={showModal} onHide={() => setShowModal(false)}>
            <Modal.Header closeButton>
              <Modal.Title>Adicionar Exame</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <div className="historico-upload-section">
                <label htmlFor="fileUpload" className="custom-file-upload">
                  <FontAwesomeIcon icon={faUpload} /> Escolher Arquivo
                </label>
                <input
                  type="file"
                  className="form-control-file"
                  id="fileUpload"
                  onChange={handleFileChange}
                  style={{ display: 'none' }}
                />
                <span>{fileName ? `Arquivo Selecionado: ${fileName}` : 'Nenhum arquivo selecionado'}</span>
                <input
                  type="text"
                  placeholder="Descrição do Exame"
                  value={formData.descricao}
                  onChange={(e) => setFormData((prevState) => ({ ...prevState, descricao: e.target.value }))}
                  className="form-control mb-3"
                />
                <input
                  type="date"
                  value={formData.dataAssociada}
                  onChange={(e) => setFormData((prevState) => ({ ...prevState, dataAssociada: e.target.value }))}
                  className="form-control mb-3"
                />
              </div>
            </Modal.Body>
            <Modal.Footer>
              <Button variant="primary" onClick={handleAddExam}>
                Adicionar
              </Button>
            </Modal.Footer>
          </Modal>
        </div>
      </div>
    </div>
  );
};

export default Exames;
