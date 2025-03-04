namespace Labs_MP2.lab5;

public class test_graph_a
{
    public static void TestGraphA()
    {
        Graph graph = new Graph(6);

        // Добавление ребер
        graph.AddEdge(0, 1, 4);
        graph.AddEdge(0, 2, 3);
        graph.AddEdge(1, 2, 1);
        graph.AddEdge(1, 3, 2);
        graph.AddEdge(2, 3, 4);
        graph.AddEdge(3, 4, 2);
        graph.AddEdge(4, 5, 6);

        Console.WriteLine("DFS (from vertex 0):");
        graph.DFS(0);

        Console.WriteLine("BFS (from vertex 0):");
        graph.BFS(0);

        Console.WriteLine("Dijkstra (from vertex 0):");
        var dijkstraResult = graph.Dijkstra(0);
        foreach (var kvp in dijkstraResult)
            Console.WriteLine($"Vertex {kvp.Key}: Distance {kvp.Value}");

        Console.WriteLine("Kruskal MST Weight:");
        Console.WriteLine(graph.Kruskal());

        Console.WriteLine("Prim MST Weight:");
        Console.WriteLine(graph.Prim());

        Console.WriteLine("Floyd-Warshall All-Pairs Shortest Paths:");
        var floydWarshallResult = graph.FloydWarshall();
        for (int i = 0; i < graph.vertexCount; i++)
        {
            for (int j = 0; j < graph.vertexCount; j++)
            {
                Console.Write(floydWarshallResult[i, j] != int.MaxValue ? floydWarshallResult[i, j].ToString().PadLeft(3) : "INF".PadLeft(3));
            }
            Console.WriteLine();
        }
    }
}