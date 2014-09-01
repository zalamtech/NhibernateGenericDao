#region copyright
// ------------------------------------------------------------------------
// <copyright file="NullChecksFilterVisitor.cs" company="Zalamtech SARL">
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
// <date>2014-9-1 18:18</date>
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