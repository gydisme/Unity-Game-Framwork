using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.Events;

[CustomEditor(typeof(MethodCaller))]
public class MethodCallerEditor: Editor
{
	private bool _enabledRunInEditorMode = false;
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		MethodCaller methodCaller = target as MethodCaller;

		if( !Application.isPlaying )
		{
			_enabledRunInEditorMode = EditorGUILayout.Toggle( "Enable Run in Editor Mode", _enabledRunInEditorMode );
			if( _enabledRunInEditorMode )
			{
				EditorGUILayout.HelpBox( "UnityEventCallStates are called when set to 'Editor & Runtime' only", MessageType.Warning );
			}
		}

		if( Application.isPlaying || _enabledRunInEditorMode )
		{
			if( methodCaller.call != null && GUILayout.Button( "Run" ) )
			{
				methodCaller.call.Invoke( );
			}
		}
	}
}