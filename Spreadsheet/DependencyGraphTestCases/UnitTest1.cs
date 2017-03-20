using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Dependencies;
/// <summary>
/// Tests Written by Lingxi Zhong last edit 1/31/17
/// U0770136
/// </summary>
namespace DependencyGraphTestCases
{
    /// <summary>
    /// Tests for the class "Dependency Graph"
    /// </summary>
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
        [ExpectedException(typeof(ArgumentNullException))]
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
        [ExpectedException(typeof(ArgumentNullException))]
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
        public void GetDependentsWithEmptyDependent()
        {
            DependencyGraph test = new DependencyGraph();
            var result = test.GetDependents("s");
            string result2 = "";
            foreach(string q in result)
            {
                result2 = result2 + q;
            }
            Assert.AreEqual("", result2);

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
            Assert.AreEqual(4, result);
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


        // This is not a unit test for a reason, I found that a mixture of successive method calls can produce some bugs
        [TestMethod]
        public void AlmightySuperMegaSuccesiveCallMultiMethodTest()
        {
            // setup
            DependencyGraph data = new DependencyGraph();
            data.AddDependency("a", "b");
            data.AddDependency("a", "c");
            data.AddDependency("b", "d");
            data.AddDependency("d", "d");
            data.AddDependency("a", "g");
            data.AddDependency("s", "t");
            //part1 (p1 vars) (remove element s, t)
            data.RemoveDependency("s", "t");
            int p1Size = data.Size; // should be 5
            Boolean p1Bool = data.HasDependents("s"); // should be false
            var p1Var = data.GetDependees("t"); // should be empty
            string p1StrResult = "";
            foreach(string p1str in p1Var)
            {
                p1StrResult = p1StrResult + p1str;
            }
            Assert.AreEqual(5, p1Size);
            Assert.AreEqual(false, p1Bool);
            Assert.AreEqual("", p1StrResult);
            //part2 (p2 vars) replace s with list that has t2
            HashSet<string> p2HashInput = new HashSet<string>();
            p2HashInput.Add("t2");
            data.ReplaceDependents("s", p2HashInput);
            int p2Size = data.Size; //Should be 6 
            Boolean p2EETResult = data.HasDependees("t"); // Should be false
            Boolean p2ENTResult = data.HasDependents("s"); // Should be true
            var p2Var = data.GetDependees("t2"); //should contain "s"
            string p2StrResult = "";
            foreach(string p2Str in p2Var)
            {
                p2StrResult = p2StrResult + p2Str;
            }
            Assert.AreEqual(6, p2Size);
            Assert.AreEqual(false, p2EETResult);
            Assert.AreEqual(true, p2ENTResult);
            Assert.AreEqual("s", p2StrResult);
            //part3 (p3 vars) delete with replace. Setup for remove call in p4
            HashSet<string> empty = new HashSet<string>();
            data.ReplaceDependents("s", empty);
            int p3Size = data.Size; //should be 5
            Boolean p3ENTBool = data.HasDependents("s"); // should be false
            Boolean p3EETBool = data.HasDependees("t2"); // should be false
            Assert.AreEqual(5, p3Size);
            Assert.AreEqual(false, p3ENTBool);
            Assert.AreEqual(false, p3EETBool);
            //part4 (p4 vars) remove nonexistent replaced element s, t2; shouldn't error out or change size
            data.RemoveDependency("s", "t2");
            int p4Size = data.Size; // should still be 5
            Assert.AreEqual(5, p4Size);
            //part5 (p5 vars) replace dependees Big Call
            HashSet<string> p5Hash = new HashSet<string>();
            p5Hash.Add("a");
            p5Hash.Add("b");
            p5Hash.Add("c");
            data.ReplaceDependees("d", p5Hash);
            int p5Size = data.Size; // should be 6
            Boolean p5Bool = data.HasDependents("d"); //should be false
            Assert.AreEqual(6, p5Size);
            Assert.AreEqual(false, p5Bool);
            var p5Var1 = data.GetDependents("b");
            string p5result1 = "";
            foreach(string p5str1 in p5Var1)
            {
                p5result1 = p5result1 + p5str1;
            }
            Assert.AreEqual("d", p5result1);
            var p5Var2 = data.GetDependents("a");
            string p5result2 = "";
            foreach(string p5str2 in p5Var2)
            {
                p5result2 = p5result2 + p5str2;
            }
            Assert.AreEqual("bcgd", p5result2);
            var p5Var3 = data.GetDependees("d");
            string p5result3 = "";
            foreach(string p5str3 in p5Var3)
            {
                p5result3 = p5result3 + p5str3;
            }
            Assert.AreEqual("abc", p5result3);
            //part 6 replaceDependants Big Call
            HashSet<string> p6Hash = new HashSet<string>();
            p6Hash.Add("d");
            p6Hash.Add("g");
            p6Hash.Add("c");
            p6Hash.Add("b");
            p6Hash.Add("t");
            p6Hash.Add("t2");
            data.ReplaceDependents("d", p6Hash);
            int p6Size = data.Size; // Should be 12
            Assert.AreEqual(12, p6Size);
            var p6Var1 = data.GetDependents("d");
            string p6Result1 = "";
            foreach(string p6str1 in p6Var1)
            {
                p6Result1 = p6Result1 + p6str1;
            }
            Assert.AreEqual("dgcbtt2", p6Result1);
            var p6Var2 = data.GetDependees("d");
            string p6Result2 = "";
            foreach(string p6str2 in p6Var2)
            {
                p6Result2 = p6Result2 + p6str2;
            }
            Assert.AreEqual("abcd", p6Result2);
            var p6Var3 = data.GetDependees("g");
            string p6Result3 = "";
            foreach(string p6str3 in p6Var3)
            {
                p6Result3 = p6Result3 + p6str3;
            }
            Assert.AreEqual("ad", p6Result3);
            var p6Var4 = data.GetDependees("t2");
            string p6Result4 = "";
            foreach (string p6str4 in p6Var4)
            {
                p6Result4 = p6Result4 + p6str4;
            }
            Assert.AreEqual("d", p6Result4);
            //part 7
            data.ReplaceDependees("t", empty);
            int p7Size = data.Size;
            Assert.AreEqual(11, p7Size);
            Boolean p7Bool = data.HasDependees("t");
            Assert.AreEqual(false, p7Bool);
            //part 8
            HashSet<string> p8Hash = new HashSet<string>();
            p8Hash.Add("t");
            p8Hash.Add("t2");
            data.ReplaceDependents("a", p8Hash);
            int p8Size = data.Size;
            Assert.AreEqual(9, p8Size);
            var p8Var1 = data.GetDependees("d");
            string p8StrResult1 = "";
            foreach(string p8Str1 in p8Var1)
            {
                p8StrResult1 = p8StrResult1 + p8Str1;
            }
            Assert.AreEqual("bcd", p8StrResult1);
            var p8Var2 = data.GetDependees("g");
            string p8StrResult2 = "";
            foreach(string p8Str2 in p8Var2)
            {
                p8StrResult2 = p8StrResult2 + p8Str2;
            }
            Assert.AreEqual("d", p8StrResult2);
            var p8Var3 = data.GetDependees("t2");
            string p8StrResult3 = "";
            foreach(string p8Str3 in p8Var3)
            {
                p8StrResult3 = p8StrResult3 + p8Str3;
            }
            Assert.AreEqual("da", p8StrResult3);
            var p8Var4 = data.GetDependees("t");
            string p8StrResult4 = "";
            foreach (string p8Str4 in p8Var4)
            {
                p8StrResult4 = p8StrResult4 + p8Str4;
            }
            Assert.AreEqual("a", p8StrResult4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNull() //Will it compile?
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNull()
        {
            DependencyGraph test = new DependencyGraph();
            test.RemoveDependency(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDependeesNull()
        {
            DependencyGraph test = new DependencyGraph();
            test.GetDependees(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDependentsNull()
        {
            DependencyGraph test = new DependencyGraph();
            test.GetDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HasDependents()
        {
            DependencyGraph test = new DependencyGraph();
            test.HasDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HasDependees()
        {
            DependencyGraph test = new DependencyGraph();
            test.HasDependees(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceDependents()
        {
            DependencyGraph test = new DependencyGraph();
            test.ReplaceDependents(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceDependees()
        {
            DependencyGraph test = new DependencyGraph();
            test.ReplaceDependees(null, null);
        }

        [TestMethod]
        public void CloneConstructorTestWithEmptyReferenceTest()
        {
            DependencyGraph test = new DependencyGraph();
            DependencyGraph clone = new DependencyGraph(test);
            clone.AddDependency("s", "t");
            Boolean result1 = test.HasDependents("s");
            Assert.AreEqual(false, result1);
        }

        [TestMethod]
        public void CloneConstructorTestWithFullList()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("1", "2");
            test.AddDependency("2", "3");
            DependencyGraph clone = new DependencyGraph(test);
            Boolean result1 = clone.HasDependents("1");
            Boolean result2 = clone.HasDependees("3");
            Assert.AreEqual(true, result1);
            Assert.AreEqual(true, result2);
            clone.RemoveDependency("1", "2");
            Boolean result3 = test.HasDependents("1");
            Assert.AreEqual(true, result3);
        }

        [TestMethod]
        public void CloneConstructorStressTest()
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
            DependencyGraph stressClone = new DependencyGraph(stress);
            Boolean result = stressClone.HasDependents("x99999");
            Assert.AreEqual(true, result);
        }
    }
}
