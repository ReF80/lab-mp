namespace Labs_MP2.lab3;

public class test_btree_map
{
    public static void testBtreeMap()
    {
        var btreeMap = new BTreeMap<string, int>(3); // Минимальная степень t = 3

        // Добавление элементов
        btreeMap.Add("apple", 5);
        btreeMap.Add("banana", 3);
        btreeMap.Add("cherry", 7);
        btreeMap.Add("date", 10);
        btreeMap.Add("elderberry", 15);

        // Получение значений
        Console.WriteLine($"Value for 'apple': {btreeMap["apple"]}");
        Console.WriteLine($"Value for 'banana': {btreeMap["banana"]}");

        // Изменение значения
        btreeMap["apple"] = 10;
        Console.WriteLine($"Updated value for 'apple': {btreeMap["apple"]}");

        // Проверка наличия ключа
        Console.WriteLine($"Contains 'cherry': {btreeMap.ContainsKey("cherry")}");
        Console.WriteLine($"Contains 'grape': {btreeMap.ContainsKey("grape")}");

        // Удаление всех элементов
        btreeMap.Clear();
        Console.WriteLine($"Is empty after clear: {btreeMap.IsEmpty()}");
    }
}