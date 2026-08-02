using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using KesselSabacc.Utils;
using System.Reflection;
using System.Runtime.InteropServices;
using PlasticGui.WorkspaceWindow.QueryViews.Labels;

namespace KesselSabacc.Editor
{
	[CustomPropertyDrawer( typeof( SubclassSelectorAttribute ) )]
	public class SubclassSelectorPropertyDrawer : PropertyDrawer
	{
		static readonly Dictionary<Type, SubclassLabelTypePairs> _typeCache = new();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if ( property.propertyType != SerializedPropertyType.ManagedReference )
			{
				EditorGUI.HelpBox( position, "[SubclassSelector] requires [SerializedReference]", MessageType.Error );
				return;
			}

			var subclassAttr = (SubclassSelectorAttribute)attribute;
			var baseType = subclassAttr.BaseType ?? GetFieldBaseType();
			var subTypePairs = GetSubClasses( baseType, subclassAttr.ShowNull );
			var labels = subTypePairs.Labels;
			var types = subTypePairs.Types;

			var currentTypeName = property.managedReferenceFullTypename;
			int currentIndex = Array.FindIndex( types, t => t != null && MatchesManagedTypeName( t, currentTypeName ) );
			if ( currentIndex < 0 )
			{
				currentIndex = subclassAttr.ShowNull ? 0 : -1;
			}

			var lineRect = new Rect( position.x, position.y, position.width, EditorGUIUtility.singleLineHeight );
			int newIndex = EditorGUI.Popup( lineRect, label, currentIndex, labels.Select( l => new GUIContent( l ) ).ToArray() );

			if ( newIndex != currentIndex )
			{
				Undo.RecordObject( property.serializedObject.targetObject, "Change Subclass" );
				property.managedReferenceValue = newIndex >= 0 && types[newIndex] != null
					? Activator.CreateInstance( types[newIndex] )
					: null;
				property.serializedObject.ApplyModifiedProperties();
			}

			if ( property.managedReferenceValue != null )
			{
				EditorGUI.indentLevel++;
				var childRect = new Rect( position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, position.height );
				DrawChildren( childRect, property );
				EditorGUI.indentLevel--;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float h = EditorGUIUtility.singleLineHeight;
			if ( property.propertyType == SerializedPropertyType.ManagedReference
				&& property.managedReferenceValue != null )
			{
				h += 2;
				foreach ( var child in GetDirectChildren( property ) )
				{
					h += EditorGUI.GetPropertyHeight( child, true ) + EditorGUIUtility.standardVerticalSpacing;
				}
			}

			return h;
		}

		private static void DrawChildren(Rect startRect, SerializedProperty property)
		{
			float y = startRect.y;
			foreach ( var child in GetDirectChildren( property ) )
			{
				float childHeight = EditorGUI.GetPropertyHeight( child, true );
				EditorGUI.PropertyField( new Rect( startRect.x, y, startRect.width, childHeight ), child, true );
				y += childHeight + EditorGUIUtility.standardVerticalSpacing;
			}
		}

		private static IEnumerable<SerializedProperty> GetDirectChildren(SerializedProperty parent)
		{
			var focus = parent.Copy();
			var endProperty = focus.GetEndProperty();

			if ( !focus.NextVisible( true ) ) yield break;

			while ( !SerializedProperty.EqualContents( focus, endProperty ) )
			{
				yield return focus.Copy();
				if ( !focus.NextVisible( false ) )
				{
					break;
				}
			}
		}

		private Type GetFieldBaseType()
		{
			Type fieldType = fieldInfo.FieldType;
			if ( fieldType.IsArray ) return fieldType.GetElementType();
			if ( fieldType.IsGenericType ) return fieldType.GetGenericArguments()[0];
			return fieldType;
		}

		private static SubclassLabelTypePairs GetSubClasses(Type baseType, bool includeNull)
		{
			if ( _typeCache.TryGetValue( baseType, out var cached ) )
			{
				Type[] found = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany( (assembly) =>
					{
						try
						{
							return assembly.GetTypes();
						}
						catch
						{
							return Array.Empty<Type>();
						}
					} )
					.Where( (t) =>
					{
						return !t.IsAbstract && !t.IsInterface && baseType.IsAssignableFrom( t );
					} )
					.OrderBy( (t) => t.Name )
					.ToArray();

				string[] labels = found.Select( t => t.Name ).ToArray();

				_typeCache[baseType] = cached = new SubclassLabelTypePairs( labels, found );
			}

			if ( includeNull )
				return new SubclassLabelTypePairs(
					cached.Labels.Prepend( "(None)" ).ToArray(),
					cached.Types.Prepend<Type>( null ).ToArray()
				);

			return cached;
		}

		private static bool MatchesManagedTypeName(Type t, string managedTypeName)
		{
			if ( string.IsNullOrEmpty( managedTypeName ) )
				return false;

			string[] nameParts = managedTypeName.Split( ' ' );
			return nameParts.Length == 2 && nameParts[1] == t.FullName;
		}

		private class SubclassLabelTypePairs
		{
			public readonly string[] Labels;
			public readonly Type[] Types;

			public SubclassLabelTypePairs(string[] labels, Type[] types)
			{
				Labels = labels;
				Types = types;
			}
		}
	}
}
