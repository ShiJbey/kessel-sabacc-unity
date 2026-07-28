using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KesselSabacc.UI.Components
{
	/// <summary>
	/// The ChipCounter displays the players's maximum number of owned chips
	/// compared to their current number of available chips. Unavailable chips
	/// are faded to indicate they have been lost and are not available for
	/// draw actions.
	/// </summary>
	public class RemainingChipCounter : UIComponent
	{
		/// <summary>
		/// The image used
		/// </summary>
		[SerializeField]
		private GameObject _chipPrefab;
		[SerializeField]
		private RectTransform _chipsContainer;
		[SerializeField]
		private TMP_Text _valueLabel;


		private int _currentChipCount = 0;
		private int _maxChipCount = 0;

		/// <summary>
		/// Instantiated chips displayed in GUI.
		/// </summary>
		private List<GameObject> _chips = new();

		public void Start()
		{
			_chipPrefab.gameObject.SetActive(false);
		}

		public void SetMaxChipCount(int value)
		{
			_maxChipCount = value;
			UpdateChips();
		}

		public void SetCurrentChipCount(int value)
		{
			_currentChipCount = value;
			_maxChipCount = Math.Max(_maxChipCount, value);
			_valueLabel.SetText(value.ToString());
			UpdateChips();
		}

		private void UpdateChips()
		{
			// Destroy all the instantiated chips
			foreach (var chip in _chips)
			{
				Destroy(chip);
			}
			_chips.Clear();

			// Instantiate the max between the current and starting chips
			// Change the color of the first n ships, where n is
			// max_chips - current_chips
			int usedChipCount = _maxChipCount - _currentChipCount;

			for (int i = 0; i < _maxChipCount; i++)
			{
				GameObject obj = Instantiate( _chipPrefab, _chipsContainer );
				obj.SetActive(true);
				if (i < usedChipCount)
				{
					obj.GetComponent<MaskedChip>().ShowOverlay();
				}
				_chips.Add(obj);
			}
		}
	}
}
