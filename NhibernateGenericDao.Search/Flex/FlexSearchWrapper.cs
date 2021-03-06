﻿#region copyright
// ------------------------------------------------------------------------
// <copyright file="FlexSearchWrapper.cs" company="Zalamtech SARL">
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
using System;
using System.Collections.Generic;

namespace Com.Googlecode.Genericdao.Search.Flex
{
    public class FlexSearchWrapper : ISearch
    {
        private FlexSearch _search;

        public FlexSearchWrapper(FlexSearch flexSearch)
        {
            _search = flexSearch;
        }


        public int GetFirstResult()
        {
            return _search.FirstResult;
        }

        public int GetMaxResults()
        {
            return _search.MaxResults;
        }

        public int GetPage()
        {
            return _search.Page;
        }

        public Type GetSearchClass()
        {
            return string.IsNullOrEmpty(_search.SearchClassName) ? null : Type.GetType(_search.SearchClassName);
        }

        public IList<Filter> GetFilters()
        {
            return _search.GetFilters();
        }

        public bool IsDisjunction()
        {
            return _search.Disjunction;
        }

        public IList<Sort> GetSorts()
        {
            return _search.GetSorts();
        }

        public IList<Field> GetFields()
        {
            return _search.GetFields();
        }

        public bool IsDistinct()
        {
            return _search.Distinct;
        }

        public IList<string> GetFetches()
        {
            return _search.GetFetches();
        }

        public int GetResultMode()
        {
            return _search.ResultMode;
        }
    }
}