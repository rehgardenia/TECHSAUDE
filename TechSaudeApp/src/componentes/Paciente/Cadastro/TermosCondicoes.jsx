import "bootstrap/dist/css/bootstrap.min.css";
import "@fortawesome/fontawesome-free/css/all.min.css";
import { useNavigate } from "react-router-dom";
import "./Links.css";
//import InputMask from 'react-input-mask';

const CadastroPaciente = () => {
  const navigate = useNavigate();
  return (
    <div className="container-fluid bg-secondary py-5">
      <div className="container bg-white p-4 border">
        <div className="header d-flex justify-content-center align-items-center">
        <button
              type="button"
              className="btn btn-secondary"
              onClick={() => navigate("/cadastro")}
            ></button>
          <img
            src="logotech.png"
            alt="Logo"
            className="logo"
            style={{ width: 120 }}
          />
          <h1 className="ml-3">TechSaúde</h1>
        </div>
        <div className="terms-and-conditions container p-4">
          <h1>Termos e Condições de Uso – TechSaúde</h1>
          <p>Última atualização: [10/10/2004]</p>
          <p>
            Bem-vindo ao TechSaúde. Os presentes Termos e Condições regem o uso
            da plataforma TechSaúde, que visa fornecer soluções tecnológicas
            inovadoras para melhorar o acesso e a gestão da saúde.
          </p>

          <h2>1. Aceitação dos Termos</h2>
          <p>
            Ao utilizar a plataforma TechSaúde, o usuário concorda com estes
            Termos e Condições de Uso. Caso não concorde com qualquer um dos
            termos, recomenda-se que o usuário cesse imediatamente o uso da
            plataforma.
          </p>

          <h2>2. Definições</h2>
          <ul>
            <li>
              <strong>Usuário:</strong> Qualquer pessoa que acessa ou utiliza os
              serviços da plataforma TechSaúde.
            </li>
            <li>
              <strong>Serviços:</strong> Soluções tecnológicas voltadas para a
              gestão da saúde oferecidas pela TechSaúde, incluindo aplicativos,
              sistemas de prontuários eletrônicos e demais funcionalidades.
            </li>
            <li>
              <strong>Dados Pessoais:</strong> Informações fornecidas pelos
              usuários, como nome, CPF, dados médicos, entre outros, que são
              tratados pela plataforma.
            </li>
          </ul>

          <h2>3. Uso da Plataforma</h2>
          <p>
            O usuário concorda em utilizar a TechSaúde de maneira lícita, sem
            infringir a legislação vigente e sem praticar atos que possam
            comprometer a segurança e integridade da plataforma.
          </p>
          <ul>
            <li>O usuário não poderá usar a plataforma para:</li>
            <ul>
              <li>
                Transmitir vírus, malware ou qualquer outro código malicioso;
              </li>
              <li>
                Realizar atividades fraudulentas ou que causem prejuízos a
                terceiros;
              </li>
              <li>
                Compartilhar informações médicas de outras pessoas sem
                autorização expressa.
              </li>
            </ul>
          </ul>

          <h2>4. Cadastro de Usuário</h2>
          <p>
            Para utilizar os serviços da TechSaúde, o usuário deve fornecer
            informações verdadeiras, precisas e completas durante o processo de
            cadastro. É de responsabilidade do usuário manter as informações
            atualizadas.
          </p>

          <h2>5. Privacidade e Proteção de Dados</h2>
          <p>
            TechSaúde respeita a privacidade dos seus usuários e compromete-se a
            proteger seus dados pessoais de acordo com a Lei Geral de Proteção
            de Dados (LGPD). Para mais detalhes sobre como suas informações são
            tratadas, consulte nossa Política de Privacidade.
          </p>

          <h2>6. Responsabilidades</h2>
          <p>TechSaúde não se responsabiliza por:</p>
          <ul>
            <li>
              Erros ou falhas de operação dos serviços, incluindo interrupções
              temporárias ou falhas de conexão.
            </li>
            <li>
              Consequências de decisões tomadas pelo usuário com base em
              informações fornecidas pela plataforma.
            </li>
            <li>
              Uso indevido dos dados e informações da plataforma por terceiros
              não autorizados.
            </li>
          </ul>

          <h2>7. Propriedade Intelectual</h2>
          <p>
            Todos os direitos de propriedade intelectual relacionados à
            TechSaúde, incluindo, mas não se limitando a, marcas, logotipos,
            software, design e conteúdo, pertencem ao Grupo composto por Alicia
            Rodrigues, Cibelly Angel, Grasielly Ribeiro, Renata Gardenia e
            Sophia Freire. O uso não autorizado desses direitos é estritamente
            proibido.
          </p>

          <h2>8. Modificações nos Termos</h2>
          <p>
            TechSaúde reserva-se o direito de alterar estes Termos e Condições a
            qualquer momento. As modificações serão comunicadas aos usuários por
            meio da plataforma, e o uso continuado após a alteração implicará
            aceitação dos novos termos.
          </p>

          <h2>9. Legislação Aplicável</h2>
          <p>
            Estes Termos e Condições serão regidos e interpretados de acordo com
            as leis brasileiras. Qualquer disputa que surja em relação ao uso da
            plataforma será resolvida no foro da comarca Cubatão, São Paulo.
          </p>

          <h2>10. Contato</h2>
          <p>
            Caso tenha dúvidas sobre estes Termos e Condições ou precise de
            assistência, entre em contato conosco pelo e-mail: techsaude@gmail.com .
          </p>
        </div>
      </div>
    </div>
  );
};

export default CadastroPaciente;
