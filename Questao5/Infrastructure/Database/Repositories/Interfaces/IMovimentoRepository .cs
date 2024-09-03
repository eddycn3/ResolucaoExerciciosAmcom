using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Database.Repositories.Interfaces
{
    public interface IMovimentoRepository
    {
        Task AddAsync(Movimento movimento);
        Task<decimal> GetSumByTipoAsync(string contaCorrenteId, TipoMovimento dEBITO);
    }
}
