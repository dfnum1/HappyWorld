/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DiyConfig
作    者:	HappLI
描    述:	DIY配置
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using System.IO;
using TopGame.Core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.Data
{
    //[CreateAssetMenu]
    public class DIYBlockDatas : ScriptableObject
    {
        [System.Serializable]
        public struct Item
        {
            public ushort id;
            public string file;
            public static Item DEF = new Item() { id = 0, file = null };

            public bool isValid
            {
                get { return id > 0 && !string.IsNullOrEmpty(file); }
            }
        }
        [System.Serializable]
        public struct TypeItem
        {
            public byte type;
            public string strName;
            public List<Item> items;
#if UNITY_EDITOR
            [System.NonSerialized]
            public bool bExpand;
#endif
        }
        [System.Serializable]
        public class Block : Framework.Core.VariablePoolAble
        {
            public byte ID;
            public string strName;
            public List<TypeItem> blocks;
#if UNITY_EDITOR
            [System.NonSerialized]
            public bool bExpand;
#endif
            public void Destroy()
            {
            }
        }
        public Vector2Int cellSize = new Vector2Int(3, 3);
        public List<Block> datas;

        private Dictionary<int, Item> m_vBlocks = null;
        private static DIYBlockDatas ms_Instance = null;
        //-----------------------------------------------------
        private void OnEnable()
        {
            ms_Instance = this;
            ATerrainLayers.TerrainGridSize = cellSize;
        }
        //-----------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
        }
        //-----------------------------------------------------
        public static Item FindItem(byte skin, byte type, ushort id)
        {
#if UNITY_EDITOR
            if (ms_Instance == null)
                ms_Instance = GetBlockDatas();
#endif
            if (ms_Instance == null) return Item.DEF;
            return ms_Instance.GetItem(skin, type, id);
        }
        //-----------------------------------------------------
        public Item GetItem(byte skin, byte type, ushort id)
        {
            if (datas == null) return Item.DEF;
            if(m_vBlocks == null || m_vBlocks.Count<=0)
            {
                m_vBlocks = new Dictionary<int, Item>();
                for (int i =0; i < datas.Count; ++i)
                {
                    Block block = datas[i];
                    if (block.blocks == null) continue;
                    for(int j =0; j < block.blocks.Count; ++j)
                    {
                        TypeItem typeItem = block.blocks[j];
                        if (typeItem.items == null) continue;
                        for (int k = 0; k < typeItem.items.Count; ++k)
                        {
                            int key = typeItem.items[k].id << 16 | typeItem.type << 8 | block.ID;
                           m_vBlocks[key] = typeItem.items[k];
                        }
                    }
                }
            }
            int findKey = id << 16 | type << 8 | skin;
            Item findItem;
            if (m_vBlocks.TryGetValue(findKey, out findItem))
                return findItem;
            return Item.DEF;
        }
        //-----------------------------------------------------
        public static int ConvertPosYToGridY(float posY, Vector2Int cell)
        {
            int tempY = (int)(posY * 100);
            return tempY / (cell.y * 50);
        }
        //-----------------------------------------------------
        public static float ConvertGridYToPosY(int gridY, Vector2Int cell)
        {
            float halfY = cell.y*0.5f;
            return gridY * halfY;
        }
        //-----------------------------------------------------
        public static Vector2Int ConvertWorldPosToGrid(Vector3 worldPos, Vector2Int cell)
        {
            int halfX = cell.x * 50;
            int halfY = cell.y * 50;
            float tX = Mathf.Floor(worldPos.x * 100);
            float tY = Mathf.Floor(worldPos.z * 100);
            int gX = Mathf.RoundToInt(tX / halfX)*100;
            int gY = Mathf.RoundToInt(tY / halfY) * 100;
            return new Vector2Int(gX, gY);
        }
        //-----------------------------------------------------
        public static Vector3Int ConvertWorldPosToGrid3D(Vector3 worldPos, Vector2Int cell)
        {
            int halfX = cell.x * 50;
            int halfY = cell.y * 50;
            float tX = Mathf.Floor(worldPos.x * 100);
            float tY = Mathf.Floor(worldPos.z * 100);
            int gX = Mathf.RoundToInt(tX / halfX) * 100;
            int gY = Mathf.RoundToInt(tY / halfY) * 100;
            return new Vector3Int(gX, ConvertPosYToGridY(worldPos.y, cell), gY);
        }
        //-----------------------------------------------------
        public static Vector3 ConvertGridToWorldPos(Vector3Int gridPos, Vector2Int cell)
        {
            float halfX = cell.x * 0.5f;
            float halfY = cell.y * 0.5f;
            return new Vector3(gridPos.x * 0.01f * halfX, ConvertGridYToPosY(gridPos.y, cell), gridPos.z * 0.01f * halfY);
        }
        //-----------------------------------------------------
        public static Vector3 ConvertGridToWorldPos(Vector2Int gridPos, Vector2Int cell, int posY=0)
        {
            float halfX = cell.x * 0.5f;
            float halfY = cell.y * 0.5f;
            return new Vector3(gridPos.x*0.01f* halfX, ConvertGridYToPosY(posY, cell), gridPos.y * 0.01f * halfY);
        }
        //-----------------------------------------------------
        public static Vector3 AdjustWorldPos(Vector3 worldPos, Vector2Int cell)
        {
            Vector3 world = ConvertGridToWorldPos(ConvertWorldPosToGrid(worldPos, cell), cell);
            world.y = worldPos.y;
            return world;
        }
        //-----------------------------------------------------
        public static Vector2Int ConvertWorldPosToGrid(Vector3 worldPos)
        {
#if UNITY_EDITOR
            if (ms_Instance == null) ms_Instance = GetBlockDatas();
#endif
            if (ms_Instance == null) return Vector2Int.zero;
            return ConvertWorldPosToGrid(worldPos, ms_Instance.cellSize);
        }
        //-----------------------------------------------------
        public static Vector3 ConvertGridToWorldPos(Vector3Int gridPos)
        {
#if UNITY_EDITOR
            if (ms_Instance == null) ms_Instance = GetBlockDatas();
#endif
            if (ms_Instance == null) return Vector3.zero;
            return ConvertGridToWorldPos(gridPos, ms_Instance.cellSize);
        }
        //-----------------------------------------------------
        public static Vector3Int ConvertWorldPosToGrid3D(Vector3 worldPos)
        {
#if UNITY_EDITOR
            if (ms_Instance == null) ms_Instance = GetBlockDatas();
#endif
            if (ms_Instance == null) return Vector3Int.zero;

            return ConvertWorldPosToGrid3D(worldPos, ms_Instance.cellSize);
        }
        //-----------------------------------------------------
        public static Vector3 ConvertGridToWorldPos(Vector2Int gridPos, int posY = 0)
        {
#if UNITY_EDITOR
            if (ms_Instance == null) ms_Instance = GetBlockDatas();
#endif
            if (ms_Instance == null) return Vector3.zero;
            return ConvertGridToWorldPos(gridPos, ms_Instance.cellSize, posY);
        }
        //-----------------------------------------------------
        public static Vector3 AdjustWorldPos(Vector3 worldPos)
        {
#if UNITY_EDITOR
            if (ms_Instance == null) ms_Instance = GetBlockDatas();
#endif
            if (ms_Instance == null) return worldPos;

            Vector3 result = ConvertGridToWorldPos(ConvertWorldPosToGrid(worldPos, ms_Instance.cellSize), ms_Instance.cellSize);
            result.y = worldPos.y;
            return result;
        }
        //-----------------------------------------------------
        public static float ConvertGridYToPosY(int gridY)
        {
#if UNITY_EDITOR
            if (ms_Instance == null) ms_Instance = GetBlockDatas();
#endif
            if (ms_Instance == null) return gridY;
            float halfY = ms_Instance.cellSize.y * 0.5f;
            return gridY * halfY;
        }
        //-----------------------------------------------------
        public static int ConvertPosYToGridY(float posY)
        {
#if UNITY_EDITOR
            if (ms_Instance == null) ms_Instance = GetBlockDatas();
#endif
            if (ms_Instance == null) return (int)posY;
            int tempY = (int)(posY * 100);
            return tempY / (ms_Instance.cellSize.y * 50);
        }
        //-----------------------------------------------------
        public static Vector2Int GetCellSize()
        {
#if UNITY_EDITOR
            if (ms_Instance == null) ms_Instance = GetBlockDatas();
#endif
            if (ms_Instance == null) return new Vector2Int(3, 3);
            return ms_Instance.cellSize;
        }
        //-----------------------------------------------------
#if UNITY_EDITOR
        //-----------------------------------------------------
        public static DIYBlockDatas GetBlockDatas()
        {
            return AssetDatabase.LoadAssetAtPath<DIYBlockDatas>("Assets/Datas/Config/DIYBlocks.asset");
        }
#else
        public static DIYBlockDatas GetBlockDatas()
        {
            return ms_Instance;
        }     
#endif
    }
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DIYBlockDatas), true)]
    public class DIYBlockDatasEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DIYBlockDatas blockDatas = target as DIYBlockDatas;
            blockDatas.cellSize = EditorGUILayout.Vector2IntField("单位大小", blockDatas.cellSize);

            if (blockDatas.datas == null)
            {
                blockDatas.datas = new List<DIYBlockDatas.Block>();
            }

            for (int i =0; i < blockDatas.datas.Count; ++i)
            {
                DIYBlockDatas.Block block = blockDatas.datas[i];
                if(!DrawBlock(ref block))
                {
                    blockDatas.datas.RemoveAt(i);
                    break;
                }
        //        blockDatas.datas[i] = block;
            }
            if(GUILayout.Button("添加"))
            {
                byte maxId = 0;
                for (int i = 0; i < blockDatas.datas.Count; ++i) maxId = (byte)Mathf.Max(maxId, blockDatas.datas[i].ID);
                    blockDatas.datas.Add(new DIYBlockDatas.Block() {  ID = ++maxId});
            }
            if(GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            serializedObject.ApplyModifiedProperties();
        }
        //-----------------------------------------------------
        public static bool DrawBlock(ref DIYBlockDatas.Block block, bool bDIYEditor = false, System.Action<DIYBlockDatas.Item> OnSelectItem = null)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            if(bDIYEditor)
            {
                block.bExpand = true;
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                block.bExpand = EditorGUILayout.Foldout(block.bExpand, block.strName + "[" + block.ID + "]");

                if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    if (EditorUtility.DisplayDialog("提示", "确认移除主题", "确定", "取消"))
                        return false;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (block.bExpand)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                block.ID = (byte)EditorGUILayout.IntField("皮肤ID", (byte)block.ID);
                if (block.ID <= 0) EditorGUILayout.HelpBox("id 不能<=0", MessageType.Error);
                EditorGUILayout.EndHorizontal();
                block.strName = EditorGUILayout.TextField("名字", block.strName);
                if (block.blocks == null) block.blocks = new List<DIYBlockDatas.TypeItem>();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("资源列表");
                if(GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                {
                    byte maxId = 0;
                    for (int i = 0; i < block.blocks.Count; ++i)
                        maxId = (byte)Mathf.Max(block.blocks[i].type, maxId);
                    block.blocks.Add(new DIYBlockDatas.TypeItem() { type = ++maxId });
                }
                EditorGUILayout.EndHorizontal();
                for (int i =0; i < block.blocks.Count; ++i)
                {
                    DIYBlockDatas.TypeItem typeItem = block.blocks[i];
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
                    {
                        if (EditorUtility.DisplayDialog("提示", "确认移除", "确定", "取消"))
                            block.blocks.RemoveAt(i);
                        break;
                    }
                    typeItem.bExpand = EditorGUILayout.Foldout(typeItem.bExpand, typeItem.strName + "[" + typeItem.type + "]");
                    EditorGUILayout.EndHorizontal();
                    if (typeItem.items == null) typeItem.items = new List<DIYBlockDatas.Item>();
                    if(typeItem.bExpand)
                    {
                        EditorGUI.indentLevel++;
                        typeItem.type = (byte)EditorGUILayout.IntField("类型", typeItem.type);

                        EditorGUILayout.BeginHorizontal();
                        typeItem.strName = EditorGUILayout.TextField("描述名", typeItem.strName);
                        if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                        {
                            typeItem.bExpand = true;
                            ushort maxId = 0;
                            for (int j = 0; j < typeItem.items.Count; ++j)
                                maxId = (ushort)Mathf.Max(typeItem.items[j].id, maxId);
                            typeItem.items.Add(new DIYBlockDatas.Item() { id = ++maxId });
                        }
                        EditorGUILayout.EndHorizontal();
                        for (int j = 0; j < typeItem.items.Count; ++j)
                        {
                            DIYBlockDatas.Item item = typeItem.items[j];
                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel++;
                            if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
                            {
                                if (EditorUtility.DisplayDialog("提示", "确认移除", "确定", "取消"))
                                    typeItem.items.RemoveAt(j);
                                break;
                            }
                            if(OnSelectItem!=null)
                            {
                                if (GUILayout.Button("选择", new GUILayoutOption[] { GUILayout.Width(50) }))
                                {
                                    OnSelectItem(item);
                                }
                            }
                            EditorGUIUtility.labelWidth = 100;
                            item.id = (ushort)EditorGUILayout.IntField("ID", item.id);
                            EditorGUIUtility.labelWidth = labelWidth;
                            GameObject preab = EditorGUILayout.ObjectField(AssetDatabase.LoadAssetAtPath<GameObject>(item.file), typeof(GameObject), false) as GameObject;
                            if (preab != null)
                            {
                                if (preab.GetComponent<Logic.DIYBlock>() == null)
                                    EditorGUILayout.HelpBox("地块没添加 DIYBlock 组件", MessageType.Error);
                            }
                            EditorGUI.indentLevel--;
                            EditorGUILayout.EndHorizontal();
                            item.file = AssetDatabase.GetAssetPath(preab);
                            typeItem.items[j] = item;
                        }
                        EditorGUI.indentLevel--;
                    }
                    block.blocks[i] = typeItem;
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
            return true;
        }
    }
#endif
}

