using System;
using UnityEngine;

namespace KesselSabacc.Utils
{
	/// <summary>
	/// USe with [SerializeReference] attribute to display a subclass picker in the Inspector.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public class SubclassSelectorAttribute : PropertyAttribute
	{
		/// <summary>
		/// Overrides the base type clan in the property drawer.
		/// Defaults to the field's declared type.
		/// </summary>
		public Type BaseType { get; }
		/// <summary>
		/// When true, the property drawer allows designers to not set a type.
		/// </summary>
		public bool ShowNull { get; }

		public SubclassSelectorAttribute(bool showNull = true, Type baseType = null)
		{
			ShowNull = showNull;
			BaseType = baseType;
		}
	}
}
