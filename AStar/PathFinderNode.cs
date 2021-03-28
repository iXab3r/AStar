﻿using System.Runtime.InteropServices;

namespace AStar
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PathFinderNode
    {
        
        /// <summary>
        /// Distance from home
        /// </summary>
        public int G;
        
        /// <summary>
        /// Heuristic
        /// </summary>
        public int H;

        /// <summary>
        /// This nodes parent
        /// </summary>
        public Position ParentNode;
        
        /// <summary>
        /// If the node is open or closed
        /// </summary>
        public bool? Open;
        
        /// <summary>
        /// Gone + Heuristic (H)
        /// </summary>
        public int F => G + H;
        
        /// <summary>
        /// If the node has been considered yet
        /// </summary>
        public bool HasBeenVisited => Open.HasValue;

        public PathFinderNode(int g, int h, Position parentNode, bool? open)
        {
            G = g;
            H = h;
            ParentNode = parentNode;
            Open = open;
        }
    }
}
