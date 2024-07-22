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
		[GraphNodeDrawGUIMethod(896849859)]
		public static bool Framework_Core_SkillInformation_GetSkillByTag_UInt32Skill(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableUser>(pVarsFactor, vOutTemp,0);
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
		[GraphNodeDrawGUIMethod(-564122994)]
		public static bool Framework_Core_SkillInformation_EnableSkill_BooleanVoid(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 0);
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
		[GraphNodeDrawGUIMethod(550137578)]
		public static bool Framework_Core_SkillInformation_CanInterrupt_Boolean(GraphNode pNode)
		{
			bool bChanged = false;
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
		[GraphNodeDrawGUIMethod(328911757)]
		public static bool Framework_Core_SkillInformation_GetInterruptFactor_FFloat(GraphNode pNode)
		{
			bool bChanged = false;
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
		[GraphNodeDrawGUIMethod(467988297)]
		public static bool Framework_Core_SkillInformation_ClearSkill_Void(GraphNode pNode)
		{
			bool bChanged = false;
			if(bChanged)
			{
				VariableFactory pVarsFactor = AgentTreeManager.getInstance().GetVariableFactory();
				if(AgentTreeEditor.Instance) AgentTreeEditor.Instance.AdjustMaxGuid();
				List<Port> vInTemp = (pNode.BindNode.inArgvs!=null)?new List<Port>(pNode.BindNode.inArgvs):null;
				List<Port> vOutTemp = (pNode.BindNode.outArgvs!=null)?new List<Port>(pNode.BindNode.outArgvs):null;
				pNode.BindNode.ClearArgv();
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
		[GraphNodeDrawGUIMethod(-936478076)]
		public static bool Framework_Core_SkillInformation_CanSkill_UInt32BooleanBooleanBoolean(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 1);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = false;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = false;
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
		[GraphNodeDrawGUIMethod(-1034254774)]
		public static bool Framework_Core_SkillInformation_CanSkillByTag_UInt32BooleanBooleanBoolean(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 0);
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 1);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = false;
					pNode.BindNode.AddInPort(pvar);
				}
				{
					Port pvar=null;
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableBool>(pVarsFactor, vInTemp, 2);
					pvar.GetVariable<Framework.Plugin.AT.VariableBool>().mValue = false;
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
		[GraphNodeDrawGUIMethod(862262881)]
		public static bool Framework_Core_SkillInformation_ForceDoSkill_UInt32Void(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 0);
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
		[GraphNodeDrawGUIMethod(1272877384)]
		public static bool Framework_Core_SkillInformation_ForceDoSkillByTag_UInt32Void(GraphNode pNode)
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
					pvar = AgentTreeEditorUtils.BuildOriVariableCommonNew<Framework.Plugin.AT.VariableInt>(pVarsFactor, vInTemp, 0);
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
