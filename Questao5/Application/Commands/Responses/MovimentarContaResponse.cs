namespace Questao5.Application.Commands.Responses
{
    public class MovimentarContaResponse
    {
        public string IdMovimentacaoGerada { get; private set; }

        public MovimentarContaResponse(string idMovimentacaoGerada) =>
            IdMovimentacaoGerada = idMovimentacaoGerada;

    }
}
