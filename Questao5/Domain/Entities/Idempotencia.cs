using Questao5.Domain.Entities.Base;

namespace Questao5.Domain.Entities
{
    public class Idempotencia
    {
        public Guid ChaveIdempotencia { get; private set; }
        public string Requisicao { get; private set; }
        public string Resultado { get; private set; }

        public Idempotencia(Guid chaveIndepotencia, string requisicao, string resultado)
        {
            Requisicao = requisicao;
            Resultado = resultado;
            ChaveIdempotencia = chaveIndepotencia;
        }
        public Idempotencia() { }
    }
}
