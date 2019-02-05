using Core.Helper;
using Core.Infra.Data.Entidade;
using Core.Infra.Data.Interface;
using Core.Infra.Data.Negocio.Interface;
using Core.Infra.Data.Sql;
using Gerador.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Infra.Data.Negocio.Servico
{
    public class TabelaServico : ITabelaServico
    {
        Diretorio dir = null;
        string path = @"C:\Temp\";
        string Atributo = string.Empty;
        string Using = string.Empty;
        string NameSpaceProjeto = string.Empty;
        string NomeClasse = string.Empty;
        string Anotacoes = string.Empty;
        bool ExisteArquivo = false;
        string NaoExisteClasse = string.Empty;
        string[] root = new string[18];
        string diretorios = string.Empty;
        string CampoChavePrimaria = string.Empty;
        string NomeTabelaSemSchema = string.Empty;

        public string DePropriedade
        {
            get { return "\t\tpublic"; }
        }

        public string DeModificador
        {
            get { return "{ get; set; }"; }
        }

        ITabelaRepositorio _iTabelaRepositorio;


        public TabelaServico(ITabelaRepositorio iTabelaRepositorio)
        {
            _iTabelaRepositorio = iTabelaRepositorio;
        }

        public void Dispose(){}

        public IEnumerable<Tabela> ObterTodasTabelaOracle()
        {
            return _iTabelaRepositorio.Obter(TabelaSql.ObterTodasTabelaOracle, null);
        }
        public IEnumerable<Tabela> ObterPropriedadeTabelaOracle(string NomeTabela)
        {
            return _iTabelaRepositorio.Obter(TabelaSql.ObterPropriedadeTabelaOracle, new { NomeTabela = NomeTabela });
        }

        public Tabela ObterPrimaryKeyTabelaOracle(string NomeColuna, string NomeTabela)
        {
            return _iTabelaRepositorio.Obter(TabelaSql.ObterPrimaryKeyTabelaOracle, new { NomeColuna = NomeColuna, NomeTabela = NomeTabela }).FirstOrDefault();
        }

        /// <summary>
        /// Classe Entidade
        /// </summary>
        /// <param name="NomeTabela"></param>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        public string CriandoClasseEntidade(string NomeTabela, string NameSpace, string NameSchema)
        {
            Using = string.Empty;
            NameSpaceProjeto = string.Empty;
            NomeClasse = string.Empty;
            Atributo = string.Empty;
            NomeTabelaSemSchema = string.Empty;

            if (NameSchema.Equals(NomeTabela.Substring(0, 3)))
            {
                var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = _NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 4);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }
            else
            {
                //var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 1);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }


            dir = new Diretorio();
            string _NameSpacesFisico = path + dir.DirInfraEntidadeFisico(NameSpace);
            string _NameSpacesLogico = dir.DirInfraEntidadeLogico(NameSpace);
            NaoExisteClasse = _NameSpacesFisico + NomeTabelaSemSchema + ".cs";

            //// Determine whether the directory exists.
            //if (Directory.Exists(@path))
            //{
            //    ExisteArquivo = System.IO.File.Exists(NaoExisteClasse);
            //    if (ExisteArquivo)
            //        return "Essa classe já existe.";
            //}

            Using += "using System;" + Environment.NewLine;
            Using += "using System.ComponentModel.DataAnnotations;" + Environment.NewLine;

            //NameSpace
            NameSpaceProjeto += "namespace " + _NameSpacesLogico;

            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "{";
            NameSpaceProjeto += Environment.NewLine;

            //Nome da Classe
            NomeClasse = "\tpublic class " + NomeTabelaSemSchema + Environment.NewLine;
            NomeClasse += "\t{";
            NomeClasse += Environment.NewLine;

            //Construtor
            NomeClasse += "\t\tpublic " + NomeTabelaSemSchema + "()" + Environment.NewLine;
            NomeClasse += "\t\t{" + Environment.NewLine;
            NomeClasse += "\t\t\tValidacao = new Validacao.Validacao()" + Environment.NewLine;
            NomeClasse += "\t\t\t{" + Environment.NewLine;
            NomeClasse += "\t\t\t\tIsValidacao = true" + Environment.NewLine;
            NomeClasse += "\t\t\t};" + Environment.NewLine;
            NomeClasse += "\t\t}" + Environment.NewLine;
            NomeClasse += Environment.NewLine;

            var objTabela = ObterPropriedadeTabelaOracle(NomeTabela);

            foreach (var item in objTabela)
            {
                Anotacoes += "\t\t[Display(Name = \"" + item.Coluna + "\")]" + Environment.NewLine;

                if (item.NaoNulo.Equals("N"))
                {
                    var objChave = ObterPrimaryKeyTabelaOracle(item.Coluna.Trim(), item.Nome.Trim());
                    var _Coluna = item.Coluna.ToLower();
                    var _ColunaUpper = _Coluna.Length > 1 ? char.ToUpper(_Coluna[0]) + _Coluna.Substring(1) : _Coluna.ToUpper();

                    if (objChave != null)
                    {
                        if (objChave.ChavePrimaria.Equals("P"))
                        {
                            Anotacoes = string.Empty;
                            Anotacoes += "\t\t[Key]" + Environment.NewLine;
                            Anotacoes += "\t\t[Display(Name = \"" + _ColunaUpper + "\")]" + Environment.NewLine;
                        }
                    }

                    if (TipoDados.RetornaTipoDado(item.TipoDado).Equals("string"))
                        Anotacoes += "\t\t[StringLength(" + item.Tamanho + ")]" + Environment.NewLine;

                    if (item.Escala != 0)
                    {
                        if (TipoDados.RetornaTipoDado(item.TipoDado).Trim().Equals("int"))
                        {
                            Anotacoes += "\t\t[DataType(DataType.Currency)]" + Environment.NewLine;
                            Anotacoes += "\t\t[Required(ErrorMessage = \"Campo " + item.Coluna + " não pode ser branco\")]" + Environment.NewLine;
                            Atributo = DePropriedade + " decimal " + _ColunaUpper + " " + DeModificador + Environment.NewLine;
                        }
                    }
                    else
                    {
                        Anotacoes += "\t\t[Required(ErrorMessage = \"Campo " + _ColunaUpper + " não pode ser branco\")]" + Environment.NewLine;
                        Atributo = DePropriedade + " " + TipoDados.RetornaTipoDado(item.TipoDado) + " " + _ColunaUpper + " " + DeModificador + Environment.NewLine;
                    }
                }
                else
                {
                    var _Coluna = item.Coluna.ToLower();
                    var _ColunaUpper = _Coluna.Length > 1 ? char.ToUpper(_Coluna[0]) + _Coluna.Substring(1) : _Coluna.ToUpper();

                    if (TipoDados.RetornaTipoDado(item.TipoDado).Trim().Equals("int"))
                        Atributo = DePropriedade + " Nullable<" + TipoDados.RetornaTipoDado(item.TipoDado) + "> " + _ColunaUpper + " " + DeModificador + Environment.NewLine;

                    if (TipoDados.RetornaTipoDado(item.TipoDado).Trim().Equals("string"))
                    {
                        Anotacoes += "\t\t[StringLength(" + item.Tamanho + ")]" + Environment.NewLine;
                        Atributo = DePropriedade + " " + TipoDados.RetornaTipoDado(item.TipoDado) + " " + _ColunaUpper + " " + DeModificador + Environment.NewLine;
                    }

                    if (TipoDados.RetornaTipoDado(item.TipoDado).Trim().Equals("DateTime"))
                    {
                        Anotacoes += "\t\t[DisplayFormat(DataFormatString = \"{0:dd/MM/yyyy}" + "\")]" + Environment.NewLine;
                        Atributo = DePropriedade + " Nullable<" + TipoDados.RetornaTipoDado(item.TipoDado) + "> " + _ColunaUpper + " " + DeModificador + Environment.NewLine;
                    }

                    if (TipoDados.RetornaTipoDado(item.TipoDado).Trim().Equals("byte[]"))
                        Atributo = DePropriedade + " " + TipoDados.RetornaTipoDado(item.TipoDado) + " " + _ColunaUpper + " " + DeModificador + Environment.NewLine;

                    if (item.Escala != 0)
                    {
                        if (TipoDados.RetornaTipoDado(item.TipoDado).Trim().Equals("int"))
                        {
                            Anotacoes += "\t\t[DataType(DataType.Currency)]" + Environment.NewLine;
                            Atributo = DePropriedade + " Nullable<decimal> " + _ColunaUpper + " " + DeModificador + Environment.NewLine;
                        }
                    }
                }

                Anotacoes += Atributo + Environment.NewLine;
                Atributo = string.Empty;
            }

            Anotacoes += "\t\t[ScaffoldColumn(false)]" + Environment.NewLine;
            Anotacoes += "\t\tpublic Validacao.Validacao Validacao;" + Environment.NewLine;

            NomeClasse += Anotacoes;

            NomeClasse += "\t}";
            NameSpaceProjeto += NomeClasse;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "}";
            Using += NameSpaceProjeto;

            using (FileStream fs = new FileStream(NaoExisteClasse, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(Using);
                }
            }
            return "Classe " + NomeTabelaSemSchema + ".cs criada com sucesso. Caminho: " + _NameSpacesFisico + NomeTabelaSemSchema + ".cs";
        }

        /// <summary>
        /// Classe de MapeamentoEntidade
        /// </summary>
        /// <param name="NomeTabela"></param>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        public string CriandoClasseMapeamentoEntidade(string NomeTabela, string NameSpace, string NameSchema)
        {
            Using = string.Empty;
            NameSpaceProjeto = string.Empty;
            NomeClasse = string.Empty;
            Atributo = string.Empty;

            NomeTabelaSemSchema = string.Empty;

            if (NameSchema.Equals(NomeTabela.Substring(0, 3)))
            {
                var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = _NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 4);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }
            else
            {
                //var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 1);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }

            dir = new Diretorio();
            string _NameSpacesFisico = path + dir.DirInfraMapeamentoEntidadeFisico(NameSpace);
            string _NameSpacesLogico = dir.DirInfraMapeamentoEntidadeLogico(NameSpace);
            NaoExisteClasse = _NameSpacesFisico + NomeTabelaSemSchema + "Map.cs";


            //// Determine whether the directory exists.
            //if (Directory.Exists(@path))
            //{
            //    ExisteArquivo = System.IO.File.Exists(NaoExisteClasse);
            //    if (ExisteArquivo)
            //        return "Essa classe já existe.";
            //}

            Using += "using System;" + Environment.NewLine;
            Using += "using SEACore.Infra.Entidades;" + Environment.NewLine;
            Using += "using System.ComponentModel.DataAnnotations.Schema;" + Environment.NewLine;
            Using += "using System.Data.Entity.ModelConfiguration;" + Environment.NewLine;

            //NameSpace do Projeto
            NameSpaceProjeto += "namespace " + _NameSpacesLogico;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "{";
            NameSpaceProjeto += Environment.NewLine;

            //Nome da classe
            NomeClasse = "\tpublic class " + NomeTabelaSemSchema + "Map " + ": EntityTypeConfiguration<" + NomeTabelaSemSchema + ">" + Environment.NewLine;
            NomeClasse += "\t{";
            NomeClasse += Environment.NewLine;

            //Método Construtor da Classe
            NomeClasse += "\t\tpublic " + NomeTabelaSemSchema + "Map()" + Environment.NewLine;
            NomeClasse += "\t\t{" + Environment.NewLine;

            var objTabela = ObterPropriedadeTabelaOracle(NomeTabela);

            Atributo += "\t\t\tthis.ToTable(\"" + NomeTabela.ToUpper() + "\", \"" + NameSchema + "\");" + Environment.NewLine;

            foreach (var item in objTabela)
            {
                var _Coluna = item.Coluna.ToLower();
                var _ColunaUpper = _Coluna.Length > 1 ? char.ToUpper(_Coluna[0]) + _Coluna.Substring(1) : _Coluna.ToUpper();
                Atributo += "\t\t\tthis.Property(t => t." + _ColunaUpper + ").HasColumnName(\"" + item.Coluna.ToUpper() + "\"); " + Environment.NewLine;
            }

            NomeClasse += Atributo;// + Environment.NewLine;
            NomeClasse += "\t\t}" + Environment.NewLine;
            NomeClasse += "\t}";
            NameSpaceProjeto += NomeClasse;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "}";
            Using += NameSpaceProjeto;

            using (FileStream fs = new FileStream(NaoExisteClasse, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(Using);
                }
            }
            return "Classe " + NomeTabelaSemSchema + "Map.cs criada com sucesso. Caminho: " + _NameSpacesFisico + NomeTabelaSemSchema + "Map.cs";
        }

        /// <summary>
        /// Classe Interfaces
        /// </summary>
        /// <param name="NomeTabela"></param>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        public string CriandoInterfaces(string NomeTabela, string NameSpace, string NameSchema)
        {
            Using = string.Empty;
            NameSpaceProjeto = string.Empty;
            NomeClasse = string.Empty;
            Atributo = string.Empty;

            NomeTabelaSemSchema = string.Empty;

            if (NameSchema.Equals(NomeTabela.Substring(0, 3)))
            {
                var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = _NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 4);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }
            else
            {
                //var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 1);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }

            dir = new Diretorio();
            string _NameSpacesFisico = path + dir.DirInfraInterfacesFisico(NameSpace);
            string _NameSpacesLogico = dir.DirInfraInterfacesLogico(NameSpace);
            NaoExisteClasse = _NameSpacesFisico + "I" + NomeTabelaSemSchema + ".cs";

            //// Determine whether the directory exists.
            //if (Directory.Exists(@path))
            //{
            //    ExisteArquivo = System.IO.File.Exists(NaoExisteClasse);
            //    if (ExisteArquivo)
            //        return "Essa classe já existe.";
            //}

            Using += "using SEACore.Infra.Entidades;" + Environment.NewLine;
            Using += "using SEACore.Repositorio;" + Environment.NewLine;

            NameSpaceProjeto += "namespace " + _NameSpacesLogico;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "{";
            NameSpaceProjeto += Environment.NewLine;

            //Nome da classe
            NomeClasse = "\tpublic interface I" + NomeTabelaSemSchema + " : IRepositorio<" + NomeTabelaSemSchema + ">" + Environment.NewLine;
            NomeClasse += "\t{";
            NomeClasse += Environment.NewLine;

            NomeClasse += "\t}";
            NameSpaceProjeto += NomeClasse;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "}";
            Using += NameSpaceProjeto;

            using (FileStream fs = new FileStream(NaoExisteClasse, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(Using);
                }
            }
            return "Interface I" + NomeTabelaSemSchema + ".cs criada com sucesso. Caminho: " + _NameSpacesFisico + "I" + NomeTabelaSemSchema + ".cs";
        }

        /// <summary>
        /// Classe Dao
        /// </summary>
        /// <param name="NomeTabela"></param>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        public string CriandoClasseDao(string NomeTabela, string NameSpace, string NameSchema)
        {
            Using = string.Empty;
            NameSpaceProjeto = string.Empty;
            NomeClasse = string.Empty;
            Atributo = string.Empty;

            NomeTabelaSemSchema = string.Empty;

            if (NameSchema.Equals(NomeTabela.Substring(0, 3)))
            {
                var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = _NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 4);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }
            else
            {
                //var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 1);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }

            dir = new Diretorio();
            string _NameSpacesFisico = path + dir.DirInfraDaoFisico(NameSpace);
            string _NameSpacesLogico = dir.DirInfraDaoLogico(NameSpace);
            NaoExisteClasse = _NameSpacesFisico + NomeTabelaSemSchema + "Dao.cs";

            //// Determine whether the directory exists.
            //if (Directory.Exists(@path))
            //{
            //    ExisteArquivo = System.IO.File.Exists(NaoExisteClasse);
            //    if (ExisteArquivo)
            //        return "Essa classe já existe.";
            //}

            Using += "using SEACore.Contexto;" + Environment.NewLine;
            Using += "using SEACore.Infra.Entidades;" + Environment.NewLine;
            Using += "using SEACore.Infra.Interfaces;" + Environment.NewLine;
            Using += "using SEACore.Repositorio;" + Environment.NewLine;

            NameSpaceProjeto += "namespace " + dir.DirInfraDaoLogico(NameSpace);
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "{";
            NameSpaceProjeto += Environment.NewLine;

            //Nome da classe
            NomeClasse = "\tpublic class " + NomeTabelaSemSchema + "Dao " + ": Repositorio<" + NomeTabelaSemSchema + ">, " + "I" + NomeTabelaSemSchema + Environment.NewLine;
            NomeClasse += "\t{";
            NomeClasse += Environment.NewLine;

            //Método Construtor da Classe
            NomeClasse += "\t\tpublic " + NomeTabelaSemSchema + "Dao(SEAContexto db) : base(db) { }" + Environment.NewLine;

            NomeClasse += "\t}";
            NameSpaceProjeto += NomeClasse;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "}";
            Using += NameSpaceProjeto;

            using (FileStream fs = new FileStream(NaoExisteClasse, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(Using);
                }
            }
            return "Classe " + NomeTabelaSemSchema + "Dao.cs criada com sucesso. Caminho: " + _NameSpacesFisico + NomeTabelaSemSchema + "Dao.cs";
        }

        /// <summary>
        /// Classe InterfaceServico
        /// </summary>
        /// <param name="NomeTabela"></param>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        public string CriandoClasseInterfacesServico(string NomeTabela, string NameSpace, string NameSchema)
        {
            Using = string.Empty;
            NameSpaceProjeto = string.Empty;
            NomeClasse = string.Empty;
            Atributo = string.Empty;

            NomeTabelaSemSchema = string.Empty;

            if (NameSchema.Equals(NomeTabela.Substring(0, 3)))
            {
                var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = _NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 4);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }
            else
            {
                //var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 1);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }

            dir = new Diretorio();
            string _NameSpacesFisico = path + dir.DirNegocioInterfaceFisico(NameSpace);
            string _NameSpacesLogico = dir.DirNegocioInterfaceLogico(NameSpace);
            NaoExisteClasse = _NameSpacesFisico + "I" + NomeTabelaSemSchema + "Servico.cs";

            //// Determine whether the directory exists.
            //if (Directory.Exists(@path))
            //{
            //    ExisteArquivo = System.IO.File.Exists(NaoExisteClasse);
            //    if (ExisteArquivo)
            //        return "Essa classe já existe.";
            //}

            Using += "using System;" + Environment.NewLine;
            Using += "using SEACore.Infra.Entidades;" + Environment.NewLine;
            Using += "using System.Collections.Generic;" + Environment.NewLine;

            NameSpaceProjeto += "namespace " + _NameSpacesLogico;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "{";
            NameSpaceProjeto += Environment.NewLine;

            //Nome da classe
            NomeClasse = "\tpublic interface I" + NomeTabelaSemSchema + "Servico : IDisposable" + Environment.NewLine;
            NomeClasse += "\t{";
            NomeClasse += Environment.NewLine;

            NomeClasse += "\t\t" + NomeTabelaSemSchema + " Salvar(" + NomeTabelaSemSchema + " " + NomeTabelaSemSchema.ToLower() + ");" + Environment.NewLine;
            NomeClasse += "\t\t" + NomeTabelaSemSchema + " ObterPorId(int id);" + Environment.NewLine;
            NomeClasse += "\t\t" + "IEnumerable<" + NomeTabelaSemSchema + "> Lista" + NomeTabelaSemSchema + "();" + Environment.NewLine;

            NomeClasse += "\t}";
            NameSpaceProjeto += NomeClasse;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "}";
            Using += NameSpaceProjeto;

            using (FileStream fs = new FileStream(NaoExisteClasse, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(Using);
                }
            }
            return "Interface I" + NomeTabelaSemSchema + "Servico.cs criada com sucesso. Caminho: " + _NameSpacesFisico + "I" + NomeTabelaSemSchema + "Servico.cs";
        }

        /// <summary>
        /// Classe Servico
        /// </summary>
        /// <param name="NomeTabela"></param>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        public string CriandoClasseServico(string NomeTabela, string NameSpace, string NameSchema)
        {
            Using = string.Empty;
            NameSpaceProjeto = string.Empty;
            NomeClasse = string.Empty;
            Atributo = string.Empty;
            CampoChavePrimaria = string.Empty;

            NomeTabelaSemSchema = string.Empty;

            if (NameSchema.Equals(NomeTabela.Substring(0, 3)))
            {
                var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = _NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 4);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }
            else
            {
                //var _NomeTabela = NomeTabela.Remove(0, 3);
                var _PrimeiroNome = NomeTabela.Substring(0, 1);
                var _RestoNome = NomeTabela.Remove(0, 1);
                NomeTabelaSemSchema = _PrimeiroNome + _RestoNome.ToLower();
            }

            dir = new Diretorio();
            string _NameSpacesFisico = path + dir.DirNegocioServicoFisico(NameSpace);
            string _NameSpacesLogico = dir.DirNegocioServicoLogico(NameSpace);
            NaoExisteClasse = _NameSpacesFisico + NomeTabelaSemSchema + "Servico.cs";

            //// Determine whether the directory exists.
            //if (Directory.Exists(@path))
            //{
            //    ExisteArquivo = System.IO.File.Exists(NaoExisteClasse);
            //    if (ExisteArquivo)
            //        return "Essa classe já existe.";
            //}

            Using += "using System;" + Environment.NewLine;
            Using += "using SEACore.Infra.Entidades;" + Environment.NewLine;
            Using += "using SEACore.Infra.Interfaces;" + Environment.NewLine;
            Using += "using SEACore.MensagemSistema;" + Environment.NewLine;
            Using += "using SEACore.Negocio.Interface;" + Environment.NewLine;
            Using += "using System.Collections.Generic;" + Environment.NewLine;

            NameSpaceProjeto += "namespace " + _NameSpacesLogico;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "{";
            NameSpaceProjeto += Environment.NewLine;

            //Nome da classe
            NomeClasse = "\tpublic class " + NomeTabelaSemSchema + "Servico: I" + NomeTabelaSemSchema +"Servico" + Environment.NewLine;
            NomeClasse += "\t{";
            NomeClasse += Environment.NewLine;

            //Declarando da Interface Servico
            NomeClasse += "\t\tI" + NomeTabelaSemSchema + " _i" + NomeTabelaSemSchema + ";" + Environment.NewLine;
            NomeClasse += Environment.NewLine;

            //Construtor
            NomeClasse += "\t\tpublic " + NomeTabelaSemSchema + "Servico(I" + NomeTabelaSemSchema + " i" + NomeTabelaSemSchema + ")" + Environment.NewLine;
            NomeClasse += "\t\t{" + Environment.NewLine;
            NomeClasse += "\t\t\t_i" + NomeTabelaSemSchema + " = " + "i" + NomeTabelaSemSchema + ";" + Environment.NewLine;
            NomeClasse += "\t\t}" + Environment.NewLine;
            NomeClasse += Environment.NewLine;

            NomeClasse += "\t\tpublic void Dispose(){}" + Environment.NewLine + Environment.NewLine;

            var objTabela = ObterPropriedadeTabelaOracle(NomeTabela);
            var objChave = new Tabela();

            foreach (var item in objTabela)
            {
                if (item.NaoNulo.Equals("N"))
                {
                    objChave = ObterPrimaryKeyTabelaOracle(item.Coluna.Trim(), item.Nome.Trim());
                    break;
                }
            }

            if (objChave != null)
            {
                if (objChave.ChavePrimaria.Equals("P"))
                {
                    CampoChavePrimaria = NomeTabelaSemSchema.ToLower() + "." + objChave.Coluna;

                    //Inicio Método Salvar
                    NomeClasse += "\t\t/// <summary>" + Environment.NewLine;
                    NomeClasse += "\t\t/// Método Salvar " + NomeTabelaSemSchema + Environment.NewLine;
                    NomeClasse += "\t\t/// </summary>" + Environment.NewLine;
                    NomeClasse += "\t\t/// <param name=" + NomeTabelaSemSchema.ToLower() + ">Object</param>" + Environment.NewLine;
                    NomeClasse += "\t\t/// <returns>Object</returns>" + Environment.NewLine;
                    NomeClasse += "\t\tpublic " + NomeTabelaSemSchema + " Salvar(" + NomeTabelaSemSchema + " " + NomeTabelaSemSchema.ToLower() + ")" + Environment.NewLine;
                    NomeClasse += "\t\t{" + Environment.NewLine;

                    //Inicio Try
                    NomeClasse += "\t\t\ttry" + Environment.NewLine;
                    NomeClasse += "\t\t\t{" + Environment.NewLine;

                    //If de Inclusao
                    NomeClasse += "\t\t\t\tif(" + CampoChavePrimaria + "==0)" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t{" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t\t _i" + NomeTabelaSemSchema + ".Salvar(" + NomeTabelaSemSchema.ToLower() + ");" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t\t" + NomeTabelaSemSchema.ToLower() + ".Validacao.Mensagem = Mensagens.Sucesso;" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t\t" + NomeTabelaSemSchema.ToLower() + ".Validacao.IsValidacao = true;" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t\t" + NomeTabelaSemSchema.ToLower() + ".Validacao.Operacao = \"I\";" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t}" + Environment.NewLine;

                    //Else de Alteracao
                    NomeClasse += "\t\t\t\telse" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t{" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t\t _i" + NomeTabelaSemSchema + ".Alterar(" + NomeTabelaSemSchema.ToLower() + ");" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t\t" + NomeTabelaSemSchema.ToLower() + ".Validacao.Mensagem = Mensagens.Alteracao;" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t\t" + NomeTabelaSemSchema.ToLower() + ".Validacao.IsValidacao = true;" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t\t" + NomeTabelaSemSchema.ToLower() + ".Validacao.Operacao = \"A\";" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t}" + Environment.NewLine;

                    //Fim do Try
                    NomeClasse += "\t\t\t}" + Environment.NewLine;
                    //Inicio do Catch
                    NomeClasse += "\t\t\tcatch (Exception ex)" + Environment.NewLine;
                    NomeClasse += "\t\t\t{" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t" + NomeTabelaSemSchema.ToLower() + ".Validacao.Mensagem = Mensagens.Erro  + \"" + "\" + ex.Message;" + Environment.NewLine;
                    NomeClasse += "\t\t\t\t" + NomeTabelaSemSchema.ToLower() + ".Validacao.IsValidacao = false;" + Environment.NewLine;
                    //Fim do Catch
                    NomeClasse += "\t\t\t}" + Environment.NewLine;
                    NomeClasse += Environment.NewLine;
                    NomeClasse += "\t\t\treturn " + NomeTabelaSemSchema.ToLower() + ";";
                    NomeClasse += Environment.NewLine;
                    //Fim do Metodo Salvar
                    NomeClasse += "\t\t}" + Environment.NewLine;
                    NomeClasse += Environment.NewLine;
                }
            }

            //Inicio do Metodo ObterPorId
            NomeClasse += "\t\t/// <summary>" + Environment.NewLine;
            NomeClasse += "\t\t/// Método ObterPorId " + Environment.NewLine;
            NomeClasse += "\t\t/// </summary>" + Environment.NewLine;
            NomeClasse += "\t\t/// <param name=" + objChave.Coluna + ">Object</param>" + Environment.NewLine;
            NomeClasse += "\t\t/// <returns>int</returns>" + Environment.NewLine;
            NomeClasse += "\t\tpublic " + NomeTabelaSemSchema + " ObterPorId(int id)" + Environment.NewLine;
            NomeClasse += "\t\t{" + Environment.NewLine;
            NomeClasse += "\t\t\treturn _i" + NomeTabelaSemSchema + ".Obter(exp=> exp." + objChave.Coluna + " == id);" + Environment.NewLine;
            NomeClasse += "\t\t}" + Environment.NewLine;
            NomeClasse += Environment.NewLine;
            //Fim do Metodo ObterPorId

            //Inicio do Metodo Lista
            NomeClasse += "\t\t/// <summary>" + Environment.NewLine;
            NomeClasse += "\t\t/// Método Lista " + Environment.NewLine;
            NomeClasse += "\t\t/// </summary>" + Environment.NewLine;
            NomeClasse += "\t\t/// <returns>IEnumerable<" + NomeTabelaSemSchema + "></returns>" + Environment.NewLine;
            NomeClasse += "\t\tpublic " + "IEnumerable<" + NomeTabelaSemSchema + "> Lista" + NomeTabelaSemSchema + "()" + Environment.NewLine;
            NomeClasse += "\t\t{" + Environment.NewLine;
            NomeClasse += "\t\t\treturn _i" + NomeTabelaSemSchema + ".Obter();" + Environment.NewLine;
            NomeClasse += "\t\t}" + Environment.NewLine;

            //Fim do Metodo Lista

            NomeClasse += "\t}";
            NameSpaceProjeto += NomeClasse;
            NameSpaceProjeto += Environment.NewLine;
            NameSpaceProjeto += "}";
            Using += NameSpaceProjeto;

            using (FileStream fs = new FileStream(NaoExisteClasse, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(Using);
                }
            }
            return "Classe" + NomeTabelaSemSchema + "Servico.cs criada com sucesso. Caminho: " + _NameSpacesFisico + NomeTabelaSemSchema + "Servico.cs";
        }

        public string CriaDiretorios(string NameSpace)
        {
            dir = new Diretorio();

            root[0] = NameSpace + dir.Barra + dir.Conexao;
            root[1] = NameSpace + dir.Barra + dir.Contexto;
            root[2] = NameSpace + dir.Barra + dir.Helper;
            root[3] = NameSpace + dir.Barra + dir.Infra;
            root[4] = NameSpace + dir.Barra + dir.Infra + dir.Barra + dir.Dao;
            root[5] = NameSpace + dir.Barra + dir.Infra + dir.Barra + dir.Entidades;
            root[6] = NameSpace + dir.Barra + dir.Infra + dir.Barra + dir.Interfaces;
            root[7] = NameSpace + dir.Barra + dir.Infra + dir.Barra + dir.MapeamentoEntidades;
            root[8] = NameSpace + dir.Barra + dir.Infra + dir.Barra + dir.Repositorio;
            root[9] = NameSpace + dir.Barra + dir.Infra + dir.Barra + dir.SQL;
            root[10] = NameSpace + dir.Barra + dir.Infra + dir.Barra + dir.TransferenciaObjeto;
            root[11] = NameSpace + dir.Barra + dir.InjecaoDependencia;
            root[12] = NameSpace + dir.Barra + dir.MensagemSistema;
            root[13] = NameSpace + dir.Barra + dir.Negocio;
            root[14] = NameSpace + dir.Barra + dir.Negocio + dir.Barra + dir.Interface;
            root[15] = NameSpace + dir.Barra + dir.Negocio + dir.Barra + dir.Servico;
            root[16] = NameSpace + dir.Barra + dir.Validacao;

            if (Directory.Exists(@path))
            {
                for (int i = 0; i < root.Length; i++)
                {
                    //Criando Diretório
                    if (!Directory.Exists(@path + root[i]))
                        Directory.CreateDirectory(@path + root[i]);
                }
            }

            for (int i = 0; i < root.Length; i++)
            {
                diretorios += root[i] + "<br />";
            }
            return diretorios;
        }
    }
}
