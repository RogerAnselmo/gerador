using Core.Infra.Data.Interface;
using Core.Infra.Data.Negocio.Interface;
using Core.Infra.Data.Negocio.Servico;
using Core.Infra.Data.Repositorio;
using SimpleInjector;

namespace Core.Infra.IoC
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container)
        {
            // Lifestyle.Transient => Uma instancia para cada solicitacao;
            // Lifestyle.Singleton => Uma instancia unica para a classe
            // Lifestyle.Scoped => Uma instancia unica para o request

            // Negocio
            container.RegisterPerWebRequest<ITabelaServico, TabelaServico>();
           
            // Interfaces e Repositorio
            container.RegisterPerWebRequest<ITabelaRepositorio, TabelaRepositorio>();           
        }
    }
}
