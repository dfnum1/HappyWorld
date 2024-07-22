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
		[GraphNodeDrawGUIMethod(1234380764)]
		public static bool TopGame_Core_AnimPathData_GetCamera_Camera(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableMonoScript>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(-1785411771)]
		public static bool TopGame_Core_AnimPathData_GetSlot_Int32Transform(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableObject>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(1895479964)]
		public static bool TopGame_Core_AnimPathData_GetSlot_1_StringTransform(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableString>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableObject>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(373338554)]
		public static bool TopGame_Core_AnimPathData_BindTrackObject_StringAInstanceAbleBooleanIUserDataVoid(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 5)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableString>(pVarsFactor, vInTemp, 1);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableMonoScript>(pVarsFactor, vInTemp, 2);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 3);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 4);
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
		[GraphNodeDrawGUIMethod(1922817012)]
		public static bool TopGame_Core_AnimPathData_GetPosition_Vector3(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
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
		[GraphNodeDrawGUIMethod(-2046579737)]
		public static bool TopGame_Core_AnimPathData_GetEulerAngle_Vector3(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
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
		[GraphNodeDrawGUIMethod(430030540)]
		public static bool TopGame_Core_AnimPathData_GetLookAt_Vector3(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
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
		[GraphNodeDrawGUIMethod(1934461359)]
		public static bool TopGame_Core_AnimPathData_GetFov_Single(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
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
		[GraphNodeDrawGUIMethod(-606313989)]
		public static bool TopGame_Core_AnimPathData_SkipTo_SingleVoid(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 2)) bChanged = true;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableFloat>(pVarsFactor, vInTemp, 1);
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
		[GraphNodeDrawGUIMethod(-1945101661)]
		public static bool TopGame_Core_AnimPathData_SkipDo_Void(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 0);
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
	}
}
#endif
