#define Debug_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace HierarchyHelper
{
	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	public static class HierarchyChangedDetecter
	{
		public class HierarchySnapshot
		{
			public Transform me;
			public Transform parent;
			public string name;
		}

		public enum EChangeType
		{
			Renamed,
			Created,
			Parented,
			Deleted
		}

		static List<HierarchySnapshot> _hierarchySnapshots = null;
		static List<Transform> _hierarchyTransforms = null;

		public delegate void onHierarchyChnaged( EChangeType type, HierarchySnapshot snapshot );
		public static onHierarchyChnaged OnHierarchyChnaged = delegate( EChangeType type, HierarchySnapshot snapshot ) {};

		static HierarchyChangedDetecter()
		{
			_hierarchySnapshots = new List<HierarchySnapshot>();
			_hierarchyTransforms = new List<Transform>();

			Transform[] all = GameObject.FindObjectsOfType<Transform>();
			foreach( Transform t in all )
			{
				HierarchySnapshot h = CreateSnapshot( t );
				_hierarchySnapshots.Add( h );
				_hierarchyTransforms.Add( t );
			}

			EditorApplication.hierarchyWindowChanged += OnHierarchyChangeCheck;
			#if Debug
			OnHierarchyChnaged += OnHierarchyChange;
			#endif
		}

		static HierarchySnapshot CreateSnapshot( Transform t )
		{
			HierarchySnapshot h = new HierarchySnapshot();
			h.name = t.name;
			h.parent = t.parent;
			h.me = t;
			return h;
		}

		static void OnHierarchyChangeCheck()
		{
			bool found = false;
			for( int i=0;i<_hierarchySnapshots.Count;)
			{
				HierarchySnapshot h = _hierarchySnapshots[i];
				if( h.me == null )
				{
					_hierarchySnapshots.RemoveAt( i );
					_hierarchyTransforms.RemoveAt( i );
					OnHierarchyChnaged( EChangeType.Deleted, h );
					found = true;
					continue;
				}

				else if( h.parent != h.me.parent )
				{
					OnHierarchyChnaged( EChangeType.Parented, h );
					h.parent = h.me.parent;
					found = true;
					break;
				}

				else if( h.name != h.me.name )
				{
					OnHierarchyChnaged( EChangeType.Renamed, h );
					h.name = h.me.name;
					found = true;
					break;
				}

				i++;
			}

			if( !found )
			{
				Transform[] all = GameObject.FindObjectsOfType<Transform>();
				foreach( Transform t in all )
				{
					if( !_hierarchyTransforms.Contains( t ) )
					{
						HierarchySnapshot h = CreateSnapshot( t );
						_hierarchySnapshots.Add( h );
						_hierarchyTransforms.Add( t );

						OnHierarchyChnaged( EChangeType.Created, h );
					}
				}
			}
		}

		static void OnHierarchyChange( EChangeType type, HierarchySnapshot snapshot )
		{
			string log = type.ToString();
			if( snapshot.me != null )
			{
				log += " name:" + snapshot.me.name + ", parent: " + snapshot.me.parent;
			}
			log += " snapshot name: " + snapshot.name + ", parent: " + snapshot.parent;
			Debug.Log( log );
		}
	}
}
