#region copyright
// ------------------------------------------------------------------------
// <copyright file="IMutableSearch.cs" company="Zalamtech SARL">
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

namespace Com.Googlecode.Genericdao.Search
{
    public interface IMutableSearch : ISearch
    {
        IMutableSearch SetFirstResult(int firstResult);

        IMutableSearch SetMaxResults(int maxResults);

        IMutableSearch SetPage(int page);

        IMutableSearch SetSearchClass(Type searchClass);

        IMutableSearch SetFilters(IList<Filter> filters);

        IMutableSearch SetDisjunction(bool disjunction);

        IMutableSearch SetSorts(IList<Sort> sorts);

        IMutableSearch SetFields(IList<Field> fields);

        IMutableSearch SetDistinct(bool distinct);

        IMutableSearch SetFetches(IList<String> fetches);

        IMutableSearch SetResultMode(int resultMode);
    }
}