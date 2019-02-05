using Core.Infra.Data.Entidade;
using System;
using System.Collections.Generic;

namespace Core.Infra.Data.Negocio.Interface
{
    public interface ITabelaServico : IDisposable
    {
        IEnumerable<Tabela> ObterTodasTabelaOracle();
        IEnumerable<Tabela> ObterPropriedadeTabelaOracle(string NomeTabela);
        Tabela ObterPrimaryKeyTabelaOracle(string NomeColuna, string NomeTabela);
        string CriandoClasseEntidade(string NomeTabela, string NameSpace, string NameSchema);
        string CriandoClasseMapeamentoEntidade(string NomeTabela, string NameSpace, string NameSchema);
        string CriandoInterfaces(string NomeTabela, string NameSpace, string NameSchema);
        string CriandoClasseDao(string NomeTabela, string NameSpace, string NameSchema);
        string CriandoClasseInterfacesServico(string NomeTabela, string NameSpace, string NameSchema);
        string CriandoClasseServico(string NomeTabela, string NameSpace, string NameSchema);
        string CriaDiretorios(string NameSpace);
    }
}
