using Europa.Data.Model;
using Europa.Extensions;
using NHibernate;
using NHibernate.Type;
using System;

namespace Treinamento.Domain.Core.Data
{
    public class DefaultInterceptor : EmptyInterceptor
    {
        public override bool OnSave(object entity,
                                    object id,
                    object[] state,
                    string[] propertyNames,
                    IType[] types)
        {
            bool onActionResult = this.OnProcess(entity, id, state, propertyNames, types);
            return onActionResult;
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            bool onActionResult = this.OnProcess(entity, id, currentState, propertyNames, types);
            return onActionResult;
        }

        private bool OnProcess(object entity,
                                    object id,
                    object[] state,
                    string[] propertyNames,
                    IType[] types)
        {
            if (entity is BaseEntity)
            {
                if (state[AuditUtil.POSITION_CREATED_BY].IsEmpty())
                {
                    state[AuditUtil.POSITION_CREATED_BY] = 1;
                    state[AuditUtil.POSITION_CREATED_AT] = DateTime.Now;
                }
                state[AuditUtil.POSITION_UPDATED_BY] = 1;
                state[AuditUtil.POSITION_UPDATED_AT] = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}
