using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.Common;
using NUnit.Framework;

namespace LComplete.Framework.UnitTests.Common
{
    [TestFixture]
    public class StringUtilsTests
    {
        [Test]
        public void TruncateWidth_Truncate_LengthRight()
        {
            string str = "大白鲨的复仇ABCD";
            string truncateStr = StringUtils.TruncateWidth(str, 14);
            Assert.AreEqual(8, truncateStr.Length);
        }
    }
}
