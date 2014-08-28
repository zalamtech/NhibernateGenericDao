#region copyright
// ------------------------------------------------------------------------
// <copyright file="Field.cs" company="Zalamtech SARL">
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
using System.Runtime.Serialization;
using System.Text;

namespace Com.Googlecode.Genericdao.Search
{
    [Serializable]
    public class Field : ISerializable
    {
        // ReSharper disable InconsistentNaming
        public static readonly string ROOT_ENTITY = "";
        public const int OP_PROPERTY = 0;
        public const int OP_COUNT = 1;
        public const int OP_COUNT_DISTINCT = 2;
        public const int OP_MAX = 3;
        public const int OP_MIN = 4;
        public const int OP_SUM = 5;
        public const int OP_AVG = 6;
        public const int OP_CUSTOM = 999;
        
        protected string _property;
        protected string _key;
        protected int _operator;
        // ReSharper restore InconsistentNaming

        public Field()
        {
        }

        public Field(string property)
        {
            _property = property;
        }

        public Field(string property, string key)
        {
            _property = property;
            _key = key;
        }

        public Field(string property, int theOperator)
        {
            _property = property;
            _operator = theOperator;
        }

        public Field(string property, int theOperator, string key)
        {
            _property = property;
            _operator = theOperator;
            _key = key;
        }

        public string Property
        {
            get { return _property; }
            set { _property = value; }
        }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public int Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        protected bool Equals(Field other)
        {
            return string.Equals(_property, other._property) && string.Equals(_key, other._key) && _operator == other._operator;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyFieldInGetHashCode
                var hashCode = (_property != null ? _property.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_key != null ? _key.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _operator;
                // ReSharper restore NonReadonlyFieldInGetHashCode
                return hashCode;
            }
        }

        public override bool Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Field)obj);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            var parens = true;
            switch (_operator)
            {
                case OP_AVG: sb.Append("AVG("); break;
                case OP_COUNT: sb.Append("COUNT("); break;
                case OP_COUNT_DISTINCT: sb.Append("COUNT_DISTINCT("); break;
                case OP_MAX: sb.Append("MAX("); break;
                case OP_MIN: sb.Append("MIN("); break;
                case OP_PROPERTY: parens = false; break;
                case OP_SUM: sb.Append("SUM("); break;
                case OP_CUSTOM: sb.Append("CUSTOM: "); parens = false; break;
                default: sb.Append("**INVALID OPERATOR: (" + _operator + ")** "); parens = false; break;
            }

            sb.Append(_property ?? "null");

            if (parens)
                sb.Append(")");

            if (_key == null)
            {
                return sb.ToString();
            }

            //sb.Append(" as `"); => mysql
            sb.Append(" as ");
            sb.Append(_key);
            //sb.Append("`"); => mysql

            return sb.ToString();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Property", _property);
            info.AddValue("Key", _key);
            info.AddValue("Operator", _operator);
        }
    }
}