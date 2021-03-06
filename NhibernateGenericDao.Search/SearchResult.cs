﻿#region copyright
// ------------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="Zalamtech SARL">
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
using System.Collections.Generic;

namespace Com.Googlecode.Genericdao.Search
{
    public class SearchResult<T>
    {
        // ReSharper disable InconsistentNaming
        protected IList<T> _result;
        protected int _totalCount = -1;
        // ReSharper restore InconsistentNaming

        public IList<T> Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }
    }
}