using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Validations;
using Questao5.Infrastructure.Database.Repositories.Interfaces;

namespace Questao5.Application.Handlers
{
    public class GetSaldoContaQueryHandler : IRequestHandler<GetSaldoContaQuery, SaldoContaResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public GetSaldoContaQueryHandler(
            IContaCorrenteRepository contaCorrenteRepository,
            IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<SaldoContaResponse> Handle(GetSaldoContaQuery query, CancellationToken cancellationToken)
        {
            // Valida se a conta existe
            var contaCorrente = await _contaCorrenteRepository.GetByIdAsync(query.ContaCorrenteId);
            if (contaCorrente == null)
                throw new BusinessException("Conta corrente inválida.", "INVALID_ACCOUNT");

            // Valida se esta ativa
            if (!contaCorrente.Ativo)
                throw new BusinessException("Conta corrente inativa.", "INACTIVE_ACCOUNT");

            // Calcula o saldo
            decimal creditos = await _movimentoRepository.GetSumByTipoAsync(query.ContaCorrenteId, TipoMovimento.CREDITO);
            decimal debitos = await _movimentoRepository.GetSumByTipoAsync(query.ContaCorrenteId, TipoMovimento.DEBITO);
            var saldo = creditos - debitos;

            return new SaldoContaResponse(
                contaCorrente.Numero,
                contaCorrente.Nome,
                DateTime.Now,
                saldo
            );
        }
    }

}
