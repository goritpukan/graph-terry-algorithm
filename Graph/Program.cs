using System.Text.Json;

namespace Graph;

public static class Program
{
    public static void Main()
    {
        var graph = DeserializeMatrix("../../../graphs/graph-1/graph.json");
        PrintMatrix(graph);
        
        var path = TerryAlgorithm(graph, 6, 7);
        for(int i = 0; i < path.Length; i++)
        {
            Console.Write(path[i] + (i == path.Length - 1 ? "" : "->"));
        }
    }
    
    static void PrintMatrix(int[,] adjacencyMatrix)
    {
        int rows = adjacencyMatrix.GetLength(0);
        int cols = adjacencyMatrix.GetLength(1);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(adjacencyMatrix[i, j] + ", ");
            }
            Console.WriteLine();
        }
    }

    static int[] TerryAlgorithm(int[,] adjacencyMatrix, int vStart, int vEnd)
    {
        var stack = new Stack<int>();
        vStart--;
        vEnd--;
        int row = vStart;
        int column = 0;
        
        var matrix = (int[,])adjacencyMatrix.Clone();

        while (row != vEnd)
        {
            if (column == matrix.GetLength(1))
            {
                if (stack.Count > 0)
                {
                    row = stack.Peek();
                    column = 0;
                    stack.Pop();
                    continue;
                }
            }
            if (matrix[row, column] == 1)
            {
                matrix[row, column] = 0;
                matrix[column, row] = 0;
                stack.Push(row);
                row = column;
                column = 0;

            }else
            {
                column++;
            }
        }

        stack.Push(vEnd);
        return stack
            .ToArray()
            .Reverse()
            .Select(x => x + 1)
            .ToArray();
    }

    static void SerializeMatrix(int[,] adjacencyMatrix, string path)
    {
        int[][] jagged = new int[adjacencyMatrix.GetLength(0)][];
        for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
        {
            jagged[i] = new int[adjacencyMatrix.GetLength(1)];
            for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
            {
                jagged[i][j] = adjacencyMatrix[i, j];
            }
        }
        string json = JsonSerializer.Serialize(jagged);
        File.WriteAllText(path, json);
    }

    static int[,] DeserializeMatrix(string path)
    {   
        string json = File.ReadAllText(path);
        int[][] jagged = JsonSerializer.Deserialize<int[][]>(json);
        
        int rows = jagged.Length;
        int cols = jagged[0].Length;
        int[,] matrix2D = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix2D[i, j] = jagged[i][j];
            }
        }
        return matrix2D;
    }
   
}