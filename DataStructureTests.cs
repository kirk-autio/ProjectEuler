using Xunit;
using Xunit.Abstractions;

namespace ProjectEuler
{
    public class DataStructureTests : Problems
    {
        public DataStructureTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void UnbalancedBinaryTreePreOrder()
        {
            var tree = new UnbalancedTree<int>(10);

            tree.Add(5);
            tree.Add(9);
            tree.Add(4);
            tree.Add(20);
            tree.Add(13);
            tree.Add(14);
            tree.Add(21);

            Assert.Equal("10 5 4 9 20 13 14 21", tree.PreOrder());
        }

        [Fact]
        public void UnbalancedBinaryTreeHeight_BalancedInput()
        {
            var tree = new UnbalancedTree<int>(10);

            tree.Add(5);
            tree.Add(9);
            tree.Add(4);
            tree.Add(20);
            tree.Add(13);
            tree.Add(14);
            tree.Add(21);

            Assert.Equal(4, tree.GetHeight());
        }
        
        [Fact]
        public void UnbalancedBinaryTreeHeight_UnbalancedInput()
        {
            var tree = new UnbalancedTree<int>(4);

            tree.Add(5);
            tree.Add(9);
            tree.Add(13);
            tree.Add(20);
            tree.Add(23);
            tree.Add(24);
            tree.Add(25);

            Assert.Equal(8, tree.GetHeight());
        }

        [Fact]
        public void AVLTreeHeight()
        {
            
        }
    }
}