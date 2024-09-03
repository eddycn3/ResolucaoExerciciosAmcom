using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class GetSaldoContaQuery : IRequest<SaldoContaResponse>
    {
        public string ContaCorrenteId { get; private set; }

        public GetSaldoContaQuery(string contaCorrenteId)
        {
            ContaCorrenteId = contaCorrenteId;
        }
    }
}
