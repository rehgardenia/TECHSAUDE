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
          <h1>Política de Privacidade – TechSaúde Última</h1>
          <p>Última atualização: [10/10/2004]</p>
          <p>
            Na TechSaúde, levamos a sua privacidade a sério. Esta Política de
            Privacidade descreve como coletamos, usamos, armazenamos e
            compartilhamos suas informações pessoais ao utilizar nossa
            plataforma. Ao acessar ou utilizar nossos serviços, você concorda
            com os termos desta política.
          </p>

          <h2>1. Coleta de Informações</h2>
          <ul>
            <li>
              <strong>1.1. Informações Fornecidas pelo Usuário:</strong> Ao se
              cadastrar na TechSaúde, coletamos as seguintes informações: Dados
              pessoais: nome, CPF, e-mail, endereço, telefone. Informações de
              saúde: histórico médico, diagnósticos, prescrições, resultados de
              exames.
            </li>
            <li>
              <strong>1.2. Informações Coletadas Automaticamente: </strong>{" "}
              Quando o usuário interage com nossa plataforma, podemos coletar
              automaticamente: Dados de navegação: endereço IP, tipo de
              dispositivo, sistema operacional, navegador. Dados de uso: páginas
              visitadas, tempo de navegação, cliques e interações com o site.
            </li>
          </ul>

          <h2>
            2. Uso das Informações As informações coletadas pela TechSaúde são
            utilizadas para:{" "}
          </h2>
          <ul>
            <li>Fornecer e melhorar os serviços oferecidos pela plataforma;</li>
            <li>
              Personalizar a experiência do usuário, recomendando conteúdos
              relevantes;
            </li>
            <li>
              Gerenciar consultas, tratamentos e outras interações com
              profissionais de saúde;
            </li>
            <li>
              Enviar notificações e comunicações relacionadas aos serviços (ex.:
              lembretes de consultas, atualizações de saúde);
            </li>
            <li>
              Processar pagamentos e garantir a segurança de transações
              financeiras;
            </li>
            <li>
              Cumprir obrigações legais e regulatórias, quando necessário.
            </li>
          </ul>

          <h2>3. Compartilhamento de Informações</h2>
          <ul>
            <li>
              <strong>3.1. Compartilhamento com Terceiros:</strong> A TechSaúde
              pode compartilhar informações pessoais com terceiros nas seguintes
              circunstâncias: Com profissionais de saúde autorizados pelo
              usuário, para fins de diagnóstico, tratamento e acompanhamento
              médico. Com prestadores de serviços contratados (ex.: empresas de
              hospedagem de dados, sistemas de pagamento) que auxiliam na
              operação da plataforma. Quando exigido por lei, ordem judicial ou
              outras solicitações governamentais.
            </li>
            <li>
              <strong>3.2. Proteção de Dados:</strong> Todos os terceiros com
              quem compartilhamos dados são obrigados a seguir os mesmos padrões
              de proteção de dados aplicados pela TechSaúde e não podem utilizar
              essas informações para finalidades diferentes das aqui descritas.
            </li>
          </ul>

          <h2>4. Segurança das Informações.</h2>
          <p>
            A segurança de suas informações pessoais é prioridade para a
            TechSaúde. Implementamos medidas técnicas e organizacionais
            adequadas, incluindo:
            <ul>
              <li>
                Criptografia de dados sensíveis, tanto em repouso quanto em
                trânsito.
              </li>
              <li>
                Controles de acesso restritos a funcionários e prestadores de
                serviços.
              </li>
              <li>
                {" "}
                Monitoramento constante para identificar e mitigar potenciais
                riscos de segurança.
              </li>
            </ul>
            No entanto, nenhum sistema é completamente seguro. Embora façamos o
            possível para proteger suas informações, não podemos garantir a
            segurança absoluta de todos os dados transmitidos através da
            internet.
          </p>

          <h2>5. Retenção de Dados</h2>
          <p>
            As informações pessoais serão mantidas apenas pelo tempo necessário
            para cumprir as finalidades descritas nesta Política de Privacidade,
            exceto quando um período de retenção mais longo for exigido ou
            permitido por lei. Após esse período, os dados serão excluídos ou
            anonimizados de forma irreversível.
          </p>

          <h2>6. Direitos dos Usuários.</h2>
          <p>
            Você, como usuário da TechSaúde, tem os seguintes direitos em
            relação às suas informações pessoais:
            <ul>
              <li>Solicitar uma cópia das informações que temos sobre você.</li>
              <li>
                Correção: Corrigir dados pessoais incorretos ou desatualizados.
              </li>
              <li>
                Exclusão: Solicitar a exclusão de suas informações pessoais,
                quando aplicável.
              </li>
              <li>
                Restrição de Processamento: Solicitar a limitação do uso de seus
                dados em determinadas situações.
              </li>
              <li>
                Portabilidade: Solicitar a transferência de suas informações
                pessoais para outra plataforma ou serviço.
              </li>
            </ul>
            Para exercer qualquer um desses direitos, entre em contato conosco
            através dos canais fornecidos no final desta política.
          </p>

          <h2>7. Cookies e Tecnologias de Rastreamento</h2>
          <p>
            TechSaúde utiliza cookies e tecnologias similares para melhorar a
            experiência do usuário na plataforma. Cookies são pequenos arquivos
            armazenados no seu dispositivo que ajudam a personalizar o conteúdo
            e lembrar suas preferências. Tipos de cookies que utilizamos:
            Cookies essenciais: Necessários para o funcionamento básico da
            plataforma. Cookies de desempenho: Coletam informações sobre como os
            usuários interagem com o site, permitindo melhorias na experiência.
            Cookies de marketing: Utilizados para mostrar anúncios relevantes,
            com base em suas preferências de navegação. Você pode controlar o
            uso de cookies através das configurações do seu navegador, no
            entanto, algumas funcionalidades da plataforma podem ser impactadas
            caso opte por desativá-los.
          </p>

          <h2>8. Alterações nesta Política</h2>
          <p>
            TechSaúde reserva-se o direito de modificar esta Política de
            Privacidade a qualquer momento. Qualquer alteração significativa
            será comunicada aos usuários através da plataforma ou por e-mail. O
            uso contínuo dos nossos serviços após a modificação implica
            aceitação da nova versão da política.
          </p>

          <h2>9. Contato</h2>
          <p>
            Se tiver dúvidas ou preocupações sobre esta Política de Privacidade
            ou sobre o tratamento de suas informações pessoais, entre em contato
            conosco: E-mail: techsaude@gmail.com Endereço: [Endereço físico, se
            aplicável]
          </p>
        </div>
      </div>
    </div>
  );
};

export default CadastroPaciente;
