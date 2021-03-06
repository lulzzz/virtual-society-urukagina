﻿namespace Vs.Core.Collections.NodeTree
{
    //------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //------------------------------------------------------------------------------
    public interface ITreeNode<T>
    {

        //------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        //------------------------------------------------------------------------------
        T Parent { get; set; }

        //------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        //------------------------------------------------------------------------------
        bool IsLeaf { get; }

        //------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        //------------------------------------------------------------------------------
        bool IsRoot { get; }

        //------------------------------------------------------------------------------
        T GetRootNode();

        //------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        //------------------------------------------------------------------------------
        string GetFullyQualifiedName();
    }
}
