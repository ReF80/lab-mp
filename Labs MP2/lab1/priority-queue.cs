namespace Labs_MP2.lab1;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> heap = [];

    private bool IsEmpty()
    {
        return heap.Count == 0;
    }
    
    public int Count()
    {
        return heap.Count;
    }
    
    public void Enqueue(T item)
    {
        heap.Add(item); 
        HeapifyUp(heap.Count - 1);
    }
    
    public T Dequeue()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Queue is empty.");
        
        Swap(0, heap.Count - 1);
        var maxItem = heap[heap.Count - 1]; // Сохраняем максимальный элемент
        heap.RemoveAt(heap.Count - 1); 
        
        HeapifyDown(0);

        return maxItem;
    }
    
    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Queue is empty.");
        
        return heap[0];
    }
    
    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2; 
            if (heap[index].CompareTo(heap[parentIndex]) <= 0)
                break; 

            Swap(index, parentIndex); 
            index = parentIndex; // Переходим к родительскому элементу
        }
    }
    
    private void HeapifyDown(int index)
    {
        int size = heap.Count;
        while (true)
        {
            int leftChildIndex = 2 * index + 1; 
            int rightChildIndex = 2 * index + 2; 
            int largest = index; // Предполагаем, что наибольший элемент — это текущий

            // Находим наибольший элемент среди текущего, левого и правого потомков
            if (leftChildIndex < size && heap[leftChildIndex].CompareTo(heap[largest]) > 0)
                largest = leftChildIndex;
            if (rightChildIndex < size && heap[rightChildIndex].CompareTo(heap[largest]) > 0)
                largest = rightChildIndex;

            if (largest == index)
                break; 

            Swap(index, largest); 
            index = largest; 
        }
    }
    
    private void Swap(int i, int j)
    {
        (heap[i], heap[j]) = (heap[j], heap[i]);
    }
}