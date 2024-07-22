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
		[GraphNodeDrawGUIMethod(1699782104)]
		public static bool Framework_BattlePlus_APlayer_IsFrontRow_Boolean(GraphNode pNode)
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
	}
}
#endif
