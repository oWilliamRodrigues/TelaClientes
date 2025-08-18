namespace Europa.Web
{
    public interface ISessionAttributes
    {
        long GetUserPrimaryKey();

        long GetAccessPrimaryKey();

        bool HasPermission(string codigoUnidadeFuncional, string comandoFuncionalidade);
    }
}
