
/** DinolensMessageLog.cs
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
using TMPro;

#endregion

namespace Cuberoot.Dinocular
{
	#region DinocularMessageLog

	/// <summary>
	/// __TODO_ANNOTATE__
	///</summary>

	public sealed class DinocularMessageLog : MonoBehaviour
	{
		#region Fields

		public UnityEngine.UI.ScrollRect ScrollRect;
		public GameObject EntryPrefab;
		public int MaxEntries;

		#endregion
		#region Members

		private const string DEFAULT_PREFIX = "|";

		private RectTransform _Rect;
		private LinkedList<GameObject> _Entries = new LinkedList<GameObject>();

		#endregion
		#region Properties

		#endregion
		#region Methods

		#region OnValidate

		private void OnValidate()
		{
			_Rect = GetComponent<RectTransform>();
		}

		#endregion

		#region Print

		public void Print(in string message, in Color color, in string prefix = DEFAULT_PREFIX)
		{
			GameObject entry = Instantiate(EntryPrefab, transform);

			_Entries.AddLast(entry);

			var text = entry.GetComponentInChildren<TextMeshProUGUI>();

			text.text = prefix + " " + message;
			text.color = color;

			float height = text.preferredHeight;

			_Rect.sizeDelta = new Vector2(_Rect.sizeDelta.x, _Rect.sizeDelta.y + height);

			ScrollRect.verticalNormalizedPosition = ScrollRect.preferredHeight;
		}
		public void Print(in string message, in MessageType type)
		{
			Color color; string prefix;
			switch (type)
			{
				case MessageType.Info:
					color = Color.white;
					prefix = "|";
					break;
				case MessageType.Response:
					color = Color.cyan;
					prefix = ">";
					break;
				case MessageType.Warning:
					color = Color.yellow;
					prefix = "~";
					break;
				case MessageType.Error:
					color = Color.red;
					prefix = "*";
					break;
				default:
					color = Color.white;
					prefix = " ";
					break;
			}

			Print(message, color, prefix);
		}
		public void Print(in string message) =>
			Print(message, MessageType.Info);

		#endregion
		#region Clear
		public void Clear()
		{
			foreach (var i in transform.GetChildren())
				Destroy(i.gameObject);

			_Rect.sizeDelta = new Vector2(_Rect.sizeDelta.x, 0f);
		}
		public void Clear(int quantity)
		{

		}
		#endregion
		#endregion
	}
	#endregion

	#region (enum) MessageType

	public enum MessageType
	{
		Info,
		Response,
		Warning,
		Error
	}

	#endregion
}
