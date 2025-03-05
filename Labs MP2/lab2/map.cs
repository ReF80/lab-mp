namespace Labs_MP2.lab2;

public class Node<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }
    public Node<TKey, TValue>? Parent { get; set; }
    public Node<TKey, TValue>? Left { get; set; }
    public Node<TKey, TValue>? Right { get; set; }
    public bool IsRed { get; set; } = true; // По умолчанию новый узел красный

    public Node(TKey key, TValue value)
    {
        Key = key;
        Value = value;
        Parent = null;
        Left = null;
        Right = null;
    }
}

// Сравнитель для ключей
public class Comparator<TKey> where TKey : IComparable<TKey>
{
    public bool LessThan(TKey a, TKey b)
    {
        return a.CompareTo(b) < 0;
    }
}

public class Map<TKey, TValue> where TKey : IComparable<TKey>
{
    private Node<TKey, TValue>? root;
    private Comparator<TKey> comparator;
    
    public Map()
    {
        root = null;
        comparator = new Comparator<TKey>();
    }
    
    public Map(Map<TKey, TValue> other)
    {
        root = CopyTree(other.root);
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
    
    public void Add(TKey key, TValue value)
    {
        if (root == null)
        {
            root = new Node<TKey, TValue>(key, value)
            {
                IsRed = false // Корень всегда черный
            };
        }
        else
        {
            InsertNode(new Node<TKey, TValue>(key, value));
        }
    }

    // Получение значения по ключу через индексатор
    public TValue this[TKey key]
    {
        get
        {
            var node = FindNode(key);
            if (node == null)
                throw new KeyNotFoundException("Ключ не найден.");
            return node.Value;
        }
        set
        {
            var node = FindNode(key);
            if (node == null)
                Add(key, value); // Если ключ не существует, добавляем новую пару
            else
                node.Value = value; // Иначе обновляем значение
        }
    }

   
    public bool ContainsKey(TKey key)
    {
        return FindNode(key) != null;
    }

    private Node<TKey, TValue>? FindNode(TKey key)
    {
        var current = root;
        while (current != null)
        {
            int cmp = key.CompareTo(current.Key);
            if (cmp == 0)
                return current;
            current = comparator.LessThan(key, current.Key) ? current.Left : current.Right;
        }
        return null;
    }

    private void InsertNode(Node<TKey, TValue>? newNode)
    {
        Node<TKey, TValue>? parent = null;
        var current = root;

        while (current != null)
        {
            parent = current;
            int cmp = newNode.Key.CompareTo(current.Key);
            if (cmp < 0)
                current = current.Left;
            else if (cmp > 0)
                current = current.Right;
            else
                throw new ArgumentException("Ключ уже существует.");
        }

        newNode.Parent = parent;
        if (parent == null)
            root = newNode;
        else if (comparator.LessThan(newNode.Key, parent.Key))
            parent.Left = newNode;
        else
            parent.Right = newNode;

        FixInsert(newNode);
    }

    private void FixInsert(Node<TKey, TValue>? node)
    {
        while (node != root && node.Parent.IsRed)
        {
            if (node.Parent == node.Parent.Parent?.Left)
            {
                var uncle = node.Parent.Parent.Right;
                if (uncle != null && uncle.IsRed)
                {
                    node.Parent.IsRed = false;
                    uncle.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Right)
                    {
                        node = node.Parent;
                        RotateLeft(node);
                    }
                    node.Parent.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    RotateRight(node.Parent.Parent);
                }
            }
            else
            {
                var uncle = node.Parent.Parent?.Left;
                if (uncle != null && uncle.IsRed)
                {
                    node.Parent.IsRed = false;
                    uncle.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Left)
                    {
                        node = node.Parent;
                        RotateRight(node);
                    }
                    node.Parent.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    RotateLeft(node.Parent.Parent);
                }
            }
        }
        root.IsRed = false;
    }

    private void RotateLeft(Node<TKey, TValue>? x)
    {
        var y = x.Right;
        x.Right = y.Left;
        if (y.Left != null)
            y.Left.Parent = x;
        y.Parent = x.Parent;
        if (x.Parent == null)
            root = y;
        else if (x == x.Parent.Left)
            x.Parent.Left = y;
        else
            x.Parent.Right = y;
        y.Left = x;
        x.Parent = y;
    }

    private void RotateRight(Node<TKey, TValue>? x)
    {
        var y = x.Left;
        x.Left = y.Right;
        if (y.Right != null)
            y.Right.Parent = x;
        y.Parent = x.Parent;
        if (x.Parent == null)
            root = y;
        else if (x == x.Parent.Right)
            x.Parent.Right = y;
        else
            x.Parent.Left = y;
        y.Right = x;
        x.Parent = y;
    }

    private static Node<TKey, TValue>? CopyTree(Node<TKey, TValue>? node)
    {
        if (node == null)
            return null;

        var newNode = new Node<TKey, TValue>(node.Key, node.Value)
        {
            IsRed = node.IsRed,
            Parent = null,
            Left = CopyTree(node.Left),
            Right = CopyTree(node.Right)
        };

        if (newNode.Left != null)
            newNode.Left.Parent = newNode;
        if (newNode.Right != null)
            newNode.Right.Parent = newNode;

        return newNode;
    }

    private static void DestroyTree(Node<TKey, TValue>? node)
    {
        if (node == null)
            return;

        DestroyTree(node.Left);
        DestroyTree(node.Right);
    }
}