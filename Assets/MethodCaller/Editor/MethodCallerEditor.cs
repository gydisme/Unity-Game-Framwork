using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.Events;

[CustomEditor(typeof(MethodCaller))]
public class MethodCallerEditor: Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if( !Application.isPlaying )
		{
			EditorGUILayout.LabelField( "Run in Play mode only" );
			return;
		}
		else
		{
			MethodCaller methodCaller = target as MethodCaller;

			if( methodCaller.call != null && GUILayout.Button( "Run" ) )
			{
				methodCaller.call.Invoke( );
			}
			
			if( GUI.changed )
			{
				EditorUtility.SetDirty( target );
			}
		}
	}
}