using UnityEngine;
using System.Collections;

public class RenderQueueModifier : MonoBehaviour
{
	public enum RenderType
	{
		FRONT,
		BACK
	}

	public UIWidget m_target = null;
	public RenderType m_type = RenderType.FRONT;

	Renderer[] _renderers;
	int _lastQueue = 0;

	void Start ()
	{
	 	_renderers = GetComponentsInChildren<Renderer>();
	}
	
	void FixedUpdate() {
		if( m_target == null || m_target.drawCall == null )
			return;
		int queue = m_target.drawCall.renderQueue;
		queue += m_type == RenderType.FRONT ? 1 : -1;
		if( _lastQueue != queue )
		{
			_lastQueue = queue;

			foreach( Renderer r in _renderers )
			{
				r.material.renderQueue = _lastQueue;
			}
		}
	}
}
