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
		[GraphNodeDrawGUIMethod(51290915)]
		public static bool Framework_Core_ActorParameter_GetSkill_SkillInformation(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(1157948153)]
		public static bool Framework_Core_ActorParameter_GetActorType_EActorType(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(-1646839704)]
		public static bool Framework_Core_ActorParameter_SetActorID_Int32Void(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(-2134776207)]
		public static bool Framework_Core_ActorParameter_GetActorID_Int32(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(755826881)]
		public static bool Framework_Core_ActorParameter_SetClassify_ByteVoid(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableByte>(pVarsFactor, vInTemp, 1);
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
		[GraphNodeDrawGUIMethod(1347125581)]
		public static bool Framework_Core_ActorParameter_GetClassify_Byte(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableByte>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(-584972700)]
		public static bool Framework_Core_ActorParameter_IsElement_EElementTypeBoolean(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(237601304)]
		public static bool Framework_Core_ActorParameter_SetCareerID_ByteVoid(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableByte>(pVarsFactor, vInTemp, 1);
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
		[GraphNodeDrawGUIMethod(-1403348922)]
		public static bool Framework_Core_ActorParameter_GetCareerID_Byte(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableByte>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(-1560158775)]
		public static bool Framework_Core_ActorParameter_SetDutyID_ByteVoid(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableByte>(pVarsFactor, vInTemp, 1);
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
		[GraphNodeDrawGUIMethod(-1981728411)]
		public static bool Framework_Core_ActorParameter_GetDutyID_Byte(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableByte>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(996557793)]
		public static bool Framework_Core_ActorParameter_SetCareerWeakness_UInt16Void(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(540480927)]
		public static bool Framework_Core_ActorParameter_GetCareerWeakness_UInt16(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(-1911841323)]
		public static bool Framework_Core_ActorParameter_SetRoleGUID_Int64Void(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableLong>(pVarsFactor, vInTemp, 1);
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
		[GraphNodeDrawGUIMethod(241641066)]
		public static bool Framework_Core_ActorParameter_GetRoleGUID_Int64(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableLong>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(89875509)]
		public static bool Framework_Core_ActorParameter_GetQuality_Byte(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableByte>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(-1737352074)]
		public static bool Framework_Core_ActorParameter_GetMaxLevel_UInt16(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(-686842736)]
		public static bool Framework_Core_ActorParameter_GetLevel_UInt16(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(949693102)]
		public static bool Framework_Core_ActorParameter_SetName_StringVoid(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableString>(pVarsFactor, vInTemp, 1);
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
		[GraphNodeDrawGUIMethod(1889457625)]
		public static bool Framework_Core_ActorParameter_GetName_String(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableString>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(744227636)]
		public static bool Framework_Core_ActorParameter_IsShielded_Boolean(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(-1543552952)]
		public static bool Framework_Core_ActorParameter_GetShieldedTime_FFloat(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(-206240772)]
		public static bool Framework_Core_ActorParameter_SetModelHeight_FFloatVoid(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(1362761195)]
		public static bool Framework_Core_ActorParameter_GetModelHeight_FFloat(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(413264564)]
		public static bool Framework_Core_ActorParameter_IsBoydPartBroken_UInt32Boolean(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(1985406151)]
		public static bool Framework_Core_ActorParameter_DoHpDrop_Int32UInt32BooleanVariablePoolAbleBooleanBooleanInt32(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 7)) bChanged = true;
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableInt>().mValue = -1;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 3);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 4);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 5);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 6);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
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
		[GraphNodeDrawGUIMethod(233586886)]
		public static bool Framework_Core_ActorParameter_AppendHP_Int32BooleanVariablePoolAbleInt32(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 3);
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
		[GraphNodeDrawGUIMethod(423197503)]
		public static bool Framework_Core_ActorParameter_ClearAttrStat_EAttrTypeBooleanVoid(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
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
		[GraphNodeDrawGUIMethod(-327976723)]
		public static bool Framework_Core_ActorParameter_GetStatAttr_EAttrTypeBooleanFFloat(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
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
		[GraphNodeDrawGUIMethod(218770426)]
		public static bool Framework_Core_ActorParameter_AppendShield_Int32Int32(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(-1248007065)]
		public static bool Framework_Core_ActorParameter_AppendShieldDuration_FFloatBooleanVoid(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
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
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
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
		[GraphNodeDrawGUIMethod(-274496393)]
		public static bool Framework_Core_ActorParameter_DoSpCost_Int32BooleanVariablePoolAbleInt32(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 3);
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
		[GraphNodeDrawGUIMethod(-118626230)]
		public static bool Framework_Core_ActorParameter_AppendSP_Int32BooleanVariablePoolAbleInt32(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 3);
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
		[GraphNodeDrawGUIMethod(1329042990)]
		public static bool Framework_Core_ActorParameter_HasPosture_Boolean(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(917025435)]
		public static bool Framework_Core_ActorParameter_IsExhaustioning_Boolean(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(1865885123)]
		public static bool Framework_Core_ActorParameter_DropPosture_Int32BooleanVariablePoolAbleInt32(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 3);
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
		[GraphNodeDrawGUIMethod(326899574)]
		public static bool Framework_Core_ActorParameter_AppendPosture_Int32BooleanVariablePoolAbleInt32(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = true;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vInTemp, 3);
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
		[GraphNodeDrawGUIMethod(1623966774)]
		public static bool Framework_Core_ActorParameter_GetBaseAttr_EAttrTypeFFloat(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(-1142089227)]
		public static bool Framework_Core_ActorParameter_GetExternAttr_EBuffAttrTypeFFloat(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(-613243463)]
		public static bool Framework_Core_ActorParameter_UpdataAttr_Void(GraphNode pNode)
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
		[GraphNodeDrawGUIMethod(146676832)]
		public static bool Framework_Core_ActorParameter_AddpendBaseAttr_EAttrTypeFFloatVoid(GraphNode pNode)
		{
			bool bChanged = false;
			if(!bChanged && (pNode.BindNode.inArgvs == null || pNode.BindNode.inArgvs.Length != 3)) bChanged = true;
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
	}
}
#endif
