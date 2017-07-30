using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessAnalyser.Explorer.Rules;

namespace ChessAnalyser.Explorer.Database
{
    [Serializable]
    class MoveTreeContainer<StoredData>
    {
        private readonly TreeNode<StoredData> _root = new TreeNode<StoredData>(null);

        public void SetPathInformation(Move[] path, StoredData[] info)
        {
            if (path.Length + 1 != info.Length)
                throw new InvalidOperationException("For each POSITION (not a move) in path exactly one StoredData object is required");

            throw new NotImplementedException();
        }

        public StoredData[] GetPathInformation(Move[] path)
        {
            var currentNode = _root;
            var result = new StoredData[path.Length + 1];
            for (var i = 0; i < path.Length; ++i)
            {
                result[i] = currentNode.Data;
                currentNode = currentNode.GetChild(path[i]);
            }

            return result;
        }

        public Move[] GetBranches(Move[] path)
        {
            var currentNode = _root;
            foreach (var move in path)
            {
                currentNode = currentNode.GetChild(move);
            }

            return currentNode.GetBranches().ToArray();
        }
    }

    [Serializable]
    class TreeNode<StoredData>
    {
        private readonly TreeNode<StoredData> _parent;

        private readonly Dictionary<Move, TreeNode<StoredData>> _children = new Dictionary<Move, TreeNode<StoredData>>();

        internal StoredData Data;

        internal TreeNode(TreeNode<StoredData> parent)
        {
            _parent = parent;
        }

        internal TreeNode<StoredData> GetChild(Move move)
        {
            if (!_children.TryGetValue(move, out TreeNode<StoredData> child))
                _children[move] = child = new TreeNode<StoredData>(this);

            return child;
        }

        internal IEnumerable<Move> GetBranches()
        {
            return _children.Keys;
        }
    }
}
