using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Com.Googlecode.Genericdao.Search.Test
{
    [TestClass]
    public class InternalUtilTest
    {
        [TestMethod]
        public void TestConvertIfNeeded()
        {
            Assert.AreEqual(null, InternalUtil.ConvertIfNeeded(null, typeof(Double)));

            Assert.AreEqual(13.0, InternalUtil.ConvertIfNeeded(13L, typeof(Double)));
            Assert.AreEqual(typeof(Double), InternalUtil.ConvertIfNeeded(13L, typeof(Double)).GetType());
            Assert.AreEqual(13.0, InternalUtil.ConvertIfNeeded(13, typeof(Double)));
            Assert.AreEqual(typeof(Double), InternalUtil.ConvertIfNeeded(13, typeof(Double)).GetType());
            Assert.AreEqual(47.5, InternalUtil.ConvertIfNeeded(47.5f, typeof(Double)));
            Assert.AreEqual(typeof(Double), InternalUtil.ConvertIfNeeded(47.5f, typeof(Double)).GetType());
            Assert.AreEqual(47.5, InternalUtil.ConvertIfNeeded(47.5, typeof(Double)));
            Assert.AreEqual(typeof(Double), InternalUtil.ConvertIfNeeded(47.5, typeof(Double)).GetType());
            Assert.AreEqual(47.5, InternalUtil.ConvertIfNeeded("47.5", typeof(Double)));
            Assert.AreEqual(typeof(Double), InternalUtil.ConvertIfNeeded("47.5", typeof(Double)).GetType());

            Assert.AreEqual(13L, InternalUtil.ConvertIfNeeded(13, typeof(Int64)));
            Assert.AreEqual(typeof(Int64), InternalUtil.ConvertIfNeeded(13, typeof(Int64)).GetType());
            Assert.AreEqual(13L, InternalUtil.ConvertIfNeeded(13L, typeof(Int64)));
            Assert.AreEqual(typeof(Int64), InternalUtil.ConvertIfNeeded(13L, typeof(Int64)).GetType());
            Assert.AreEqual(13L, InternalUtil.ConvertIfNeeded(13d, typeof(Int64)));
            Assert.AreEqual(typeof(Int64), InternalUtil.ConvertIfNeeded(13d, typeof(Int64)).GetType());
            Assert.AreEqual(13L, InternalUtil.ConvertIfNeeded(13f, typeof(Int64)));
            Assert.AreEqual(typeof(Int64), InternalUtil.ConvertIfNeeded(13f, typeof(Int64)).GetType());
            Assert.AreEqual(13L, InternalUtil.ConvertIfNeeded("13", typeof(Int64)));
            Assert.AreEqual(typeof(Int64), InternalUtil.ConvertIfNeeded("13", typeof(Int64)).GetType());

            Assert.AreEqual("13", InternalUtil.ConvertIfNeeded("13", typeof(String)));
            Assert.AreEqual(typeof(String), InternalUtil.ConvertIfNeeded("13", typeof(String)).GetType());
            Assert.AreEqual("13", InternalUtil.ConvertIfNeeded(13L, typeof(String)));
            Assert.AreEqual(typeof(String), InternalUtil.ConvertIfNeeded(13L, typeof(String)).GetType());
            Assert.AreEqual("13", InternalUtil.ConvertIfNeeded(13, typeof(String)));
            Assert.AreEqual(typeof(String), InternalUtil.ConvertIfNeeded(13, typeof(String)).GetType());
            Assert.AreEqual("13", InternalUtil.ConvertIfNeeded(13.0f, typeof(String)));
            Assert.AreEqual(typeof(String), InternalUtil.ConvertIfNeeded(13.0f, typeof(String)).GetType());
            Assert.AreEqual(typeof(string), InternalUtil.ConvertIfNeeded(typeof(string), typeof(string)).GetType());

            var actual1 = InternalUtil.ConvertIfNeeded(typeof (string), typeof (Type));
            Assert.AreEqual(typeof(string), actual1);
            var actual2 = InternalUtil.ConvertIfNeeded(typeof(object), typeof(Type));
            Assert.AreEqual(typeof(object), actual2);
        }
    }
}
