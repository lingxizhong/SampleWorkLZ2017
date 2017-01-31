using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;

namespace DependencyGraphTestCases
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            DependencyGraph test = new DependencyGraph();
        }

        [TestMethod]
        public void SizeMethodTest()
        {
            DependencyGraph test = new DependencyGraph();
            int result = test.Size;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void BasicAddTest()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            int result = test.Size;
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void SecondaryAddTestMultiElement()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            int result = test.Size;
            Assert.AreEqual(4, result);

        }
    }
}
