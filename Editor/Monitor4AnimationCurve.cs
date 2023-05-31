using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HierarchyHelper;

public class Monitor4AnimationCurve : EditorWindow
{
	private Transform _monitorTransform = null;

	void OnEnable ()
	{
		if( EditorApplication.isPlayingOrWillChangePlaymode )
			return;

		_monitorTransform = null;
		HierarchyChangedDetecter.OnHierarchyChnaged += MonitorGameObject;
	}

	void OnDisable()
	{
		HierarchyChangedDetecter.OnHierarchyChnaged -= MonitorGameObject;
	}

	string GetRelativeName( Transform t, bool includeSelf )
	{
		if( t == null )
			return string.Empty;
		
		string path = includeSelf ? t.name : string.Empty;
		while( t != _monitorTransform && t.parent != null )
		{
			t = t.parent.gameObject.transform;
			path = t.name + "/" + path;
		}

		int i = path.IndexOf( "/" );
		if( i < 0 )
			path = "";
		else
			path = path.Substring( i+1 ) ;

		return path;
	}

	string GetLastRelativeName( HierarchyChangedDetecter.EChangeType type, HierarchyChangedDetecter.HierarchySnapshot snapshot )
	{
		if( type == HierarchyChangedDetecter.EChangeType.Renamed )
		{
			return GetRelativeName( snapshot.me, false ) + snapshot.name;
		}
		else if( type == HierarchyChangedDetecter.EChangeType.Parented )
		{
			if( snapshot.parent == null || !snapshot.parent.IsChildOf( _monitorTransform ) )
				return string.Empty;
			
			string path = GetRelativeName( snapshot.parent, true );
			if( string.IsNullOrEmpty( path ) )
				return snapshot.me.name;
			else
				return path + "/" + snapshot.me.name;
		}
		else if( type == HierarchyChangedDetecter.EChangeType.Created )
		{
		}
		else if( type == HierarchyChangedDetecter.EChangeType.Deleted )
		{
		}

		return string.Empty;
	}

	void MonitorGameObject( HierarchyChangedDetecter.EChangeType type, HierarchyChangedDetecter.HierarchySnapshot snapshot )
	{
		if( _monitorTransform == null )
			return;

		if( type == HierarchyChangedDetecter.EChangeType.Deleted )
		{
			if( snapshot.parent == null )
				return;
			if( !snapshot.parent.IsChildOf( _monitorTransform ) && snapshot.parent != _monitorTransform )
				return;
		}

		if( type == HierarchyChangedDetecter.EChangeType.Parented )
		{
			if( snapshot.me.parent == null )
				return;
		}

		string oldPath = GetLastRelativeName( type, snapshot );
		string path = GetRelativeName( snapshot.me, true );

		Debug.LogWarning( oldPath + " => " + path );

		if( string.IsNullOrEmpty( oldPath ) )
			return;
		
		bool changed = false;
		AnimationClip[] anims = AnimationUtility.GetAnimationClips( _monitorTransform.gameObject );

		if( anims != null && anims.Length > 0 )
		{
			foreach( AnimationClip ac in anims )
			{
				EditorCurveBinding[] objectCurveBinding = AnimationUtility.GetObjectReferenceCurveBindings( ac );
				EditorCurveBinding[] curveDataBinding = AnimationUtility.GetCurveBindings( ac );

				for (int i=0; i<objectCurveBinding.Length; i++)
				{
					if( objectCurveBinding[i].path.CompareTo( oldPath ) == 0 || objectCurveBinding[i].path.StartsWith( oldPath + "/" ) )
					{
						int index = objectCurveBinding[i].path.IndexOf( oldPath );
						string newPath = path + objectCurveBinding[i].path.Substring( index + oldPath.Length );

						ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve( ac, objectCurveBinding[i] );
						AnimationUtility.SetObjectReferenceCurve( ac, objectCurveBinding[i], null );
						objectCurveBinding[i].path = newPath;
						AnimationUtility.SetObjectReferenceCurve( ac, objectCurveBinding[i], keyframes );

						changed = true;
					}
				}

				for (int i=0; i<curveDataBinding.Length; i++)
				{
					if( curveDataBinding[i].path.CompareTo( oldPath ) == 0 || curveDataBinding[i].path.StartsWith( oldPath + "/" ) )
					{
						int index = curveDataBinding[i].path.IndexOf( oldPath );
						string newPath = path + curveDataBinding[i].path.Substring( index + oldPath.Length );

						AnimationCurve c = AnimationUtility.GetEditorCurve( ac, curveDataBinding[i] );
						AnimationUtility.SetEditorCurve( ac, curveDataBinding[i], null );
						curveDataBinding[i].path = newPath;
						AnimationUtility.SetEditorCurve( ac, curveDataBinding[i], c );

						changed = true;
					}
				}
			}
				
		}

		if( changed )
		{
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorApplication.RepaintAnimationWindow();
		}
	}

	void OnGUI ()
	{	
		_monitorTransform = EditorGUILayout.ObjectField( "Monitor", _monitorTransform, typeof( Transform ), true ) as Transform;
	}
	
	[MenuItem("Tools/Monitor for Animation Curve", false, 0)]
	static public void OpenWindow()
	{
		EditorWindow.GetWindow<Monitor4AnimationCurve>( false, "Monitor4AnimationCurve", true );
	}
}