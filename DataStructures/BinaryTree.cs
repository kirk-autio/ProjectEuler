using System;
using System.Collections.Generic;
using ProjectEuler.DataStructures;

namespace ProjectEuler.DataStructures
{
    public interface ITreeNode<T>
    {
        T Value { get; }
        ITreeNode<T> Left { get; }
        ITreeNode<T> Right { get; }
        public int GetHeight();
    }
    
    public partial class BinaryTree<T>
    {
        public class TreeNode : ITreeNode<T>
        {
            public TreeNode(T value)
            {
                Value = value;
            }  

            ITreeNode<T> ITreeNode<T>.Left => Left;
            ITreeNode<T> ITreeNode<T>.Right => Right;
            
            public T Value { get; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }
            public int GetHeight() => 1 + Math.Max(Left?.GetHeight() ?? 0, Right?.GetHeight() ?? 0);
            public int GetBalance() => (Left?.GetHeight() ?? 0) - (Right?.GetHeight() ?? 0);
        }
    }
    
    public abstract partial class BinaryTree<T> where T: IComparable
    {
        protected BinaryTree(T value)
        {
            HeadNode = new TreeNode(value);
        }

        protected TreeNode HeadNode { get; }

        private Stack<ITreeNode<T>> ParentFor(T value, Stack<ITreeNode<T>> path)
        {
            var parent = path.Peek() ?? HeadNode;
            if (value.CompareTo(parent.Value) < 0) {
                if (parent.Left == null) return path;

                path.Push(parent.Left);
                return ParentFor(value, path);
            }

            if (value.CompareTo(parent.Value) > 0) {
                if (parent.Right == null) return path;

                path.Push(parent.Right);
                return ParentFor(parent.Value, path);
            }

            return null;
        }

        protected Stack<ITreeNode<T>> ParentFor(T value) => ParentFor(value, new Stack<ITreeNode<T>>());
        protected virtual Stack<ITreeNode<T>> AddWithStack(T value)
        {
            var nodeStack = ParentFor(value);
            var parent = (TreeNode) nodeStack.Peek();
            if (value.CompareTo(parent.Value) < 0) {
                parent.Left = new TreeNode(value);
                nodeStack.Push(parent.Left);
            } else {
                parent.Right = new TreeNode(value);
                nodeStack.Push(parent.Right);
            }

            return nodeStack;
        }

        public void Add(T value) => AddWithStack(value);
        
        public abstract void Delete(T value);
        
        public bool Contains(T value)
        {
            if (value.CompareTo(HeadNode.Value) == 0)
                return true;
            
            var parent = ParentFor(value)?.Peek();
            if (parent == null) return false;
            
            return parent.Left.Value.CompareTo(value) == 0 || parent.Right.Value.CompareTo(value) == 0;
        }
        
        public int GetHeight() => HeadNode.GetHeight();

        private void PreOrder(TreeNode node, Action<TreeNode> action)
        {
            if (node == null) return;
            
            action.Invoke(node);
            PreOrder(node.Left, action);
            PreOrder(node.Right, action);
        }

        public void PreOrder(Action<ITreeNode<T>> action) => PreOrder(HeadNode, action);

        private void InOrder(TreeNode node, Action<TreeNode> action)
        {
            if (node == null) return;

            InOrder(node.Left, action);
            action.Invoke(node);
            InOrder(node.Right, action);
        }

        public void InOrder(Action<ITreeNode<T>> action) => InOrder(HeadNode, action);

        private void PostOrder(TreeNode node, Action<TreeNode> action)
        {
            if (node == null) return;

            PostOrder(node.Left, action);
            PostOrder(node.Right, action);
            action.Invoke(node);
        }

        public void PostOrder(Action<ITreeNode<T>> action) => PostOrder(HeadNode, action);
    }
}

public class UnbalancedTree<T> : BinaryTree<T> where T: IComparable
{
    public UnbalancedTree(T value) : base(value) { }

    public override void Delete(T value)
    {
        var parent = (TreeNode) ParentFor(value)?.Peek();
        if (parent == null) return;

        if (parent.Left.Value.CompareTo(value) == 0)
            parent.Left = parent.Left.Left;
        else if (parent.Right.Value.CompareTo(value) == 0)
            parent.Right = parent.Right.Right;
    }
}

public class AVLTree<T> : BinaryTree<T> where T : IComparable
{
    public AVLTree(T value) : base(value) { }
    
    private void Rotate(TreeNode node1, TreeNode node2, bool rotateRight)
    {
        if (rotateRight) {
            node2.Left = node1;
            node1.Left = node1.Right;
        } else {
            node2.Right = node1;
            node1.Right = node1.Left;
        }
    }
    
    protected override Stack<ITreeNode<T>> AddWithStack(T value)
    {
        var path = base.AddWithStack(value);

        TreeNode grandchild = null;
        TreeNode parent = (TreeNode) path.Pop();
        while (path.Count > 0 && Math.Abs(((TreeNode) path.Peek()).GetBalance()) < 2) {
            grandchild = parent;
            parent = (TreeNode) path.Pop();
        }
        if (path.Count == 0) return path;

        var firstUnbalanced = (TreeNode) path.Pop();
        if (firstUnbalanced.Left == parent && parent.Left == grandchild) {
            Rotate(parent, firstUnbalanced, true);
        } else if (firstUnbalanced.Right == parent && parent.Right == grandchild) {
            Rotate(parent, firstUnbalanced, false);
        }

        return path;
    }

    public override void Delete(T value)
    {
        throw new NotImplementedException();
    }
}