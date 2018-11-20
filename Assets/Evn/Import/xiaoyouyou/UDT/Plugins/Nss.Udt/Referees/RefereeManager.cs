using System.Collections.Generic;
using UnityEngine;

namespace Nss.Udt.Referee {
	public class RefereeManagerBase : MonoBehaviour {
	}
	
	/// <summary>
	/// Referee manager.
	/// 
	/// Generic class that should be inherited into an organic manager for
	/// the components/gameobjects you want to maintain references too
	/// without costly Find calls through the Unity API.
	/// </summary>
	/// <description>
	/// The derived class will have two requirements:
	/// 1] Must be added to a gameobject in the scene
	/// 2] Must be added to Script Execution Order
	///    before DEFAULT TIME, specifically before
	///    the generic <T> script.
	/// 
	/// The target component that will be cached has
	/// two requirements:
	/// 1] OnEnable must call the referee's Add method
	/// 2] OnDisable must call the referee's Remove method
	/// </description>
	public class RefereeManager<T> : RefereeManagerBase where T : MonoBehaviour {
		
		public static RefereeManager<T> Instance;	// Singleton object
		public List<T> References = new List<T>();	// Reference cache
		
		#region Unity3D Events
		/// <summary>
		/// Unity3D OnEnable event
		/// 
		/// Ensure the singleton is instantiated and accessible.
		/// </summary>
		public void OnEnable() {
			if(Instance == null) {
				Instance = this;
			}
		}
		#endregion
		
		#region Core Methods
		/// <summary>
		/// Add the specified reference.
		/// </summary>
		/// <param name='reference'>
		/// Reference.
		/// </param>
		public void Add(T reference) {
			References.Add(reference);
		}

		/// <summary>
		/// Remove the specified reference.
		/// </summary>
		/// <param name='reference'>
		/// Reference.
		/// </param>
		public void Remove(T reference) {
			References.Remove(reference);
		}
		#endregion
		
		#region Find Method Helpers
		/// <summary>
		/// Finds reference the by tag.
		/// </summary>
		/// <returns>
		/// The reference by tag.
		/// </returns>
		/// <param name='tag'>
		/// Tag.
		/// </param>
		/// <param name='ignoreCase'>
		/// Ignore case.
		/// </param>
		public T FindByTag(string tag, bool ignoreCase = false) {
			if(ignoreCase)
				return References.Find(t => t.tag.Equals(tag, System.StringComparison.OrdinalIgnoreCase)) as T;
			
			return References.Find(t => t.tag.Equals(tag)) as T;
		}

		/// <summary>
		/// Finds all references by tag.
		/// </summary>
		/// <returns>
		/// All references by tag.
		/// </returns>
		/// <param name='tag'>
		/// Tag.
		/// </param>
		/// <param name='ignoreCase'>
		/// Ignore case.
		/// </param>
		public List<T> FindAllByTag(string tag, bool ignoreCase = false) {
			if(ignoreCase)
				return References.FindAll(t => t.tag.Equals(tag, System.StringComparison.OrdinalIgnoreCase)) as List<T>;
			
			return References.FindAll(t => t.tag.Equals(tag)) as List<T>;
		}

		/// <summary>
		/// Finds the reference by name.
		/// </summary>
		/// <returns>
		/// The reference by name.
		/// </returns>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='ignoreCase'>
		/// Ignore case.
		/// </param>
		public T FindByName(string name, bool ignoreCase = false) {
			if(ignoreCase)
				return References.Find(t => t.name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
			
			return References.Find(t => t.name.Equals(name));
		}

		/// <summary>
		/// Finds all references by name.
		/// </summary>
		/// <returns>
		/// All references by name.
		/// </returns>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='ignoreCase'>
		/// Ignore case.
		/// </param>
		public List<T> FindAllByName(string name, bool ignoreCase = false) {
			if(ignoreCase)
				return References.FindAll(t => t.name.Equals(name, System.StringComparison.OrdinalIgnoreCase)) as List<T>;
			
			return References.FindAll(t => t.name.Equals(name)) as List<T>;
		}
		#endregion
	}
}