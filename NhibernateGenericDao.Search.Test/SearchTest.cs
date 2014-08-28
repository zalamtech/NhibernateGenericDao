#region copyright
// ------------------------------------------------------------------------
// <copyright file="SearchTest.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 12:29</date>
//  ------------------------------------------------------------------------
#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Com.Googlecode.Genericdao.Search.Test
{
    [TestClass]
    public class SearchTest
    {
        [TestMethod]
        public void TestToString()
        {
            Search s1;
            System.Diagnostics.Debug.WriteLine(s1);

            var s2 = new Search();
            s2.SetSearchClass(typeof(SearchTest));
            s2.SetDisjunction(true);
            s2.SetFirstResult(-44);
            s2.SetMaxResults(19);
            s2.SetPage(0);
            s2.SetResultMode(SearchConstants.RESULT_ARRAY);

            s2.AddField("home.type");
            s2.AddFilterEqual("father.firstName", "ABC");
            s2.AddSortAsc("home.address.type");
            System.Diagnostics.Debug.WriteLine(s2);

            var s3 = new Search();
            s3.SetSearchClass(typeof(Search));

            s3.AddField("home", Field.OP_AVG);
            s3.AddField("sally's home", Field.OP_COUNT);
            s3.AddField("pork", Field.OP_COUNT_DISTINCT);
            s3.AddField("some pig", Field.OP_MAX);
            s3.AddField(new Field("", Field.OP_MIN));
            s3.AddField(new Field(null, Field.OP_SUM));
            s3.AddField("4th limb", 6000);
            s3.AddField((Field)null);

            s3.AddFilterGreaterThan("gt", "nine");
            s3.AddFilterLessThan("lt", 9);
            s3.AddFilterGreaterOrEqual("ge", 8.2293);
            s3.AddFilterLessOrEqual("le", null);
            s3.AddFilterAnd(Filter.NotEqual("ne", 11), Filter.In("mine.in", 22, 23, 24, 25, "Cartons"), Filter.Or(Filter
                    .Not(Filter.NotIn("marm.not.in", 33, 34, 35)), Filter.Like("dog.like", "mant*s"), Filter.Ilike(
                    "cat.ilike", "Mon%")));
            s3.AddFilter(new Filter(null, null, -3));
            s3.AddFilter(null);
            s3.AddFilterNull("nullProp");
            s3.AddFilterNotNull("notNullProp");
            s3.AddFilterEmpty("emptyProp");
            s3.AddFilterNotEmpty("notEmptyProp");

            s3.AddSort("more.sorts", true);
            s3.AddSort(new Sort(null, false));
            s3.AddSort(null);
            
            System.Diagnostics.Debug.WriteLine(s3);
        }
    }
}