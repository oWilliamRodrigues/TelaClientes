using NHibernate;

namespace Europa.Treinamento.Domain.Services
{
    public abstract class BaseService
    {
        public ISession _session { get; set; }

        protected ISession Session { get { return _session; } }

        protected BaseService()
        {

        }

        protected BaseService(ISession Session)
        {
            _session = Session;
        }

    }

}
