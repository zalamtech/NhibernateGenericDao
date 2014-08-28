#region copyright
// ------------------------------------------------------------------------
// <copyright file="ExampleOptions.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
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