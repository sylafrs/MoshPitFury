using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
	private Level level;

	private void OnEnable()
	{
		this.level = target as Level;
	}

	private void OnSceneGUI()
	{
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.blue;
		style.alignment = TextAnchor.MiddleCenter;

		if (level && level.SpawnPoints != null)
		{
			for (int i = 0; i < level.SpawnPoints.Length; i++)
			{
				Handles.Label(level.SpawnPoints[i].position + Vector3.up * 10, "Spawn #" + (i + 1), style);
			}
		}
	}	
}
