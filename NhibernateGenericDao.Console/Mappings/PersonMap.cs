#region copyright
// ------------------------------------------------------------------------
// <copyright file="PersonMap.cs" company="Zalamtech SARL">
//	Copyright 2014.
// </copyright>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// <author>Abdoul DIALLO</author>
// <date>2014-8-28 23:21</date>
// ------------------------------------------------------------------------
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