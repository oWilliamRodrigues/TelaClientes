namespace Europa.Data.Dialect
{
    public static class DecomposeExtension
    {
        public static int Decompose(this int value, int dayOfWeek)
        {
            int baseValue = 64;
            int aux = value;
            while (baseValue > 0 && dayOfWeek <= baseValue)
            {
                if (aux >= baseValue)
                {
                    if (baseValue == dayOfWeek)
                    {
                        return 1;
                    }
                    aux = aux - baseValue;
                }
                baseValue = baseValue / 2;
            }
            return 0;
        }
    }
}
