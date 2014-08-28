#region copyright
// ------------------------------------------------------------------------
// <copyright file="BaseSearchProcessor.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.Googlecode.Genericdao.Search
{
    public abstract class BaseSearchProcessor
    {
        // ReSharper disable InconsistentNaming
        protected static int QLTYPE_HQL = 0;
        protected static int QLTYPE_EQL = 1;
        protected int _qlType;
        protected IMetadataUtil _metadataUtil;
        protected string _rootAlias = "_it";
        protected const string ROOT_PATH = "";
        // ReSharper enable InconsistentNaming

        protected BaseSearchProcessor(int qlType, IMetadataUtil metadataUtil)
        {
            if (metadataUtil == null)
            {
                throw new ArgumentException("A SearchProcessor cannot be initialized with a null MetadataUtil.");
            }
            _qlType = qlType;
            _metadataUtil = metadataUtil;
        }

        public int QlType
        {
            get { return _qlType; }
            set { _qlType = value; }
        }

        public IMetadataUtil MetadataUtil
        {
            get { return _metadataUtil; }
            set { _metadataUtil = value; }
        }

        public string RootAlias
        {
            get { return _rootAlias; }
            set { _rootAlias = value; }
        }

        /**
	     * Generate the QL string for a given search. Fill paramList with the values
	     * to be used for the query. All parameters within the query string are
	     * specified as named parameters ":pX", where X is the index of the
	     * parameter value in paramList.
	     */
        public String GenerateQL(Type entityClass, ISearch search, IList<object> paramList)
        {
            if (entityClass == null)
                throw new NullReferenceException("The entity class for a search cannot be null");

            var ctx = new SearchContext(entityClass, _rootAlias, paramList);

            var fields = CheckAndCleanFields(search.GetFields());

            ApplyFetches(ctx, CheckAndCleanFetches(search.GetFetches()), fields);

            var select = GenerateSelectClause(ctx, fields, search.IsDistinct());
            var where = GenerateWhereClause(ctx, CheckAndCleanFilters(search.GetFilters()), search.IsDisjunction());
            var orderBy = GenerateOrderByClause(ctx, CheckAndCleanSorts(search.GetSorts()));
            var from = GenerateFromClause(ctx, true);

            var sb = new StringBuilder();
            sb.Append(select);
            sb.Append(from);
            sb.Append(where);
            sb.Append(orderBy);

            var query = sb.ToString();
            //if (logger.isDebugEnabled())
            //    logger.debug("generateQL:\n  " + query);
            return query;
        }

        public string GenerateRowCountQL(Type entityClass, ISearch search, IList<object> paramList)
        {
            if (entityClass == null)
                throw new NullReferenceException("The entity class for a search cannot be null");

            var ctx = new SearchContext(entityClass, _rootAlias, paramList);

            var where = GenerateWhereClause(ctx, CheckAndCleanFilters(search.GetFilters()), search.IsDisjunction());
            var from = GenerateFromClause(ctx, false);

            bool useOperator = false, notUseOperator = false;
            var fields = search.GetFields();
            if (fields != null)
            {
                foreach (var field in fields)
                {
                    switch (field.Operator)
                    {
                        case Field.OP_AVG:
                        case Field.OP_COUNT:
                        case Field.OP_COUNT_DISTINCT:
                        case Field.OP_MAX:
                        case Field.OP_MIN:
                        case Field.OP_SUM:
                            useOperator = true;
                            break;
                        default:
                            notUseOperator = true;
                            break;
                    }
                }
            }

            if (useOperator && notUseOperator)
            {
                throw new Exception("A search can not have a mix of fields with theOperators and fields without theOperators.");
            }

            if (useOperator)
            {
                return null; // if we're using column theOperators, the query will
                // always return 1 result.
            }

            var sb = new StringBuilder();
            if (!search.IsDistinct())
            {
                sb.Append("select count(").Append(_rootAlias).Append(")");
            }
            else
            {
                switch (fields.Count)
                {
                    case 0:
                        sb.Append("select count(distinct ");
                        sb.Append(_rootAlias).Append(")");
                        break;
                    case 1:
                        {
                            sb.Append("select count(distinct ");
                            var prop = fields.First().Property;
                            sb.Append(String.IsNullOrEmpty(prop) ? ctx.GetRootAlias() : GetPathRef(ctx, prop));
                            sb.Append(")");
                        }
                        break;
                    default:
                        throw new ArgumentException("Unfortunately, Hibernate Generic DAO does not currently support "
                                                           + "the count operation on a search that has distinct set with multiple fields.");
                }
            }

            sb.Append(from);
            sb.Append(where);

            var query = sb.ToString();
            //if (logger.isDebugEnabled())
            //	logger.debug("generateRowCountQL:\n  " + query);
            return query;
        }

        /**
	     * Internal method for generating the select clause based on the fields of
	     * the given search.
	     */
        protected string GenerateSelectClause(SearchContext ctx, IList<Field> fields, bool distinct)
        {

            StringBuilder sb = null;
            bool useOperator = false, notUseOperator = false;
            var first = true;

            if (fields != null)
            {
                foreach (var field in fields)
                {
                    if (first)
                    {
                        sb = new StringBuilder("select ");
                        if (distinct)
                            sb.Append("distinct ");
                        first = false;
                    }
                    else
                    {
                        sb.Append(", ");
                    }

                    if (field.Operator == Field.OP_CUSTOM)
                    {
                        // Special case for custom theOperator.
                        if (field.Property == null)
                        {
                            sb.Append("null");
                        }
                        else
                        {
                            AppendCustomExpression(sb, ctx, field.Property);
                        }
                    }
                    else
                    {
                        string prop;
                        if (String.IsNullOrEmpty(field.Property))
                        {
                            prop = ctx.GetRootAlias();
                        }
                        else
                        {
                            var aliasNodeForProperty = GetAliasForPathIfItExists(ctx, field.Property);
                            prop = aliasNodeForProperty != null ? aliasNodeForProperty.Alias : GetPathRef(ctx, field.Property);
                        }

                        switch (field.Operator)
                        {
                            case Field.OP_AVG:
                                sb.Append("avg(");
                                useOperator = true;
                                break;
                            case Field.OP_COUNT:
                                sb.Append("count(");
                                useOperator = true;
                                break;
                            case Field.OP_COUNT_DISTINCT:
                                sb.Append("count(distinct ");
                                useOperator = true;
                                break;
                            case Field.OP_MAX:
                                sb.Append("max(");
                                useOperator = true;
                                break;
                            case Field.OP_MIN:
                                sb.Append("min(");
                                useOperator = true;
                                break;
                            case Field.OP_SUM:
                                sb.Append("sum(");
                                useOperator = true;
                                break;
                            default:
                                notUseOperator = true;
                                break;
                        }

                        sb.Append(prop);
                        if (useOperator)
                        {
                            sb.Append(")");
                        }
                    }
                }
            }

            if (first)
            {
                // there are no fields
                if (distinct)
                    return "select distinct " + ctx.GetRootAlias();
                
                return "select " + ctx.GetRootAlias();
            }
            if (useOperator && notUseOperator)
            {
                throw new Exception("A search can not have a mix of fields with theOperators and fields without theOperators.");
            }
            return sb.ToString();
        }

        /**
	     * Apply the fetch list to the alias tree in the search context.
	     */
        protected void ApplyFetches(SearchContext ctx, IList<String> fetches, IList<Field> fields)
        {
            if (fetches == null) return;
            // apply fetches
            bool hasFetches = false, hasFields = false;
            foreach (var fetch in fetches)
            {
                GetOrCreateAlias(ctx, fetch, true);
                hasFetches = true;
            }

            if (!hasFetches || fields == null) return;

            // don't fetch nodes whose ancestors aren't found in the select
            // clause
            var fieldProps = new List<String>();
            foreach (var field in fields)
            {
                if (field.Operator == Field.OP_PROPERTY)
                {
                    fieldProps.Add(field.Property + ".");
                }
                hasFields = true;
            }

            if (!hasFields) return;

            foreach (var node in from node in ctx.Aliases.Values
                                 where node.Fetch
                                 let hasAncestor = fieldProps.Any(field => node.GetFullPath().StartsWith(field))
                                 where !hasAncestor
                                 select node)
            {
                node.Fetch = false;
            }
        }

        /**
	     * Internal method for generating from clause. This method should be called
	     * after generating other clauses because it relies on the aliases they
	     * create. This method takes every path that is called for in the other
	     * clauses and makes it available as an alias using left joins. It also adds
	     * join fetching for properties specified by <code>fetches</code> if
	     * <code>doEagerFetching</code> is <code>true</code>. <b>NOTE:</b> When
	     * using eager fetching, <code>applyFetches()</code> must be executed first.
	     */
        protected string GenerateFromClause(SearchContext ctx, bool doEagerFetching)
        {
            var sb = new StringBuilder(" from ");

            sb.Append(MetadataUtil.Get(ctx.RootClass).GetEntityName());
            sb.Append(" ");
            sb.Append(ctx.GetRootAlias());
            sb.Append(GenerateJoins(ctx, doEagerFetching));
            return sb.ToString();
        }

        /**
	     * Internal method for generating the join portion of the from clause. This
	     * method should be called after generating other clauses because it relies
	     * on the aliases they create. This method takes every path that is called
	     * for in the other clauses and makes it available as an alias using left
	     * joins. It also adds join fetching for properties specified by
	     * <code>fetches</code> if <code>doEagerFetching</code> is <code>true</code>
	     * . <b>NOTE:</b> When using eager fetching, <code>applyFetches()</code>
	     * must be executed first.
	     */
        protected string GenerateJoins(SearchContext ctx, bool doEagerFetching)
        {
            var sb = new StringBuilder();

            // traverse alias graph breadth-first
            var queue = new Queue<AliasNode>();

            //ctx.Aliases.Get(ROOT_PATH)
            AliasNode rootNode;
            ctx.Aliases.TryGetValue(ROOT_PATH, out rootNode);
            queue.Enqueue(rootNode);

            while (queue.Any())
            {
                var node = queue.Dequeue();
                if (node.Parent != null)
                {
                    sb.Append(" left join ");
                    if (doEagerFetching && node.Fetch)
                        sb.Append("fetch ");
                    sb.Append(node.Parent.Alias);
                    sb.Append(".");
                    sb.Append(node.Property);
                    sb.Append(" as ");
                    sb.Append(node.Alias);
                }

                foreach (var child in node.Children)
                {
                    queue.Enqueue(child);
                }
            }

            return sb.ToString();
        }

        /**
         * Internal method for generating order by clause. Uses sort options from
         * search.
         */
        protected string GenerateOrderByClause(SearchContext ctx, IList<Sort> sorts)
        {
            if (sorts == null)
                return "";

            StringBuilder sb = null;
            var first = true;
            foreach (var sort in sorts)
            {
                if (first)
                {
                    sb = new StringBuilder(" order by ");
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }
                if (sort.CustomExpression)
                {
                    AppendCustomExpression(sb, ctx, sort.Property);
                }
                else if (sort.IgnoreCase && _metadataUtil.Get(ctx.RootClass, sort.Property).IsString())
                {
                    sb.Append("lower(");
                    sb.Append(GetPathRef(ctx, sort.Property));
                    sb.Append(")");
                }
                else
                {
                    sb.Append(GetPathRef(ctx, sort.Property));
                }
                sb.Append(sort.Desc ? " desc" : " asc");
            }

            return first ? "" : sb.ToString();
        }

        /**
         * Internal method for generating where clause for given search. Uses filter
         * options from search.
         */
        protected string GenerateWhereClause(SearchContext ctx, IList<Filter> filters, bool isDisjunction)
        {
            string content;
            if (filters == null || filters.Count == 0)
            {
                return "";
            }

            if (filters.Count == 1)
            {
                content = FilterToQL(ctx, filters.ElementAt(0));
            }
            else
            {
                var junction = new Filter(null, filters, isDisjunction ? Filter.OP_OR : Filter.OP_AND);
                content = FilterToQL(ctx, junction);
            }

            return (content == null) ? "" : " where " + content;
        }

        /**
	     * Recursively generate the QL fragment for a given search filter option.
	     */
        protected string FilterToQL(SearchContext ctx, Filter filter)
        {
            var property = filter.Property;
            var value = filter.Value;
            var theOperator = filter.Operator;

            // for IN and NOT IN, if value is empty list, return false, and true
            // respectively
            if (theOperator == Filter.OP_IN || theOperator == Filter.OP_NOT_IN)
            {
                if (value is IList && ((IList)value).Count == 0)
                {
                    return theOperator == Filter.OP_IN ? "1 = 2" : "1 = 1";
                }
                if (value is Object[] && ((Object[])value).Length == 0)
                {
                    return theOperator == Filter.OP_IN ? "1 = 2" : "1 = 1";
                }
            }

            // convert numbers to the expected type if needed (ex: Integer to Long)
            if (filter.IsTakesListOfValues())
            {
                value = PrepareValue(ctx.RootClass, property, value, true);
            }
            else if (filter.IsTakesSingleValue())
            {
                value = PrepareValue(ctx.RootClass, property, value, false);
            }

            IMetadata metadata;

            switch (theOperator)
            {
                case Operator.OP_NULL:
                    return GetPathRef(ctx, property) + " is null";
                case Operator.OP_NOT_NULL:
                    return GetPathRef(ctx, property) + " is not null";
                case Operator.OP_IN:
                    return GetPathRef(ctx, property) + " in (" + Param(ctx, value) + ")";
                case Operator.OP_NOT_IN:
                    return GetPathRef(ctx, property) + " not in (" + Param(ctx, value) + ")";
                case Operator.OP_EQUAL:
                    return GetPathRef(ctx, property) + " = " + Param(ctx, value);
                case Operator.OP_NOT_EQUAL:
                    return GetPathRef(ctx, property) + " != " + Param(ctx, value);
                case Operator.OP_GREATER_THAN:
                    return GetPathRef(ctx, property) + " > " + Param(ctx, value);
                case Operator.OP_LESS_THAN:
                    return GetPathRef(ctx, property) + " < " + Param(ctx, value);
                case Operator.OP_GREATER_OR_EQUAL:
                    return GetPathRef(ctx, property) + " >= " + Param(ctx, value);
                case Operator.OP_LESS_OR_EQUAL:
                    return GetPathRef(ctx, property) + " <= " + Param(ctx, value);
                case Operator.OP_LIKE:
                    return GetPathRef(ctx, property) + " like " + Param(ctx, value.ToString());
                case Operator.OP_ILIKE:
                    return "lower(" + GetPathRef(ctx, property) + ") like lower(" + Param(ctx, value.ToString()) + ")";
                case Operator.OP_AND:
                case Operator.OP_OR:
                    if (!(value is IList))
                    {
                        return null;
                    }

                    var op = filter.Operator == Operator.OP_AND ? " and " : " or ";

                    var sb = new StringBuilder("(");
                    var first = true;
                    foreach (var filterStr in ((IList)value).OfType<Filter>()
                        .Select(o => FilterToQL(ctx, o))
                        .Where(filterStr => filterStr != null))
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            sb.Append(op);
                        }
                        sb.Append(filterStr);
                    }

                    if (first)
                        return null;

                    sb.Append(")");
                    return sb.ToString();
                case Operator.OP_NOT:
                    if (!(value is Filter))
                    {
                        return null;
                    }
                    var filterStr2 = FilterToQL(ctx, (Filter)value);
                    if (filterStr2 == null)
                        return null;

                    return "not " + filterStr2;
                case Operator.OP_EMPTY:
                    metadata = _metadataUtil.Get(ctx.RootClass, property);
                    if (metadata.IsCollection())
                    {
                        return "not exists elements(" + GetPathRef(ctx, property) + ")";
                    }

                    if (metadata.IsString())
                    {
                        var pathRef = GetPathRef(ctx, property);
                        return "(" + pathRef + " is null or " + pathRef + " = '')";
                    }

                    return GetPathRef(ctx, property) + " is null";

                case Operator.OP_NOT_EMPTY:
                    metadata = _metadataUtil.Get(ctx.RootClass, property);
                    if (metadata.IsCollection())
                    {
                        return "exists elements(" + GetPathRef(ctx, property) + ")";
                    }

                    if (metadata.IsString())
                    {
                        var pathRef = GetPathRef(ctx, property);
                        return "(" + pathRef + " is not null and " + pathRef + " != '')";
                    }

                    return GetPathRef(ctx, property) + " is not null";
                case Operator.OP_SOME:
                    if (!(value is Filter))
                    {
                        return null;
                    }

                    var s1 = GenerateSimpleAllOrSome(ctx, property, (Filter)value, "some");
                    if (s1 != null)
                    {
                        return s1;
                    }

                    return "exists " + GenerateSubquery(ctx, property, (Filter)value);

                case Operator.OP_ALL:
                    if (!(value is Filter))
                    {
                        return null;
                    }

                    var s2 = GenerateSimpleAllOrSome(ctx, property, (Filter)value, "all");
                    if (s2 != null)
                    {
                        return s2;
                    }

                    return "not exists " + GenerateSubquery(ctx, property, Negate((Filter)value));

                case Filter.OP_NONE:
                    if (!(value is Filter))
                    {
                        return null;
                    }

                    // NOTE: Using "all" for the simple all or some is logically
                    // incorrect. It should be "some". However,
                    // because of a bug in how Hibernate 3.1.1 tries to simplify
                    // "not ... some/all ...) it actually ends
                    // up working as desired. TODO: If and when the Hibernate bug is
                    // fixed, this should be switched to "some".
                    var s3 = GenerateSimpleAllOrSome(ctx, property, (Filter)value, "all");
                    if (s3 != null)
                    {
                        return "not ( " + s3 + " )";
                    }

                    return "not exists " + GenerateSubquery(ctx, property, (Filter)value);

                case Filter.OP_CUSTOM:
                    var values = filter.GetValuesAsList() ?? new List<object>();

                    var sbCustom = new StringBuilder();
                    AppendCustomExpression(sbCustom, ctx, filter.Property, values);
                    return sbCustom.ToString();
                default:
                    throw new ArgumentException("Filter comparison ( " + theOperator + " ) is invalid.");
            }
        }

        /**
	 * Generate a QL string for a subquery on the given property that uses the
	 * given filter. This is used by SOME, ALL and NONE filters.
	 * 
	 * @param ctx
	 *            - a new context just for this sub-query
	 * @param property
	 *            - the property of the main query that points to the collection
	 *            on which to query
	 * @param filter
	 *            - the filter to use for the where clause of the sub-query
	 */
        protected string GenerateSubquery(SearchContext ctx, string property, Filter filter)
        {
            var ctx2 = new SearchContext
            {
                RootClass = _metadataUtil.Get(ctx.RootClass, property).GetClass(),
                ParamList = ctx.ParamList,
                NextAliasNum = ctx.NextAliasNum,
                NextSubqueryNum = ctx.NextSubqueryNum
            };
            ctx2.SetRootAlias(_rootAlias + (ctx.NextSubqueryNum++));

            var filters = new List<Filter>(1) { filter };
            var where = GenerateWhereClause(ctx2, filters, false);
            var joins = GenerateJoins(ctx2, false);
            ctx.NextAliasNum = ctx2.NextAliasNum;
            ctx.NextSubqueryNum = ctx2.NextSubqueryNum;

            var sb = new StringBuilder();
            sb.Append("(from ");
            sb.Append(GetPathRef(ctx, property));
            sb.Append(" ");
            sb.Append(ctx2.GetRootAlias());
            sb.Append(joins);
            sb.Append(where);
            sb.Append(")");

            return sb.ToString();
        }

        /**
         * <p>
         * In the case of simple ALL/SOME/NONE filters, a simpler hql syntax is used
         * (which is also compatible with collections of values). Simple filters
         * include ALL/SOME/NONE filters with exactly one sub-filter where that
         * filter applies to the elements of the collection directly (as opposed to
         * their properties) and the operator is =, !=, <, <=, >, or >=.
         * 
         * <p>
         * For example:
         * 
         * <pre>
         * Filter.some(&quot;some_collection_of_strings&quot;, Filter.equal(&quot;&quot;, &quot;Bob&quot;)
         * Filter.all(&quot;some_collection_of_numbers&quot;, Filter.greaterThan(null, 23)
         * </pre>
         * 
         * If the filter meets these criteria as a simple ALL/SOME/NONE filter, the
         * QL string for the filter will be returned. If not, <code>null</code> is
         * returned.
         * 
         * @param ctx
         *            - the context of the SOME/ALL/NONE filter
         * @param property
         *            - the property of the SOME/ALL/NONE filter
         * @param filter
         *            - the sub-filter that is the value of the SOME/ALL/NONE filter
         * @param operation
         *            - a string used to fill in the collection operation. The value
         *            should be either "some" or "all".
         */
        protected String GenerateSimpleAllOrSome(SearchContext ctx, String property, Filter filter, String operation)
        {
            //TODO to be validated, below intial
            //if (filter.Property != null && !filter.Property.Equals(""))
            if (String.IsNullOrEmpty(filter.Property))
                return null;

            string op;

            switch (filter.Operator)
            {
                case Operator.OP_EQUAL:
                    op = " = ";
                    break;
                case Operator.OP_NOT_EQUAL:
                    op = " != ";
                    break;
                case Operator.OP_LESS_THAN:
                    op = " > ";
                    break;
                case Operator.OP_LESS_OR_EQUAL:
                    op = " >= ";
                    break;
                case Operator.OP_GREATER_THAN:
                    op = " < ";
                    break;
                case Operator.OP_GREATER_OR_EQUAL:
                    op = " <= ";
                    break;
                default:
                    return null;
            }

            var value = InternalUtil.ConvertIfNeeded(filter.Value, _metadataUtil.Get(ctx.RootClass, property).GetClass());
            return Param(ctx, value) + op + operation + " elements(" + GetPathRef(ctx, property) + ")";
        }

        /**
         * Convert a property value to the expected type for that property. Ex. a
         * Long to and Integer.
         * 
         * @param isCollection
         *            <code>true</code> if the value is a collection of values, for
         *            example with IN and NOT_IN operators.
         * @return the converted value.
         */
        protected object PrepareValue(Type rootClass, string property, object value, bool isCollection)
        {
            if (value == null)
                return null;

            Type expectedClass;
            if (property != null && ("class".Equals(property) || property.EndsWith(".class")))
            {
                //TODO has to be validated
                expectedClass = typeof(object);
            }
            else if (property != null && ("size".Equals(property) || property.EndsWith(".size")))
            {
                expectedClass = typeof(Int32);
            }
            else
            {
                expectedClass = _metadataUtil.Get(rootClass, property).GetClass();
            }

            // convert numbers to the expected type if needed (ex: Integer to Long)
            if (isCollection)
            {
                // Check each element in the collection.
                object[] val2;

                if (value is ICollection)
                {
                    val2 = new object[((ICollection)value).Count];
                    var i = 0;
                    foreach (var item in (ICollection)value)
                    {
                        val2[i++] = InternalUtil.ConvertIfNeeded(item, expectedClass);
                    }
                }
                else
                {
                    val2 = new object[((object[])value).Length];
                    var i = 0;
                    foreach (var item in (object[])value)
                    {
                        val2[i++] = InternalUtil.ConvertIfNeeded(item, expectedClass);
                    }
                }
                return val2;
            }

            return InternalUtil.ConvertIfNeeded(value, expectedClass);
        }

        /**
         * Return a filter that negates the given filter.
         */
        protected Filter negate(Filter filter)
        {
            return Filter.Not(AddExplicitNullChecks(filter));
        }

        /**
         * Used by {@link #negate(Filter)}. There's a complication with null values
         * in the database so that !(x == 1) is not the opposite of (x == 1). Rather
         * !(x == 1 and x != null) is the same as (x == 1). This method applies the
         * null check explicitly to all filters included in the given filter tree.
         */
        protected Filter AddExplicitNullChecks(Filter filter)
        {
            return SearchUtil.WalkFilter(filter, new NullChecksFilterVisitor(), false);
        }

        /**
         * append a custom expression to the string builder, replacing any
         * property tokens (i.e "{prop}") with a reference to the property. 
         */
        protected void AppendCustomExpression(StringBuilder sb, SearchContext ctx, string expression)
        {
            var regex = new Regex(@"(\{[\w\.]*\})", RegexOptions.Compiled);
            var lastEnd = 0;
            foreach (Match matcher in regex.Matches(expression))
            {
                sb.Append(expression.Substring(lastEnd, matcher.Index));
                sb.Append(GetPathRef(ctx, expression.Substring(matcher.Index + 1, matcher.Length - 1)));
                lastEnd = matcher.Length;
            }
            sb.Append(expression.Substring(lastEnd));
        }

        /**
         * append a custom expression to the string builder, replacing any
         * property tokens (i.e "{prop}") with a reference to the property and
         * value tokens (i.e. "?n") with params. 
         */
        protected void AppendCustomExpression(StringBuilder sb, SearchContext ctx, string expression, IList values)
        {
            var regex = new Regex(@"(\{[\w\.]*\})|(\?\d+\b)", RegexOptions.Compiled);
            var lastEnd = 0;
            var expArray = expression.ToCharArray();
            foreach (Match matcher in regex.Matches(expression))
            {
                sb.Append(expression.Substring(lastEnd, matcher.Index));
                if (expArray[matcher.Index] == '{')
                {
                    sb.Append(GetPathRef(ctx, expression.Substring(matcher.Index + 1, matcher.Length - 1)));
                }
                else
                {
                    var valueIndex = int.Parse(expression.Substring(matcher.Index + 1, matcher.Length));
                    if (valueIndex == 0)
                    {
                        throw new ArgumentException("This custom filter expression (" + expression + ") contains a value placeholder for zero (?0), but placeholders should be numbered starting at ?1.");
                    }
                    if (values == null || values.Count == 0)
                    {
                        throw new ArgumentException("This custom filter expression (" + expression + ") calls for a value placeholder number " + valueIndex + ", but no values were specified.");
                    }
                    if (valueIndex > values.Count)
                    {
                        throw new ArgumentException("This custom filter expression (" + expression + ") calls for a value placeholder number " + valueIndex + ", but only " + values.Count + " values were specified.");
                    }
                    sb.Append(Param(ctx, values[valueIndex - 1]));
                }

                lastEnd = matcher.Length;
            }

            sb.Append(expression.Substring(lastEnd));
        }

        /**
         * Add value to paramList and return the named parameter string ":pX".
         */
        protected string Param(SearchContext ctx, object value)
        {
            if (value.GetType().IsClass && Type.GetTypeCode(value.GetType()) != TypeCode.String)
            {
                return value.GetType().Name;
            }

            if (value.GetType().IsAssignableFrom(typeof(IList)))
            {
                var sb = new StringBuilder();
                var first = true;
                foreach (var o in (ICollection)value)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(",");
                    }

                    ctx.ParamList.Add(o);
                    sb.Append(":p");
                    sb.Append(ctx.ParamList.Count);
                }
                return sb.ToString();
            }

            if (value.GetType().IsArray)
            {
                var sb = new StringBuilder();
                var first = true;
                foreach (var o in (object[])value)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(",");
                    }
                    ctx.ParamList.Add(o);
                    sb.Append(":p");
                    sb.Append(ctx.ParamList.Count);
                }
                return sb.ToString();
            }

            ctx.ParamList.Add(value);
            return ":p" + ctx.ParamList.Count;
        }

        /**
         * Given a full path to a property (ex. department.manager.salary), return
         * the reference to that property that uses the appropriate alias (ex.
         * a4_manager.salary).
         */
        protected String GetPathRef(SearchContext ctx, String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return ctx.GetRootAlias();
            }

            var parts = SplitPath(ctx, path);

            return GetOrCreateAlias(ctx, parts[0], false).Alias + "." + parts[1];
        }

        /**
         * Split a path into two parts. The first part will need to be aliased. The
         * second part will be a property of that alias. For example:
         * (department.manager.salary) would return [department.manager, salary].
         */
        protected String[] SplitPath(SearchContext ctx, String path)
        {
            if (String.IsNullOrEmpty(path))
                return null;

            var pos = path.LastIndexOf('.');

            if (pos == -1)
            {
                return new[] { "", path };
            }

            var lastSegment = path.Substring(pos + 1);
            var currentPath = path;
            var first = true;

            // Basically gobble up as many segments as possible until we come to
            // an entity. Entities must become aliases so we can use our left
            // join feature.
            // The exception is that if a segment is an id, we want to skip the
            // entity preceding it because (entity.id) is actually stored in the
            // same table as the foreign key.
            while (true)
            {
                if (_metadataUtil.IsId(ctx.RootClass, currentPath))
                {
                    // if it's an id property
                    // skip one segment
                    if (pos == -1)
                    {
                        return new [] { "", path };
                    }
                    pos = currentPath.LastIndexOf('.', pos - 1);
                }
                else if (!first && _metadataUtil.Get(ctx.RootClass, currentPath).IsEntity())
                {
                    // when we reach an entity (excluding the very first
                    // segment), we're done
                    return new [] { currentPath, path.Substring(currentPath.Length + 1) };
                }

                first = pos != -1 && lastSegment.Equals("size")
                        && _metadataUtil.Get(ctx.RootClass, currentPath.Substring(0, pos)).IsCollection();

                // For size, we need to go back to the 'first' behavior
                // for the next segment.

                // if that was the last segment, we're done
                if (pos == -1)
                {
                    return new [] { "", path };
                }
                // proceed to the next segment
                currentPath = currentPath.Substring(0, pos);
                pos = currentPath.LastIndexOf('.');
                lastSegment = pos == -1 ? currentPath : currentPath.Substring(pos + 1);
            }

            // 1st
            // if "id", go 2; try again
            // if component, go 1; try again
            // if entity, go 1; try again
            // if size, go 1; try 1st again

            // successive
            // if "id", go 2; try again
            // if component, go 1; try again
            // if entity, stop
        }

        /**
         * Given a full path to an entity (ex. department.manager), return the alias
         * to reference that entity (ex. a4_manager). If there is no alias for the
         * given path, one will be created.
         */
        protected AliasNode GetOrCreateAlias(SearchContext ctx, String path, bool setFetch)
        {
            var foundNode = GetAliasForPathIfItExists(ctx, path);

            if (foundNode != null)
            {
                if (setFetch)
                    SetFetchOnAliasNodeAndAllAncestors(foundNode);

                return foundNode;
            }

            var parts = SplitPath(ctx, path);

            var pos = parts[1].LastIndexOf('.');

            var alias = "a" + (ctx.NextAliasNum++) + "_" + (pos == -1 ? parts[1] : parts[1].Substring(pos + 1));

            var node = new AliasNode(parts[1], alias);

            // set up path recursively
            GetOrCreateAlias(ctx, parts[0], setFetch).AddChild(node);

            if (setFetch)
                SetFetchOnAliasNodeAndAllAncestors(node);

            ctx.Aliases.Add(path, node);

            return node;
        }

        /**
         * Given a full path to an entity (ex. department.manager), return the alias
         * to reference that entity (ex. a4_manager). If there is no alias for the
         * given path, one will be created.
         * 
         * @return the associated AliasNode or <code>null</code> if none.
         */
        protected AliasNode GetAliasForPathIfItExists(SearchContext ctx, String path)
        {
            AliasNode node;
            if (String.IsNullOrEmpty(path))
            {
                ctx.Aliases.TryGetValue(ROOT_PATH, out node);
                return node;
            }

            ctx.Aliases.TryGetValue(path, out node);
            return node;
        }

        protected void SetFetchOnAliasNodeAndAllAncestors(AliasNode node)
        {
            while (node.Parent != null)
            {
                node.Fetch = true;
                node = node.Parent;
            }
        }

        /**
	     * <ol>
	     * <li>Check for injection attack in property strings. <li>The field list
	     * may not contain nulls.
	     * </ol>
	     */
        protected IList<Field> CheckAndCleanFields(IList<Field> fields)
        {
            if (fields == null)
                return null;

            foreach (var field in fields)
            {
                if (field == null)
                {
                    throw new ArgumentException("The search contains a null field.");
                }
                if (field.Property != null && field.Operator != Field.OP_CUSTOM)
                    SecurityCheckProperty(field.Property);
            }

            return fields;
        }

        /**
	     * <ol>
	     * <li>Check for injection attack in property strings. <li>Remove null
	     * fetches from the list.
	     * </ol>
	     */
        protected IList<string> CheckAndCleanFetches(IList<String> fetches)
        {
            return SearchUtil.WalkList(fetches, new SecurityItemVisitor(), true);
        }

        /**
	     * Regex pattern for a valid property name/path.
	     */
        protected static Regex INJECTION_CHECK = new Regex(@"^[\w\.]*$", RegexOptions.Compiled);

        /**
	     * Used by <code>securityCheck()</code> to check a property string for
	     * injection attack.
	     */
        protected static void SecurityCheckProperty(String property)
        {
            if (property == null)
                return;
            if (!INJECTION_CHECK.IsMatch(property))
                throw new ArgumentException(
                        "A property used in a Search may only contain word characters (alphabetic, numeric and underscore \"_\") and dot \".\" seperators. This constraint was violated: "
                                + property);
        }

        /**
	     * <ol>
	     * <li>Check for injection attack in property strings. <li>Remove null sorts
	     * from the list.
	     * </ol>
	     */
	    protected IList<Sort> CheckAndCleanSorts(IList<Sort> sorts) {
		    return SearchUtil.WalkList(sorts, new SortItemVisitor(), true);
	    }

        /**
	     * <ol>
	     * <li>Check for injection attack in property strings. <li>Check for values
	     * that are incongruent with the operator. <li>Remove null filters from the
	     * list. <li>Simplify out junctions (and/or) that have only one sub-filter.
	     * <li>Remove filters that require sub-filters but have none
	     * (and/or/not/some/all/none)
	     * </ol>
	     */
        protected IList<Filter> CheckAndCleanFilters(IList<Filter> filters)
        {
            return SearchUtil.WalkFilters(filters, new CheckAndCleanFilterVisitor(), true);
        }

        private static readonly ExampleOptions defaultExampleOptions = new ExampleOptions();

	    public Filter GetFilterFromExample(object example) {
		    return GetFilterFromExample(example, null);
	    }

	    public Filter GetFilterFromExample(object example, ExampleOptions options) {
		    if (example == null)
			    return null;
		    if (options == null)
			    options = defaultExampleOptions;

		    var filters = new List<Filter>();
		    var path = new LinkedList<string>();
		    var metadata = _metadataUtil.Get(example.GetType());
		    GetFilterFromExampleRecursive(example, metadata, options, path, filters);

		    if (filters.Count == 0) {
			    return null;
		    } 
            
            return filters.Count == 1 ? filters[0] : new Filter("AND", filters, Filter.OP_AND);
	    }

	    private void GetFilterFromExampleRecursive(object example, IMetadata metadata, ExampleOptions options,
			    LinkedList<String> path, IList<Filter> filters) {
		    if (metadata.IsEntity() && !metadata.GetIdType().IsEmbeddable()) {
			    var id = metadata.GetIdValue(example);
			    if (id != null) {
				    filters.Add(Filter.Equal(ListToPath(path, "id"), id));
				    return;
			    }
		    }

		    foreach (var property in metadata.GetProperties()) {
			    if (options.ExcludeProps != null && options.ExcludeProps.Count != 0) {
				    if (options.ExcludeProps.Contains(ListToPath(path, property)))
					    continue;
			    }

			    var pMetadata = metadata.GetPropertyType(property);
			    if (pMetadata.IsCollection()) {
				    // ignore collections
			    } else {
				    var value = metadata.GetPropertyValue(example, property);
				    if (value == null) {
					    if (!options.ExcludeNulls) {
						    filters.Add(Filter.IsNull(ListToPath(path, property)));
					    }
				    } else if (options.ExcludeZeros &&  InternalUtil.IsNumeric(value) && ((IConvertible) value).ToInt64(CultureInfo.InvariantCulture) == 0) {
					    // ignore zeros
				    } else {
					    if (pMetadata.IsEntity() || pMetadata.IsEmbeddable()) {
						    path.AddLast(property);
						    GetFilterFromExampleRecursive(value, pMetadata, options, path, filters);
						    path.RemoveLast();
					    } else if (pMetadata.IsString()
							    && (options.LikeMode != ExampleOptions.EXACT || options.IgnoreCase)) {
						    var val = value.ToString();
						    switch (options.LikeMode) {
						    case ExampleOptions.START:
							    val = val + "%";
							    break;
						    case ExampleOptions.END:
							    val = "%" + val;
							    break;
						    case ExampleOptions.ANYWHERE:
							    val = "%" + val + "%";
							    break;
						    }
						    filters.Add(new Filter(ListToPath(path, property), val,
								    options.IgnoreCase ? Filter.OP_ILIKE : Filter.OP_LIKE));
					    } else {
						    filters.Add(Filter.Equal(ListToPath(path, property), value));
					    }
				    }
			    }
		    }
	    }

	    private static string ListToPath(IEnumerable<string> list, string lastProperty) {
		    var sb = new StringBuilder();
		    foreach (var prop in list) {
			    sb.Append(prop).Append(".");
		    }
		    sb.Append(lastProperty);
		    return sb.ToString();
	    }

        /**
	     * Return a filter that negates the given filter.
	     */
        protected Filter Negate(Filter filter)
        {
            return Filter.Not(AddExplicitNullChecks(filter));
        }

        protected sealed class CheckAndCleanFilterVisitor : FilterVisitor
        {
            public override Filter VisitBefore(Filter filter) {
				if (filter == null) {
					return null;
				}
				
				// If the operator needs a value and no value is specified, ignore this filter.
				// Only NULL, NOT_NULL, EMPTY, NOT_EMPTY and CUSTOM do not need a value.
				if (filter.Value == null && !filter.IsTakesNoValue()) {
					return null;
				}

                if (filter.Value == null) return filter;

                if (filter.IsTakesListOfSubFilters()) {
                    // make sure that filters that take lists of filters
                    // actually have lists of filters for their values
                    if (filter.Value is IList) {
                        foreach (var o in from object o in (IList) filter.Value where !(o is Filter) select o)
                        {
                            throw new ArgumentException(
                                "The search has a filter ("
                                + filter
                                + ") for which the value should be a List of Filters but there is an element in the list that is of type: "
                                + o.GetType());
                        }
                    } else {
                        throw new ArgumentException(
                            "The search has a filter ("
                            + filter
                            + ") for which the value should be a List of Filters but is not a list. The actual type is "
                            + filter.Value.GetType());
                    }
                } else if (filter.IsTakesSingleSubFilter()) {
                    // make sure filters that take filters actually have
                    // filters for their values
                    if (!(filter.Value is Filter)) {
                        throw new ArgumentException("The search has a filter (" + filter
                                                    + ") for which the value should be of type Filter but is of type: "
                                                    + filter.Value.GetType());
                    }
                }

                return filter;
			}

            public override Filter VisitAfter(Filter filter)
            {
                if (filter == null)
                    return null;

                if (filter.Operator == Filter.OP_CUSTOM)
                {
                    if (String.IsNullOrEmpty(filter.Property))
                    {
                        throw new ArgumentException("This search has a custom filter with no expression specified.");
                    }
                }
                else if (!filter.IsTakesNoProperty())
                {
                    SecurityCheckProperty(filter.Property);
                }

                // Remove operators that take sub filters but have none
                // assigned. Replace conjunctions that only have a single
                // sub-filter with that sub-filter.
                if (filter.IsTakesSingleSubFilter())
                {
                    if (filter.Value == null)
                    {
                        return null;
                    }
                }
                else if (filter.IsTakesListOfSubFilters())
                {
                    if (filter.Value == null)
                    {
                        return null;
                    }
                    
                    var list = filter.Value as IList<Filter>;
                    if (list == null || list.Count == 0)
                    {
                        return null;
                    }
                        
                    if (list.Count == 1)
                    {
                        return list[0];
                    }
                }

                return filter;
            }
        }

        protected sealed class SortItemVisitor : ItemVisitor<Sort>
        {
            public override Sort Visit(Sort sort)
            {
                if (!sort.CustomExpression)
                {
                    SecurityCheckProperty(sort.Property);
                }
                return sort;
            }
        }

        protected sealed class SecurityItemVisitor : ItemVisitor<string>
        {
            public override string Visit(string fetch)
            {
                SecurityCheckProperty(fetch);
                return fetch;
            }
        }

        protected sealed class AliasNode
        {
            private string _property;
            private string _alias;
            bool _fetch;
            private AliasNode _parent;
            private IList<AliasNode> _children = new List<AliasNode>();

            public AliasNode(string property, String alias)
            {
                _property = property;
                _alias = alias;
            }

            public string Property
            {
                get { return _property; }
                set { _property = value; }
            }

            public string Alias
            {
                get { return _alias; }
                set { _alias = value; }
            }

            public bool Fetch
            {
                get { return _fetch; }
                set { _fetch = value; }
            }

            public AliasNode Parent
            {
                get { return _parent; }
                set { _parent = value; }
            }

            public IList<AliasNode> Children
            {
                get { return _children; }
                set { _children = value; }
            }

            public void AddChild(AliasNode node)
            {
                _children.Add(node);
                node._parent = this;
            }

            public string GetFullPath()
            {
                if (_parent == null)
                    return "";

                if (_parent._parent == null)
                    return _property;

                return _parent.GetFullPath() + "." + _property;
            }
        }

        protected sealed class SearchContext
        {
            private Type _rootClass;
            private IDictionary<string, AliasNode> _aliases = new Dictionary<string, AliasNode>();
            private IList<object> _paramList;
            private int _nextAliasNum = 1;
            private int _nextSubqueryNum = 1;

            public SearchContext()
            {
            }

            public SearchContext(Type rootClass, string rootAlias, IList<object> paramList)
            {
                _rootClass = rootClass;
                SetRootAlias(rootAlias);
                _paramList = paramList;
            }

            public void SetRootAlias(string rootAlias)
            {
                _aliases.Add(ROOT_PATH, new AliasNode(ROOT_PATH, rootAlias));
            }

            public string GetRootAlias()
            {
                if (!_aliases.ContainsKey(ROOT_PATH)) return null;

                AliasNode node;
                _aliases.TryGetValue(ROOT_PATH, out node);

                return node != null ? node.Alias : null;
            }

            public IDictionary<string, AliasNode> Aliases
            {
                get { return _aliases; }
                set { _aliases = value; }
            }

            public Type RootClass
            {
                get { return _rootClass; }
                set { _rootClass = value; }
            }

            public IList<object> ParamList
            {
                get { return _paramList; }
                set { _paramList = value; }
            }

            public int NextAliasNum
            {
                get { return _nextAliasNum; }
                set { _nextAliasNum = value; }
            }

            public int NextSubqueryNum
            {
                get { return _nextSubqueryNum; }
                set { _nextSubqueryNum = value; }
            }
        }
    }

}

//3428076661 439773