
/** DinocularPanel.cs
*
*	Created by LIAM WOFFORD of CUBEROOT SOFTWARE, LLC.
*
*	Free to use or modify, with or without creditation,
*	under the Creative Commons 0 License.
*/

#region Includes

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using InputContext = UnityEngine.InputSystem.InputAction.CallbackContext;

#endregion

namespace Cuberoot.Dinocular
{
	/// <summary>
	/// __TODO_ANNOTATE__
	///</summary>

	public sealed class DinocularPanel : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private GameObject Wrapper;

		#endregion
		#region Members

		public UnityEvent OnOpen;

		#endregion
		#region Properties



		#endregion
		#region Methods

		public void ToggleOpen()
		{
			Wrapper.SetActive(!Wrapper.activeSelf);
			OnOpen.Invoke();
		}

		public void OnInputToggleOpen(InputContext context)
		{
			if (context.started)
				ToggleOpen();
		}

		#endregion
	}
}
