using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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

        [TestMethod]
        public void DuplicateAddFunc()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            test.AddDependency("a", "b");
            int result = test.Size;
            Assert.AreEqual(4, result);
        }
        [TestMethod]
        public void HasDepdendentsShouldReturnTrue()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            Boolean result = test.HasDependents("a");
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void HasDependantsElementNonExistent()
        {
            DependencyGraph test = new DependencyGraph();
            Boolean result = test.HasDependents("A");
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void HasDependantsWithNullElement()
        {
            DependencyGraph test = new DependencyGraph();
            test.HasDependents(null);
        } 
        
        [TestMethod]
        public void HasDependeeShouldReturnTrue()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            Boolean result = test.HasDependees("d");
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void HasDependeeShouldReturnFalse()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            Boolean result = test.HasDependees("a");
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void HasDependeeWithNullInput()
        {
            DependencyGraph test = new DependencyGraph();
            test.HasDependees(null);
        } 

        [TestMethod]
        public void GetDependentsBasicFuncTest()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            var temp = test.GetDependents("a");
            string result = "";
            foreach(string q in temp)
            {
                result = result + q;
            }
            Assert.AreEqual("bc", result);
        }

        [TestMethod]
        public void GetDependeesBasicFuncTest()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            var temp = test.GetDependees("d");
            string result = "";
            foreach(string q in temp)
            {
                result = result + q;
            }
            Assert.AreEqual("bd", result);
        }

        [TestMethod]
        public void GetDependeesBasicFunTestNoDependee()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            var temp = test.GetDependees("a");
            string result = "";
            foreach(string q in temp)
            {
                result = result + q;
            }
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void RemoveDependencyBasicFuncTest()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            test.RemoveDependency("a", "b");
            int result = test.Size;
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void RemoveNonExistentElementTest()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            test.RemoveDependency("z", "b");
            int result = test.Size;
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void ReplaceDependantsBasicTest()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            HashSet<string> temp = new HashSet<string>();
            temp.Add("f");
            temp.Add("g");
            test.ReplaceDependents("a", temp);
            int result = test.Size;
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void ReplaceDependentsTestWithNonExistantCurrentDependant()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            HashSet<string> temp = new HashSet<String>();
            temp.Add("f");
            temp.Add("g");
            test.ReplaceDependents("c", temp);
            int result = test.Size;
            Assert.AreEqual(6, result);
            var temp2 = test.GetDependents("c");
            string result2 = "";
            foreach(string q in temp2)
            {
                result2 = result2 + q;
            }
            Assert.AreEqual("fg", result2);
        }

        [TestMethod]
        public void ReplaceDependeesBasicTest()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            HashSet<string> temp = new HashSet<string>();
            temp.Add("a");
            temp.Add("c");
            test.ReplaceDependees("d", temp);
            int result = test.Size;
            Assert.AreEqual(6, result);
            var result2 = test.GetDependees("d");
            string result3 = "";
            foreach (string q in result2)
            {
                result3 = result3 + q;
            }
            Assert.AreEqual("ac", result3);
        }

        [TestMethod]
        public void ReplaceDependeesTestWithEmptyDependee()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("a", "b");
            test.AddDependency("a", "c");
            test.AddDependency("b", "d");
            test.AddDependency("d", "d");
            HashSet<string> temp = new HashSet<string>();
            temp.Add("a");
            temp.Add("c");
            test.ReplaceDependees("a", temp);
            int result = test.Size;
            Assert.AreEqual(6, result);
            var result2 = test.GetDependees("a");
            string result3 = "";
            foreach (string q in result2)
            {
                result3 = result3 + q;
            }
            Assert.AreEqual("ac", result3);
        }

        [TestMethod]
        public void AddStressTest()
        {
            HashSet<String> randomStrings = new HashSet<string>();
            DependencyGraph stress = new DependencyGraph();
            for(int i = 0; i < 100000; i++)
            {
                randomStrings.Add("x" + i);
            }
            foreach (string q in randomStrings)
            {
                stress.AddDependency(q, q);
            }
        }

        [TestMethod]
        public void RemoveStressTest()
        {
            HashSet<String> randomStrings = new HashSet<string>();
            DependencyGraph stress = new DependencyGraph();
            for (int i = 0; i < 100000; i++)
            {
                randomStrings.Add("x" + i);
            }
            foreach (string q in randomStrings)
            {
                stress.AddDependency(q, q);
            }
            foreach (string p in randomStrings)
            {
                stress.RemoveDependency(p, p);
            }
        }

        [TestMethod]
        public void replaceDependentStressTest()
        {
            HashSet<String> randomStrings = new HashSet<string>();
            DependencyGraph stress = new DependencyGraph();
            for (int i = 0; i < 100000; i++)
            {
                randomStrings.Add("x" + i);
            }
            stress.ReplaceDependents("q", randomStrings);
        }

        [TestMethod]
        public void replaceDependeeStressTest()
        {
            HashSet<String> randomStrings = new HashSet<string>();
            DependencyGraph stress = new DependencyGraph();
            for (int i = 0; i < 100000; i++)
            {
                randomStrings.Add("x" + i);
            }
            stress.ReplaceDependees("q", randomStrings);
        }
    }
}
