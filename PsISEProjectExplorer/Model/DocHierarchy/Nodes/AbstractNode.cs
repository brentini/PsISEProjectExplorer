﻿using PsISEProjectExplorer.Enums;
using System;
using System.Collections.Generic;

namespace PsISEProjectExplorer.Model.DocHierarchy.Nodes
{
    public abstract class AbstractNode : INode
    {
        public abstract NodeType NodeType { get; }
        public virtual string Path { get; private set; }
        public virtual string Name { get; private set; }
        public virtual bool IsValid { get; protected set; }
        public virtual string Metadata { get; protected set; }
        public INode Parent { get; private set; }
        public ISet<INode> Children { get; private set; }

        protected AbstractNode(string path, string name, INode parent) : this(path, name, parent, true, null)
        {
        }

        protected AbstractNode(string path, string name, INode parent, bool isValid, string metadata)
        {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            this.Path = path;
            this.Name = name;
            this.IsValid = isValid;
            this.Metadata = metadata;
            this.Parent = parent;
            this.Children = new SortedSet<INode>(DefaultNodeComparer.NodeComparer);
            if (this.Parent != null)
            {
                if (!this.Parent.Children.Add(this))
                {
                    throw new InvalidOperationException(String.Format("Adding element '{0}' failed", path));
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is INode))
            {
                return false;
            }
            var node = (INode)obj;
            return (node.Path == this.Path);
        }

        public override int GetHashCode()
        {
            return this.Path.GetHashCode();
        }

        public void Remove()
        {
            if (this.Parent != null)
            {
                this.Parent.Children.Remove(this);
            }
        }
    }
}
