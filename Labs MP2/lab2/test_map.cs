namespace Labs_MP2.lab2;

public class test_map
{
    public static void testMap()
    {
        var map = new Map<int, string>();
        
        map.Add(1, "One");
        map.Add(2, "Two");
        map.Add(3, "Three");
        
        Console.WriteLine(map[1]); 
        Console.WriteLine(map[2]); 
        
        map[2] = "New Two";
        Console.WriteLine(map[2]); 
        
        Console.WriteLine(map.ContainsKey(3)); 
        Console.WriteLine(map.ContainsKey(4));
        
        map.Clear();
        Console.WriteLine(map.IsEmpty());
    }
}