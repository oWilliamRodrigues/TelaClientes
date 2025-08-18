using NHibernate.Exceptions;

namespace Europa.Data
{
    public class ConstraintViolationExceptionWrapper
    {
        public static bool IsConstraintViolationException(GenericADOException exp)
        {
            return exp.InnerException != null && exp.InnerException.Message.Contains("ORA-02292");
        }
    }
}
