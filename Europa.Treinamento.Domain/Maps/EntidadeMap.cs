using Europa.Data;
using Europa.Data.Model;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using NHibernate.Type;

namespace Europa.Treinamento.Domain.Maps
{
    public class EntidadeMap : BaseClassMap<Entidade>
    {
        public EntidadeMap()
        {
            // Aqui se define o nome da tabela do banco de dados que abrigará a entidade
            Table("TBL_ENTIDADES");
            // Definimos aqui a coluna que abrigará a chave primária (ID) da entidade juntamente com a sequência que gerará o número de ID
            Id(reg => reg.Id).Column("ID_ENTIDADE").GeneratedBy.Sequence("SEQ_ENTIDADES");
            // Daqui pra baixo se define o mapeamento das demais propriedades da classe que define a entidade.
            // O objetivo é relacionar Propriedade de Classe com Coluna de Tabela de banco de dados.
            Map(reg => reg.Nome).Column("NM_ENTIDADE") // Define qual coluna da tabela representa esta propriedade de classe
                .Length(DatabaseStandardDefinitions.STD_CARACTER_LENGTH_EXTENDED) // Define o tamanho máximo de caracteres que essa propriedade aceita
                .Not.Nullable(); // Define que esta propriedade não é nula
            Map(reg => reg.Sobrenome).Column("DS_SOBRENOME").Length(DatabaseStandardDefinitions.STD_CARACTER_LENGTH_EXTENDED).Not.Nullable();
            Map(reg => reg.Idade).Column("NR_IDADE").Not.Nullable();
            Map(reg => reg.DataNascimento).Column("DT_NASCIMENTO").Not.Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO")
                .CustomType<EnumType<Situacao>>() // Aqui definimos um tipo customizado para o nosso mapeamento, neste caso é um tipo enumerado Situacao
                .Not.Nullable();
            // Referencia uma unidade
            References(x => x.Unidade).Column("ID_UNIDADE").ForeignKey("FK_ENTIDADE_X_UNIDADE_01").Not.Nullable();
        }
    }
}
