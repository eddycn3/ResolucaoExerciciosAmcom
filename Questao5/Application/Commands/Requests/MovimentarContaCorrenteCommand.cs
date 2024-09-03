using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCommand : IRequest<MovimentarContaResponse>
    {
        public Guid RequestId { get; set; }
        public Guid ContaCorrenteId { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
    }
}
