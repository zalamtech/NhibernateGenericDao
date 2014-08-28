#region copyright
// ------------------------------------------------------------------------
// <copyright file="Person.cs" company="Zalamtech SARL">
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
using System;
using System.Collections.Generic;
using Com.Googlecode.Genericdao.Dao.Entity;

namespace Com.Googlecode.Genericdao.Console.Entities
{
    public class Person : Entity<int>
    {
        public virtual City City { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime BirthDay { get; set; }

        public virtual string Notes { get; set; }

        public virtual DateTime RecordDate { get; set; }

        public virtual IList<Phone> Phones { get; set; }

        public Person()
        {
            Notes = "";
            RecordDate = DateTime.Now;
        }
    }
}