
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Resuscitate;

namespace ResuscitateTest
{
    [TestClass]
    public class UnitTest1
    {
        [UITestMethod]
        public void TestMethod1()
        {
            MainPage mp = new MainPage();
        }
    }
}
