using Questao5.Domain.Entities.Base;
using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class ContaCorrente : Entity
    {
        public override string Id
        {
            get => IdContaCorrente;
            set => IdContaCorrente = value;
        }

        public string IdContaCorrente { get; private set; }
        public int Numero { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        public ContaCorrente(int numero, string nome, bool ativo)
        {
            Numero = numero;
            Nome = nome;
            Ativo = ativo;
        }

        public ContaCorrente() { }
    }
}
