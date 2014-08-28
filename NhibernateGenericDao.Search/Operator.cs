#region copyright
// ------------------------------------------------------------------------
// <copyright file="Operator.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
namespace Com.Googlecode.Genericdao.Search
{
    public static class Operator
    {
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
    }
}
