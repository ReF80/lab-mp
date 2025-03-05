public class Node<TKey, TValue> where TKey : IComparable<TKey>
{
    public List<TKey> Keys { get; set; } // Список ключей
    public List<TValue> Values { get; set; } // Список значений
    public List<Node<TKey, TValue>> Children { get; set; } // Список дочерних узлов
    public bool IsLeaf { get; set; } // Флаг, указывающий является ли узел листом

    public Node(bool isLeaf)
    {
        Keys = new List<TKey>();
        Values = new List<TValue>();
        Children = new List<Node<TKey, TValue>>();
        IsLeaf = isLeaf;
    }
}

// Сравнитель для ключей
public class Comparator<TKey> where TKey : IComparable<TKey>
{
    public bool LessThan(TKey a, TKey b)
    {
        return a.CompareTo(b) < 0;
    }

    public int Compare(TKey a, TKey b)
    {
        return a.CompareTo(b);
    }
}

// Основной класс BTreeMap
public class BTreeMap<TKey, TValue> where TKey : IComparable<TKey>
{
    private Node<TKey, TValue>? root;
    private int t; // Минимальная степень B-дерева
    private Comparator<TKey> comparator;
    
    public BTreeMap(int degree)
    {
        if (degree < 2)
            throw new ArgumentException("Минимальная степень B-дерева должна быть ≥ 2.");
        root = null;
        t = degree;
        comparator = new Comparator<TKey>();
    }
    
    public BTreeMap(BTreeMap<TKey, TValue> other)
    {
        root = CopyTree(other.root);
        t = other.t;
        comparator = new Comparator<TKey>();
    }
    
    public bool IsEmpty()
    {
        return root == null;
    }
    
    public void Clear()
    {
        DestroyTree(root);
        root = null;
    }

    // Добавление пары ключ-значение
    public void Add(TKey key, TValue value)
    {
        if (root == null)
        {
            root = new Node<TKey, TValue>(true);
            root.Keys.Add(key);
            root.Values.Add(value);
        }
        else
        {
            if (root.Keys.Count == (2 * t - 1))
            {
                var newRoot = new Node<TKey, TValue>(false);
                newRoot.Children.Add(root);
                SplitChild(newRoot, 0);
                InsertNonFull(newRoot, key, value);
                root = newRoot;
            }
            else
            {
                InsertNonFull(root, key, value);
            }
        }
    }

    // Получение значения по ключу через индексатор
    public TValue this[TKey key]
    {
        get
        {
            var value = Search(root, key);
            if (EqualityComparer<TValue>.Default.Equals(value, default(TValue)))
                throw new KeyNotFoundException("Ключ не найден.");
            return value;
        }
        set
        {
            var node = FindNode(root, key);
            if (node == null)
                Add(key, value); // Если ключ не существует, добавляем новую пару
            else
            {
                int index = node.Keys.BinarySearch(key, Comparer<TKey>.Default);
                if (index >= 0)
                    node.Values[index] = value; // Обновляем значение
            }
        }
    }

    // Поиск по ключу
    public bool ContainsKey(TKey key)
    {
        return Search(root, key) != null;
    }

    // Приватные методы для работы с деревом

    private void InsertNonFull(Node<TKey, TValue>? node, TKey key, TValue value)
    {
        int i = node.Keys.Count - 1;

        if (node.IsLeaf)
        {
            while (i >= 0 && comparator.LessThan(key, node.Keys[i]))
            {
                node.Keys[i + 1] = node.Keys[i];
                node.Values[i + 1] = node.Values[i];
                i--;
            }
            node.Keys[i + 1] = key;
            node.Values[i + 1] = value;
        }
        else
        {
            while (i >= 0 && comparator.LessThan(key, node.Keys[i]))
                i--;

            i++;

            if (node.Children[i].Keys.Count == (2 * t - 1))
            {
                SplitChild(node, i);
                if (comparator.LessThan(key, node.Keys[i]))
                    i--;
            }

            InsertNonFull(node.Children[i], key, value);
        }
    }

    private void SplitChild(Node<TKey, TValue>? parent, int index)
    {
        var child = parent.Children[index];
        var newNode = new Node<TKey, TValue>(child.IsLeaf);

        newNode.Keys.AddRange(child.Keys.GetRange(t, t - 1));
        newNode.Values.AddRange(child.Values.GetRange(t, t - 1));

        if (!child.IsLeaf)
        {
            newNode.Children.AddRange(child.Children.GetRange(t, t));
        }

        child.Keys.RemoveRange(t, child.Keys.Count - t);
        child.Values.RemoveRange(t, child.Values.Count - t);
        if (!child.IsLeaf)
            child.Children.RemoveRange(t, child.Children.Count - t);

        parent.Keys.Insert(index, child.Keys[t - 1]);
        parent.Values.Insert(index, child.Values[t - 1]);
        parent.Children.Insert(index + 1, newNode);
    }

    private TValue? Search(Node<TKey, TValue>? node, TKey key)
    {
        if (node == null)
            return default;

        int i = 0;
        while (i < node.Keys.Count && comparator.LessThan(key, node.Keys[i]))
            i++;

        if (i < node.Keys.Count && comparator.Compare(key, node.Keys[i]) == 0)
            return node.Values[i];

        if (node.IsLeaf)
            return default;

        return Search(node.Children[i], key);
    }

    private Node<TKey, TValue>? FindNode(Node<TKey, TValue>? node, TKey key)
    {
        if (node == null)
            return null;

        int i = 0;
        while (i < node.Keys.Count && comparator.LessThan(key, node.Keys[i]))
            i++;

        if (i < node.Keys.Count && comparator.Compare(key, node.Keys[i]) == 0)
            return node;

        if (node.IsLeaf)
            return null;

        return FindNode(node.Children[i], key);
    }

    private static Node<TKey, TValue>? CopyTree(Node<TKey, TValue>? node)
    {
        if (node == null)
            return null;

        var newNode = new Node<TKey, TValue>(node.IsLeaf)
        {
            Keys = new List<TKey>(node.Keys),
            Values = new List<TValue>(node.Values),
            Children = new List<Node<TKey, TValue>>()
        };

        foreach (var child in node.Children)
        {
            newNode.Children.Add(CopyTree(child));
        }

        return newNode;
    }

    private void DestroyTree(Node<TKey, TValue>? node)
    {
        if (node == null)
            return;

        foreach (var child in node.Children)
        {
            DestroyTree(child);
        }
    }
}