using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HierarchyHelper
{
	[InitializeOnLoad]
	public static class HierarchyChangedDetector
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

		readonly static List<HierarchySnapshot> _hierarchySnapshots = null;
		readonly static List<Transform> _hierarchyTransforms = null;

		public delegate void onHierarchyChanged( EChangeType type, HierarchySnapshot snapshot );
		public static onHierarchyChanged OnHierarchyChanged = delegate( EChangeType type, HierarchySnapshot snapshot ) {};

		static HierarchyChangedDetector()
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

			EditorApplication.hierarchyChanged += HandleHierarchyChange;
		}

		static HierarchySnapshot CreateSnapshot( Transform t )
		{
			HierarchySnapshot h = new HierarchySnapshot();
			h.name = t.name;
			h.parent = t.parent;
			h.me = t;
			return h;
		}

		static void HandleHierarchyChange()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode) return;
			bool found = false;
			for( int i=0;i<_hierarchySnapshots.Count;)
			{
				HierarchySnapshot h = _hierarchySnapshots[i];
				if( h.me == null )
				{
					_hierarchySnapshots.RemoveAt( i );
					_hierarchyTransforms.RemoveAt( i );
					OnHierarchyChanged( EChangeType.Deleted, h );
					found = true;
					continue;
				}

				else if( h.parent != h.me.parent )
				{
					OnHierarchyChanged( EChangeType.Parented, h );
					h.parent = h.me.parent;
					found = true;
					break;
				}

				else if( h.name != h.me.name )
				{
					OnHierarchyChanged( EChangeType.Renamed, h );
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

						OnHierarchyChanged( EChangeType.Created, h );
					}
				}
			}
		}
	}
}
