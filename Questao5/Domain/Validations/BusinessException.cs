namespace Questao5.Domain.Validations
{
    public class BusinessException : Exception
    {
        public string Mensagem { get; private set; }
        public string Tipo { get; private set; }
        public BusinessException(string error) : base(error)
        { }

        public BusinessException(string mensagem, string tipo) 
        {
            Mensagem = mensagem;
            Tipo = tipo;
        }
    }
}
