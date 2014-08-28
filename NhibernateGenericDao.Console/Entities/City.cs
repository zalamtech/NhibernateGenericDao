#region copyright
// ------------------------------------------------------------------------
// <copyright file="City.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 20:13</date>
//  ------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Com.Googlecode.Genericdao.Dao.Entity;

namespace Com.Googlecode.Genericdao.Console.Entities
{
    public class City : Entity<int>
    {
        public virtual string Name { get; set; }
        public virtual IList<Person> People { get; set; }
    }
}