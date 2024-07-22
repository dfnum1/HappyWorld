using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

namespace TopGame.ED
{
	public class FxInfo
	{
		public string PrefabPath = "";
		public string FxTransformPath = null;
		public bool IsMeshRenderer = false;
	}

	public class ParticleSortLayerSetterEditorLogic
    {
		int BeginIndex = 1;
        private string RootParticlePath = "Assets/Datas/Particles";
        Dictionary<Material, List<FxInfo>> MaterialToFxList = new Dictionary<Material, List<FxInfo>>();
        //-----------------------------------------------------
        public void OnEnable()
        {
            
        }
        //-----------------------------------------------------
        public void OnDisable()
        {
            GUI.FocusControl("");
            EditorUtility.ClearProgressBar();
        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {

        }
        //-----------------------------------------------------
        public void OnEvent(Event evt)
        {
            
        }
        //-----------------------------------------------------
        public void SaveData()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
        //-----------------------------------------------------
        public void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("粒子预制体根目录:");
            RootParticlePath = GUILayout.TextField(RootParticlePath);
            if (GUILayout.Button("..."))
            {
                string path = EditorUtility.OpenFolderPanel("数据存储路径", "", "");
                if (path != "" && path.StartsWith(Application.dataPath))
                {
                    RootParticlePath = string.Format("Assets{0}", path.Substring(Application.dataPath.Length, path.Length - Application.dataPath.Length));
                }
            }
            GUILayout.EndHorizontal();
            BeginIndex = EditorGUILayout.IntField("启始层级节点", BeginIndex);
            if (BeginIndex == 0)
            {
                Debug.LogWarning("Unity自身存在BUG SortingLayer设置为0不会合批，强制调节为1");
                BeginIndex = 1;
            }

            if (GUILayout.Button("替换"))
            {
                CollectFileData();
            }
        }
        //------------------------------------------------------
		string BuildTransformPath(Transform cur, Transform root = null)
		{
			if (cur == root)
				return "";

			string rt = cur.name;

			cur = cur.parent;

			while (cur != null && cur != root)
			{
				rt = string.Format("{0}/{1}", cur.name, rt);
				cur = cur.parent;
			}

			if (rt.EndsWith(" (UnityEngine.Transform)"))
			{
				rt = rt.Substring(0, rt.Length - " (UnityEngine.Transform)".Length);
			}

			return rt;
		}
        //------------------------------------------------------
		void CollectFileData()
		{
			MaterialToFxList.Clear();

			List<string> withoutExtensions = new List<string>() { ".prefab" };
			string[] files = Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
				.Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

			int startIndex = 0;

			EditorApplication.update = delegate ()
			{
				string file = files[startIndex];

				file = file.Substring(Application.dataPath.Length - "Assets".Length, file.Length - Application.dataPath.Length + "Assets".Length);

				bool isCancel = EditorUtility.DisplayCancelableProgressBar("搜寻特效文件", file, (float)startIndex / (float)files.Length);

				var obj = AssetDatabase.LoadAssetAtPath(file.Replace('\\','/'), typeof(UnityEngine.Object));

				GameObject prefab = obj as GameObject;

				if (prefab != null)
				{
					var particleRendereres = prefab.GetComponentsInChildren<ParticleSystemRenderer>();

					for (int i = 0; i < particleRendereres.Length; ++i)
					{
						var particleRenderer = particleRendereres[i];

						if (particleRenderer.sharedMaterial == null)
							continue;

						List<FxInfo> fxInfos = null;
						if (!MaterialToFxList.TryGetValue(particleRenderer.sharedMaterial, out fxInfos))
						{
							fxInfos = new List<FxInfo>();
							MaterialToFxList.Add(particleRenderer.sharedMaterial, fxInfos);
						}

						FxInfo info = new FxInfo();
						info.PrefabPath = file;
						info.FxTransformPath = BuildTransformPath(particleRenderer.transform, prefab.transform);

						if (fxInfos.Find(infoInList =>
						{
							return infoInList.PrefabPath.CompareTo(info.PrefabPath) == 0
								&& infoInList.FxTransformPath.CompareTo(info.FxTransformPath) == 0;
						}) == null)
						{
							fxInfos.Add(info);
						}
					}

					var meshRendereres = prefab.GetComponentsInChildren<MeshRenderer>();

					for (int i = 0; i < meshRendereres.Length; ++i)
					{
						var meshRenderer = meshRendereres[i];

						if (meshRenderer.sharedMaterial == null)
							continue;

						List<FxInfo> fxInfos = null;
						if (!MaterialToFxList.TryGetValue(meshRenderer.sharedMaterial, out fxInfos))
						{
							fxInfos = new List<FxInfo>();
							MaterialToFxList.Add(meshRenderer.sharedMaterial, fxInfos);
						}

						FxInfo info = new FxInfo();
						info.PrefabPath = file;
						info.IsMeshRenderer = true;
						info.FxTransformPath = BuildTransformPath(meshRenderer.transform, prefab.transform);

						if (fxInfos.Find(infoInList =>
						{
							return infoInList.PrefabPath.CompareTo(info.PrefabPath) == 0
								&& infoInList.FxTransformPath.CompareTo(info.FxTransformPath) == 0;
						}) == null)
						{
							fxInfos.Add(info);
						}
					}

				}

				startIndex++;
				if (isCancel || startIndex >= files.Length)
				{
					EditorUtility.ClearProgressBar();
					EditorApplication.update = null;
					startIndex = 0;
					Debug.Log("搜寻结束");

					SettingSortingLayer();
				}

			};
		}
        //------------------------------------------------------
		void SettingSortingLayer()
		{
			if (MaterialToFxList.Count == 0)
				return;

			List<FxInfo>[] datas = new List<FxInfo>[MaterialToFxList.Values.Count];
			MaterialToFxList.Values.CopyTo(datas, 0);

			if (BeginIndex == 0)
				BeginIndex = 1;

			int curIndex = BeginIndex;
			int dataIndex = 0;

			EditorApplication.update = delegate ()
			{
				var value = datas[dataIndex];

				bool isCancel = false;

				for (int i = 0; i < value.Count; ++i)
				{
					isCancel |= EditorUtility.DisplayCancelableProgressBar("替换特效文件渲染层级", value[i].PrefabPath, (float)dataIndex / (float)datas.Length);

					var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(value[i].PrefabPath);
					Transform transform = prefab.transform;
					if (!string.IsNullOrEmpty(value[i].FxTransformPath))
					{
						transform = prefab.transform.Find(value[i].FxTransformPath);
					}

					if (transform == null)
					{
						Debug.LogErrorFormat(string.Format("transform {0} is not exsit in prefab {1}!", value[i].FxTransformPath, value[i].PrefabPath));
						break;
					}

					if (value[i].IsMeshRenderer)
					{
						var renderer = transform.GetComponent<MeshRenderer>();
						var sorter = transform.GetComponent<Core.MeshRenderSorter>();
						if (renderer == null)
						{
							continue;
						}

						if (sorter == null)
							sorter = transform.gameObject.AddComponent<Core.MeshRenderSorter>();

						sorter.sortingOrder = curIndex;

						renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
						renderer.receiveShadows = false;
						renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
					}
					else
					{
						var renderer = transform.GetComponent<ParticleSystemRenderer>();
						if (renderer == null)
							continue;
						renderer.sortingOrder = curIndex;

						renderer.shadowCastingMode			= UnityEngine.Rendering.ShadowCastingMode.Off;
						renderer.receiveShadows				= false;
						renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
					}

                    var collers = transform.GetComponentsInChildren<Collider>();
                    foreach (var com in collers)
                    {
                        //com.enabled = false;
                        UnityEngine.Object.DestroyImmediate(com, true);
                    }

                    if (isCancel)
					{
						break;
					}
				}

				++dataIndex;
				++curIndex;

				if (isCancel || dataIndex >= datas.Length)
				{
					EditorUtility.ClearProgressBar();
					EditorApplication.update = null;
					Debug.Log(string.Format("替换结束，最后一个索引值为{0}", dataIndex));
					BeginIndex = dataIndex + 1;
					dataIndex = 0;
                    SaveData();
                }
			};
		}
	}
}
