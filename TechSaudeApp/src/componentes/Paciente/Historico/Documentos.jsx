import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Sidebar from '../Sidebar';
import Header from '../Header';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUpload, faDownload, faPlus } from '@fortawesome/free-solid-svg-icons';
import { Modal, Button } from 'react-bootstrap';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Historico.css';

const Documentos = () => {
  const [documentos, setDocumentos] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    file: null,
    descricao: '',
    dataAssociada: '',
  });

  const navigate = useNavigate();
  const apiUrl = import.meta.env.VITE_API_URL;
  const idUsuario = JSON.parse(localStorage.getItem('idUsuario'));

  useEffect(() => {
    const fetchDocumentos = async () => {
      try {
        const response = await axios.get(`${apiUrl}/api/Documento/outros`, {
          params: {
            pacienteId: idUsuario,
          },
        });
         // Verifique se a resposta é válida e contém os dados esperados
         if (response.data && response.data.data && Array.isArray(response.data.data.$values)) {
          setDocumentos(response.data.data.$values);
        } else {
          console.error('Dados inesperados recebidos:', response.data);
          setDocumentos([]); // Define exames como vazio se a estrutura não estiver correta
        }
      } catch (error) {
        console.error('Erro ao buscar exames:', error);
        setDocumentos([]); // Limpa a lista de exames em caso de erro
      }
    };
  
    fetchDocumentos();
  }, [idUsuario]);

  const [fileName, setFileName] = useState(''); // Estado para armazenar o nome do arquivo

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    setFormData((prevState) => ({
      ...prevState,
      file: file,
    }));
    setFileName(file ? file.name : ''); // Atualiza o nome do arquivo
  };

  const handleAddDocumentos = async () => {
    if (!formData.file || !formData.descricao || !formData.dataAssociada) {
      alert('Por favor, preencha todos os campos!');
      return;
    }

    const uploadFormData = new FormData();
    uploadFormData.append('file', formData.file);
    uploadFormData.append('categoria', 'Outros');
    uploadFormData.append('pacienteId', idUsuario);
    uploadFormData.append('descricao', formData.descricao);
    uploadFormData.append('dataAssociada', formData.dataAssociada);

    try {
      console.log("Usuario : " + idUsuario);

      const response = await axios.post(`${apiUrl}/api/Documento/upload`, uploadFormData, {
        headers: {
          'accept': 'text/plain',
        },
      });

      console.log('Arquivo enviado com sucesso:', response.data);
      if (!response.data.sucesso) {
        alert('Erro ao enviar arquivo:' + response.data.message);
      }
     
      setDocumentos([...documentos,  response.data.data]);

      setShowModal(false);

    } catch (error) {
      console.error('Erro ao enviar o arquivo:', error);
      alert('Erro ao enviar o arquivo.');
    }
  };
  const handleDownload = async (documento) => {
    try {
      const response = await axios.get(`${apiUrl}/api/Documento/download/${documento.documentoID}`, {
        responseType: 'blob', // Esperando um blob para download
      });
  
      // Criar um link para download
      const url = window.URL.createObjectURL(new Blob([response.data]));
  
      // Tentar obter a extensão do arquivo a partir do Content-Type
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
      } else {
        extension = ''; // Ou defina um padrão, como .txt
      }
  
      const fileName = documento.fileName ? `${documento.fileName}${extension}` : `arquivo${extension}`;
      
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', fileName); // Use o nome do arquivo determinado
      document.body.appendChild(link);
      link.click();
      link.remove();
  
      console.log('Arquivo baixado com sucesso');
    } catch (error) {
      console.error('Erro ao baixar o arquivo:', error);
      alert('Erro ao baixar o arquivo.');
    }
  };
  
  const formatData = (dateString) => {
    if (!dateString) return '';
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Mês começa do 0
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
              style={{ marginLeft: 'auto' }} // Botão à direita
              onClick={() => setShowModal(true)}
            >
              <FontAwesomeIcon icon={faPlus} /> Adicionar Documento
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
              {documentos.length === 0 ? (
                <tr>
                  <td colSpan="4" style={{ textAlign: 'center' }}>Nenhum Documento Encontrado</td>
                </tr>
              ) : (
                documentos.map((documento) => (
                  <tr key={documento?.documentoID || 'default-key'}>
                    <td>{documento?.documentoID || 'N/A'}</td>
                    <td>{documento?.descricao || 'N/A'}</td>
                    <td>{formatData(documento?.dataAssociada)}</td>
                    <td className="historico-acao">
                      <button onClick={() => handleDownload(documento)} className="historico-botao-download" >
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
              <Modal.Title>Adicionar Documento</Modal.Title>
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
                  style={{ display: 'none' }} // Esconder o input de file nativo
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
              <Button variant="primary" onClick={handleAddDocumentos}>
                Adicionar
              </Button>
            </Modal.Footer>
          </Modal>
        </div>
      </div>
    </div>
  );
};

export default Documentos;
