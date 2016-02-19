// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016
//Revised by Neal Phelps 2/4/2016 U0669056 for CS 3500
//Updated by Neal Phelps on 2/11/2016

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
        //member variables for size and the new dictionary
        private int size;
        private Dictionary<String, HashSet<String>> dictionary;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dictionary = new Dictionary<string, HashSet<string>>();
            size = 0;
        }
        /// <summary>
        /// Creates a dependency graph based on the given dependency graph argument
        /// It will have the same dependencies and be identical, however they can be manipluated
        /// indiviudally without effecting the other
        /// </summary>
        /// <param name="dg"></param>
        public DependencyGraph(DependencyGraph dg)
        {
            Dictionary<string, HashSet<String>> d = dg.getDictionary();
            dictionary = new Dictionary<string, HashSet<string>>();
            foreach (KeyValuePair<string, HashSet<string>> pair in d)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
            size = dg.Size;
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// If s == null throws ArgumentNullException.
        /// </summary>
        public bool HasDependents(string s)
        {
            if(s == null)
            {
                throw new ArgumentNullException();
            }
            if (dictionary.ContainsKey(s))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty. If s == null throws ArgumentNullException.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            foreach (HashSet<String> set in dictionary.Values)
            {
                if (set.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  If s == null throws ArgumentNullException.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            HashSet<String> dependents = new HashSet<string>();
            //CHANGE
            try
            {
                dependents = dictionary[s];
                return dependents;
            }
            catch(KeyNotFoundException)
            {
                
            }
            return dependents;
        }

        /// <summary>
        /// Enumerates dependees(s).  If s == null throws ArgumentNullException.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            List<String> list = new List<String>();
            //get keys
            foreach(KeyValuePair<String, HashSet<String>> p in dictionary)
            {
                if (p.Value.Contains(s))
                {
                    list.Add(p.Key);
                }
            }
            return list;
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// If s or t == null throws ArgumentNullException.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException();
            }
            if (dictionary.ContainsKey(s))
            {
                var values = dictionary[s];
                if (values.Contains(t))
                {
                    return;
                }
                values.Add(t);
                size++;
            }
            else
            {
                HashSet<String> newSet = new HashSet<String>();
                newSet.Add(t);
                dictionary.Add(s, newSet);
                size++;
            }
        }
        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// If s or t == null throws ArgumentNullException.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException();
            }
            if (dictionary.ContainsKey(s))
            {
                var values = dictionary[s];
                if(values.Count == 1 && values.Contains(t))
                {
                    dictionary.Remove(s);
                    size--;
                }
                else
                {
                    HashSet<String> temp = new HashSet<string>();
                    foreach(String str in values)
                    {
                        if(str != t)
                        {
                            temp.Add(str);
                        }
                    }
                    dictionary.Remove(s);
                    dictionary.Add(s, temp);
                    size--;
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// If s or t == null throws ArgumentNullException.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null || newDependents == null)
            {
                throw new ArgumentNullException();
            }
            HashSet<String> temp = new HashSet<string>(); ;
            if (dictionary.ContainsKey(s))
            {
                dictionary.Remove(s);
                foreach (String str in newDependents)
                {
                    temp.Add(str);
                }
                dictionary.Add(s, temp);
                size++;
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// If s or t == null throws ArgumentNullException.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null || newDependees == null)
            {
                throw new ArgumentNullException();
            }
            //transfer newDependees to liest
            List<String> list = new List<string>();
            foreach(String str in dictionary.Keys)
            {
                list.Add(str);
            }
            foreach(String str in list)
            {
                RemoveDependency(str, t);
            }
            foreach(String str in newDependees)
            {
                AddDependency(str, t);
                size++;
            }
        }
        /// <summary>
        /// return the dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, HashSet<String>> getDictionary()
        {
            return dictionary;
        }
    }
}