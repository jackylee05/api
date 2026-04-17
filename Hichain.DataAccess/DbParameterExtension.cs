using System.Data.Common;
using System.Linq;

namespace Hichain.DataAccess
{
    public static class DbParameterExtension
    {
        /// <summary>
        /// Normalize parameter array: return empty array when null and filter out null entries.
        /// </summary>
        public static DbParameter[] ToDbParameter(params DbParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return System.Array.Empty<DbParameter>();

            return parameters.Where(p => p != null).ToArray()!;
        }
    }
}
