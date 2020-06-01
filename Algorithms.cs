using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ProjectEuler
{
    public class Algorithms : Problems
    {
        public Algorithms(ITestOutputHelper output) : base(output) { }

        public static IEnumerable<object[]> DeletionDistanceTests()
        {
            yield return new object[] {"at", "cat", 1};
            yield return new object[] {"thoughts", "slough", 6};
        }

        public int DeleteDistance(string first, string second, int firstIndex, int secondIndex)
        {
            if (firstIndex == 0) return secondIndex;
            if (secondIndex == 0) return firstIndex;

            if (first[firstIndex] == second[secondIndex]) 
                return DeleteDistance(first, second, firstIndex - 1, secondIndex - 1);

            if (firstIndex == secondIndex && firstIndex == 1) return 2;

            return 2 + Math.Min(DeleteDistance(first, second, firstIndex - 1, secondIndex), DeleteDistance(first, second, firstIndex, secondIndex - 1));
        }
        
        [Theory, MemberData(nameof(DeletionDistanceTests))]
        public void DeletionDistance(string first, string second, int expectedResult)
        {
            Assert.Equal(expectedResult, DeleteDistance(first, second, first.Length-1, second.Length-1));
        }

        public static IEnumerable<object[]> Obstacles()
        {
            yield return new object[]
                { 10, 8, new Point(2,7), new[] { new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4), new Point(1, 5), new Point(1, 6), new Point(1, 7), new Point(3, 0), new Point(3, 1), new Point(2, 6), new Point(3, 6) }, 13 };
        }
        
        [Theory, MemberData(nameof(Obstacles))]
        public void FindWayOut(int mapWidth, int mapHeight, Point exit, Point[] obstacles, int movesToExit)
        {
            Assert.Equal(movesToExit, BFS(mapWidth, mapHeight, exit, obstacles));
        }

        private static int BFS(int mapWidth, int mapHeight, Point exit, Point[] obstacles)
        {
            var horizontalMove = new[] { 1, -1, 0, 0 };
            var verticalMove = new[] { 0, 0, 1, -1 };
            
            var visited = new bool[mapWidth, mapHeight];
            var level = new int[mapWidth, mapHeight];

            var queue = new Queue<Point>();
            queue.Enqueue(new Point(0, 0));

            while (queue.Count > 0) {
                var position = queue.Dequeue();
                visited[position.X, position.Y] = true;

                for (int i = 0; i < horizontalMove.Length; i++) {
                    var newPosition = new Point(position.X + horizontalMove[i], position.Y + verticalMove[i]);
                    if (newPosition.Equals(exit)) return level[position.X, position.Y] + 1;
                    
                    if (newPosition.X < 0 || newPosition.X >= mapWidth) continue;
                    if (newPosition.Y < 0 || newPosition.Y >= mapHeight) continue;
                    if (obstacles.Contains(newPosition) || visited[newPosition.X, newPosition.Y]) continue;

                    level[newPosition.X, newPosition.Y] = level[position.X, position.Y] + 1;
                    queue.Enqueue(newPosition);
                }
            }

            return -1;
        }
        
        private static readonly Dictionary<int, int> PathsCache = new Dictionary<int, int> {{0, 1}, {1, 1}};

        [Theory, InlineData(4, 5)]
        public void NumberOfSteps(int n, int expectedResult)
        {
            Assert.Equal(expectedResult, PathCount(n, new[] {1,2}));
            
            int PathCount(int n, int[] increments)
            {
                if (PathsCache.TryGetValue(n, out var cachedPaths)) return cachedPaths;
                
                var paths = 0;
                foreach (var increment in increments) {
                    paths += PathCount(n - increment, increments);
                }

                PathsCache.Add(n, paths);
                return paths;
            }
        }
    }
}