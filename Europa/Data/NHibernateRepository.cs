using Europa.Data.Model;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace Europa.Data
{
    public class NHibernateRepository<TEntity> where TEntity : BaseEntity
    {
        public ISession _session { get; set; }

        public NHibernateRepository()
        {

        }

        public NHibernateRepository(ISession session)
        {
            _session = session;
        }

        protected ISession Session { get { return _session; } }

        public IQueryable<TEntity> Queryable()
        {
            return _session.Query<TEntity>();
        }

        public TEntity FindById(long id)
        {
            return _session.Get<TEntity>(id);
        }

        public virtual void Save(TEntity entity)
        {
            _session.SaveOrUpdate(entity);
        }

        public void Delete(TEntity entity)
        {
            _session.Delete(entity);
        }

        public void Flush()
        {
            Session.Flush();
        }
    }
}
