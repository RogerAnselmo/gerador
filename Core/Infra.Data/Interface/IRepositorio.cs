using System.Collections.Generic;

namespace Core.Infra.Data.Interface
{
    public interface IRepositorio<TEntidade> where TEntidade : class
    {
        IEnumerable<TEntidade> Obter(string sqlString, object objectParams);
    }
}
