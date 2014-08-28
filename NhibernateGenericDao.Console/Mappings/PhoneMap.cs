#region copyright
// ------------------------------------------------------------------------
// <copyright file="PhoneMap.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 23:52</date>
//  ------------------------------------------------------------------------
#endregion

using Com.Googlecode.Genericdao.Console.Entities;
using FluentNHibernate.Mapping;

namespace Com.Googlecode.Genericdao.Console.Mappings
{
    public class PhoneMap : ClassMap<Phone>
    {
        public PhoneMap()
        {
            Id(p => p.Id).Column("PhoneId").GeneratedBy.Identity();
            Map(p => p.Number).Length(50).Not.Nullable();
            Map(p => p.Type).Not.Nullable();
            Map(p => p.RecordDate).Not.Nullable();
            Map(p => p.PersonId);
            //References(p => p.Owner).Column("PersonId").Not.Nullable().Not.LazyLoad();
        }
    }
}