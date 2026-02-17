using UnityEditor;

#if UNITY_EDITOR

namespace AoV.Editor
{
	/// <summary>
	/// Use the default IMGUI Inspector for serialized types that are broken in EditorAttributes
	/// </summary>
	[CustomEditor(typeof(AoV.System.CheckpointSystem))]
	public class UseDefaultInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			EditorGUILayout.LabelField("Using Default IMGUI");
		}
	}

}

#endif