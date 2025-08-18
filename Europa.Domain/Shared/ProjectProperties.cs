using Europa.Commons;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Europa.Domain.Shared
{
    public static class ProjectProperties
    {
        #region Static Definitions

        public static string ConnectionStringPostgres { get; } = "CS_TREINAMENTO";
        public static string CodigoProjeto { get; } = "DES0000";

        #endregion
    }
}