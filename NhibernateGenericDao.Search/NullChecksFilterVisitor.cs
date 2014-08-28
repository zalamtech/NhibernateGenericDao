#region copyright
// ------------------------------------------------------------------------
// <copyright file="NullChecksFilterVisitor.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
namespace Com.Googlecode.Genericdao.Search
{
    public class NullChecksFilterVisitor : FilterVisitor
    {
        public override Filter VisitAfter(Filter filter)
        {
            if (filter.IsTakesSingleValue() || filter.IsTakesListOfValues())
            {
                return Filter.And(filter, Filter.IsNotNull(filter.Property));
            }
            
            return filter;
        }
    }
}