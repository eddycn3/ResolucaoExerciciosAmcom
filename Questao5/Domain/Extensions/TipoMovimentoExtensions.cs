using Questao5.Domain.Enumerators;
using Questao5.Domain.Validations;

namespace Questao5.Domain.Extensions
{
    public static class TipoMovimentoExtensions
    {
        private static readonly Dictionary<string, TipoMovimento> StringToEnumMap = new Dictionary<string, TipoMovimento>
        {
            { "C", TipoMovimento.CREDITO },
            { "D", TipoMovimento.DEBITO }
        };

        public static TipoMovimento ToTipoMovimento(this string tipoString)
        {
            if (StringToEnumMap.TryGetValue(tipoString, out var tipo))
            {
                return tipo;
            }
            else
            {
                throw new BusinessException($"Tipo movimento inválido.","INVALID_MOVIMENT_TYPE");
            }
        }

        public static string ToCode(this TipoMovimento tipo)
        {
            return tipo switch
            {
                TipoMovimento.CREDITO => "C",
                TipoMovimento.DEBITO => "D",
                _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, null)
            };
        }
    }




}
