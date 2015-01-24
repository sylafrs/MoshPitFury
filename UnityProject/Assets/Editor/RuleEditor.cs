using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Rule))]
public class RuleEditor : Editor
{
	private Rule rule;

	private void OnEnable()
	{
		this.rule = target as Rule;
	}

	private void OnSceneGUI()
	{
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.blue;
		style.alignment = TextAnchor.MiddleCenter;

		if (rule && rule.SpawnPoints != null)
		{
			for (int i = 0; i < rule.SpawnPoints.Length; i++)
			{
				Handles.Label(rule.SpawnPoints[i].position + Vector3.up * 10, "Spawn #" + (i + 1), style);
			}
		}
	}	
}
