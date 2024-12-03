const validarNome = (nome) => /^[A-Za-zÀ-ÖØ-ÿ\s]+$/.test(nome);

const validarEmail = (email) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

const validarSenhas = (senha, confirmaSenha) => senha === confirmaSenha;

const validarDataNascimento = (dataNascimento) => {
  const regex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
  if (!regex.test(dataNascimento)) return false;
  const [dia, mes, ano] = dataNascimento.split('/').map(Number);
  if (mes < 1 || mes > 12) return false;
  const diasNoMes = new Date(ano, mes, 0).getDate();
  if (dia < 1 || dia > diasNoMes) return false;
  if (ano < 1900 || ano > new Date().getFullYear()) return false;
  const dataObj = new Date(ano, mes - 1, dia);
  return (
    dataObj.getFullYear() === ano &&
    dataObj.getMonth() === mes - 1 &&
    dataObj.getDate() === dia
  );
};

const validarCns = (cns) => /^\d{15}$/.test(cns);

const validarTelefone = (telefone) => {
  const regex = /^\(?([0-9]{2})\)?[-. ]?([0-9]{5})[-. ]?([0-9]{4})$/;
  if (!regex.test(telefone)) return false;
  const formattedTelefone = telefone.replace(regex, "($1) $2-$3");
  return formattedTelefone;
};

export {
  validarNome,
  validarEmail,
  validarSenhas,
  validarDataNascimento,
  validarCns,
  validarTelefone,
};