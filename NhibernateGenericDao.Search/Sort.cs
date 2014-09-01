#region copyright
// ------------------------------------------------------------------------
// <copyright file="Sort.cs" company="Zalamtech SARL">
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
using System.Text;

namespace Com.Googlecode.Genericdao.Search
{
    [Serializable]
    public class Sort
    {
        // ReSharper disable InconsistentNaming
        protected string _property;
        protected bool _desc = false;
        protected bool _ignoreCase = false;
        protected bool _customExpression = false;
        // ReSharper enable InconsistentNaming

        public Sort()
        {
        }

        public Sort(string property, bool desc, bool ignoreCase)
        {
            _property = property;
            _desc = desc;
            _ignoreCase = ignoreCase;
        }

        public Sort(string property, bool desc)
        {
            _property = property;
            _desc = desc;
        }

        public Sort(string property)
        {
            _property = property;
        }

        public Sort(bool isCustomExpression, string property, bool desc)
        {
            _customExpression = isCustomExpression;
            _property = property;
            _desc = desc;
        }

        public Sort(bool isCustomExpression, string property)
        {
            _customExpression = isCustomExpression;
            _property = property;
        }

        public string Property
        {
            get { return _property; }
            set { _property = value; }
        }

        public bool Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public bool IgnoreCase
        {
            get { return _ignoreCase; }
            set { _ignoreCase = value; }
        }

        public bool CustomExpression
        {
            get { return _customExpression; }
            set { _customExpression = value; }
        }

        public static Sort Asc(string property)
        {
            return new Sort(property);
        }

        public static Sort Ascending(string property, bool ignoreCase)
        {
            return new Sort(property, ignoreCase);
        }

        public static Sort Descending(string property)
        {
            return new Sort(property, true);
        }

        public static Sort Descending(string property, bool ignoreCase)
        {
            return new Sort(property, true, ignoreCase);
        }

        public static Sort CustomExpressionAsc(string expression)
        {
            return new Sort(true, expression);
        }

        public static Sort CustomExpressionDesc(string expression)
        {
            return new Sort(true, expression, true);
        }

        public override bool Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Sort) obj);
        }

        protected bool Equals(Sort other)
        {
            return string.Equals(_property, other._property) && _desc.Equals(other._desc) && _ignoreCase.Equals(other._ignoreCase) && _customExpression.Equals(other._customExpression);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyFieldInGetHashCode
                var hashCode = (_property != null ? _property.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _desc.GetHashCode();
                hashCode = (hashCode * 397) ^ _ignoreCase.GetHashCode();
                hashCode = (hashCode * 397) ^ _customExpression.GetHashCode();
                // ReSharper restore NonReadonlyFieldInGetHashCode
                return hashCode;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (_customExpression)
            {
                sb.Append("CUSTOM: ");
            }

            if (_property == null)
            {
                sb.Append("null");
            }
            else
            {
                sb.Append("`");
                sb.Append(_property);
                sb.Append("`");
            }
            sb.Append(_desc ? " desc" : " asc");
            if (_ignoreCase && !_customExpression)
            {
                sb.Append(" (ignore case)");
            }
            return sb.ToString();
        }
    }
}