#region copyright
// ------------------------------------------------------------------------
// <copyright file="ExampleOptions.cs" company="Zalamtech SARL">
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
using System.Collections.Generic;

namespace Com.Googlecode.Genericdao.Search
{
    public class ExampleOptions
    {
        // ReSharper disable InconsistentNaming
        public const int EXACT = 0;
	    public const int START = 1;
	    public const int END = 2;
        public const int ANYWHERE = 3;
        // ReSharper restore InconsistentNaming

        private IList<string> _excludeProps;
        private int _likeMode = EXACT;
        private bool _excludeNulls = true;
        private bool _excludeZeros;
        private bool _ignoreCase;

        public ExampleOptions ExcludeProp(string property)
        {
            if (_excludeProps == null)
                _excludeProps = new List<string>();
            _excludeProps.Add(property);
            return this;
        }

        public IList<string> ExcludeProps
        {
            get { return _excludeProps; }
            set { _excludeProps = value; }
        }

        public bool ExcludeNulls
        {
            get { return _excludeNulls; }
            set { _excludeNulls = value; }
        }

        // ReSharper disable ConvertToAutoProperty
        public bool ExcludeZeros
        {
            get { return _excludeZeros; }
            set { _excludeZeros = value; }
        }

        public bool IgnoreCase
        {
            get { return _ignoreCase; }
            set { _ignoreCase = value; }
        }

        public int LikeMode
        {
            get { return _likeMode; }
            set { _likeMode = value; }
        }
        // ReSharper restore ConvertToAutoProperty
    }
}