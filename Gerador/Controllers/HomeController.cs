using Core.Infra.Data.Negocio.Interface;
using System.Linq;
using System.Web.Mvc;

namespace Gerador.Controllers
{
    public class HomeController : Controller
    {
        ITabelaServico _iTabelaServico;

        public HomeController(ITabelaServico iTabelaServico)
        {
            _iTabelaServico = iTabelaServico;
        }

        public ActionResult Index()
        {
            ListaTabela();
            return View();
        }

        public PartialViewResult Propriedade(string NomeTabela)
        {
            var objTabela = _iTabelaServico.ObterPropriedadeTabelaOracle(NomeTabela);
            return PartialView(objTabela);
        }

        private void ListaTabela()
        {
            var listaTabela = _iTabelaServico.ObterTodasTabelaOracle().ToList();
            ViewBag.ListaTabela = string.Empty;
            ViewBag.ListaTabela = new SelectList(listaTabela, "Nome", "Nome", "");
        }

        [HttpPost]
        public string Salvar(string NomeTabela, string NameSpace, string NameSchema)
        {
            string retorno = string.Empty;

            retorno += _iTabelaServico.CriandoClasseEntidade(NomeTabela, NameSpace, NameSchema) + "<br />";
            retorno += _iTabelaServico.CriandoClasseMapeamentoEntidade(NomeTabela, NameSpace, NameSchema) + "<br />";
            retorno += _iTabelaServico.CriandoInterfaces(NomeTabela, NameSpace, NameSchema) + "<br />";
            retorno += _iTabelaServico.CriandoClasseDao(NomeTabela, NameSpace, NameSchema) + "<br />";
            retorno += _iTabelaServico.CriandoClasseInterfacesServico(NomeTabela, NameSpace, NameSchema) + "<br />";
            retorno += _iTabelaServico.CriandoClasseServico(NomeTabela, NameSpace, NameSchema);
            return retorno;
        }


        /// <summary>
        /// Criando Todos os SubDiretorios do Projeto Core
        /// </summary>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        [HttpPost]
        public string CriaDiretorios(string NameSpace)
        {
            return _iTabelaServico.CriaDiretorios(NameSpace);
        }
    }
}