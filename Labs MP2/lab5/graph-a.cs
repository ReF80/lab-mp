using System;
using System.Collections.Generic;
using System.Linq;

public class Graph
{
    private Dictionary<int, List<Edge>> adjacencyList; // Список смежности
    public int vertexCount; // Количество вершин

    public Graph(int vertexCount)
    {
        this.vertexCount = vertexCount;
        adjacencyList = new Dictionary<int, List<Edge>>();
        for (int i = 0; i < vertexCount; i++)
            adjacencyList[i] = new List<Edge>();
    }

    // Добавление ребра
    public void AddEdge(int from, int to, int weight = 1)
    {
        adjacencyList[from].Add(new Edge(to, weight));
        adjacencyList[to].Add(new Edge(from, weight)); // Для неориентированного графа
    }

    // Поиск в глубину (DFS)
    public void DFS(int startVertex)
    {
        bool[] visited = new bool[vertexCount];
        Stack<int> stack = new Stack<int>();
        stack.Push(startVertex);

        while (stack.Count > 0)
        {
            int vertex = stack.Pop();
            if (!visited[vertex])
            {
                Console.Write(vertex + " ");
                visited[vertex] = true;

                foreach (var edge in adjacencyList[vertex])
                {
                    if (!visited[edge.To])
                        stack.Push(edge.To);
                }
            }
        }
        Console.WriteLine();
    }

    // Поиск в ширину (BFS)
    public void BFS(int startVertex)
    {
        bool[] visited = new bool[vertexCount];
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(startVertex);
        visited[startVertex] = true;

        while (queue.Count > 0)
        {
            int vertex = queue.Dequeue();
            Console.Write(vertex + " ");

            foreach (var edge in adjacencyList[vertex])
            {
                if (!visited[edge.To])
                {
                    queue.Enqueue(edge.To);
                    visited[edge.To] = true;
                }
            }
        }
        Console.WriteLine();
    }

    // Алгоритм Дейкстры
    public Dictionary<int, int> Dijkstra(int startVertex)
    {
        var distances = Enumerable.Repeat(int.MaxValue, vertexCount).ToDictionary(v => v, v => int.MaxValue);
        distances[startVertex] = 0;

        var priorityQueue = new SortedSet<(int distance, int vertex)>();
        priorityQueue.Add((0, startVertex));

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Min;
            priorityQueue.Remove(current);

            int currentVertex = current.vertex;
            int currentDistance = current.distance;

            if (currentDistance > distances[currentVertex])
                continue;

            foreach (var edge in adjacencyList[currentVertex])
            {
                int newDistance = currentDistance + edge.Weight;
                if (newDistance < distances[edge.To])
                {
                    priorityQueue.Remove((distances[edge.To], edge.To));
                    distances[edge.To] = newDistance;
                    priorityQueue.Add((newDistance, edge.To));
                }
            }
        }

        return distances;
    }

    // Алгоритм Крускала
    public int Kruskal()
    {
        var edges = new List<Edge>();
        foreach (var kvp in adjacencyList)
        {
            foreach (var edge in kvp.Value)
            {
                if (edge.To > kvp.Key) // Избегаем дублирования ребер
                    edges.Add(new Edge(edge.To, edge.Weight));
            }
        }

        edges.Sort((a, b) => a.Weight.CompareTo(b.Weight));

        var uf = new UnionFind(vertexCount);
        int mstWeight = 0;

        foreach (var edge in edges)
        {
            if (uf.Union(edge.From, edge.To))
                mstWeight += edge.Weight;
        }

        return mstWeight;
    }

    // Алгоритм Прима
    public int Prim()
    {
        var included = new bool[vertexCount];
        var minHeap = new SortedSet<(int weight, int vertex)>();
        minHeap.Add((0, 0));

        int mstWeight = 0;

        while (minHeap.Count > 0)
        {
            var current = minHeap.Min;
            minHeap.Remove(current);

            int vertex = current.vertex;
            int weight = current.weight;

            if (included[vertex])
                continue;

            included[vertex] = true;
            mstWeight += weight;

            foreach (var edge in adjacencyList[vertex])
            {
                if (!included[edge.To])
                    minHeap.Add((edge.Weight, edge.To));
            }
        }

        return mstWeight;
    }

    // Алгоритм Флойда-Уоршалла
    public int[,] FloydWarshall()
    {
        int[,] dist = new int[vertexCount, vertexCount];

        // Инициализация расстояний
        for (int i = 0; i < vertexCount; i++)
        {
            for (int j = 0; j < vertexCount; j++)
            {
                if (i == j)
                    dist[i, j] = 0;
                else
                    dist[i, j] = int.MaxValue;
            }
        }

        foreach (var kvp in adjacencyList)
        {
            foreach (var edge in kvp.Value)
            {
                dist[kvp.Key, edge.To] = edge.Weight;
            }
        }

        // Выполнение алгоритма
        for (int k = 0; k < vertexCount; k++)
        {
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    if (dist[i, k] != int.MaxValue && dist[k, j] != int.MaxValue &&
                        dist[i, k] + dist[k, j] < dist[i, j])
                    {
                        dist[i, j] = dist[i, k] + dist[k, j];
                    }
                }
            }
        }

        return dist;
    }
}

// Класс для представления ребра
public class Edge
{
    public int To { get; set; }
    public int From { get; set; }
    public int Weight { get; set; }

    public Edge(int to, int weight)
    {
        To = to;
        Weight = weight;
    }
}

// Класс для объединения множеств (для алгоритма Крускала)
public class UnionFind
{
    private int[] parent;
    private int[] rank;

    public UnionFind(int size)
    {
        parent = Enumerable.Range(0, size).ToArray();
        rank = new int[size];
    }

    public int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]);
        return parent[x];
    }

    public bool Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);

        if (rootX == rootY)
            return false;

        if (rank[rootX] > rank[rootY])
            parent[rootY] = rootX;
        else if (rank[rootX] < rank[rootY])
            parent[rootX] = rootY;
        else
        {
            parent[rootY] = rootX;
            rank[rootX]++;
        }

        return true;
    }
}