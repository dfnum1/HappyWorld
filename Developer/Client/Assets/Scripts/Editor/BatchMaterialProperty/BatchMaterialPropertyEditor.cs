using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorInternal;

namespace TopGame.ED
{
	public class BatchMaterialPropertyEditor : EditorWindow
    {
        static BatchMaterialPropertyEditor ms_Instance = null;
        public enum ETab
        {
            ModifyProperty,
            ReplaceShader,
        }
        static string[] TABS = new string[] { "修改属性", "替换材质" };
        //------------------------------------------------------
		[MenuItem("Tools/材质属性修改器", false, 10)]
		static void GeneratorAvatarMask()
		{
            if (ms_Instance == null)
            {
                ms_Instance = EditorWindow.GetWindow<BatchMaterialPropertyEditor>();
            }
            ms_Instance.Show();
            ms_Instance.titleContent = new GUIContent("材质属性修改器");
            ms_Instance.minSize = new Vector2(600, 300);
        }

        List<UnityEngine.Object> m_vFiles = new List<UnityEngine.Object>();
        private ReorderableList m_list;

        ETab m_eTab = ETab.ModifyProperty;

        private Shader m_pOldShader = null;

        private string m_strCopyOldPropName = null;
        private string m_strPropertyName;
        private Vector4 m_PropertyValue;
        Texture m_pPropertyTexture = null;
        private bool m_bAmount = false;
        private string m_pShaderName="";
        private Shader m_pShader = null;
        List<string> m_vShaderPop = new List<string>();
        ShaderUtil.ShaderPropertyType m_PropertyType = ShaderUtil.ShaderPropertyType.Float;
        List<string> m_vMaterialNamePop = new List<string>();
        List<string> m_vOldMaterialNamePop = new List<string>();
        enum PropertyValue
        {
            Float,
            Int,
            Vec4,
            Color,
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            m_list = new ReorderableList(m_vFiles, typeof(UnityEngine.Object), true, true, true, true);
            m_list.drawElementCallback = DrawElement;
            //  m_list.onRemoveCallback = RemoveElement;
            m_pShaderName = "";
            m_pShader = null;
            ShaderInfo[] shaders = UnityEditor.ShaderUtil.GetAllShaderInfo();
            for(int i = 0; i < shaders.Length; ++i)
            {
                m_vShaderPop.Add(shaders[i].name);
            }
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            EditorUtility.ClearProgressBar();
        }
        //------------------------------------------------------
        void OnGUI()
		{
            GUILayout.BeginHorizontal();
            Color color = GUI.color;
            for(int i = 0; i < TABS.Length; ++i)
            {
                if (m_eTab == (ETab)i) GUI.color = Color.red;
                else GUI.color = color;
                if(GUILayout.Button(TABS[i]))
                {
                    m_eTab = (ETab)i;
                }
            }
            GUI.color = color;
            GUILayout.EndHorizontal();
            m_list.DoLayoutList();

            if(m_eTab == ETab.ReplaceShader)
            {
                int index = m_pOldShader?m_vShaderPop.IndexOf(m_pOldShader.name):-1;
                index = EditorGUILayout.Popup("指定需要替换的原Shader", index, m_vShaderPop.ToArray());
                if (index >= 0 && index <= m_vShaderPop.Count)
                {
                    m_pOldShader = Shader.Find(m_vShaderPop[index]);
                }
                if (m_pOldShader != null)
                {
                    m_vOldMaterialNamePop.Clear();
                    int cnt = ShaderUtil.GetPropertyCount(m_pOldShader);
                    for (int i = 0; i < cnt; ++i)
                    {
                        m_vOldMaterialNamePop.Add(ShaderUtil.GetPropertyName(m_pOldShader, i));
                    }
                }
            }

            {
                int index = m_vShaderPop.IndexOf(m_pShaderName);
                index = EditorGUILayout.Popup("Shader", index, m_vShaderPop.ToArray());
                if (index >= 0 && index <= m_vShaderPop.Count)
                {
                    m_pShaderName = m_vShaderPop[index];
                    m_pShader = Shader.Find(m_pShaderName);
                    if (m_pShader != null)
                    {
                        m_vMaterialNamePop.Clear();
                        int cnt = ShaderUtil.GetPropertyCount(m_pShader);
                        for (int i = 0; i < cnt; ++i)
                        {
                            m_vMaterialNamePop.Add(ShaderUtil.GetPropertyName(m_pShader, i));
                        }
                    }
                }

            }

            if (m_PropertyType != ShaderUtil.ShaderPropertyType.TexEnv)
             m_bAmount = EditorGUILayout.Toggle("相对值", m_bAmount);

            int matIndex= m_vMaterialNamePop.IndexOf(m_strPropertyName);
            matIndex = EditorGUILayout.Popup("属性名", matIndex, m_vMaterialNamePop.ToArray());
            if (matIndex >= 0 && matIndex < m_vMaterialNamePop.Count)
            {
                m_strPropertyName = m_vMaterialNamePop[matIndex];
                m_PropertyType = ShaderUtil.GetPropertyType(m_pShader, matIndex);
            }
            if (!string.IsNullOrEmpty(m_strPropertyName))
            {
                GUILayout.BeginHorizontal();
                if (m_PropertyType == ShaderUtil.ShaderPropertyType.Float)
                    m_PropertyValue.x = EditorGUILayout.FloatField(m_PropertyValue.x);
                else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Range)
                    m_PropertyValue.x = EditorGUILayout.FloatField(m_PropertyValue.x);
                else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Vector)
                    m_PropertyValue = EditorGUILayout.Vector4Field("", m_PropertyValue);
                else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Color)
                    m_PropertyValue = EditorGUILayout.ColorField(new GUIContent(""), m_PropertyValue, true, true, true);
                else if (m_PropertyType == ShaderUtil.ShaderPropertyType.TexEnv)
                    m_pPropertyTexture = EditorGUILayout.ObjectField(m_pPropertyTexture, typeof(Texture), false) as Texture;
                GUILayout.EndHorizontal();

