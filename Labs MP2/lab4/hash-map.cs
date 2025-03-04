using System;
using System.Collections.Generic;

// Класс HashMap, основанный на хеш-таблице с методом цепочек
public class HashMap<TKey, TValue>
{
    private List<List<KeyValuePair<TKey, TValue>>> buckets; // Вектор со списками элементов
    private int count; // Число элементов
    private int bucketCount; // Число списков
    private double maxLoadFactor; // Максимальный уровень загруженности

    // Конструктор
    public HashMap(int initialBucketCount = 10, double maxLoadFactor = 2.0)
    {
        if (initialBucketCount <= 0)
            throw new ArgumentException("Initial bucket count must be positive.");
        if (maxLoadFactor <= 0)
            throw new ArgumentException("Max load factor must be positive.");

        buckets = new List<List<KeyValuePair<TKey, TValue>>>(initialBucketCount);
        for (int i = 0; i < initialBucketCount; i++)
            buckets.Add(new List<KeyValuePair<TKey, TValue>>());

        count = 0;
        bucketCount = initialBucketCount;
        this.maxLoadFactor = maxLoadFactor;
    }

    // Удаление всех элементов
    public void Clear()
    {
        foreach (var bucket in buckets)
            bucket.Clear();
        count = 0;
    }

    // Добавление пары ключ-значение
    public void Add(TKey key, TValue value)
    {
        int index = GetBucketIndex(key);

        // Проверяем, существует ли уже такой ключ
        var bucket = buckets[index];
        for (int i = 0; i < bucket.Count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(bucket[i].Key, key))
            {
                bucket[i] = new KeyValuePair<TKey, TValue>(key, value); // Обновляем значение
                return;
            }
        }

        // Если ключ не найден, добавляем новую пару
        bucket.Add(new KeyValuePair<TKey, TValue>(key, value));
        count++;

        // Перехеширование, если уровень загруженности превышает максимальный
        if ((double)count / bucketCount > maxLoadFactor)
            ResizeBuckets(2 * bucketCount + 1);
    }

    // Получение значения по ключу через индексатор
    public TValue this[TKey key]
    {
        get
        {
            TValue value;
            if (TryGetValue(key, out value))
                return value;
            throw new KeyNotFoundException("Key not found in the hash map.");
        }
        set
        {
            Add(key, value);
        }
    }

    // Удаление элемента по ключу
    public bool Remove(TKey key)
    {
        int index = GetBucketIndex(key);
        var bucket = buckets[index];

        for (int i = 0; i < bucket.Count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(bucket[i].Key, key))
            {
                bucket.RemoveAt(i);
                count--;
                return true;
            }
        }

        return false; // Ключ не найден
    }

    // Получение числа элементов
    public int Count => count;

    // Получение текущего уровня загруженности
    public double LoadFactor => (double)count / bucketCount;

    // Получение и изменение коэффициента загруженности
    public double MaxLoadFactor
    {
        get => maxLoadFactor;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Max load factor must be positive.");
            maxLoadFactor = value;

            // Если текущий уровень загруженности превышает новый максимальный, перехешируем
            if (LoadFactor > maxLoadFactor)
                ResizeBuckets(bucketCount);
        }
    }

    // Попытка получить значение по ключу
    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = GetBucketIndex(key);
        var bucket = buckets[index];

        foreach (var pair in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
            {
                value = pair.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    // Закрытый метод перехеширования
    private void ResizeBuckets(int newBucketCount)
    {
        if (newBucketCount < bucketCount)
            return; // Новое число списков меньше текущего

        var oldBuckets = buckets;
        buckets = new List<List<KeyValuePair<TKey, TValue>>>(newBucketCount);
        for (int i = 0; i < newBucketCount; i++)
            buckets.Add(new List<KeyValuePair<TKey, TValue>>());

        count = 0;
        bucketCount = newBucketCount;

        foreach (var bucket in oldBuckets)
        {
            foreach (var pair in bucket)
            {
                Add(pair.Key, pair.Value); // Перехешируем каждый элемент
            }
        }
    }

    // Вычисление индекса корзины для ключа
    private int GetBucketIndex(TKey key)
    {
        return Math.Abs(key.GetHashCode()) % bucketCount;
    }
}