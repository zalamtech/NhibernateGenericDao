#region copyright
// ------------------------------------------------------------------------
// <copyright file="Filter.cs" company="Zalamtech SARL">
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
using System.Text;

namespace Com.Googlecode.Genericdao.Search
{
    public class Filter
    {
        // ReSharper disable InconsistentNaming
        public static readonly string ROOT_ENTITY = "";
        public const int OP_EQUAL = 0;
        public const int OP_NOT_EQUAL = 1;
        public const int OP_LESS_THAN = 2;
        public const int OP_GREATER_THAN = 3;
        public const int OP_LESS_OR_EQUAL = 4;
        public const int OP_GREATER_OR_EQUAL = 5;
        public const int OP_LIKE = 6;
        public const int OP_ILIKE = 7;
        public const int OP_IN = 8;
        public const int OP_NOT_IN = 9;
        public const int OP_NULL = 10;
        public const int OP_NOT_NULL = 11;
        public const int OP_EMPTY = 12;
        public const int OP_NOT_EMPTY = 13;
        public const int OP_AND = 100;
        public const int OP_OR = 101;
        public const int OP_NOT = 102;
        public const int OP_SOME = 200;
        public const int OP_ALL = 201;
        public const int OP_NONE = 202;
        public const int OP_CUSTOM = 999;
        // ReSharper enable InconsistentNaming

        protected string _property;
        protected object _value;
        protected int _operator;

        public Filter()
        {
        }

        public Filter(string property, object value, int theOperator)
        {
            _property = property;
            _value = value;
            _operator = theOperator;
        }

        public Filter(string property, object value)
        {
            _property = property;
            _value = value;
            _operator = OP_EQUAL;
        }

