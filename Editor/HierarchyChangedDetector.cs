using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

namespace HierarchyHelper
{
	public class HierarchyChangedDetector : IDisposable
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

		readonly List<HierarchySnapshot> _hierarchySnapshots = null;
		readonly List<Transform> _hierarchyTransforms = null;

		readonly Action<EChangeType, HierarchySnapshot> _onHierarchyChange = ( EChangeType type, HierarchySnapshot snapshot ) => {};
		readonly Transform _root;

		public HierarchyChangedDetector(
			Transform root,
			Action<EChangeType, HierarchySnapshot> onHierarchyChange)
		{
			_root = root;
			_hierarchySnapshots = new List<HierarchySnapshot>();
			_hierarchyTransforms = new List<Transform>();
			_onHierarchyChange = onHierarchyChange;

			foreach( Transform t in GetHierarchy() )
			{
				HierarchySnapshot h = CreateSnapshot( t );
				_hierarchySnapshots.Add( h );
				_hierarchyTransforms.Add( t );
			}

			EditorApplication.hierarchyChanged += HandleHierarchyChange;
		}

		public void Dispose()
		{
			EditorApplication.hierarchyChanged -= HandleHierarchyChange;
		}

		Transform[] GetHierarchy() =>
			_root.GetComponentsInChildren<Transform>(includeInactive: true);

		static HierarchySnapshot CreateSnapshot( Transform t )
		{
			HierarchySnapshot h = new HierarchySnapshot();
			h.name = t.name;
			h.parent = t.parent;
			h.me = t;
			return h;
		}

		void HandleHierarchyChange()
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
					_onHierarchyChange( EChangeType.Deleted, h );
					found = true;
					continue;
				}

				else if( h.parent != h.me.parent )
				{
					_onHierarchyChange( EChangeType.Parented, h );
					h.parent = h.me.parent;
					found = true;
					break;
				}

				else if( h.name != h.me.name )
				{
					_onHierarchyChange( EChangeType.Renamed, h );
					h.name = h.me.name;
					found = true;
					break;
				}

				i++;
			}

			if( !found )
			{
				foreach( Transform t in GetHierarchy() )
				{
					if( !_hierarchyTransforms.Contains( t ) )
					{
						HierarchySnapshot h = CreateSnapshot( t );
						_hierarchySnapshots.Add( h );
						_hierarchyTransforms.Add( t );

						_onHierarchyChange( EChangeType.Created, h );
					}
				}
			}
		}
	}
}
