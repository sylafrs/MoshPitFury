using UnityEditor;
using UnityEngine;
using System.Reflection;

public static class RemoveLabelsFromCrowd
{
	[MenuItem("Action/RemoveLabelsFromCrowd")]
	public static void action()
	{
		Crowd[] players = GameObject.FindObjectsOfType<Crowd>();
		foreach (Crowd p in players)
		{
			var editorGUIUtilityType = typeof(EditorGUIUtility);
			var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
			var args = new object[] { p.gameObject, null };
			editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);


		}
	}
}
