namespace TechSaude.Server.DTO
{
    public class AlergiaSaidaDTO
    {
        public int historicoId { get; set; }
        public int alergiaId { get; set; }
        public string descricao { get; set; } = string.Empty;
        public string medicamento { get; set; } = string.Empty;

    }
    public class DoencaSaidaDTO
    {
        public int historicoId { get; set; }
        public int doencaId { get; set; }
        public string descricao { get; set; } = string.Empty;
        public string medicamento { get; set; } = string.Empty;

    }
    public class VacinaSaidaDTO
    {
        public int historicoId { get; set; }
        public int vacinaId { get; set; }
        public string nome { get; set; } = string.Empty;
        public string unidadeSaude { get; set; } = string.Empty;

        public string lote { get; set;} = string.Empty;
        public DateTime data { get;set;} 

    }
}