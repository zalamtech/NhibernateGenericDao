#region copyright
// ------------------------------------------------------------------------
// <copyright file="Phone.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 20:16</date>
//  ------------------------------------------------------------------------
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