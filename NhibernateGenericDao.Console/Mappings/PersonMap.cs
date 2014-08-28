#region copyright
// ------------------------------------------------------------------------
// <copyright file="PersonMap.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 23:51</date>
//  ------------------------------------------------------------------------
#endregion

using Com.Googlecode.Genericdao.Console.Entities;
using FluentNHibernate.Mapping;

namespace Com.Googlecode.Genericdao.Console.Mappings
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Table("People");
            Id(p => p.Id).Column("PersonId").GeneratedBy.Identity();
            References(p => p.City).Column("CityId").Cascade.None().Not.LazyLoad();
            Map(p => p.Name, "Name").Length(50).Not.Nullable();
            Map(p => p.BirthDay).Not.Nullable();
            Map(p => p.Notes).Length(255);
            Map(p => p.RecordDate).Not.Nullable();
            HasMany(p => p.Phones).KeyColumn("PersonId").Inverse().Cascade.All();
        }
    }
}