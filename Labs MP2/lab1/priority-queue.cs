// Класс Priority Queue, который реализует адаптер для управления приоритетами
namespace Labs_MP2.lab1;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> _heap = [];

    private bool IsEmpty()
    {
        return _heap.Count == 0;
    }
    
    public int Count()
    {
        return _heap.Count;
    }
    
    public void Enqueue(T item)
    {
        _heap.Add(item); 
        HeapifyUp(_heap.Count - 1);
    }
    
    public T Dequeue()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Queue is empty.");
        
        Swap(0, _heap.Count - 1);
        var maxItem = _heap[_heap.Count - 1]; // Сохраняем максимальный элемент
        _heap.RemoveAt(_heap.Count - 1); 
        
        HeapifyDown(0);

        return maxItem;
    }
    
    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Queue is empty.");
        
        return _heap[0];
    }
    
    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2; // Находим индекс родительского элемента
            if (_heap[index].CompareTo(_heap[parentIndex]) <= 0)
                break; // Если текущий элемент меньше или равен родительскому, завершаем

            Swap(index, parentIndex); // Иначе меняем местами
            index = parentIndex; // Переходим к родительскому элементу
        }
    }
    
    private void HeapifyDown(int index)
    {
        int size = _heap.Count;
        while (true)
        {
            int leftChildIndex = 2 * index + 1; 
            int rightChildIndex = 2 * index + 2; 
            int largest = index; // Предполагаем, что наибольший элемент — это текущий

            // Находим наибольший элемент среди текущего, левого и правого потомков
            if (leftChildIndex < size && _heap[leftChildIndex].CompareTo(_heap[largest]) > 0)
                largest = leftChildIndex;
            if (rightChildIndex < size && _heap[rightChildIndex].CompareTo(_heap[largest]) > 0)
                largest = rightChildIndex;

            if (largest == index)
                break; 

            Swap(index, largest); 
            index = largest; 
        }
    }
    
    private void Swap(int i, int j)
    {
        (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
    }
}