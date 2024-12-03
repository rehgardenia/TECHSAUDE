namespace TechSaude.Server.Models
{
    public class ServerResponse<T>
    {
        public bool Sucesso { get; set; }
        public string Message { get; set; } = string.Empty;
        //public int StatusCode { get; set; } = 200;
        public T? Data { get; set; }
        public string Token { get; set; } = string.Empty;
    }

}