        public string Property
        {
            get { return _property; }
            set { _property = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public int Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        public static Filter Equal(string property, object value)
        {
            return new Filter(property, value, OP_EQUAL);
        }

        public static Filter LessThan(string property, object value)
        {
            return new Filter(property, value, OP_LESS_THAN);
        }

        public static Filter GreaterThan(string property, object value)
        {
            return new Filter(property, value, OP_GREATER_THAN);
        }

        public static Filter LessOrEqual(string property, object value)
        {
            return new Filter(property, value, OP_LESS_OR_EQUAL);
        }

        public static Filter GreaterOrEqual(string property, object value)
        {
            return new Filter(property, value, OP_GREATER_OR_EQUAL);
        }

        public static Filter In(string property, IList value)
        {
            return new Filter(property, value, OP_IN);
        }

        public static Filter In(string property, params object[] value)
        {
            return new Filter(property, value, OP_IN);
        }

        public static Filter NotIn(string property, IList value)
        {
            return new Filter(property, value, OP_NOT_IN);
        }

        public static Filter NotIn(string property, params object[] value)
        {
            return new Filter(property, value, OP_NOT_IN);
        }

        public static Filter Like(string property, object value)
        {
            return new Filter(property, value, OP_LIKE);
        }

        public static Filter Ilike(string property, object value)
        {
            return new Filter(property, value, OP_ILIKE);
        }

        /// <summary>
        /// Create a new Filter using the != operator.
        /// </summary>
        public static Filter NotEqual(string property, object value)
        {
            return new Filter(property, value, OP_NOT_EQUAL);
        }

        /// <summary>
        /// Create a new Filter using the IS NULL operator.
        /// </summary>
        public static Filter IsNull(string property)
        {
            return new Filter(property, true, OP_NULL);
        }

        /// <summary>
        /// Create a new Filter using the IS NOT NULL operator.
        /// </summary>
        public static Filter IsNotNull(string property)
        {
            return new Filter(property, true, OP_NOT_NULL);
        }

        /// <summary>
        /// Create a new Filter using the IS EMPTY operator.
        /// </summary>
        public static Filter IsEmpty(string property)
        {
            return new Filter(property, true, OP_EMPTY);
        }

        /// <summary>
        /// Create a new Filter using the IS NOT EMPTY operator.
        /// </summary>
        public static Filter IsNotEmpty(string property)
        {
            return new Filter(property, true, OP_NOT_EMPTY);
        }

        /// <summary>
        /// Create a new Filter using the AND operator.
        /// 
        /// <p>This takes a variable number of parameters. Any number of
        /// <see cref="Filter" /> can be specified.
        /// </p>
        /// </summary>
        public static Filter And(params Filter[] filters)
        {
            var filter = new Filter("AND", null, OP_AND);
            foreach (var f in filters)
            {
                filter.Add(f);
            }

            return filter;
        }

        /// <summary>
        /// Create a new Filter using the OR operator.
        /// 
        /// <p>This takes a variable number of parameters. Any number of
        /// <see cref="Filter" /> can be specified.
        /// </p>
        /// </summary>
        public static Filter Or(params Filter[] filters)
        {
            var filter = And(filters);
            filter._property = "OR";
            filter._operator = OP_OR;
            return filter;
        }

        /// <summary>
        /// Create a new Filter using the NOT operator.
        /// </summary>
        public static Filter Not(Filter filter)
        {
            return new Filter("NOT", filter, OP_NOT);
        }

        /// <summary>
        /// Create a new Filter using the SOME operator.
        /// </summary>
        public static Filter Some(string property, Filter filter)
        {
            return new Filter(property, filter, OP_SOME);
        }

        /// <summary>
        /// Create a new Filter using the ALL operator.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static Filter All(string property, Filter filter)
        {
            return new Filter(property, filter, OP_ALL);
        }

        /// <summary>
        /// Create a new Filter using the NONE operator. This is equivalent to NOT SOME.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static Filter None(string property, Filter filter)
        {
            return new Filter(property, filter, OP_NONE);
        }

        public static Filter Custom(string expression)
        {
            return new Filter(expression, null, OP_CUSTOM);
        }

        public static Filter Custom(string expression, params object[] values)
        {
            return new Filter(expression, values, OP_CUSTOM);
        }

        public static Filter Custom(string expression, IList values)
        {
            return new Filter(expression, values, OP_CUSTOM);
        }

        public void Add(Filter filter)
        {
            if (!(_value is IList))
            {
                _value = new ArrayList();
            }
            ((IList) _value).Add(filter);
        }

        public void Remove(Filter filter)
        {
            if (!(_value is IList))
            {
                return;
            }
            ((IList) _value).Remove(filter);
        }

        public IList GetValuesAsList()
        {
            if (_value == null)
            {
                return null;
            }

            var value = _value as IList;
            if (value != null)
            {
                return value;
            }

            if (_value.GetType().IsArray)
            {
                var list = new ArrayList();
                var values = _value as IEnumerable;
                if (values == null) return list;
                foreach (var o in values)
                {
                    list.Add(o);
                }

                return list;
            }
            else
            {
                var list = new ArrayList {_value};
                return list;
            }
        }

        public bool IsTakesSingleValue()
        {
            return _operator <= 7;
        }

        public bool IsTakesListOfValues()
        {
            return _operator == OP_IN || _operator == OP_NOT_IN;
        }

        public bool IsTakesNoValue()
        {
            return (_operator >= 10 && _operator <= 13) || _operator == OP_CUSTOM;
        }

        public bool IsTakesSingleSubFilter()
        {
            return _operator == OP_NOT || (_operator >= 200 && _operator < 300);
        }

        public bool IsTakesListOfSubFilters()
        {
            return _operator == OP_AND || _operator == OP_OR;
        }

        public bool IsTakesNoProperty()
        {
            return _operator >= 100 && _operator <= 102;
        }

        public override bool Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Filter) obj);
        }

        protected bool Equals(Filter other)
        {
            if (_operator != other._operator)
                return false;
            if (_property == null)
            {
                if (other._property != null)
                    return false;
            }
            else if (!_property.Equals(other._property))
                return false;
            if (_value == null)
            {
                if (other._value != null)
                    return false;
            }
            else if (!_value.Equals(other._value))
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyFieldInGetHashCode
                var hashCode = (_property != null ? _property.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_value != null ? _value.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _operator;
                // ReSharper restore NonReadonlyFieldInGetHashCode
                return hashCode;
            }
        }

