namespace Labs_MP2.lab4;

public class test_hash_map
{
    public static void TestHashMap()
    {
        HashMap<string, int> hashMap = new HashMap<string, int>(5, 2.0);

        // Добавление элементов
        hashMap.Add("apple", 5);
        hashMap.Add("banana", 3);
        hashMap.Add("cherry", 7);

        // Получение значений
        Console.WriteLine($"Value for 'apple': {hashMap["apple"]}");
        Console.WriteLine($"Value for 'banana': {hashMap["banana"]}");

        // Изменение значения
        hashMap["apple"] = 10;
        Console.WriteLine($"Updated value for 'apple': {hashMap["apple"]}");

        // Проверка наличия ключа
        if (hashMap.TryGetValue("cherry", out int cherryValue))
            Console.WriteLine($"Value for 'cherry': {cherryValue}");
        else
            Console.WriteLine("Key 'cherry' not found.");

        // Удаление элемента
        if (hashMap.Remove("banana"))
            Console.WriteLine("Removed 'banana'.");
        else
            Console.WriteLine("Key 'banana' not found.");

        // Попытка получить удаленный ключ
        try
        {
            Console.WriteLine($"Value for 'banana': {hashMap["banana"]}");
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }

        // Информация о состоянии хеш-таблицы
        Console.WriteLine($"Number of elements: {hashMap.Count}");
        Console.WriteLine($"Load factor: {hashMap.LoadFactor:F2}");
        Console.WriteLine($"Max load factor: {hashMap.MaxLoadFactor:F2}");

        // Изменение коэффициента загруженности
        hashMap.MaxLoadFactor = 1.5;
        Console.WriteLine($"Updated max load factor: {hashMap.MaxLoadFactor:F2}");
    }
}