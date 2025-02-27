﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShopAssist.Models
{
    [Serializable]
    public class GraphEdge
    {
        public GraphNode From { get; set; }
        public GraphNode To { get; set; }
        [XmlIgnore]
        public int Weight { get; set; }
        public int CenterX { get; set; } //Used for display
        public int CenterY { get; set; } //Used for display

        public GraphEdge() { } //Only used for XML serialization

        public GraphEdge(GraphNode from, GraphNode to, int weight)
        {
            this.From = from;
            this.To = to;
            this.Weight = weight;
        }

        public override string ToString()
        {
            return $"{this.From.Name} -> {this.To.Name}";
        }
    }
}
