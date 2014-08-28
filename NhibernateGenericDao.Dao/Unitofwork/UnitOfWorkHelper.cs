#region copyright
// ------------------------------------------------------------------------
// <copyright file="UnitOfWorkHelper.cs" company="Zalamtech SARL">
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
using System.Reflection;

namespace Com.Googlecode.Genericdao.Dao.Unitofwork
{
    public class UnitOfWorkHelper
    {
        public static bool IsDaoMethod(MethodInfo methodInfo)
        {
            return IsDaoClass(methodInfo.DeclaringType);
        }

        public static bool IsDaoClass(Type type)
        {
            return typeof(IGenericDao).IsAssignableFrom(type);
        }

        public static bool HasUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        } 
    }
}