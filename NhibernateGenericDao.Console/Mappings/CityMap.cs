#region copyright
// ------------------------------------------------------------------------
// <copyright file="CityMap.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 23:50</date>
//  ------------------------------------------------------------------------
#endregion

using Com.Googlecode.Genericdao.Console.Entities;
using FluentNHibernate.Mapping;

namespace Com.Googlecode.Genericdao.Console.Mappings
{
    public class CityMap : ClassMap<City>
    {
        public CityMap()
        {
            Id(c => c.Id).Column("CityId").GeneratedBy.Identity();
            Map(c => c.Name).Not.Nullable();
            //HasMany(c => c.People).KeyColumn("CityId").Inverse().Cascade.All();
        }
    }
}