                matIndex = m_vOldMaterialNamePop.IndexOf(m_strCopyOldPropName);
                matIndex = EditorGUILayout.Popup("使用原属性覆盖到新", matIndex, m_vOldMaterialNamePop.ToArray());
                if (matIndex >= 0 && matIndex < m_vOldMaterialNamePop.Count)
                {
                    m_strCopyOldPropName = m_vOldMaterialNamePop[matIndex];
                }
            }

            if (GUILayout.Button("替换Shader"))
            {
                ReplaceShader();
            }

            if(!string.IsNullOrEmpty(m_strPropertyName) && GUILayout.Button("批量修改属性"))
            {
                ModifyShaderProperty();
            }
        }
        //------------------------------------------------------
        void RemoveElement(ReorderableList list)
        {

        }
        //------------------------------------------------------
        void DrawElement(Rect rect, int index, bool selected, bool focused)
        {
            m_vFiles[index] = EditorGUI.ObjectField(rect,m_vFiles[index], typeof(UnityEngine.Object), true);
        }
        //------------------------------------------------------
        void ReplaceShader()
        {
            if (EditorUtility.DisplayDialog("提示", "确定批量操作?", "确定", "取消"))
            {
                List<Material> vMaterials = new List<Material>();
                List<string> vPath = new List<string>();
                for (int i = 0; i < m_vFiles.Count; ++i)
                {
                    if (m_vFiles[i] == null) continue;
                    if (m_vFiles[i] is Material)
                    {
                        vMaterials.Add(m_vFiles[i] as Material);
                        continue;
                    }
                    string path = AssetDatabase.GetAssetPath(m_vFiles[i]);
                    if (AssetDatabase.IsValidFolder(path))
                    {
                        vPath.Add(path);
                    }
                }

                if (vPath.Count > 0)
                {
                    string[] assets = AssetDatabase.FindAssets("t:Material", vPath.ToArray());
                    for (int i = 0; i < assets.Length; ++i)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
                        vMaterials.Add(AssetDatabase.LoadAssetAtPath<Material>(assetPath));
                    }
                }

                EditorUtility.DisplayProgressBar("批量[" + vMaterials.Count + "]", "", 0);

                for (int i = 0; i < vMaterials.Count; ++i)
                {
                    EditorUtility.DisplayProgressBar("批量[" + vMaterials.Count + "]", vMaterials[i].name, (float)(i / vMaterials.Count));

                    if(vMaterials[i].shader != m_pOldShader) continue;

                    object propOriValue = null;
                    if (!string.IsNullOrEmpty(m_strCopyOldPropName))
                    {
                        if (vMaterials[i].HasProperty(m_strCopyOldPropName))
                        {
                            if (m_PropertyType == ShaderUtil.ShaderPropertyType.Float)
                            {
                                propOriValue = vMaterials[i].GetFloat(m_strCopyOldPropName);
                            }
                            else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Range)
                            {
                                propOriValue = vMaterials[i].GetFloat(m_strCopyOldPropName);
                            }
                            else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Vector)
                            {
                                propOriValue = vMaterials[i].GetVector(m_strCopyOldPropName);
                            }
                            else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Color)
                            {
                                propOriValue = vMaterials[i].GetColor(m_strCopyOldPropName);
                            }
                            else if (m_PropertyType == ShaderUtil.ShaderPropertyType.TexEnv)
                            {
                                propOriValue = vMaterials[i].GetTexture(m_strCopyOldPropName);
                            }
                        }
                    }

                    if (vMaterials[i].shader ==null || vMaterials[i].shader != m_pShader)
                    {
                        vMaterials[i].shader = m_pShader;
                    }
                    if (string.IsNullOrEmpty(m_strPropertyName)) continue;
                    MaterialProperty[] props = MaterialEditor.GetMaterialProperties(new Material[] { vMaterials[i] });
                    if (props == null || props.Length <= 0) continue;
                    string propName = m_strPropertyName.Trim().ToLower();
                    for (int j = 0; j < props.Length; ++j)
                    {
                        string prop = "";
                        if (props[j].displayName != null)
                        {
                            props[j].displayName.Trim().ToLower();
                            if (prop.CompareTo(propName) == 0)
                            {
                                propName = props[j].name;
                                break;
                            }
                        }

                        prop = props[j].name.Trim().ToLower();
                        if (prop.CompareTo(propName) == 0)
                        {
                            propName = props[j].name;
                            break;
                        }
                    }

                    if (vMaterials[i].HasProperty(propName))
                    {
                        if (m_PropertyType == ShaderUtil.ShaderPropertyType.Float)
                        {
                            float val = m_PropertyValue.x;
                            if (propOriValue!=null)
                            {
                                val = (float)propOriValue;
                            }
                            if (m_bAmount)
                            {
                                val += vMaterials[i].GetFloat(propName);
                            }
                            vMaterials[i].SetFloat(propName, val);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Range)
                        {
                            float val = m_PropertyValue.x;
                            if (propOriValue != null)
                            {
                                val = (float)propOriValue;
                            }
                            if (m_bAmount)
                            {
                                val += vMaterials[i].GetFloat(propName);
                            }
                            vMaterials[i].SetFloat(propName, val);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Vector)
                        {
                            Vector4 val = m_PropertyValue;
                            if (propOriValue != null)
                            {
                                val = (Vector4)propOriValue;
                            }
                            if (m_bAmount)
                            {
                                val += vMaterials[i].GetVector(propName);
                            }
                            vMaterials[i].SetVector(propName, val);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Color)
                        {
                            Color val = m_PropertyValue;
                            if (propOriValue != null)
                            {
                                val = (Color)propOriValue;
                            }
                            if (m_bAmount)
                            {
                                val += vMaterials[i].GetColor(propName);
                            }
                            vMaterials[i].SetColor(propName, val);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else if (m_PropertyType == ShaderUtil.ShaderPropertyType.TexEnv)
                        {
                            Texture val = m_pPropertyTexture;
                            if (propOriValue != null)
                            {
                                val = (Texture)propOriValue;
                            }
                            vMaterials[i].SetTexture(propName, val);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else
                            Debug.LogWarning("材质" + vMaterials[i].name + " 的\"" + m_strPropertyName + "\" 数据类型不能被批量化");
                    }
                    else
                        Debug.LogWarning("材质" + vMaterials[i].name + " 没有属性\"" + m_strPropertyName + "\"");
                }
                EditorUtility.ClearProgressBar();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
        //------------------------------------------------------
        void ModifyShaderProperty()
        {
            if (EditorUtility.DisplayDialog("提示", "确定批量操作?", "确定", "取消"))
            {
                List<Material> vMaterials = new List<Material>();
                List<string> vPath = new List<string>();
                for (int i = 0; i < m_vFiles.Count; ++i)
                {
                    if (m_vFiles[i] == null) continue;
                    if (m_vFiles[i] is Material)
                    {
                        vMaterials.Add(m_vFiles[i] as Material);
                        continue;
                    }
                    string path = AssetDatabase.GetAssetPath(m_vFiles[i]);
                    if (AssetDatabase.IsValidFolder(path))
                    {
                        vPath.Add(path);
                    }
                }

                if (vPath.Count > 0)
                {
                    string[] assets = AssetDatabase.FindAssets("t:Material", vPath.ToArray());
                    for (int i = 0; i < assets.Length; ++i)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
                        vMaterials.Add(AssetDatabase.LoadAssetAtPath<Material>(assetPath));
                    }
                }

                EditorUtility.DisplayProgressBar("批量[" + vMaterials.Count + "]", "", 0);

                for (int i = 0; i < vMaterials.Count; ++i)
                {
                    EditorUtility.DisplayProgressBar("批量[" + vMaterials.Count + "]", vMaterials[i].name, (float)(i / vMaterials.Count));

                    if (vMaterials[i].shader == null || vMaterials[i].shader != m_pShader) continue;

                    MaterialProperty[] props = MaterialEditor.GetMaterialProperties(new Material[] { vMaterials[i] });
                    if (props == null || props.Length <= 0) continue;
                    string propName = m_strPropertyName.Trim().ToLower();
                    for (int j = 0; j < props.Length; ++j)
                    {
                        string prop = "";
                        if (props[j].displayName != null)
                        {
                            props[j].displayName.Trim().ToLower();
                            if (prop.CompareTo(propName) == 0)
                            {
                                propName = props[j].name;
                                break;
                            }
                        }

                        prop = props[j].name.Trim().ToLower();
                        if (prop.CompareTo(propName) == 0)
                        {
                            propName = props[j].name;
                            break;
                        }
                    }

                    if (vMaterials[i].HasProperty(propName))
                    {
                        if (m_PropertyType == ShaderUtil.ShaderPropertyType.Float)
                        {
                            float val = m_PropertyValue.x;
                            if (m_bAmount)
                            {
                                val += vMaterials[i].GetFloat(propName);
                            }
                            vMaterials[i].SetFloat(propName, val);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Range)
                        {
                            float val = m_PropertyValue.x;
                            if (m_bAmount)
                            {
                                val += vMaterials[i].GetFloat(propName);
                            }
                            vMaterials[i].SetFloat(propName, val);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Vector)
                        {
                            Vector4 val = m_PropertyValue;
                            if (m_bAmount)
                            {
                                val += vMaterials[i].GetVector(propName);
                            }
                            vMaterials[i].SetVector(propName, val);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else if (m_PropertyType == ShaderUtil.ShaderPropertyType.Color)
                        {
                            Color val = m_PropertyValue;
                            if (m_bAmount)
                            {
                                val += vMaterials[i].GetColor(propName);
                            }
                            vMaterials[i].SetColor(propName, val);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else if (m_PropertyType == ShaderUtil.ShaderPropertyType.TexEnv)
                        {
                            Texture propTex = m_pPropertyTexture;
                            vMaterials[i].SetTexture(propName, propTex);
                            EditorUtility.SetDirty(vMaterials[i]);
                        }
                        else
                            Debug.LogWarning("材质" + vMaterials[i].name + " 的\"" + m_strPropertyName + "\" 数据类型不能被批量化");
                    }
                    else
                        Debug.LogWarning("材质" + vMaterials[i].name + " 没有属性\"" + m_strPropertyName + "\"");
                }
                EditorUtility.ClearProgressBar();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
}
