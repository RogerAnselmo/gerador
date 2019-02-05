using Core.Infra.Data.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Core.Infra.Data.Repositorio
{
    public class Repositorio<TEntidade> : IRepositorio<TEntidade> where TEntidade : class
    {
        protected  Connection.Connection _Conexao;
        private DbConnection _Contexto;
        
        public Repositorio()
        {
            _Conexao = new Connection.Connection();
        }

        public IEnumerable<TEntidade> Obter(string sqlString, object objectParams)
        {
            using (_Contexto = _Conexao.GetOpenConnection())
            {
                return _Contexto.Query<TEntidade>(sqlString, objectParams, commandType: CommandType.Text);
            }
        }

        private int GenericoDapper(string sqlString, object objectParams, out string mensagem)
        {
            try
            {
                using (_Contexto = _Conexao.GetOpenConnection())
                {
                    _Contexto.Execute(sqlString, objectParams, commandType: CommandType.Text);
                    mensagem = null;
                    return 0;
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
                return 1;
            }
            finally
            {
                _Conexao.Desconection();
            }
        }
    }
}
