using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopGame.UI;

namespace TopGame.UI
{
    public class MainLineTaskSerialized : UserInterface
    {
        public Vector2 TaskDetailNormalPos = new Vector2(-222,0);

        [ContextMenu("查找Cell引用")]
        void FindCellReference()
        {
            if (Widgets == null || Widgets.Length == 0)
            {
                Widgets = new Widget[5];
            }
            for (int i = 1; i <= 5; i++)
            {
                var cell = GameObject.Find("cell" + i);
                if (cell && (i-1) < Widgets.Length)
                {
                    Widgets[i - 1].widget = cell.GetComponent<UISerialized>();
                }
            }
        }
    }
}
