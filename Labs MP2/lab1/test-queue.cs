using Labs_MP2.lab1;

class testQueue
{
    public static void testLab1()
    {
        var priorityQueue = new PriorityQueue<int>();
        
        priorityQueue.Enqueue(10);
        priorityQueue.Enqueue(30);
        priorityQueue.Enqueue(20);
        priorityQueue.Enqueue(50);
        priorityQueue.Enqueue(40);
        priorityQueue.Enqueue(15);
        priorityQueue.Enqueue(25);
        priorityQueue.Enqueue(35);
        
        Console.WriteLine($"Количество элементов в очереди: {priorityQueue.Count()}");
        Console.WriteLine($"Максимальный элемент: {priorityQueue.Peek()}");
        Console.WriteLine($"Извлеченный элемент: {priorityQueue.Dequeue()}");
        Console.WriteLine($"Количество элементов после очистки: {priorityQueue.Count()}");
    }
}