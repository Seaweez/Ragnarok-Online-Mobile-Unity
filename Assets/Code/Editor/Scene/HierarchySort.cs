using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EditorTool.HierarchySort
{
	//Assets/Code/Editor/Scene/HierarchySort.cs(7,28): warning CS0618: `UnityEditor.BaseHierarchySort' is obsolete: `BaseHierarchySort is no longer supported because of performance reasons'
	// public class SortByName : BaseHierarchySort  
	// {  
	// 	public override int Compare(GameObject lhs, GameObject rhs)  
	// 	{  
	// 		if (lhs == rhs) return 0;  
	// 		if (lhs == null) return -1;  
	// 		if (rhs == null) return 1;  
			
	// 		return EditorUtility.NaturalCompare(lhs.name, rhs.name);  
	// 	}  
	// } 
} // namespace EditorTool.HierarchySort
