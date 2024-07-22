using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Reflection;
using System.IO;
using System.Collections;

namespace TopGame.ED
{
    public class ProfilerDllLinker
    {
        private const BindingFlags PublicInstanceFieldFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField;

        private const BindingFlags PrivateInstanceFieldFlag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField;

        private const BindingFlags PrivateStaticFieldFlag = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField;

        private const BindingFlags PublicInstanceMethodFlag = BindingFlags.Instance | BindingFlags.Public;

        private const BindingFlags PrivateInstanceMethodFlag = BindingFlags.Instance | BindingFlags.NonPublic;

        public readonly Type InnerType;


        public object InnerObject { get; private set; }

        public ProfilerDllLinker(Type innerType)
        {
            InnerType = innerType;
        }

        public ProfilerDllLinker(object obj)
        {
            if (null == obj) return;
            InnerType = obj.GetType();
            InnerObject = obj;
        }

        public static void CopyFrom(object dst, object src, BindingFlags flags)
        {
            if (dst == null || src == null) return;
            var srcType = src.GetType();
            var dstType = dst.GetType();
            var dstFields = dstType.GetFields(flags);
            var dstArray = dstFields;
            var objectInfoType = System.Type.GetType("UnityEditor.ObjectInfo,UnityEditor.dll");
            foreach (var dstFieldInfo in dstArray)
            {
                var srcFieldInfo = srcType.GetField(dstFieldInfo.Name, flags);
                if (srcFieldInfo != null)
                {
                    if (objectInfoType == srcFieldInfo.FieldType && dstFieldInfo.FieldType == typeof(TopGame.ED.ObjectInfo))
                    {
                        object engineObjInfo = srcFieldInfo.GetValue(src);
                        object dstObjInfo = dstFieldInfo.GetValue(dst);
                        if (engineObjInfo == null || dstObjInfo == null) continue;
                        FieldInfo[] memembers = typeof(ObjectInfo).GetFields(flags);
                        for(int i =0; i < memembers.Length; ++i)
                        {
                            if(memembers[i].Name.CompareTo("referencedBy") == 0)
                            {
                                var list_field = engineObjInfo.GetType().GetField("referencedBy", flags);
                                var list = list_field.GetValue(engineObjInfo) as IList;
                                if (list == null) continue;
                                var dst_list = dstObjInfo.GetType().GetField("referencedBy", flags).GetValue(dstObjInfo) as IList;
                                var dst_name_list = dstObjInfo.GetType().GetField("referencedStrBy", flags).GetValue(dstObjInfo) as HashSet<string>;
                                var dst_classname_list = dstObjInfo.GetType().GetField("referencedClassBy", flags).GetValue(dstObjInfo) as HashSet<string>;
                                foreach (var srcChild in list)
                                {
                                    dst_list.Add(srcChild.GetType().GetField("instanceId", flags).GetValue(srcChild));
                                    string name = srcChild.GetType().GetField("name", flags).GetValue(srcChild).ToString();
                                    if(!string.IsNullOrEmpty(name) && !dst_name_list.Contains(name)) dst_name_list.Add(name);
                                    name = srcChild.GetType().GetField("className", flags).GetValue(srcChild).ToString();
                                    if (!string.IsNullOrEmpty(name) && !dst_classname_list.Contains(name)) dst_classname_list.Add(name);
                                }
                            }
                            else if (memembers[i].Name.CompareTo("referencedStrBy") == 0)
                            {

                            }
                            else if (memembers[i].Name.CompareTo("referencedClassBy") == 0)
                            {

                            }
                            else
                            {
                                var srcObjFieldInfo = engineObjInfo.GetType().GetField(memembers[i].Name, flags);
                                memembers[i].SetValue(dstObjInfo, srcObjFieldInfo.GetValue(engineObjInfo));

                            }
                        }
                    }
                    else if (dstFieldInfo.FieldType == srcFieldInfo.FieldType)
                        dstFieldInfo.SetValue(dst, srcFieldInfo.GetValue(src));
                }
            }
        }

        public void SetObject(object obj)
        {
            if (obj.GetType() == InnerType)
            {
                InnerObject = obj;
            }
        }

        public object PrivateStaticField(string fieldName)
        {
            return _GetFiled(fieldName, PrivateStaticFieldFlag);
        }

        public T PrivateStaticField<T>(string fieldName) where T : class
        {
            return PrivateStaticField(fieldName) as T;
        }

        public object PrivateInstanceField(string fieldName)
        {
            return _GetFiled(fieldName, PrivateInstanceFieldFlag);
        }

        public T PrivateInstanceField<T>(string fieldName) where T : class
        {
            return PrivateInstanceField(fieldName) as T;
        }

        public object PublicInstanceField(string fieldName)
        {
            return _GetFiled(fieldName, PublicInstanceFieldFlag);
        }

        public T PublicInstanceField<T>(string fieldName) where T : class
        {
            return PublicInstanceField(fieldName) as T;
        }

        public void CallPublicInstanceMethod(string methodName, params object[] args)
        {
            _InvokeMethod(methodName, PublicInstanceMethodFlag, args);
        }

        public void CallPrivateInstanceMethod(string methodName, params object[] args)
        {
            _InvokeMethod(methodName, PrivateInstanceMethodFlag, args);
        }

        private object _GetFiled(string fieldName, BindingFlags flags)
        {
            if (null == InnerType) return null;
            var field = InnerType.GetField(fieldName, flags);
            return field != null ? field.GetValue(InnerObject) : null;
        }

        private void _InvokeMethod(string methodName, BindingFlags flags, params object[] args)
        {
            if (InnerType == null) return;
            var method = InnerType.GetMethod(methodName, flags);
            if (method == null) return;
            method.Invoke(InnerObject, args);
        }
    }
}
