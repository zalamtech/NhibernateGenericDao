#region copyright
// ------------------------------------------------------------------------
// <copyright file="Person.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 20:17</date>
//  ------------------------------------------------------------------------
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