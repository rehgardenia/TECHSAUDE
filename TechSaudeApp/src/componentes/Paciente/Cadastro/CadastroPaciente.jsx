import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";
import "@fortawesome/fontawesome-free/css/all.min.css";
import "./CadastroPaciente.css";

const CadastroPaciente = () => {
  const apiUrl = import.meta.env.VITE_API_URL;
  const navigate = useNavigate();

  
  const [formData, setFormData] = useState({
    nomeCompleto: "",
    email: "",
    senha: "",
    confirmarSenha: "",
    dataNascimento: "",
    cns: "",
    telefone: "",
    termo: false,
    compartilhamento: false,
  });

  const [erros, setErros] = useState({});
  const [loading, setLoading] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);


  const [showSenhaMessage, setShowSenhaMessage] = useState(false);
  const [buttonEnabled, setButtonEnabled] = useState(false);

  useEffect(() => {
    const camposValidos = formData.nomeCompleto && formData.email && formData.senha &&
      formData.confirmarSenha === formData.senha && formData.dataNascimento && formData.cns && 
      formData.telefone && formData.termo && formData.compartilhamento;

    setButtonEnabled(camposValidos);
  }, [formData]); 


  // Função para validar a data no formato dd/mm/yyyy
  const validarDataNascimento = (dataNascimento) => {
    const regex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
    if (!regex.test(dataNascimento)) return false;
    const [dia, mes, ano] = dataNascimento.split("/").map(Number);
    if (mes < 1 || mes > 12) return false;

    // Validação dos dias
    const diasNoMes = new Date(ano, mes, 0).getDate();
    if (dia < 1 || dia > diasNoMes) return false;

    // Validação dos anos (ajuste o intervalo conforme necessário)
    if (ano < 1900 || ano > new Date().getFullYear()) return false;
    const dataObj = new Date(ano, mes - 1, dia);
    return (
      dataObj.getFullYear() === ano &&
      dataObj.getMonth() === mes - 1 &&
      dataObj.getDate() === dia
    );
  };

  //data de nascimento handle
  const handleInputChange = (e) => {
    const valor = e.target.value;
    const valorFormato = valor.replace(/(\d{2})(\d{2})(\d{4})/, "$1/$2/$3");
    setFormData((prevFormData) => ({
        ...prevFormData,
        dataNascimento: valorFormato,
      }))
    };

  // Funções de validação
  const validarNome = (nomeCompleto) =>
    /^[A-Za-zÀ-ÖØ-ÿ\s]+$/.test(nomeCompleto);
  const validarEmail = (email) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

  const validarSenhas = () => {
    return formData.senha === formData.confirmarSenha;
  };
  const validarConfirmaSenha = (senha,confirmarSenha) => {
    if (senha !== confirmarSenha) {
      return false;
    }
    return true;
  };

  const validarCns = (cns) => /^\d{15}$/.test(cns);

  const validarTelefone = (telefone) => {
    const regex = /^\(?([0-9]{2})\)?[-. ]?([0-9]{5})[-. ]?([0-9]{4})$/;
    if (!regex.test(telefone)) {
      return telefone; // retorna o telefone original se não estiver no formato correto
    }
    const formattedTelefone = telefone.replace(regex, "($1) $2-$3");
    return formattedTelefone;
  };
const validarCampos = (e) =>{
  const novosErros = {};
  if (!formData.nomeCompleto) novosErros.nome = "Nome é obrigatório.";
  if (!formData.email) novosErros.email = "Email é obrigatório.";
  if (!formData.senha) novosErros.senha = "Senha é obrigatória.";
  if (formData.senha !== formData.confirmarSenha) {
    novosErros.confirmarSenha = "As senhas não correspondem.";
  }
  if (!formData.dataNascimento) novosErros.dataNascimento = "Data de nascimento é obrigatória.";
  if (!formData.cns) novosErros.cns = "CNS é obrigatório.";
  if (!formData.telefone) novosErros.telefone = "Telefone é obrigatório.";
  if (!formData.termo) novosErros.termo = "Você deve aceitar os termos de uso.";
  if (!formData.compartilhamento) novosErros.termo = "Você deve aceitar os termos de compartilhamento.";

  setErros(novosErros);
  return Object.keys(novosErros).length === 0;
};

