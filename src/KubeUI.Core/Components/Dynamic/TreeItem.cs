﻿using System.Collections.Generic;
using System.ComponentModel;

namespace KubeUI.Core.Components.Dynamic
{
    public class TreeItem
    {
        public AttributeCollection Attributes { get; set; }

        public string Name { get; set; }

        public object Parent { get; set; }

        public object Object { get; set; }

        public string Summary { get; set; }

        public bool IsCollection { get; set; }

        public bool IsCollectionItem { get; set; }

        public bool HideLink { get; set; }

        public bool Minimized { get; set; } = true;

        public List<TreeItem> Children { get; set; } = new List<TreeItem>();
    }
}
