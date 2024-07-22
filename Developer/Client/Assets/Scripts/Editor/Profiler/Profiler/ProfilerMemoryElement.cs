
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace TopGame.ED
{
    public class ObjectInfo
    {
        public int instanceId;

        public long memorySize;

        public int reason;

        public List<int> referencedBy = new List<int>();
        public HashSet<string> referencedStrBy = new HashSet<string>();
        public HashSet<string> referencedClassBy = new HashSet<string>();

        public string name = "";

        public string className;
    }
    public class ProfilerMemoryElement : IComparable<ProfilerMemoryElement>
    {
        //反射 name, totalMemory, children 需要保持命名与dll里面一致
        public string name;
        public ObjectInfo memoryInfo = new ObjectInfo();
        public long totalMemory;
        public List<ProfilerMemoryElement> children = new List<ProfilerMemoryElement>();


        private int _depth;

        private ProfilerMemoryElement()
        {
        }

        public static ProfilerMemoryElement Create(ProfilerDllLinker srcMemoryElement, int depth, int filterDepth, float filterSize)
        {
            if (srcMemoryElement == null) return null;
            var dstMemoryElement = new ProfilerMemoryElement { _depth = depth };
            ProfilerDllLinker.CopyFrom(dstMemoryElement, srcMemoryElement.InnerObject,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);

            var srcChildren = srcMemoryElement.PublicInstanceField<IList>("children");
            if (srcChildren == null) return dstMemoryElement;
            foreach (var srcChild in srcChildren)
            {
                var memoryElement = Create(new ProfilerDllLinker(srcChild), depth + 1, filterDepth, filterSize);
                if (memoryElement == null) continue;
                if (filterDepth > 0 && depth > filterDepth) continue;
                if (!(memoryElement.totalMemory >= filterSize)) continue;
                dstMemoryElement.children.Add(memoryElement);
            }

            dstMemoryElement.children.Sort();
            return dstMemoryElement;
        }

        public string WriteHasRefPath()
        {
            if (memoryInfo != null && memoryInfo.name.Length > 0 && memoryInfo.referencedBy.Count > 0)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(memoryInfo.instanceId);
                string suffix = System.IO.Path.GetExtension(path).ToLower();
                if (path.Length <= 0 || suffix.CompareTo(".cs")==0 || suffix.CompareTo(".dll")== 0 || suffix.CompareTo(".fbx") == 0) return "";
                AssetImporter import = AssetImporter.GetAtPath(path);
                if (import == null || (import.assetBundleName.Length >0 && !import.assetBundleName.Contains("None"))) return "";
                return path;
            }
            return "";
        }
        public int GetReferenceCnt()
        {
            if (memoryInfo != null)
            {
                return memoryInfo.referencedBy.Count;
            }
            return 0;
        }

        public override string ToString()
        {
            var text = string.IsNullOrEmpty(name) ? "-" : name;
            if (memoryInfo != null && memoryInfo.name.Length>0)
            {
                text += "[path=" + UnityEditor.AssetDatabase.GetAssetPath(memoryInfo.instanceId) + "]->(";
                for (int i = 0; i < memoryInfo.referencedBy.Count; ++i)
                    text += memoryInfo.referencedBy[i] + ",";
                text += ")";
            }
            var text2 = "KB";
            var num = totalMemory / 1024f;
            if (num > 512f)
            {
                num /= 1024f;
                text2 = "MB";
            }

            var resultString = string.Format(new string('\t', _depth) + " {0}, {1}{2}", text, num, text2);
            return resultString;
        }

        public int CompareTo(ProfilerMemoryElement other)
        {
            if (other.totalMemory != totalMemory)
            {
                return (int)(other.totalMemory - totalMemory);
            }

            if (string.IsNullOrEmpty(name)) return -1;
            return !string.IsNullOrEmpty(other.name) ? string.Compare(name, other.name, StringComparison.Ordinal) : 1;

        }
    }
}
