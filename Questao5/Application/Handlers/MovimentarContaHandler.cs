using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;
using Questao5.Domain.Validations;
using Questao5.Infrastructure.Database.Repositories.Interfaces;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaHandler : IRequestHandler<MovimentarContaCommand, MovimentarContaResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentarContaHandler(
            IContaCorrenteRepository contaCorrenteRepository,
            IMovimentoRepository movimentoRepository,
            IIdempotenciaRepository idempotenciaRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<MovimentarContaResponse> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
        {
            var idempotencia = await _idempotenciaRepository.GetByChaveAsync(request.RequestId);
            if (idempotencia != null)
            {
                // Retorna o mesmo resultado se a requisição já foi processada
                return new MovimentarContaResponse(idempotencia.Resultado);
            }

            var contaCorrente = await _contaCorrenteRepository.GetByIdAsync(request.ContaCorrenteId.ToString());
            if (contaCorrente == null || !contaCorrente.Ativo)
                throw new BusinessException("Conta corrente inválida ou inativa", "INVALID_ACCOUNT");

            if (request.Valor <= 0)
                throw new BusinessException("Valor inválido", "INVALID_VALUE");

            if (request.TipoMovimento.ToTipoMovimento() != TipoMovimento.DEBITO && request.TipoMovimento.ToTipoMovimento() != TipoMovimento.CREDITO)
                throw new BusinessException("Tipo de movimento inválido", "INVALID_TYPE");

            var movimento = new Movimento(
                request.ContaCorrenteId.ToString(),
                request.TipoMovimento.ToTipoMovimento(),
                request.Valor
            );

            await _movimentoRepository.AddAsync(movimento);

            // Salva a idempotência
            var novoRegistroIdempotencia = new Idempotencia(
                request.RequestId,
                JsonConvert.SerializeObject(request),
                movimento.Id
            );
            await _idempotenciaRepository.AddAsync(novoRegistroIdempotencia);

            return new MovimentarContaResponse(movimento.Id);
        }
    }
}
