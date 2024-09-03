using Questao5.Domain.Entities.Base;
using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class Movimento : Entity
    {
        public override string Id
        {
            get => IdMovimento;
            set => IdMovimento = value;
        }
        public string IdMovimento { get; private set; }
        public string IdContaCorrente { get; private set; }
        public DateTime DataMovimento { get; private set; }
        public TipoMovimento TipoMovimento { get; private set; }
        public decimal Valor { get; private set; }

        public Movimento(string idContaCorrente, TipoMovimento tipoMovimento, decimal valor)
        {
            IdContaCorrente = idContaCorrente;
            DataMovimento = DateTime.Now;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }

        public Movimento() { }
    }
}
