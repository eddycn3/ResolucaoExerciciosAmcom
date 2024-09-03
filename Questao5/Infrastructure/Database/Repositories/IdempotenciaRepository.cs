using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Dapper;
using Questao5.Infrastructure.Database.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig _databaseConfig;
        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task AddAsync(Idempotencia novoRegistroIdempotencia)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var sql = @"INSERT INTO Idempotencia (chave_idempotencia, Requisicao, Resultado) 
                    VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";

            var parameters = new
            {
                ChaveIdempotencia = novoRegistroIdempotencia.ChaveIdempotencia.ToString(),
                novoRegistroIdempotencia.Requisicao,
                novoRegistroIdempotencia.Resultado
            };

            await connection.ExecuteAsync(sql, parameters);
        }

        public async Task<Idempotencia> GetByChaveAsync(Guid requestId)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var sql = @"SELECT chave_idempotencia, Requisicao, Resultado 
                    FROM Idempotencia 
                    WHERE chave_idempotencia = @ChaveIdempotencia";

            var parameters = new
            {
                ChaveIdempotencia = requestId.ToString()
            };

            var result = await connection.QueryFirstOrDefaultAsync(sql, parameters);

            if (result == null)
                return null;

            var idempotencia = new Idempotencia(
                Guid.Parse(result.chave_idempotencia.ToString()),
                result.requisicao,
                result.resultado
            );

            return idempotencia;
        }
    }
}
