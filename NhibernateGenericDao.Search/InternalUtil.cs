#region copyright
// ------------------------------------------------------------------------
// <copyright file="InternalUtil.cs" company="Zalamtech SARL">
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
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Com.Googlecode.Genericdao.Search
{
    public class InternalUtil
    {
        public static object ConvertIfNeeded(object value, Type type, Assembly assembly = null)
        {
            // Since we're returning an object, we will never be able to return a primitive value.
            // We will return the boxed type instead.
            if (type.IsPrimitive)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        type = typeof(Boolean);
                        break;
                    case TypeCode.Char:
                        type = typeof(Char);
                        break;
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                        type = typeof(Byte);
                        break;
                    case TypeCode.UInt16:
                    case TypeCode.Int16:
                        type = typeof(Int16);
                        break;
                    case TypeCode.UInt32:
                    case TypeCode.Int32:
                        type = typeof(Int32);
                        break;
                    case TypeCode.UInt64:
                    case TypeCode.Int64:
                        type = typeof(Int64);
                        break;
                    case TypeCode.Single:
                        type = typeof(Single);
                        break;
                    case TypeCode.Double:
                        type = typeof(Double);
                        break;
                }
            }

            if (value == null)
                return null;
            if (value.GetType().IsAssignableFrom(type))
                return value;

            if (Type.GetTypeCode(type) == TypeCode.String)
            {
                return value.ToString();
            }

            if (IsNumeric(type))
            {
                // the desired type is a number
                if (IsNumeric(value))
                {
                    // the value is also a number of some kind. do a conversion
                    // to the correct number type.
                    
                    if (Type.GetTypeCode(type) == TypeCode.Double)
                    {
                        return Convert.ToDouble(value);
                    }

                    if (Type.GetTypeCode(type) == TypeCode.Single)
                    {
                        return Convert.ToSingle(value);
                    }

                    if (Type.GetTypeCode(type) == TypeCode.Int64 || Type.GetTypeCode(type) == TypeCode.UInt64)
                    {
                        return Convert.ToInt64(value);
                    }

                    if (Type.GetTypeCode(type) == TypeCode.Int32 || Type.GetTypeCode(type) == TypeCode.UInt32)
                    {
                        return Convert.ToInt32(value);
                    }

                    if (Type.GetTypeCode(type) == TypeCode.Int16 || Type.GetTypeCode(type) == TypeCode.Int16)
                    {
                        return Convert.ToInt16(value);
                    }
                    
                    try
                    {
                        return Convert.ChangeType(value, type);
                    }
                    catch (ArgumentNullException)
                    {
                    }
                    catch (InvalidCastException)
                    {
                    }
                    catch (OverflowException)
                    {
                    }
                    catch (FormatException)
                    {
                    }
                }

                else
                {
                    var s = value as string;
                    if (s == null)
                    {
                        throw new InvalidCastException("Unable to convert value " + value + " to type " + type);
                    }

                    //the value is a String. attempt to parse the string
                    try
                    {
                        if (Type.GetTypeCode(type) == TypeCode.Double)
                        {
                            return Double.Parse(s, CultureInfo.InvariantCulture);
                        }

                        if (Type.GetTypeCode(type) == TypeCode.Single)
                        {
                            return Single.Parse(s, CultureInfo.InvariantCulture);
                        }

                        if (Type.GetTypeCode(type) == TypeCode.Int64)
                        {
                            return Int64.Parse(s, CultureInfo.InvariantCulture);
                        }

                        if (Type.GetTypeCode(type) == TypeCode.Int32)
                        {
                            return Int32.Parse(s, CultureInfo.InvariantCulture);
                        }

                        if (Type.GetTypeCode(type) == TypeCode.Int16)
                        {
                            return Int16.Parse(s, CultureInfo.InvariantCulture);
                        }
                    }
                    catch (ArgumentNullException)
                    {
                    }
                    catch (OverflowException)
                    {
                    }
                    catch (FormatException)
                    {
                    }
                }
            }
            else if (type == typeof(Type))
            {
                if (assembly != null)
                {
                    try
                    {
                        //return Convert.ChangeType(value, type);
                        return assembly.GetType(value.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidCastException("Unable to convert value " + value + " to type " + type, ex);
                    }
                }
                
                var asms = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var asm in asms)
                {
                    Exception exception = null;
                    try
                    {
                        //return Convert.ChangeType(value, type);
                        var resp = asm.GetType(value.ToString());
                        if (resp != null)
                        {
                            return resp;
                        }
                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex;
                    }
                    catch (ArgumentException ex)
                    {
                        exception = ex;
                    }
                    catch (FileNotFoundException ex)
                    {
                        exception = ex;
                    }
                    catch (FileLoadException ex)
                    {
                        exception = ex;
                    }
                    catch (BadImageFormatException ex)
                    {
                        exception = ex;
                    }

                    throw new InvalidCastException("Unable to convert value " + value + " to type " + type, exception);
                }
            }

            throw new InvalidCastException("Unable to convert value " + value + " to type " + type);
        }

        // Extension method, call for any object, eg "if (x.IsNumeric())..."
        public static bool IsNumeric(object x)
        {
            return (x != null && IsNumeric(x.GetType()));
        }

        // Method where you know the type of the object
        public static bool IsNumeric(Type type)
        {
            return IsNumeric(type, Type.GetTypeCode(type));
        }

        // Method where you know the type and the type code of the object
        public static bool IsNumeric(Type type, TypeCode typeCode)
        {
            return (typeCode == TypeCode.Decimal ||
                (type.IsPrimitive && typeCode != TypeCode.Object
                && typeCode != TypeCode.Boolean
                && typeCode != TypeCode.Char
                && typeCode != TypeCode.String));
        }
        
        public static string ParamDisplayString(object val)
        {
            if (val == null)
            {
                return "null";
            }

            if (val is string)
            {
                return "\"" + val + "\"";
            }

            var os = val as IList;
            if (os != null)
            {
                var sb = new StringBuilder();
                sb.Append(os.GetType());
                sb.Append(" {");
                var first = true;
                foreach (var o in os)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(", ");
                    }
                    sb.Append(ParamDisplayString(o));
                }
                sb.Append("}");
                return sb.ToString();
            }

            if (!val.GetType().IsArray)
            {
                return val.ToString();
            }

            var sbuilder = new StringBuilder();
            sbuilder.Append(val.GetType().GetElementType());
            sbuilder.Append("[] {");
            var firstItem = true;
            
            // ReSharper disable once PossibleInvalidCastException
            foreach (var o in (object[])val)
            {
                if (firstItem)
                {
                    firstItem = false;
                }
                else
                {
                    sbuilder.Append(", ");
                }
                sbuilder.Append(ParamDisplayString(o));
            }
            sbuilder.Append("}");
            return sbuilder.ToString();
        }
    }
}