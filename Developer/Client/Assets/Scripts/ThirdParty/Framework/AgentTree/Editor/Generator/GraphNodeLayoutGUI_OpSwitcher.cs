#if UNITY_EDITOR
//auto generator
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;
namespace Framework.Plugin.AT
{
	public partial class GraphNodeLayoutGUI
	{
		[GraphNodeDrawGUIMethod(60)]
		public static bool OpSwitcher_DrawOp_Clear(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
				foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
				{
					AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
				}
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(1);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(61)]
		public static bool OpSwitcher_DrawOp_Destroy(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
				foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
				{
					AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
				}
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(1);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(10)]
		public static bool OpSwitcher_DrawOp_Add(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");AgentTreeEditorUtils.vPopVariables.Add("String");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(11)]
		public static bool OpSwitcher_DrawOp_Sub(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(12)]
		public static bool OpSwitcher_DrawOp_Mul(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(13)]
		public static bool OpSwitcher_DrawOp_Dev(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(14)]
		public static bool OpSwitcher_DrawOp_Reverse(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Bool");AgentTreeEditorUtils.vPopVariables.Add("BoolList");AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(1);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(30)]
		public static bool OpSwitcher_DrawOp_EqualTo(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Bool");AgentTreeEditorUtils.vPopVariables.Add("BoolList");AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");AgentTreeEditorUtils.vPopVariables.Add("String");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(1);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(31)]
		public static bool OpSwitcher_DrawOp_AddEqualTo(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");AgentTreeEditorUtils.vPopVariables.Add("String");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(32)]
		public static bool OpSwitcher_DrawOp_SubEqualTo(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(33)]
		public static bool OpSwitcher_DrawOp_MulEqualTo(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(34)]
		public static bool OpSwitcher_DrawOp_DevEqualTo(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(20)]
		public static bool OpSwitcher_DrawOp_Dot(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Vector2");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(11);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(21)]
		public static bool OpSwitcher_DrawOp_Cross(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Vector2");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(11);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(22)]
		public static bool OpSwitcher_DrawOp_Distance(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Vector2");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(11);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(23)]
		public static bool OpSwitcher_DrawOp_CurveValue(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(24)]
		public static bool OpSwitcher_DrawOp_CurveLenth(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 1)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(25)]
		public static bool OpSwitcher_DrawOp_CurveDuration(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 1)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(40)]
		public static bool OpSwitcher_DrawOp_MinValue(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(41)]
		public static bool OpSwitcher_DrawOp_MaxValue(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(42)]
		public static bool OpSwitcher_DrawOp_Clamp(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(43)]
		public static bool OpSwitcher_DrawOp_ClampEqualTo(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(44)]
		public static bool OpSwitcher_DrawOp_Lerp(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(45)]
		public static bool OpSwitcher_DrawOp_LerpEqualTo(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.betweenVar != null)
			{
				int index = -1;
				if(!exportData.betweenVar.bAll && exportData.betweenVar.end > exportData.betweenVar.begin && exportData.betweenVar.begin> EVariableType.Null)
				{
					for(EVariableType i = exportData.betweenVar.begin; i <= exportData.betweenVar.end; ++i)
					{
						if (preValue == (int)i) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(i.ToString());
					}	
				}
				else if(exportData.betweenVar.bAll)
				{
					foreach (EVariableType v in Enum.GetValues(typeof(EVariableType)))
					{
						if (preValue == (int)v) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(v.ToString());
					}
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.betweenVar.begin);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("Int");				
AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("LongList");AgentTreeEditorUtils.vPopVariables.Add("Float");AgentTreeEditorUtils.vPopVariables.Add("FloatList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector2");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2Int");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector3");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3Int");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");				
AgentTreeEditorUtils.vPopVariables.Add("Quaternion");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("Color");AgentTreeEditorUtils.vPopVariables.Add("ColorList");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(50)]
		public static bool OpSwitcher_DrawOp_ListCount(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 1)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(51)]
		public static bool OpSwitcher_DrawOp_ListUnion(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(53)]
		public static bool OpSwitcher_DrawOp_ListUnionTo(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
					pNode.BindNode.AddOutPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(52)]
		public static bool OpSwitcher_DrawOp_ListCull(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(54)]
		public static bool OpSwitcher_DrawOp_ListCullTo(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
					pNode.BindNode.AddOutPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(55)]
		public static bool OpSwitcher_DrawOp_RemoveListAt(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
					pNode.BindNode.AddOutPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(56)]
		public static bool OpSwitcher_DrawOp_RemoveItem(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
					pNode.BindNode.AddOutPort(pvar);
				}
				{
					Port pvar=null;
					if( pNode.BindNode.GetInArgvCount() >=0)
					{
						 Variable temp = pNode.BindNode.GetInVariable(0);
						if(temp !=null && temp.IsList())
						{
							pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew(temp.GetListElementType(), pVarsFactor, vInTemp, 1);
						}
					}
					if(pvar==null)
					{
						pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(), pVarsFactor, vInTemp, 1);
					}
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(57)]
		public static bool OpSwitcher_DrawOp_AddList(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
					pNode.BindNode.AddOutPort(pvar);
				}
				{
					Port pvar=null;
					if( pNode.BindNode.GetInArgvCount() >=0)
					{
						 Variable temp = pNode.BindNode.GetInVariable(0);
						if(temp !=null && temp.IsList())
						{
							pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew(temp.GetListElementType(), pVarsFactor, vInTemp, 1);
						}
					}
					if(pvar==null)
					{
						pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(), pVarsFactor, vInTemp, 1);
					}
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(58)]
		public static bool OpSwitcher_DrawOp_GetElementAt(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					if( pNode.BindNode.GetInArgvCount() >=0)
					{
						 Variable temp = pNode.BindNode.GetInVariable(0);
						if(temp !=null && temp.IsList())
						{
							pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew(temp.GetListElementType(), pVarsFactor, vInTemp, 2);
						}
					}
					if(pvar==null)
					{
						pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(), pVarsFactor, vInTemp, 2);
					}
					pNode.BindNode.AddInPort(pvar);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(59)]
		public static bool OpSwitcher_DrawOp_IndexOfList(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("BoolList");				
AgentTreeEditorUtils.vPopVariables.Add("ByteList");AgentTreeEditorUtils.vPopVariables.Add("IntList");AgentTreeEditorUtils.vPopVariables.Add("FloatList");AgentTreeEditorUtils.vPopVariables.Add("Vector2List");AgentTreeEditorUtils.vPopVariables.Add("Vector2IntList");				
AgentTreeEditorUtils.vPopVariables.Add("Vector3List");AgentTreeEditorUtils.vPopVariables.Add("Vector3IntList");AgentTreeEditorUtils.vPopVariables.Add("Vector4List");AgentTreeEditorUtils.vPopVariables.Add("QuaternionList");AgentTreeEditorUtils.vPopVariables.Add("ColorList");				
AgentTreeEditorUtils.vPopVariables.Add("StringList");AgentTreeEditorUtils.vPopVariables.Add("CurveList");AgentTreeEditorUtils.vPopVariables.Add("UserDataList");AgentTreeEditorUtils.vPopVariables.Add("MonoScriptList");AgentTreeEditorUtils.vPopVariables.Add("ObjectList");				
			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(2);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					if( pNode.BindNode.GetInArgvCount() >=0)
					{
						 Variable temp = pNode.BindNode.GetInVariable(0);
						if(temp !=null && temp.IsList())
						{
							pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew(temp.GetListElementType(), pVarsFactor, vInTemp, 1);
						}
					}
					if(pvar==null)
					{
						pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(), pVarsFactor, vInTemp, 1);
					}
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(46)]
		public static bool OpSwitcher_DrawOp_LerpColor(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 5)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 3);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vInTemp, 4);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableColor>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(47)]
		public static bool OpSwitcher_DrawOp_LerpVector2(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableVector2>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(48)]
		public static bool OpSwitcher_DrawOp_LerpVector3(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 4)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vInTemp, 3);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableVector3>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(49)]
		public static bool OpSwitcher_DrawOp_LerpVector4(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 5)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableCurve>(pVarsFactor, vInTemp, 3);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vInTemp, 4);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableVector4>(pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(62)]
		public static bool OpSwitcher_DrawOp_Delta(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 1)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
		[GraphNodeDrawGUIMethod(63)]
		public static bool OpSwitcher_DrawOp_Random(GraphNode pNode)
		{
			bool bChanged = false;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 84;
			long preValue = pNode.BindNode.GetCustomValue();
			ATExportNodeAttrData exportData = null;
			AgentTreeEditorUtils.vPopVariables.Clear();
			if (AgentTreeEditor.Instance != null && AgentTreeEditor.Instance.ExportActions.TryGetValue((int)pNode.BindNode.GetExcudeHash(), out exportData) && exportData.canVars != null)
			{
				int index = -1;
				if(exportData.canVars.Length>0)
				{
					for(int i = 0; i < exportData.canVars.Length; ++i)
					{
						if (preValue == (int)exportData.canVars[i]) index = AgentTreeEditorUtils.vPopVariables.Count;
						AgentTreeEditorUtils.vPopVariables.Add(exportData.canVars[i].ToString());
					}	
				}
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if (index >= 0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue((int)exportData.canVars[0]);
			}
			else
			{
AgentTreeEditorUtils.vPopVariables.Add("Byte");				
AgentTreeEditorUtils.vPopVariables.Add("Int");AgentTreeEditorUtils.vPopVariables.Add("Long");AgentTreeEditorUtils.vPopVariables.Add("Float");			
				int index = AgentTreeEditorUtils.vPopVariables.IndexOf(((EVariableType)preValue).ToString());
				index = EditorGUILayout.Popup("类型", index, AgentTreeEditorUtils.vPopVariables.ToArray());
				if(index>=0 && index < AgentTreeEditorUtils.vPopVariables.Count) pNode.BindNode.SetCustomValue((int)Enum.Parse(typeof(EVariableType), AgentTreeEditorUtils.vPopVariables[index]));
				else pNode.BindNode.SetCustomValue(3);
			}
			bChanged = preValue != pNode.BindNode.GetCustomValue();
			EditorGUIUtility.labelWidth = labelWidth;
				AgentTreeEditorUtils.vPopVariables.Clear();
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(!bChanged && (pNode.BindNode.outArgvs == null || pNode.BindNode.outArgvs.Length != 1)) bChanged = true;
			if(bChanged && pNode.BindNode.GetCustomValue()>0)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew((EVariableType)pNode.BindNode.GetCustomValue(),pVarsFactor, vOutTemp,0);
					pNode.BindNode.AddOutPort(pvar);
				}
				pNode.BindNode.Save();
			}
			ATExportNodeAttrData attrData = AgentTreeEditor.Instance.GetActionNodeAttr((int)pNode.BindNode.GetExcudeHash());
			if (attrData.nolinkAttr == null)
			{
				pNode.bLink = true;
				Rect rect = GUILayoutUtility.GetLastRect();
				pNode.InLink.baseNode = pNode;
				pNode.InLink.direction = EPortIO.In;
				GraphNode.LinkField(new Vector2(rect.x - 10, 8), pNode.InLink);
				pNode.OutLink.baseNode = pNode;
				pNode.OutLink.direction = EPortIO.Out;
				GraphNode.LinkField(new Vector2(rect.width + 10, 8), pNode.OutLink);
			}
			else pNode.bLink = false;
			for(int i = 0; i < pNode.BindNode.GetInArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetInEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetInPort(i);
				port.direction = EPortIO.In;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.InArgvs.Count)
				{	
					port.SetDefaultName(attrData.InArgvs[i].DisplayName);
					port.alignType = attrData.InArgvs[i].AlignType;
					port.displayType = attrData.InArgvs[i].DisplayType;
					bShowEdit = attrData.InArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)DrawPropertyGUI.DrawVariable(pNode, port);
				if(port.variable!=null)pNode.Inputs.Add(port);
			}
			for(int i = 0; i < pNode.BindNode.GetOutArgvCount(); ++i)
			{
				ArgvPort port = pNode.BindNode.GetOutEditorPort<ArgvPort>(i);
				port.baseNode = pNode;
				port.port = pNode.BindNode.GetOutPort(i);
				port.direction = EPortIO.Out;
				port.index = i;
				bool bShowEdit = true;
				if (attrData != null && i < attrData.OutArgvs.Count)
				{
					port.SetDefaultName(attrData.OutArgvs[i].Name);
					port.alignType = attrData.OutArgvs[i].AlignType;
					port.displayType = attrData.OutArgvs[i].DisplayType;
					bShowEdit = attrData.OutArgvs[i].bShowEdit;
					if (port.alignType == null && port.port.dummyMap != null && port.port.dummyMap.Count > 0)
					{
						Variable var = port.port.dummyMap.ElementAt(0).Value;
						if (var != null && var.GetClassHashCode() != 0)
							AgentTreeUtl.ExportClasses.TryGetValue(var.GetClassHashCode(), out port.alignType);
					}
					if(!bShowEdit && port.alignType!=null)
					{
						if(port.variable!=null)port.variable.SetClassHashCode(AgentTreeUtl.TypeToHash(port.alignType));
					}
				}
				if(bShowEdit)
				{
					int checkIndex = pNode.BindNode.IndexofInArgv(port.variable);
					if (checkIndex!=-1 && checkIndex<pNode.Inputs.Count)
						DrawPropertyGUI.DrawVariable(pNode, port, pNode.Inputs[checkIndex]);
					else DrawPropertyGUI.DrawVariable(pNode, port);
				}
				if(port.variable!=null)pNode.Outputs.Add(port);
			}
			return true;
		}
	}
}
#endif
