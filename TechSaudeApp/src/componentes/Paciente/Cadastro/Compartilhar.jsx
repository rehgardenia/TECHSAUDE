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
          <h1>Compartilhamento de Informações Médicas – TechSaúde Última</h1>
          <p>Última atualização: [10/10/2004]</p>
          <p>
            Na TechSaúde, levamos a sua privacidade a sério. Esta Política de
            Privacidade descreve como coletamos, usamos, armazenamos e
            compartilhamos suas informações pessoais ao utilizar nossa
            plataforma. Ao acessar ou utilizar nossos serviços, você concorda
            com os termos desta política.
          </p>
          <h2>1. Consentimento para Compartilhamento </h2>
          <p>
            Ao utilizar os serviços da TechSaúde, o usuário concorda que as
            informações médicas inseridas na plataforma poderão ser
            compartilhadas com profissionais de saúde devidamente cadastrados,
            conforme necessário para o adequado tratamento, diagnóstico e
            monitoramento do paciente. O compartilhamento será realizado sempre
            com o consentimento explícito do usuário, em conformidade com a
            legislação vigente.
          </p>

          <h2>2. Finalidade do Compartilhamento</h2>
          <p>
            As informações médicas dos usuários são compartilhadas
            exclusivamente para os seguintes fins:
            <ul>
              <li>Diagnóstico e tratamento médico;</li>
              <li>Emissão de laudos e prescrições;</li>
              <li>
                Monitoramento contínuo da saúde do paciente por médicos,
                enfermeiros ou outros profissionais de saúde habilitados;
              </li>
              <li>
                Consultas e orientações realizadas de forma remota ou
                presencial.
              </li>
            </ul>
          </p>

          <h2>3. Segurança e Confidencialidade</h2>
          <p>
            TechSaúde compromete-se a garantir a segurança e a confidencialidade
            das informações médicas dos usuários. As informações serão
            protegidas por sistemas de criptografia e outros mecanismos de
            segurança, seguindo padrões da indústria. Apenas profissionais
            autorizados e previamente designados pelo usuário terão acesso a
            essas informações.
          </p>

          <h2>4. Compartilhamento com Terceiros</h2>
          <p>
            TechSaúde não compartilha informações médicas dos usuários com
            terceiros, exceto:
            <ul>
              <li>Quando exigido por lei ou por determinação judicial;</li>
              <li>
                Para proteger os direitos, a privacidade ou a segurança do
                usuário ou de terceiros;
              </li>
              <li>
                No caso de transferência de dados médicos a outro profissional
                de saúde, mediante autorização expressa do usuário.
              </li>
            </ul>
          </p>

          <h2>5. Direito de Acesso e Controle</h2>
          <p>
            O usuário tem o direito de controlar o compartilhamento de suas
            informações médicas. A qualquer momento, o usuário poderá: Revogar o
            consentimento para o compartilhamento de dados com determinados
            profissionais; Solicitar a remoção ou a correção de informações
            médicas incorretas; Obter uma cópia de suas informações de saúde
            mantidas na plataforma.
          </p>
          <h2> 6. Transferência Internacional de Dados</h2>
          <p>
            Em situações específicas, as informações médicas dos usuários
            poderão ser transferidas para servidores localizados fora do Brasil.
            Nesses casos, TechSaúde garantirá que as leis de proteção de dados
            pessoais do país de destino estejam de acordo com os padrões
            exigidos pela Lei Geral de Proteção de Dados (LGPD) e que a
            privacidade do usuário continue protegida.
          </p>
          <h2> 7. Exclusão de Informações Médicas </h2>
          <p>
            O usuário pode solicitar a exclusão de suas informações médicas da
            plataforma a qualquer momento. TechSaúde compromete-se a excluir
            todos os dados do usuário de seus servidores, exceto quando houver
            obrigação legal de armazenamento. Após a exclusão, não será possível
            restaurar essas informações.
          </p>
        </div>
      </div>
    </div>
  );
};

export default CadastroPaciente;
