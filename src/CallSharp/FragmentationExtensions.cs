﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CallSharp
{
  /// <summary>
  /// Contains extension methods that fragment a literal into its constituent
  /// parts for exploratory call chains. For example, 123 can be fragged into
  /// 1, 2, 3, 12, 23, 13, 100 and 20.
  /// </summary>
  public class FragmentationEngine
  {
    /// <summary>
    /// Finds all constituent parts of 
    /// </summary>
    /// <param name="source">The object to fragment.</param>
    /// <param name="partType">The type of each of the parts after fragmentation.</param>
    /// <returns></returns>
    public IEnumerable<object> Frag(object source, Type partType)
    {
      var s = source as string;
      if (s != null)
      {
        if (string.IsNullOrEmpty(s))
          yield break; // we have nothing to do here

        if (partType == typeof(string))
        {
          foreach (var part in FragToString(s))
            yield return part;
          foreach (var part in FragToInt(s))
            yield return part;
        }
        else if (partType == typeof(char))
        {
          foreach (var c in s.ToCharArray())
            yield return c;
        }
        else if (partType == typeof(char[]))
        {
          // every combination of characters in a string
          HashSet<char> allChars = new HashSet<char>(s.ToCharArray());
          for (int i = 1; i < allChars.Count; ++i)
            foreach (var x in PermuteUtils.Permute(allChars, i))
              yield return x.ToArray();
        }
      }

    }

    private IEnumerable<int> FragToInt(string text)
    {
      return Enumerable.Range(0, text.Length);
    }

    /// <summary>
    /// Every substring in a string.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private IEnumerable<string> FragToString(string text)
    {
      return Enumerable.Range(0, text.Length)
          .SelectMany(i => Enumerable.Range(0, text.Length - i + 1), (i, j) => new { i, j })
          .Where(t => t.j >= 2)
          .Select(t => text.Substring(t.i, t.j));
    }

  }
}