#region copyright
// ------------------------------------------------------------------------
// <copyright file="Phone.cs" company="Zalamtech SARL">
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
using System;
using Com.Googlecode.Genericdao.Dao.Entity;

namespace Com.Googlecode.Genericdao.Console.Entities
{
    public class Phone : Entity<int>
    {
        public virtual string Number { get; set; }

        public virtual PhoneType Type { get; set; }

        public virtual int PersonId { get; set; }

        public virtual DateTime RecordDate { get; set; }

        public Phone()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            RecordDate = DateTime.Now;
        } 
    }
}