const handleSubmit = async (e) => {
    e.preventDefault();
  
    if (isSubmitting) return;
  
    const errosValidacao = {};
    // Validações
    if (!validarNome(formData.nomeCompleto)) 
      errosValidacao.nomeCompleto = "O nome não deve conter símbolos ou números.";
    
    if (!validarEmail(formData.email)) 
      errosValidacao.email = "O e-mail fornecido é inválido.";
    
    if (!validarSenhas(formData.senha)) 
      errosValidacao.senha = "A senha deve ser forte: símbolo, número e letra maiúscula.";
  
    if (!validarConfirmaSenha(formData.senha, formData.confirmarSenha)) 
      errosValidacao.confirmarSenha = "As senhas não conferem.";
    
    if (!validarDataNascimento(formData.dataNascimento)) 
      errosValidacao.dataNascimento = "A data deve ser válida.";
    
    if (!validarCns(formData.cns)) 
      errosValidacao.cns = "O CNS deve ter 15 dígitos.";
    
    if (!validarTelefone(formData.telefone)) 
      errosValidacao.telefone = "O número de telefone precisa ter 11 números.";
  
    // Atualiza os erros
    setErros(errosValidacao);

    const  isValid = validarCampos();
    if (isValid && Object.keys(errosValidacao).length === 0){
      setLoading(true);
      setIsSubmitting(true);
  
      try {
        const response = await axios.post(`${apiUrl}/api/Pacientes`, formData, {
          headers: {
            'Content-Type': 'application/json'
          }
        });
        alert(response.data.message);
        if (response.data.sucesso) {
         console.log(response);
          navigate("/login");
        }
      } catch (error) {
        console.error('Erro ao enviar dados:', error);
        alert('Ocorreu um erro ao enviar os dados. Por favor, tente novamente.');
      } finally {
        setLoading(false);
        setIsSubmitting(false);
      }
    }

  };
  
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prevFormData) => ({
      ...prevFormData,
      [name]: type === "checkbox" ? checked : value,
    }));
  };


  return (
    <main className="cadastro-container">
      <section className="cadastro-card">
        <header className="text-center mb-4">
          <div>
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => navigate("/")}
            ></button>
            <h1 className="text-center mb-4">Cadastro Paciente</h1>
          </div>
        </header>
        <form onSubmit={handleSubmit}>
          <div className="row">
            <div className="col-md-6">
              {/* Primeira Coluna */}
              <article className="mb-3">
                <label
                  htmlFor="nomeCompleto"
                  className="form-label"
                  aria-label="Nome Completo"
                >
                  Nome Completo
                </label>
                <input
                  required
                  title="Por favor, preencha a sua senha."
                  type="text"
                  id="nomeCompleto"
                  placeholder="Nome Completo"
                  className="form-control"
                  value={formData.nomeCompleto}
                  onChange={(e) =>
                    setFormData((prevFormData) => ({
                      ...prevFormData,
                      nomeCompleto: e.target.value,
                    }))
                  }
                  aria-label="Insira o nome do paciente"
                  aria-required="true"
                  
                />
                  {erros.nomeCompleto && <p className="text-danger">{erros.nomeCompleto}</p>}
              </article>
              <article className="mb-3">
                <label htmlFor="email" className="form-label">
                  E-mail
                </label>
                <input
                  type="email"
                  id="email"
                  placeholder="nome@email.com"
                  className="form-control"
                  value={formData.email}
                  required
                  onChange={(e) =>
                    setFormData((prevFormData) => ({
                      ...prevFormData,
                      email: e.target.value,
                    }))
                  }
                  aria-label="Insira o e-mail do paciente"
                  
                />
                {erros.email && <p className="text-danger">{erros.email}</p>}
              </article>
              <article className="mb-3">
                <label
                  htmlFor="senha"
                  className="form-label"
                  aria-label="Senha"
                >
                  Senha
                </label>
                <div className="input-container">
                  <input
                    type="password"
                    id="senha"
                    placeholder="Digite sua senha"
                    className="form-control"
                    value={formData.senha}
                    required
                    onChange={(e) =>
                      setFormData((prevFormData) => ({
                        ...prevFormData,
                        senha: e.target.value,
                      }))
                    }
                    aria-label="Insira a senha do paciente"
                    aria-required="true"
                    
                  />
                  {/* <div className="vazio">
                    <label className="checkbox-label">
                      <input
                        type="checkbox"
                        id="showSenha1"
                        name="check"
                        value="checkbox"
                        hidden
                        onClick={(e) => {
                          if (e.target.checked) {
                            document.getElementById("senha").type = "text";
                          } else {
                            document.getElementById("senha").type = "password";
                          }
                        }}
                      />
                      <img src={"olhafechado.jpg"} alt="Checkbox image" />
                    </label>
                  </div> */}
                </div>
                <ul className="senha-requisitos">
                  <li>
                    {validarSenhas(formData.senha) ? (
                      <i className="fas fa-check text-sucess"></i>
                    ) : (
                      <i className="fas fa-times text-danger"></i>
                    )}
                    <span>As senhas devem ser iguais.</span>
                  </li>
                  <li>
                    {formData.senha.match(/[A-Z]/) ? (
                      <i className="fas fa-check text-success"></i>
                    ) : (
                      <i className="fas fa-times text-danger"></i>
                    )}
                    <span>Deve conter uma letra maiúscula.</span>
                  </li>
                  <li>
                    {formData.senha.match(/[0-9]/) ? (
                      <i className="fas fa-check text-success"></i>
                    ) : (
                      <i className="fas fa-times text-danger"></i>
                    )}
                    <span>Deve conter um número.</span>
                  </li>
                  <li>
                    {formData.senha.match(/[^a-zA-Z0-9]/) ? (
                      <i className="fas fa-check text-success"></i>
                    ) : (
                      <i className="fas fa-times text-danger"></i>
                    )}
                    <span>Deve conter um caractere especial.</span>
                  </li>
                </ul>
                {erros.senha && <p className="text-danger">{erros.senha}</p>}
              </article>
              <article className="mb-3">
                <label htmlFor="confirmaSenha" className="form-label">
                  Confirmar Senha
                </label>
                <div className="input-container">
                  <input
                    type="password"
                    id="confirmarSenha"
                    placeholder="Confirme sua senha"
                    className="form-control"
                    value={formData.confirmarSenha}
                    required
                    onChange={(e) =>
                      setFormData((prevFormData) => ({
                        ...prevFormData,
                        confirmarSenha: e.target.value,
                      }))
                    }
                    onFocus={() => setShowSenhaMessage(true)}
                    
                  />
                  {/* <div className="vazio">
                    <label className="checkbox-label">
                      <input
                        type="checkbox"
                        id="showSenha2"
                        name="check"
                        value="checkbox"
                        hidden
                        onClick={(e) => {
                          if (e.target.checked) {
                            document.getElementById("confirmarSenha").type =
                              "text";
                          } else {
                            document.getElementById("confirmarSenha").type =
                              "password";
                          }
                        }}
                      />
                      <img src={"olhafechado.jpg"} alt="Checkbox image" />
                    </label>
                  </div> */}
                </div>
                {showSenhaMessage && // Adicionado condição para mostrar a mensagem
                  (formData.confirmarSenha === formData.senha ? (
                    <p className="text-success">Senhas conferem!</p>
                  ) : (
                    <p className="text-danger">Senhas não conferem.</p>
                  ))}
              </article>
            </div>
            <div className="col-md-6">
              {/* Segunda Coluna */}
              <article className="mb-3">
                <label htmlFor="dataNascimento" className="form-label">
                  Data de Nascimento
                </label>
                <input
                  type="text"
                  id="dataNascimento"
                  placeholder="DD/MM/YYYY"
                  className="form-control"
                  value={formData.dataNascimento}
                  required
                  onChange={handleInputChange}
                  aria-label="Data de nascimento"
                  
                />
                {erros.dataNascimento && (
                  <p className="text-danger">{erros.dataNascimento}</p>
                )}
              </article>
              <article className="mb-3">
                <label htmlFor="cns" className="form-label">
                  CNS
                </label>
                <input
                  type="text"
                  maxLength={15}
                  id="cns"
                  placeholder="Digite seu CNS (15 dígitos)"
                  className="form-control"
                  value={formData.cns}
                  required
                  onChange={(e) =>
                    setFormData((prevFormData) => ({
                      ...prevFormData,
                      cns: e.target.value,
                    }))
                  }
                  
                />
                {erros.cns && <p className="text-danger">{erros.cns}</p>}
              </article>
              <article className="mb-3">
                <label htmlFor="telefone" className="form-label">
                  Telefone
                </label>
                <input
                  type="tel"
                  id="telefone"
                  placeholder="(00) 00000-0000"
                  className="form-control"
                  value={formData.telefone}
                  required
                  onChange={(e) => {
                    const telefoneFormatado = validarTelefone(e.target.value);
                      setFormData((prevFormData) => ({
                        ...prevFormData,
                        telefone: telefoneFormatado,
                      }))
                    
                  }}
                  
                />
                {erros.telefone && (
                  <p className="text-danger">{erros.telefone}</p>
                )}
              </article>
              <div className="row">
                <div className="col-md-12">
                  <article className="mb-3">
                    <label className="form-label">
                      <div className="form-check">
                        <input
                           type="checkbox"
                           id="termo"
                           name="termo"
                           className="form-check-input"
                           checked={formData.termo}
                           onChange={handleChange}
                           required
                          aria-label="Concordo com o Termo de Uso"
                        />
                        <label className="form-check-label" htmlFor="termo">
                          Eu li e concordo com os{" "}
                          <a
                            href="/termos-e-condicoes"
                            style={{ color: "var(--secondary-color)" }}
                          >
                            Termos e Condições
                          </a>{" "}
                          e a{" "}
                          <a
                            href="./privacidade"
                            style={{ color: "var(--secondary-color)" }}
                          >
                            Política de Privacidade.
                          </a>
                        </label>
                      </div>
                    </label>
                    {erros.termo && <p className="text-danger">{erros.termo}</p>}
                  </article>
                  <article className="mb-3">
                    <label className="form-label">
                      <div className="form-check">
                        <input
                          type="checkbox"
                          id="compartilhamento"
                          name="compartilhamento"
                          className="form-check-input"
                          checked={formData.compartilhamento}
                          onChange={handleChange}
                          required
                          aria-label="Autorizo o compartilhamento de meus dados"
                          
                        />
                        <label
                          className="form-check-label"
                          htmlFor="compartilhamento"
                        >
                          Eu permito e concordo com o {""}
                          <a
                            href="./compartilhar"
                            style={{ color: "var(--secondary-color)" }}
                          >
                            Compartilhamento de Informações Médicas
                          </a>{" "}
                          para o aplicativo e os profissionais da saúde.
                        </label>
                      </div>
                    </label>
                    {erros.compartilhamento && <p className="text-danger">{erros.compartilhamento}</p>}
                  </article>
                 

                </div>
              </div>
            </div>
          </div>
          <footer>
          <button type="submit" className="btn btn-primary" disabled={!buttonEnabled || isSubmitting}>
              {loading ? "Enviando..." : "Cadastrar"}
            </button>
            {/* <button
              type="submit"
              className="btn btn-primary"
              disabled={!buttonEnabled}
              aria-label="Cadastrar paciente"
              onClick={handleSubmit}
            >
              Enviar Dados
    
            </button> */}
          </footer>
        </form>
      </section>
    </main>
  );
};
export default CadastroPaciente;
