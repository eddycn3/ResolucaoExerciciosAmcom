using System.Text.Json.Serialization;

namespace Questao5.Configuration
{
    public class ErrorModel
    {
        public string ErrorMessage { get; set; }
        public string TipoErro { get; set; }
    }
}
