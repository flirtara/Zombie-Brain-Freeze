using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Endgame
{
	public class FontChangerWindow : EditorWindow
	{
		[MenuItem("Assets/Change Font...")]
		public static void ShowWindow()
		{
			Rect rect = new Rect(0, 0, 400, 300);
			EditorWindow.GetWindowWithRect(typeof(FontChangerWindow), rect: rect, utility: true, title: "Font changer");
		}

		private string statusText = "Inactive";
		private bool remapObjectsInScene = true;
		private bool remapObjectsInProject = true;
		private bool onlyRemapInTextComponents = false;
		private Object objectToRemapTo;
		private System.Type remapObjectType = typeof(Font);
		private int addFontSize = 0;
		private int setFontSize = -1;
		private string setFontSizeText = "10";
		private string addFontSizeText = "4";
		private string subFontSizeText = "4";

		private enum Mode
		{
			FaceChange,
			SizeSet,
			SizeAdd
		}

		private Mode mode = Mode.FaceChange;

		private bool ShouldChangeFontSize
		{
			get
			{
				return (this.mode == Mode.SizeAdd) || (this.mode == Mode.SizeSet);
			}
		}

		void OnGUI()
		{
			GUILayout.Space(5);

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Font to change to:");
			this.objectToRemapTo = EditorGUILayout.ObjectField(this.objectToRemapTo, this.remapObjectType, false);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10);

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Change font within game objects in:");
			this.remapObjectsInScene = GUILayout.Toggle(this.remapObjectsInScene, "Scene");
			this.remapObjectsInProject = GUILayout.Toggle(this.remapObjectsInProject, "Project");
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Change font within:");
			string[] componentTypeTexts = 
			{
				"All components",
				"Text components only"
			};
			int selectedComponentTypeIndex = GUILayout.SelectionGrid(
				selected: this.onlyRemapInTextComponents ? 1 : 0,
				texts: componentTypeTexts,
				xCount: 1,
				style: EditorStyles.radioButton);
			this.onlyRemapInTextComponents = (selectedComponentTypeIndex == 1);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10);

			if (GUILayout.Button("Change fonts"))
			{
				StartWorking(Mode.FaceChange);
			}

			GUILayout.Space(20);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Decrease all font sizes by:"))
			{
				this.addFontSize = -System.Convert.ToInt32(this.subFontSizeText);
				StartWorking(Mode.SizeAdd);
			}
			this.subFontSizeText = GUILayout.TextField(this.subFontSizeText);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Increase all font sizes by:"))
			{
				this.addFontSize = System.Convert.ToInt32(this.addFontSizeText);
				StartWorking(Mode.SizeAdd);
			}
			this.addFontSizeText = GUILayout.TextField(this.addFontSizeText);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Set all font sizes to:"))
			{
				this.setFontSize = System.Convert.ToInt32(this.setFontSizeText);
				StartWorking(Mode.SizeSet);
			}
			this.setFontSizeText = GUILayout.TextField(this.setFontSizeText);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(20);

			Rect progressBarRect = EditorGUILayout.BeginVertical();
			EditorGUI.ProgressBar(progressBarRect, this.progress, this.statusText);
			GUILayout.Space(pixels: 20);
			EditorGUILayout.EndVertical();

			if (this.remapObjectsInProject)
			{
				GUILayout.Space(10);

				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.wordWrap = true;

				EditorGUILayout.BeginVertical();
				GUILayout.Label("TIP: Save the Scene (Ctrl-S) after use to write the changes to the asset files.", style);
				EditorGUILayout.EndVertical();
			}
		}

		void StartWorking(Mode mode)
		{
			this.mode = mode;
			this.phase = Phase.FindingGameObjects;
		}

		enum Phase
		{
			Inactive,
			FindingGameObjects,
			FindingComponents,
			FindingMembers,
			Remapping
		}

		void Update()
		{
			bool repaint = true;

			switch (this.phase)
			{
				case Phase.FindingGameObjects: FindingGameObjects_Update(); break;
				case Phase.FindingComponents: FindingComponents_Update(); break;
				case Phase.FindingMembers: FindingMembers_Update(); break;
				case Phase.Remapping: Remapping_Update(); break;
				case Phase.Inactive: repaint = false; break;
			}

			if (repaint)
			{
				Repaint();
			}
		}

		private Phase phase = Phase.Inactive;
		private IEnumerable<GameObject> gameObjects = null;
		private HashSet<Component> components = null;
		private List<Member> members = null;
		private int gameObjectIndex = 0;
		private int componentIndex = 0;
		private float progress = 0;

		private void FindingGameObjects_Update()
		{
			// Get all the game objects in the scene and/or project.
			this.gameObjects = new List<GameObject>();
			if (this.remapObjectsInScene)
			{
				this.gameObjects = this.gameObjects.Concat(GameObject.FindObjectsOfType<GameObject>());
			}
			if (this.remapObjectsInProject)
			{
				this.gameObjects = this.gameObjects.Concat(LoadAllGameObjects());
			}

			this.statusText = string.Format("{0} objects found.", this.gameObjects.Count());

			this.phase = Phase.FindingComponents;
			this.gameObjectIndex = 0;
			this.components = new HashSet<Component>();
		}

		private void FindingComponents_Update()
		{
			GameObject gameObject = this.gameObjects.ElementAt(this.gameObjectIndex);

			System.Type componentType =
				this.onlyRemapInTextComponents ?
				typeof(UnityEngine.UI.Text) :
				typeof(Component);

			var childComponents = (from component in gameObject.GetComponentsInChildren(componentType, includeInactive: true)
								   where component != null
								   select component);

			this.components.UnionWith(childComponents);

			this.gameObjectIndex++;
			CalculateProgress(this.gameObjectIndex, this.gameObjects.Count());

			this.statusText = string.Format("Finding components... ({0} found).", this.components.Count());

			if (this.gameObjectIndex >= this.gameObjects.Count())
			{
				this.phase = Phase.FindingMembers;
				this.componentIndex = 0;
				this.members = new List<Member>();
			}
		}

		private void CalculateProgress(int index, int count)
		{
			this.progress = ((float)index / count);
		}

		private void FindingMembers_Update()
		{
			// Find the members in the components.
			Component component = this.components.ElementAt(this.componentIndex);
			this.members.AddRange(FindMembers(component));

			this.componentIndex++;
			CalculateProgress(this.componentIndex, this.components.Count());

			this.statusText = string.Format("Finding members... ({0} found).", this.members.Count());

			if (this.componentIndex >= this.components.Count())
			{
				this.phase = Phase.Remapping;
			}
		}

		private void Remapping_Update()
		{
			// Set the new object into each member.
			switch (this.mode)
			{
				case Mode.FaceChange:
					{
						this.members.ForEach(member => member.SetValue(this.objectToRemapTo));
					}
					break;

				case Mode.SizeSet:
					{
						this.members.ForEach(member => member.SetValue(this.setFontSize));
					}
					break;

				case Mode.SizeAdd:
					{
						this.members.ForEach(member => member.SetValue(System.Convert.ToInt32(member.GetValue()) + this.addFontSize));
					}
					break;
			}

			this.statusText = string.Format("Completed (Changed {0} instances).", this.members.Count());

			this.phase = Phase.Inactive;
		}

		private class Member
		{
			public Member(Component component, PropertyInfo propertyInfo)
			{
				this.component = component;
				this.propertyInfo = propertyInfo;
			}

			public Member(Component component, FieldInfo fieldInfo)
			{
				this.component = component;
				this.fieldInfo = fieldInfo;
			}

			public void SetValue(int value)
			{
				SetValueInternal(value);
			}

			public void SetValue(Object value)
			{
				SetValueInternal(value);
			}

			private void SetValueInternal(object value)
			{
				// Record undo information.
				Undo.RecordObject(this.component, "Set font");

				if (this.propertyInfo != null)
				{
					this.propertyInfo.SetValue(this.component, value, null);
				}
				else if (this.fieldInfo != null)
				{
					this.fieldInfo.SetValue(this.component, value);
				}

				// Flag the game object as dirty.
				EditorUtility.SetDirty(this.component);
			}

			public object GetValue()
			{
				if (this.propertyInfo != null)
				{
					return this.propertyInfo.GetValue(this.component, null);
				}
				else if (this.fieldInfo != null)
				{
					return this.fieldInfo.GetValue(this.component);
				}
				else
				{
					return null;
				}
			}

			public Component component = null;
			public PropertyInfo propertyInfo = null;
			public FieldInfo fieldInfo = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		private IEnumerable<Member> FindMembers(Component component)
		{
			var members = component.GetType().GetMembers();
			var properties = (from member in members
							  where member.MemberType == MemberTypes.Property
							  select member as PropertyInfo);
			var fields = (from member in members
						  where member.MemberType == MemberTypes.Field
						  select member as FieldInfo);

			IEnumerable<Member> selectedProperties;
			IEnumerable<Member> selectedFields;
			if (this.ShouldChangeFontSize)
			{
				selectedProperties = (from property in properties
									  where property.Name == "fontSize"
									  select new Member(component, property));
				selectedFields = (from field in fields
								  where field.Name == "fontSize"
								  select new Member(component, field));
			}
			else
			{
				selectedProperties = (from property in properties
									  where property.PropertyType == this.remapObjectType
									  select new Member(component, property));
				selectedFields = (from field in fields
								  where field.FieldType == this.remapObjectType
								  select new Member(component, field));
			}
			return selectedProperties.Concat(selectedFields);
		}

		private IEnumerable<GameObject> LoadAllGameObjects()
		{
			return (from asset in LoadAllAssets()
					where asset is GameObject
					select asset as GameObject);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private Object[] LoadAllAssets()
		{
			string[] GUIDs = AssetDatabase.FindAssets("");
			Object[] objectList = new Object[GUIDs.Length];
			for (int index = 0; index < GUIDs.Length; index++)
			{
				string guid = GUIDs[index];
				string assetPath = AssetDatabase.GUIDToAssetPath(guid);
				Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object)) as Object;
				objectList[index] = asset;
			}

			return objectList;
		}
	}
}
