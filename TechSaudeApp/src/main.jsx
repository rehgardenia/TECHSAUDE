import React, { Suspense, lazy } from "react";
import { createRoot } from "react-dom/client";
import {
  BrowserRouter as Router,
  Route,
  Routes,
  Navigate,
} from "react-router-dom";
import "./index.css";

// Lazy loading components
const Login = lazy(() => import("./componentes/Login"));
const CadastroPaciente = lazy(() =>
  import("./componentes/Paciente/Cadastro/CadastroPaciente")
);
const TermosCondicoes = lazy(() =>
  import("./componentes/Paciente/Cadastro/TermosCondicoes")
);
const Privacidade = lazy(() =>
  import("./componentes/Paciente/Cadastro/Privacidade")
);
const Compartilhar = lazy(() =>
  import("./componentes/Paciente/Cadastro/Compartilhar")
);
const Exames = lazy(() => import("./componentes/Paciente/Historico/Exames"));
const Receituario = lazy(() =>
  import("./componentes/Paciente/Historico/Receituario")
);
const Documentos = lazy(() =>
  import("./componentes/Paciente/Historico/Documentos")
);
const Agendamento = lazy(() =>
  import("./componentes/Paciente/Consultas/Agendamento")
);
const Agendadas = lazy(() =>
  import("./componentes/Paciente/Consultas/Agendadas")
);
const Historico = lazy(() =>
  import("./componentes/Paciente/Consultas/Historico")
);
const Vacinas = lazy(() =>
  import("./componentes/Paciente/Vacinas_Alergias/Vacinas")
);
const Alergias = lazy(() =>
  import("./componentes/Paciente/Vacinas_Alergias/Alergias")
);
const Perfil = lazy(() => import("./componentes/Paciente/Perfil/Perfil"));
const Crud = lazy(() => import("./componentes/ADM/Crud"));

const root = createRoot(document.getElementById("root"));

// Componente ErrorBoundary para capturar erros
class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true };
  }

  componentDidCatch(error, errorInfo) {
    console.error("Erro capturado: ", error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return <h1>Algo deu errado.</h1>;
    }

    return this.props.children; 
  }
}

root.render(
  <React.StrictMode>
    <Router>
      <Suspense fallback={<div>Loading...</div>}>
        <ErrorBoundary>
          <Routes>
            <Route path="/" element={<Navigate to="/login" />} />
            <Route path="/login" element={<Login />} />
            <Route path="/cadastro" element={<CadastroPaciente />} />
            <Route path="/termos-e-condicoes" element={<TermosCondicoes />} />
            <Route path="/privacidade" element={<Privacidade />} />
            <Route path="/compartilhar" element={<Compartilhar />} />
            <Route path="/exames" element={<Exames />} />
            <Route path="/receituario" element={<Receituario />} />
            <Route path="/documentos" element={<Documentos />} />
            <Route path="/agendamento" element={<Agendamento />} />
            <Route path="/agendadas" element={<Agendadas />} />
            <Route path="/historico" element={<Historico />} />
            <Route path="/vacinas" element={<Vacinas />} />
            <Route path="/alergias" element={<Alergias />} />
            <Route path="/perfil" element={<Perfil />} />
            <Route path="/crud" element={<Crud />} />
          </Routes>
        </ErrorBoundary>
      </Suspense>
    </Router>
  </React.StrictMode>
);
