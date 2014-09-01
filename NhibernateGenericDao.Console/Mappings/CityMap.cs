#region copyright
// ------------------------------------------------------------------------
// <copyright file="CityMap.cs" company="Zalamtech SARL">
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
// <date>2014-9-1 18:17</date>
// ------------------------------------------------------------------------
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