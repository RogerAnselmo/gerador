namespace Gerador.Helper
{
    public class TipoDados
    {
        public static string RetornaTipoDado(string TipoDado)
        {
            switch (TipoDado)
            {
                case "NUMBER":
                    return "int";

                case "VARCHAR2":
                    return "string";

                case "CHAR":
                    return "string";

                case "BLOB":
                    return "byte[]";

                case "DATE":
                    return "DateTime";

                case "CLOB":
                    return "string";

                default:
                    return "string";
            }
        }
    }
}