using System;
using System.IO;
using CapybaraToolkit.Filter;
using Xunit;

namespace CapybaraToolkitTests
{
    public class MxliffFilterTest
    {

        [Theory]
        [InlineData("T2")]
        public void TestParse(string tid)
        {
            var dataFolder = TestUtil.GetTestDataFolder();
            var sourceFile = Path.Combine(dataFolder, tid + ".input.mxliff");
            var filter = new MxliffFilter();
            var capyxliff = filter.Parse(sourceFile);
            var fileActual = Path.Combine(dataFolder, tid + ".output.capyxliff");
            capyxliff.Save(fileActual);

            var contentActual = TestUtil.LoadContentButIgnoreIds(fileActual);
            var fileExpected = Path.Combine(dataFolder, tid + ".expected.capyxliff");
            var contentExpected = TestUtil.LoadContentButIgnoreIds(fileExpected);
            Assert.Equal(contentExpected, contentActual);
        }
    }
}
