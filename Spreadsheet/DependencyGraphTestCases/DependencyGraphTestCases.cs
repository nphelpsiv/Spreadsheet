using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace DependencyGraphTestCases
{
    /// <summary>
    /// 
    /// Tests all methods of the dependencyGraph class
    /// </summary>
    [TestClass]
    public class DependencyGraphTestCases
    {
        /// <summary>
        /// Test if add dependencies are added correctly
        /// also confirms if HasDependees/Dependents corectly
        /// </summary>
        [TestMethod]
        public void TestAdd()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            Assert.IsTrue(d.HasDependents("a"));
            Assert.IsTrue(d.HasDependees("b"));
            Assert.IsFalse(d.HasDependents("c"));
            Assert.IsFalse(d.HasDependees("a"));
;        }

        /// <summary>
        /// Test if the size is decrementing and incrementing correctly
        /// </summary>
        [TestMethod]
        public void TestSize()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            Assert.AreEqual(4, d.Size);
            d.RemoveDependency("a", "b");
            Assert.AreEqual(3, d.Size);
            d.RemoveDependency("f", "g");
            Assert.AreEqual(3, d.Size);
        }

        /// <summary>
        ///Test if remoce dependencies are added correctly
        /// also confirms if HasDependees/Dependents corectly
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            d.RemoveDependency("a", "b");
            Assert.IsTrue(d.HasDependents("a"));
            Assert.IsFalse(d.HasDependees("b"));
            Assert.IsTrue(d.HasDependents("b"));
            Assert.IsFalse(d.HasDependents("c"));
            Assert.IsFalse(d.HasDependees("a"));
            d.RemoveDependency("d", "d");
            Assert.IsTrue(d.HasDependents("b"));
        }

        /// <summary>
        /// Test if ReplaceDependents correctly replaces
        /// </summary>
        [TestMethod]
        public void TestReplaceDependents()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            HashSet<String> temp = new HashSet<string>();
            IEnumerable<String> set = new HashSet<String>();
            temp.Add("e");
            temp.Add("f");
            d.ReplaceDependents("a", temp);
            set = d.GetDependents("a");
            bool isPass = false;
            if(Enumerable.SequenceEqual(temp, set))
            {
                isPass = true;
            }
            Assert.IsTrue(isPass);

        }
        /// <summary>
        /// Test if ReplaceDependees correctly replaces
        /// </summary>
        [TestMethod]
        public void TestReplaceDependees()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            HashSet<String> temp = new HashSet<string>();
            IEnumerable<String> set = new HashSet<String>();
            temp.Add("e");
            temp.Add("f");
            d.ReplaceDependees("d", temp);
            set = d.GetDependees("d");
            Assert.IsTrue(set.Contains("e"));
            Assert.IsTrue(set.Contains("f"));
            Assert.IsFalse(set.Contains("b"));

            d.ReplaceDependees("c", temp);
            set = d.GetDependees("c");
            Assert.IsTrue(set.Contains("e"));
            Assert.IsTrue(set.Contains("f"));
            Assert.IsFalse(set.Contains("a"));


        }

        /// <summary>
        /// Test with large sizes to see if handles well and size works correctly
        /// 
        /// </summary>
        [TestMethod]
        public void TestLargeSizes()
        {
            DependencyGraph d = new DependencyGraph();
            DependencyGraph d2 = new DependencyGraph();
            for (int i = 0; i < 1000000; i++)
            {
                d.AddDependency("a", "b");
            }
            Assert.AreEqual(1, d.Size);
            for (int i = 0; i < 1000000; i++)
            {
                d2.AddDependency(i.ToString(), "b");
            }
            Assert.AreEqual(1000000, d2.Size);
            for (int i = 0; i < 500000; i++)
            {
                d2.RemoveDependency(i + "", "b");
            }
            d2.RemoveDependency("a", "b");
            Assert.AreEqual(500000, d2.Size);
        }

        /// <summary>
        /// Test if it is getting correct dependees
        /// </summary>
        [TestMethod]
        public void TestGetDependees()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            IEnumerable<String> set = new HashSet<String>();
            //test is a is dependent of b
            set = d.GetDependees("b");
            bool isPass = false;
            foreach(String str in set)
            {
                if(str == "a")
                {
                    isPass = true;
                }
            }
            //test is a is dependee of c
            Assert.IsTrue(isPass);
            isPass = false;
            set = d.GetDependees("c");
            foreach (String str in set)
            {
                if (str == "a")
                {
                    isPass = true;
                }
            }
            Assert.IsTrue(isPass);
            //test is d is dependee of d
            Assert.IsTrue(isPass);
            isPass = false;
            set = d.GetDependees("d");
            foreach (String str in set)
            {
                if (str == "d" || str == "b")
                {
                    isPass = true;
                }
            }
            Assert.IsTrue(isPass);
        }

        /// <summary>
        /// Test if getting correct dependents
        /// </summary>
        [TestMethod]
        public void TestGetDependents()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            IEnumerable<String> set = new HashSet<String>();
            //test is b  or is dependee of a
            set = d.GetDependents("a");
            bool isPass = false;
            foreach (String str in set)
            {
                if (str == "b" || str == "c")
                {
                    isPass = true;
                }
            }
            //test is d is dependent of b
            Assert.IsTrue(isPass);
            isPass = false;
            set = d.GetDependents("b");
            foreach (String str in set)
            {
                if (str == "d")
                {
                    isPass = true;
                }
            }
            Assert.IsTrue(isPass);
            //test is d is dependent of d
            Assert.IsTrue(isPass);
            isPass = false;
            set = d.GetDependents("d");
            foreach (String str in set)
            {
                if (str == "d" || str == "b")
                {
                    isPass = true;
                }
            }
            Assert.IsTrue(isPass);
        }
    }

}
