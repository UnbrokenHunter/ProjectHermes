using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    [ExecuteInEditMode]
    public class SignScript : MonoBehaviour
    {

        #region Variables

        [PreviewField(300)]
        [LabelWidth(100)]
        [AssetSelector(Paths = "Assets/Sprites/Game Sprites/Tilemaps/Sunnyland/Signs")]
        public Sprite Sign;

    	#endregion


    	#region Methods

		private void Update()
		{
            GetComponent<SpriteRenderer>().sprite = Sign;
		}

        #endregion
    }
}
