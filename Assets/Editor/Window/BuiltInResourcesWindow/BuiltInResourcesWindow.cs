using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BuiltInResourcesWindow : EditorWindow
{
	[MenuItem( "Window/Built-in styles and icons" )] 
	public static void ShowWindow()
	{
		BuiltInResourcesWindow w = (BuiltInResourcesWindow)EditorWindow.GetWindow<BuiltInResourcesWindow>();
		w.Show();
	}

	private struct Drawing
	{
		public Rect Rect;
		public Action Draw;
	}

	private List<Drawing> Drawings;

	private float _scrollPos;
	private float _maxY;
	private Rect _oldPosition;

	private bool _showingStyles = true;
	private bool _showingIcons = false;

	private string _search = "";
	private bool _showStyleText = true;
	private bool _showIconOriginalSize = true;

	private string[] _buildIcons = new string[]
	{
		"_Help",
		"_Popup",
		"AboutWindow.MainHeader",
		"AgeiaLogo",
		"Animation.AddEvent",
		"Animation.AddKeyframe",
		"Animation.EventMarker",
		"Animation.FirstKey",
		"Animation.LastKey",
		"Animation.NextKey",
		"Animation.Play",
		"Animation.PrevKey",
		"Animation.Record",
		"Animation.SequencerLink",
		"animationvisibilitytoggleoff",
		"animationvisibilitytoggleon",
		"AnimationWrapModeMenu",
		"AnimatorController Icon",
		"AS Badge Delete",
		"AS Badge Move",
		"AS Badge New",
		"AudioMixerController Icon",
		"Avatar Icon",
		"AvatarInspector/BodyPartPicker",
		"AvatarInspector/BodySilhouette",
		"AvatarInspector/BodySIlhouette",
		"AvatarInspector/DotFill",
		"AvatarInspector/DotFrame",
		"AvatarInspector/DotFrameDotted",
		"AvatarInspector/DotSelection",
		"AvatarInspector/Head",
		"AvatarInspector/HeadZoom",
		"AvatarInspector/HeadZoomSilhouette",
		"AvatarInspector/LeftArm",
		"AvatarInspector/LeftFeetIk",
		"AvatarInspector/LeftFingers",
		"AvatarInspector/LeftFingersIk",
		"AvatarInspector/LeftHandZoom",
		"AvatarInspector/LeftHandZoomSilhouette",
		"AvatarInspector/LeftLeg",
		"AvatarInspector/MaskEditor_Root",
		"AvatarInspector/RightArm",
		"AvatarInspector/RightFeetIk",
		"AvatarInspector/RightFingers",
		"AvatarInspector/RightFingersIk",
		"AvatarInspector/RightHandZoom",
		"AvatarInspector/RightHandZoomSilhouette",
		"AvatarInspector/RightLeg",
		"AvatarInspector/Torso",
		"AvatarPivot",
		"boo Script Icon",
		"BuildSettings.Android",
		"BuildSettings.Android.Small",
		"BuildSettings.BlackBerry",
		"BuildSettings.BlackBerry.Small",
		"BuildSettings.Editor.Small",
		"BuildSettings.FlashPlayer",
		"BuildSettings.FlashPlayer.Small",
		"BuildSettings.iPhone",
		"BuildSettings.iPhone.Small",
		"BuildSettings.Metro",
		"BuildSettings.Metro.Small",
		"BuildSettings.PS4",
		"BuildSettings.PS4.Small",
		"BuildSettings.PSM",
		"BuildSettings.PSM.Small",
		"BuildSettings.PSP2",
		"BuildSettings.PSP2.Small",
		"BuildSettings.SamsungTV",
		"BuildSettings.SamsungTV.Small",
		"BuildSettings.SelectedIcon",
		"BuildSettings.Standalone",
		"BuildSettings.Standalone.Small",
		"BuildSettings.Tizen",
		"BuildSettings.Tizen.Small",
		"BuildSettings.Web",
		"BuildSettings.Web.Small",
		"BuildSettings.WP8",
		"BuildSettings.WP8.Small",
		"BuildSettings.XBox360",
		"BuildSettings.XBox360.Small",
		"BuildSettings.XboxOne",
		"BuildSettings.XboxOne.Small",
		"Camera Icon",
		"Clipboard",
		"ClothInspector.PaintTool",
		"ClothInspector.PaintValue",
		"ClothInspector.SelectTool",
		"ClothInspector.SettingsTool",
		"ClothInspector.ViewValue",
		"CloudConnect",
		"CollabChanges Icon",
		"CollabChangesConflict Icon",
		"CollabChangesDeleted Icon",
		"CollabConflict Icon",
		"CollabCreate Icon",
		"CollabDeleted Icon",
		"CollabEdit Icon",
		"CollabExclude Icon",
		"CollabMoved Icon",
		"ColorPicker.CycleColor",
		"ColorPicker.CycleSlider",
		"console.warnicon",
		"cs Script Icon",
		"EditCollider",
		"editicon.sml",
		"EyeDropper.Large",
		"GameObject Icon",
		"HorizontalSplit",
		"Icon Dropdown",
		"js Script Icon",
		"lightMeter/greenLight",
		"lightMeter/lightRim",
		"lightMeter/orangeLight",
		"lightMeter/redLight",
		"LockIcon",
		"LockIcon-On",
		"LookDevCenterLight",
		"LookDevClose",
		"LookDevEnvRotation",
		"LookDevMirrorViewsActive",
		"LookDevMirrorViewsInactive",
		"LookDevObjRotation",
		"LookDevPaneOption",
		"LookDevResetEnv",
		"LookDevShadow",
		"LookDevSideBySide",
		"LookDevSingle1",
		"LookDevSingle2",
		"LookDevSplit",
		"LookDevZone",
		"MeshRenderer Icon",
		"Mirror",
		"MonoLogo",
		"MoveTool",
		"MoveTool On",
		"PauseButton",
		"PauseButton Anim",
		"PauseButton On",
		"PlayButton",
		"PlayButton Anim",
		"PlayButton On",
		"PlayButtonProfile",
		"PlayButtonProfile Anim",
		"PlayButtonProfile On",
		"playLoopOff",
		"preAudioAutoPlayOff",
		"preAudioAutoPlayOn",
		"preAudioLoopOff",
		"preAudioLoopOn",
		"preAudioPlayOff",
		"preAudioPlayOn",
		"Prefab Icon",
		"PrefabModel Icon",
		"PrefabNormal Icon",
		"PreMatCube",
		"PreMatCylinder",
		"PreMatLight0",
		"PreMatLight1",
		"PreMatQuad",
		"PreMatSphere",
		"PreMatTorus",
		"PreTextureAlpha",
		"PreTextureMipMapHigh",
		"PreTextureMipMapLow",
		"PreTextureRGB",
		"Profiler.NextFrame",
		"Profiler.PrevFrame",
		"Profiler.Record",
		"RectTool",
		"RectTool On",
		"RectTransformBlueprint",
		"RectTransformRaw",
		"renderdoc",
		"RotateTool",
		"RotateTool On",
		"ScaleTool",
		"ScaleTool On",
		"SceneviewAudio",
		"SceneviewFx",
		"SceneviewLighting",
		"ScriptableObject Icon",
		"SettingsIcon",
		"Shader Icon",
		"SocialNetworks.FacebookShare",
		"SocialNetworks.LinkedInShare",
		"SocialNetworks.Tweet",
		"SocialNetworks.UDNLogo",
		"SocialNetworks.UDNOpen",
		"SpeedScale",
		"Sprite Icon",
		"StepButton",
		"StepButton Anim",
		"StepButton On",
		"sv_icon_dot0_pix16_gizmo",
		"sv_icon_dot1_pix16_gizmo",
		"sv_icon_dot2_pix16_gizmo",
		"sv_icon_dot3_pix16_gizmo",
		"sv_icon_dot4_pix16_gizmo",
		"sv_icon_dot5_pix16_gizmo",
		"sv_icon_dot6_pix16_gizmo",
		"sv_icon_dot7_pix16_gizmo",
		"sv_icon_dot8_pix16_gizmo",
		"sv_icon_dot9_pix16_gizmo",
		"sv_icon_dot10_pix16_gizmo",
		"sv_icon_dot11_pix16_gizmo",
		"sv_icon_dot12_pix16_gizmo",
		"sv_icon_dot13_pix16_gizmo",
		"sv_icon_dot14_pix16_gizmo",
		"sv_icon_dot15_pix16_gizmo",
		"sv_icon_dot0_sml",
		"sv_icon_dot1_sml",
		"sv_icon_dot2_sml",
		"sv_icon_dot3_sml",
		"sv_icon_dot4_sml",
		"sv_icon_dot5_sml",
		"sv_icon_dot6_sml",
		"sv_icon_dot7_sml",
		"sv_icon_dot8_sml",
		"sv_icon_dot9_sml",
		"sv_icon_dot10_sml",
		"sv_icon_dot11_sml",
		"sv_icon_dot12_sml",
		"sv_icon_dot13_sml",
		"sv_icon_dot14_sml",
		"sv_icon_dot15_sml",
		"sv_icon_name0",
		"sv_icon_name1",
		"sv_icon_name2",
		"sv_icon_name3",
		"sv_icon_name4",
		"sv_icon_name5",
		"sv_icon_name6",
		"sv_icon_name7",
		"sv_icon_none",
		"sv_label_0",
		"sv_label_1",
		"sv_label_2",
		"sv_label_3",
		"sv_label_4",
		"sv_label_5",
		"sv_label_6",
		"sv_label_7",
		"Terrain Icon",
		"TerrainInspector.TerrainToolPlants",
		"TerrainInspector.TerrainToolRaise",
		"TerrainInspector.TerrainToolSetHeight",
		"TerrainInspector.TerrainToolSettings",
		"TerrainInspector.TerrainToolSmoothHeight",
		"TerrainInspector.TerrainToolSplat",
		"TerrainInspector.TerrainToolTrees",
		"TextAsset Icon",
		"Toolbar Minus",
		"Toolbar Plus",
		"Toolbar Plus More",
		"tree_icon",
		"tree_icon_branch",
		"tree_icon_branch_frond",
		"tree_icon_frond",
		"tree_icon_leaf",
		"TreeEditor.AddBranches",
		"TreeEditor.AddLeaves",
		"TreeEditor.BranchFreeHand",
		"TreeEditor.BranchFreeHand On",
		"TreeEditor.BranchRotate",
		"TreeEditor.BranchRotate On",
		"TreeEditor.BranchTranslate",
		"TreeEditor.BranchTranslate On",
		"TreeEditor.Duplicate",
		"TreeEditor.LeafRotate",
		"TreeEditor.LeafRotate On",
		"TreeEditor.LeafTranslate",
		"TreeEditor.LeafTranslate On",
		"TreeEditor.Refresh",
		"TreeEditor.Trash",
		"UnityLogo",
		"VerticalSplit",
		"ViewToolMove",
		"ViewToolMove On",
		"ViewToolOrbit",
		"ViewToolOrbit On",
		"ViewToolZoom",
		"ViewToolZoom On",
		"WaitSpin00",
		"WaitSpin01",
		"WaitSpin02",
		"WaitSpin03",
		"WaitSpin04",
		"WaitSpin05",
		"WaitSpin06",
		"WaitSpin07",
		"WaitSpin08",
		"WaitSpin09",
		"WaitSpin10",
		"WaitSpin11",
		"WelcomeScreen.AssetStoreLogo",
	};

	void OnGUI()
	{
		if( position.width != _oldPosition.width && Event.current.type == EventType.Layout )
		{
			Drawings = null;
			_oldPosition = position;
		}

		GUILayout.BeginHorizontal();

		if( GUILayout.Toggle( _showingStyles, "Styles", EditorStyles.toolbarButton ) != _showingStyles )
		{
			_showingStyles = !_showingStyles;
			_showingIcons = !_showingStyles;
			Drawings = null;
		}

		if( GUILayout.Toggle( _showingIcons, "Icons", EditorStyles.toolbarButton ) != _showingIcons )
		{
			_showingIcons = !_showingIcons;
			_showingStyles = !_showingIcons;
			Drawings = null;
		}
		 
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		string newSearch = EditorGUILayout.DelayedTextField( _search );
		if (newSearch != _search)
		{
			_search = newSearch;
			Drawings = null;
		}

		if( _showingStyles )
		{
			bool newShow = GUILayout.Toggle( _showStyleText, "Show Text", GUILayout.Width( 150 ) );
			if( newShow != _showStyleText )
			{
				_showStyleText = newShow;
				Drawings = null;
			}
		}
		else
		{
			bool newShow = GUILayout.Toggle( _showIconOriginalSize, "Show Original Size", GUILayout.Width( 150 ) );
			if( newShow != _showIconOriginalSize )
			{
				_showIconOriginalSize = newShow;
				Drawings = null;
			}
		}

		GUILayout.EndHorizontal();

		float top = 36;
		if( Drawings == null )
		{
			string lowerSearch = _search.ToLower();

			Drawings = new List<Drawing>();

			GUIContent inactiveText = new GUIContent("inactive");
			GUIContent activeText =  new GUIContent("active");

			float x = 5.0f;
			float y = 5.0f;

			if( _showingStyles )
			{
				foreach( GUIStyle ss in GUI.skin.customStyles )
				{
					if (lowerSearch != "" && !ss.name.ToLower().Contains(lowerSearch))
						continue;

					GUIStyle thisStyle = ss;

					Drawing draw = new Drawing();

					float width = Mathf.Max(
						100.0f,
						GUI.skin.button.CalcSize( new GUIContent( ss.name ) ).x,
						ss.CalcSize( inactiveText ).x + ss.CalcSize( activeText ).x
					) + 16.0f;

					float height = 60.0f;

					if( x + width > position.width - 32 && x > 5.0f )
					{
						x = 5.0f;
						y += height + 10.0f;
					}

					draw.Rect = new Rect( x, y, width, height );

					width -= 8.0f;

					draw.Draw = () =>
					{
						if( GUILayout.Button( thisStyle.name, GUILayout.Width( width ) ) )
							CopyText( "(GUIStyle)\"" + thisStyle.name + "\"" );

						GUILayout.BeginHorizontal();
						GUILayout.Toggle( false, _showStyleText ? inactiveText : new GUIContent(""), thisStyle, GUILayout.Width( width / 2 ) );
						GUILayout.Toggle( true, _showStyleText ? activeText : new GUIContent(""), thisStyle, GUILayout.Width( width / 2 ) );
						GUILayout.EndHorizontal();
					};

					x += width + 18.0f;

					Drawings.Add( draw );
				}
			}
			else if( _showingIcons )
			{
				float rowHeight = 0.0f;

				for( int i=0;i<_buildIcons.Length;i++ )
				{
					string name = _buildIcons[i];
					if( string.IsNullOrEmpty( name ) )
						continue;

					if( !string.IsNullOrEmpty( lowerSearch ) && !name.ToLower().Contains( lowerSearch ))
						continue;

					GUIContent content = EditorGUIUtility.IconContent( name );

					Drawing draw = new Drawing();

					Vector2 nameSize = GUI.skin.button.CalcSize( new GUIContent( name ) );
					Vector2 contentSize = _showIconOriginalSize ? GUI.skin.button.CalcSize( content ) : new Vector2( 45, 45 ) ;

					float width = Mathf.Max(
						nameSize.x,
						contentSize.x
					) + 8.0f;

					float height = contentSize.y + nameSize.y + 8.0f; 

					if( x + width > position.width - 32.0f )
					{
						x = 5.0f;
						y += rowHeight + 8.0f;
						rowHeight = 0.0f;
					}

					draw.Rect = new Rect( x, y, width, height );

					rowHeight = Mathf.Max( rowHeight, height );

					width -= 8.0f;

					draw.Draw = () =>
					{
						if( GUILayout.Button( name, GUILayout.Width( width ) ) )
							CopyText( "EditorGUIUtility.IconContent( \"" + name + "\" )" );

						Rect textureRect = GUILayoutUtility.GetRect( contentSize.x, width, contentSize.y, height, GUILayout.ExpandHeight( false ), GUILayout.ExpandWidth( false ) );
						GUI.Label( textureRect, content );
					};

					x += width + 8.0f;

					Drawings.Add( draw );
				}
			}

			_maxY = y;
		}

		Rect r = position;
		r.y = top;
		r.height -= r.y;
		r.x = r.width - 16;
		r.width = 16;

		float areaHeight = position.height - top;
		_scrollPos = GUI.VerticalScrollbar( r, _scrollPos, areaHeight, 0.0f, _maxY );

		Rect area = new Rect(0, top, position.width - 16.0f, areaHeight);
		GUILayout.BeginArea( area );

		int count = 0;
		foreach( Drawing draw in Drawings )
		{
			Rect newRect = draw.Rect;
			if( _scrollPos < 0 )
				_scrollPos = 0;
			newRect.y -= _scrollPos;

			if( newRect.y + newRect.height > 0 && newRect.y < areaHeight )
			{
				GUILayout.BeginArea( newRect, GUI.skin.textField );
				draw.Draw();
				GUILayout.EndArea();

				count++;
			}
		}

		GUILayout.EndArea();
	}

	void CopyText( string pText )
	{
		TextEditor editor = new TextEditor();

		//editor.content = new GUIContent(pText); // Unity 4.x code
		editor.text= pText; // Unity 5.x code

		editor.SelectAll();
		editor.Copy();
	}
}