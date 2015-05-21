namespace LComplete.Framework.Web.Auth
{
    public static class FunctionTable
    {
        public static Functions Functions { get; private set; }

        static FunctionTable()
        {
            Functions=new Functions();
        }
    }
}
