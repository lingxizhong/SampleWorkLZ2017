// Skeleton implementation written by Joe Zachary for CS 3500, January 2017.
// Implmentation done by Lingxi Zhong U0770136 last edit 1/31/17

using System;
using System.Collections.Generic;
/// <summary>
/// Namespace for Dependency Graph
/// </summary>
namespace Dependencies
{
    /// <summary>
    /// Data structure to keep record of all dependencies for spreadsheets solution.
    /// Implementation by Lingxi Zhong U0770136
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Dictionary to hold dependents
        /// </summary>
        private Dictionary<String, HashSet<String>> dependents;
        /// <summary>
        /// Dictionary to hold dependees
        /// </summary>
        private Dictionary<String, HashSet<String>> dependees;
        /// <summary>
        /// Holds Size of Dependency Graph
        /// </summary>
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
        public DependencyGraph(DependencyGraph dg)
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            size = 0;
            foreach (KeyValuePair<string, HashSet<string>> copyValue in dependents)
            {
                foreach(string s in copyValue.Value)
                {
                    this.AddDependency(copyValue.Key, s);
                }
            }
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
            if(s == null)
            {
                throw new ArgumentNullException();
            }
            HashSet<string> emptyCheck = new HashSet<string>();
            Boolean result = dependents.TryGetValue(s, out emptyCheck);
            if (result == true)
            {
                if(emptyCheck.Count == 0)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            HashSet<string> emptyCheck = new HashSet<string>();
            Boolean result = dependees.TryGetValue(s, out emptyCheck);
            if (result == true)
            {
                if (emptyCheck.Count == 0)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if(s == null)
            {
                throw new ArgumentNullException();
            }
            if(HasDependents(s) == false)
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
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            if (HasDependees(s) == false)
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
                throw new ArgumentNullException();
            }
            Boolean flag = false;
            if(dependents.ContainsKey(s))
            {
                HashSet<string> tempDependentHashSet;
                dependents.TryGetValue(s, out tempDependentHashSet);
                flag = tempDependentHashSet.Add(t);
            }
            else
            {
                HashSet<string> newDependentHashSet = new HashSet<string>();
                flag = newDependentHashSet.Add(t);
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
            if(flag == true)
            {
                size++;
            }
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
                throw new ArgumentNullException();
            }
            if (!this.HasDependents(s) || !this.HasDependees(t))
            {
                return;
            }
            HashSet<string> tempDependentHashSet;
            dependents.TryGetValue(s, out tempDependentHashSet);
            tempDependentHashSet.Remove(t);
            HashSet<string> tempDependeeHashSet;
            dependees.TryGetValue(t, out tempDependeeHashSet);
            tempDependeeHashSet.Remove(s);
            size--;
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if(s == null || newDependents == null)
            {
                throw new ArgumentNullException();
            }
            HashSet<string> valuesToBeRemoved;
            Boolean flag = dependents.TryGetValue(s, out valuesToBeRemoved);
            if (flag == false)
            {
                valuesToBeRemoved = new HashSet<string>();
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
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if(t == null || newDependees == null)
            {
                throw new ArgumentNullException();
            }
            HashSet<string> valuesToBeRemoved;
            Boolean flag = dependees.TryGetValue(t, out valuesToBeRemoved);
            if (flag == false)
            {
                valuesToBeRemoved = new HashSet<string>();
            }
            foreach (string removeThis in valuesToBeRemoved)
            {
                HashSet<string> temp;
                dependents.TryGetValue(removeThis, out temp);
                temp.Remove(t);
                size--;
            }
            dependees.Remove(t);
            foreach (string s in newDependees)
            {
                AddDependency(s, t);
            }
        }
    }
}
