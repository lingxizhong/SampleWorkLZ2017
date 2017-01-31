// Skeleton implementation written by Joe Zachary for CS 3500, January 2017.

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<String, HashSet<String>> dependents;
        private Dictionary<String, HashSet<String>> dependees;
        private int size;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            size = 0;
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            return dependents.ContainsKey(s);
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            return dependees.ContainsKey(s);
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if(s == null || HasDependents(s) == false)
            {
                HashSet<string> empty = new HashSet<string>();
                return empty;
            }
            HashSet<string> result = new HashSet<string>();
            dependents.TryGetValue(s, out result);
            return result;
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null || HasDependees(s) == false)
            {
                HashSet<string> empty = new HashSet<string>();
                return empty;
            }
            HashSet<string> result = new HashSet<string>();
            dependees.TryGetValue(s, out result);
            return result;
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if(s == null || t == null)
            {
                throw new NullReferenceException("Input cannot be null");
            }
            if(dependents.ContainsKey(s))
            {
                HashSet<string> tempDependentHashSet;
                dependents.TryGetValue(s, out tempDependentHashSet);
                tempDependentHashSet.Add(t);
            }
            else
            {
                HashSet<string> newDependentHashSet = new HashSet<string>();
                newDependentHashSet.Add(t);
                dependents.Add(s, newDependentHashSet);
            }

            if(dependees.ContainsKey(t))
            {
                HashSet<string> tempDependeeHashSet;
                dependees.TryGetValue(t, out tempDependeeHashSet);
                tempDependeeHashSet.Add(s);
            }
            else
            {
                HashSet<string> newDependeeHashSet = new HashSet<string>();
                newDependeeHashSet.Add(s);
                dependees.Add(t, newDependeeHashSet);
            }
            size++;
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new NullReferenceException("Input cannot be null");
            }
            if (dependents.ContainsKey(s))
            {
                HashSet<string> tempDependentHashSet;
                dependents.TryGetValue(s, out tempDependentHashSet);
                tempDependentHashSet.Remove(t);
            }
            if (dependees.ContainsKey(t))
            {
                HashSet<string> tempDependeeHashSet;
                dependees.TryGetValue(t, out tempDependeeHashSet);
                tempDependeeHashSet.Remove(s);
            }
            size--;
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            HashSet<string> valuesToBeRemoved;
            Boolean flag = dependents.TryGetValue(s, out valuesToBeRemoved);
            if (flag == false)
            {
                return;
            }          
            foreach(string removeThis in valuesToBeRemoved)
            {
                HashSet<string> temp;
                dependees.TryGetValue(removeThis, out temp);
                temp.Remove(s);
                size--;
            }
            dependents.Remove(s);
            foreach(string t in newDependents)
            {
                AddDependency(s, t);
                size++;
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            HashSet<string> valuesToBeRemoved;
            Boolean flag = dependees.TryGetValue(t, out valuesToBeRemoved);
            if (flag == false)
            {
                return;
            }
            foreach (string removeThis in valuesToBeRemoved)
            {
                HashSet<string> temp;
                dependents.TryGetValue(removeThis, out temp);
                temp.Remove(t);
            }
            dependees.Remove(t);
            foreach (string s in newDependees)
            {
                AddDependency(s, t);
            }
        }
    }
}
