namespace Core.Helper
{
    public class Diretorio
    {
        public string Infra { get { return SubDiretorioSea.Infra.ToString(); } }
        public string Dao { get { return SubDiretorioSea.Dao.ToString(); } }
        public string Entidades { get { return SubDiretorioSea.Entidades.ToString(); } }
        public string Interfaces { get { return SubDiretorioSea.Interfaces.ToString(); } }
        public string MapeamentoEntidades { get { return SubDiretorioSea.MapeamentoEntidades.ToString(); } }
        public string Repositorio { get { return SubDiretorioSea.Repositorio.ToString(); } }
        public string SQL { get { return SubDiretorioSea.SQL.ToString(); } }
        public string TransferenciaObjeto { get { return SubDiretorioSea.TransferenciaObjeto.ToString(); } }
        public string InjecaoDependencia { get { return SubDiretorioSea.InjecaoDependencia.ToString(); } }
        public string MensagemSistema { get { return SubDiretorioSea.MensagemSistema.ToString(); } }
        public string Negocio { get { return SubDiretorioSea.Negocio.ToString(); } }
        public string Interface { get { return SubDiretorioSea.Interface.ToString(); } }
        public string Servico { get { return SubDiretorioSea.Servico.ToString(); } }
        public string Validacao { get { return SubDiretorioSea.Validacao.ToString(); } }
        public string Conexao { get { return SubDiretorioSea.Conexao.ToString(); } }
        public string Contexto { get { return SubDiretorioSea.Contexto.ToString(); } }
        public string Helper { get { return SubDiretorioSea.Helper.ToString(); } }
        public string Barra { get { return "\\"; } }

        public string DirInfraEntidadeFisico(string NamaSpace)
        {
            return NamaSpace + this.Barra + this.Infra + this.Barra + this.Entidades + this.Barra;
        }

        public string DirInfraEntidadeLogico(string NamaSpace)
        {
            return NamaSpace + "." + this.Infra + "." + this.Entidades;
        }

        public string DirInfraMapeamentoEntidadeFisico(string NamaSpace)
        {
            return NamaSpace + this.Barra + this.Infra + this.Barra + this.MapeamentoEntidades + this.Barra;
        }

        public string DirInfraMapeamentoEntidadeLogico(string NamaSpace)
        {
            return NamaSpace + "." + this.Infra + "." + this.MapeamentoEntidades;
        }

        public string DirInfraInterfacesFisico(string NamaSpace)
        {
            return NamaSpace + this.Barra + this.Infra + this.Barra + this.Interfaces + this.Barra;
        }

        public string DirInfraInterfacesLogico(string NamaSpace)
        {
            return NamaSpace + "." + this.Infra + "." + this.Interfaces;
        }

        public string DirInfraDaoFisico(string NamaSpace)
        {
            return NamaSpace + this.Barra + this.Infra + this.Barra + this.Dao + this.Barra;
        }

        public string DirInfraDaoLogico(string NamaSpace)
        {
            return NamaSpace + "." + this.Infra + "." + this.Dao;
        }

        public string DirNegocioInterfaceFisico(string NamaSpace)
        {
            return NamaSpace + this.Barra + this.Negocio + this.Barra + this.Interface + this.Barra;
        }

        public string DirNegocioInterfaceLogico(string NamaSpace)
        {
            return NamaSpace + "." + this.Negocio + "." + this.Interface;
        }

        public string DirNegocioServicoFisico(string NamaSpace)
        {
            return NamaSpace + this.Barra + this.Negocio + this.Barra + this.Servico + this.Barra;
        }

        public string DirNegocioServicoLogico(string NamaSpace)
        {
            return NamaSpace + "." + this.Negocio + "." + this.Servico;
        }

    }


    public enum SubDiretorio
    {
        Infra,
        Data,
        Connection,
        Entidade,
        Interface,
        Negocio,
        Servico,
        Repositorio,
        Sql,
        Ioc
    }

    public enum SubDiretorioSea
    {
        Infra,
        Dao,
        Entidades,
        Interfaces,
        MapeamentoEntidades,
        Repositorio,
        SQL,
        TransferenciaObjeto,
        InjecaoDependencia,
        MensagemSistema,
        Negocio,
        Interface,
        Servico,
        Validacao,
        Conexao,
        Contexto,
        Helper
    }
}