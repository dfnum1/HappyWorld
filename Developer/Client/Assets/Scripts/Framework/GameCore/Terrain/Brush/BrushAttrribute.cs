using System;
using UnityEngine;

namespace TopGame.Core.Brush
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct)]
    public class BrushAttribute : Attribute
    {
        public string strDisplay = "";
        public BrushAttribute(string strDisplay)
        {
            this.strDisplay = strDisplay;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class BrushNameAttribute : Attribute
    {
        public string strDisplay = "";
        public string icon;
        public BrushNameAttribute(string strDisplay, string icon="")
        {
            this.icon = icon;
            this.strDisplay = strDisplay;
        }
    }
}



