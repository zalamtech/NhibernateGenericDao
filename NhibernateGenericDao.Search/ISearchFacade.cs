#region copyright
// ------------------------------------------------------------------------
// <copyright file="ISearchFacade.cs" company="Zalamtech SARL">
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
using System.Collections;

namespace Com.Googlecode.Genericdao.Search
{
    public interface ISearchFacade
    {
        IList Search(ISearch search);

        IList Search(Type searchClass, ISearch search);

        int Count(ISearch search);

        int Count(Type searchClass, ISearch search);

        SearchResult<T> SearchAndCount<T>(ISearch search);

        SearchResult<T> SearchAndCount<T>(Type searchClass, ISearch search);

        object SearchUnique(ISearch search);

        object SearchUnique(Type searchClass, ISearch search);

        Filter GetFilterFromExample(object example);

        Filter GetFilterFromExample(object example, ExampleOptions options);
    }
}