        public override string ToString()
        {
            switch (_operator)
            {
                case OP_IN:
                    return  _property + " in (" + InternalUtil.ParamDisplayString(_value) + ")";
                case OP_NOT_IN:
                    return _property + " not in (" + InternalUtil.ParamDisplayString(_value) + ")";
                case OP_EQUAL:
                    return _property + " = " + InternalUtil.ParamDisplayString(_value);
                case OP_NOT_EQUAL:
                    return _property + " != " + InternalUtil.ParamDisplayString(_value);
                case OP_GREATER_THAN:
                    return _property + " > " + InternalUtil.ParamDisplayString(_value);
                case OP_LESS_THAN:
                    return _property + " < " + InternalUtil.ParamDisplayString(_value);
                case OP_GREATER_OR_EQUAL:
                    return _property + " >= " + InternalUtil.ParamDisplayString(_value);
                case OP_LESS_OR_EQUAL:
                    return _property + " <= " + InternalUtil.ParamDisplayString(_value);
                case OP_LIKE:
                    return _property + " LIKE " + InternalUtil.ParamDisplayString(_value);
                case OP_ILIKE:
                    return _property + " ILIKE " + InternalUtil.ParamDisplayString(_value);
                case OP_NULL:
                    return _property + " IS NULL";
                case OP_NOT_NULL:
                    return _property + " IS NOT NULL";
                case OP_EMPTY:
                    return _property + " IS EMPTY";
                case OP_NOT_EMPTY:
                    return _property + " IS NOT EMPTY";
                case OP_AND:
                case OP_OR:
                    if (!(_value is IList))
                    {
                        return (_operator == OP_AND ? "AND: " : "OR: ") + "**INVALID VALUE - NOT A LIST: (" + _value + ") **";
                    }

                    var op = _operator == OP_AND ? " and " : " or ";

                    var sb = new StringBuilder("(");
                    var first = true;
                    foreach (var o in ((IList) _value))
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            sb.Append(op);
                        }
                        if (o is Filter)
                        {
                            sb.Append(o);
                        }
                        else
                        {
                            sb.Append("**INVALID VALUE - NOT A FILTER: (" + o + ") **");
                        }
                    }
                    if (first)
                        return (_operator == OP_AND ? "AND: " : "OR: ") + "**EMPTY LIST**";

                    sb.Append(")");
                    return sb.ToString();
                case OP_NOT:
                    if (!(_value is Filter))
                    {
                        return "NOT: **INVALID VALUE - NOT A FILTER: (" + _value + ") **";
                    }
                    return "not " + _value;
                case OP_SOME:
                    if (!(_value is Filter))
                    {
                        return "SOME: **INVALID VALUE - NOT A FILTER: (" + _value + ") **";
                    }
                    return "some `" + _property + "` {" + _value + "}";
                case OP_ALL:
                    if (!(_value is Filter))
                    {
                        return "ALL: **INVALID VALUE - NOT A FILTER: (" + _value + ") **";
                    }
                    return "all `" + _property + "` {" + _value + "}";
                case OP_NONE:
                    if (!(_value is Filter))
                    {
                        return "NONE: **INVALID VALUE - NOT A FILTER: (" + _value + ") **";
                    }
                    return "none `" + _property + "` {" + _value + "}";
                case OP_CUSTOM:
                    var items = _value as IEnumerable;
                    var hasValues = false;
                    if (items != null)
                    {
                        hasValues = ((IList) items).Count > 0;
                    }

                    if (items == null || !hasValues)
                    {
                        return "CUSTOM[" + _property + "]";
                    }
                    
                    var sb2 = new StringBuilder();
                    sb2.Append("CUSTOM[").Append(_property).Append("]values(");

                    foreach (var i in items)
                    {
                        sb2.Append(i);
                    }

                    sb2.Append(")");
                    return sb2.ToString();
                default:
                    return "**INVALID OPERATOR: (" + _operator + ") - VALUE: " + InternalUtil.ParamDisplayString(_value) +
                           " **";
            }
        }
    }
}
