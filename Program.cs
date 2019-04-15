using System;
using System.Collections.Generic;
using System.Linq;
using ObjectsComparer;

namespace object_compare_test
{
    class Program
    {
        


        static void Main(string[] args)
        {
            
            var obj1 = new TestObject(new Dictionary<int, string>{
                {100, "a"},
                {200, "b"},
            },20);

            var obj2 = new TestObject(new Dictionary<int, string>{
                {200, "b"},
                {100, "a"},
            },20);

            var comparer = new ObjectsComparer.Comparer<TestObject>();
            comparer.AddComparerOverride<Dictionary<int, string>>(new DictValueComparer());
            comparer.Compare(obj1, obj2, out var differences);

            var d = differences;

            Console.WriteLine("Hello World!");
        }
    }

  public class DictValueComparer : AbstractValueComparer<Dictionary<int, string>>
  {
    public override bool Compare(Dictionary<int, string> obj1, 
    Dictionary<int, string> obj2, ComparisonSettings settings)
    {
        var differences = obj2.Where(entry => obj1[entry.Key] != entry.Value)
                 .ToDictionary(entry => entry.Key, entry => entry.Value);

        if(differences.Count == 0){
            return true;
        }
        return false;
    }
  }

  public class TestObject
    {
        public Dictionary<int, string> _dict;
        public int A;

        public TestObject(Dictionary<int, string> dict, int a) {
            _dict = dict;
            A = a;
        }
    }

  public class GenericComparer : AbstractValueComparer<IEnumerable<KeyValuePair<int, dynamic>>> {
    public override bool Compare(IEnumerable<KeyValuePair<int, dynamic>> obj1, IEnumerable<KeyValuePair<int, dynamic>> obj2, ComparisonSettings settings)
    {
        var obj1Dict = obj1.ToDictionary(entry => entry.Key, entry => entry.Value);
        var obj1List = obj1.Select(x => x.Key);

        var differences = obj2.Where(entry => obj1List.Contains(entry.Key)).ToList();

        if(differences.Count == 0){
            return true;
        }
        return false;
    }
  }
}
