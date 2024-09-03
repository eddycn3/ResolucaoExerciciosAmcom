using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Dapper;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Sqlite;
using Questao5.Domain.Extensions;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig _databaseConfig;
        public MovimentoRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public async Task AddAsync(Movimento movimento)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var sql = @"INSERT INTO Movimento (IdMovimento, IdContaCorrente, DataMovimento, TipoMovimento, Valor) 
                    VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            var parameters = new
            {
                movimento.IdMovimento,
                IdContaCorrente = movimento.IdContaCorrente.ToUpper(),
                movimento.DataMovimento,
                TipoMovimento = movimento.TipoMovimento.ToCode(),
                movimento.Valor
            };

            await connection.ExecuteAsync(sql, parameters);
        }

        public async Task<decimal> GetSumByTipoAsync(string contaCorrenteId, TipoMovimento tipoMovimento)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var query = @"
            SELECT COALESCE(SUM(Valor), 0)
            FROM Movimento
            WHERE IdContaCorrente = @ContaCorrenteId
            AND TipoMovimento = @TipoMovimento";

            var parameters = new { ContaCorrenteId = contaCorrenteId, TipoMovimento = tipoMovimento.ToCode() };

            return await connection.ExecuteScalarAsync<decimal>(query, parameters);
        }
    }
}
