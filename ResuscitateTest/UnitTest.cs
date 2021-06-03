
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Resuscitate;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ResuscitateTest
{
    [TestClass]
    public class UnitTest1
    {
        [UITestMethod]
        public void TestMethod1()
        {
            MainPage mainPage = new MainPage();
            InputTime inputTime = new InputTime();
            Resuscitation resuscitation = new Resuscitation();
        }

        [TestMethod]
        public void Addition()
        {
            Assert.AreEqual(5, 3 + 2);
        }
    }
}
