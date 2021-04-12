using System;
using System.Collections.Generic;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        Dictionary<string, Dictionary<int, HashSet<int>>> dictionaryResult = new Dictionary<string, Dictionary<int, HashSet<int>>>();

        public void Add(int id, string documentText)
        {
            var array = documentText.Split(' ', '.', ',', '!', '?', ':', '-', '\r', '\n');
            var index = 0;

            foreach (var e in array)
            {
                if (!dictionaryResult.ContainsKey(e))
                {
                    var hashList = new HashSet<int>();
                    hashList.Add(index);
                    dictionaryResult.Add(e, new Dictionary<int, HashSet<int>>());
                    dictionaryResult[e].Add(id, hashList);
                }
                else if (!dictionaryResult[e].ContainsKey(id))
                {
                    dictionaryResult[e].Add(id, new HashSet<int>());
                    dictionaryResult[e][id].Add(index);
                }
                else
                {
                    dictionaryResult[e][id].Add(index);
                }
                index += e.Length + 1;
            }
        }

        public List<int> GetIds(string word)
        {
            if (dictionaryResult.ContainsKey(word))
            {
                return new List<int>(dictionaryResult[word].Keys);
            }
            return new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            if (dictionaryResult.ContainsKey(word) && dictionaryResult[word].ContainsKey(id))
            {
                return new List<int>(dictionaryResult[word][id]);
            }
            return new List<int>();
        }

        public void Remove(int id)
        {
            foreach (var word in dictionaryResult.Keys)
            {
                if (dictionaryResult[word].ContainsKey(id))
                {
                    dictionaryResult[word].Remove(id);
                }
            }
        }
    }
}