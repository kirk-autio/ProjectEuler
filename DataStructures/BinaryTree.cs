using System;
using ProjectEuler.DataStructures;

namespace ProjectEuler.DataStructures
{
    public partial class BinaryTree<T>
    {
        protected class TreeNode
        {
            public TreeNode(T value)
            {
                Value = value;
            }  
            
            public T Value { get; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }
        }
    }
    
    public abstract partial class BinaryTree<T> where T: IComparable
    {
        protected BinaryTree(T value)
        {
            HeadNode = new TreeNode(value);
        }

        protected TreeNode HeadNode { get; }

        private TreeNode ParentFor(T value, TreeNode parentFrom) => parentFrom == null ? null :
            value.CompareTo(parentFrom.Value) < 0
                ? ParentFor(value, parentFrom.Left) ?? parentFrom
                : ParentFor(value, parentFrom.Right) ?? parentFrom;

        protected TreeNode ParentFor(T value) => ParentFor(value, HeadNode);

        public abstract void Add(T value);
        public abstract void Delete(T value);
        public abstract bool Contains(T value);
        
        private string PreOrder(TreeNode node) => node != null ? $"{node.Value} {PreOrder(node.Left)} {PreOrder(node.Right)}" : "";
        public string PreOrder() => $"{PreOrder(HeadNode)}";

        private string InOrder(TreeNode node) => node != null ? $"{InOrder(node.Left)} {node.Value} {InOrder(node.Right)}" : "";
        public string InOrder() => $"{InOrder(HeadNode)}";

        private string PostOrder(TreeNode node) => node != null ? $"{PostOrder(node.Left)} {PostOrder(node.Right)} {node.Value}" : "";
        public string PostOrder() => PostOrder(HeadNode);
    }
}

public class UnbalancedTree<T> : BinaryTree<T> where T: IComparable
{
    public UnbalancedTree(T value) : base(value) { }
    
    public override void Add(T value)
    {
        
    }

    public override void Delete(T value)
    {
        throw new NotImplementedException();
    }

    public override bool Contains(T value) => throw new NotImplementedException();
}