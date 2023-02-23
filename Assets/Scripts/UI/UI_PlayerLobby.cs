using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{

    public class PlayerMenuController : MonoBehaviour
    {
        public Dropdown colorDropdown;
        public Dropdown skinDropdown;
        public Image previewImage;

        private int colorIndex = 0;
        private int skinIndex = 0;

        void Update()
        {
            // Check for input from player's up and down arrow keys
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                colorIndex = (colorIndex + 1) % colorDropdown.options.Count;
                colorDropdown.value = colorIndex;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                colorIndex--;
                if (colorIndex < 0)
                {
                    colorIndex = colorDropdown.options.Count - 1;
                }
                colorDropdown.value = colorIndex;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                skinIndex = (skinIndex + 1) % skinDropdown.options.Count;
                skinDropdown.value = skinIndex;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                skinIndex--;
                if (skinIndex < 0)
                {
                    skinIndex = skinDropdown.options.Count - 1;
                }
                skinDropdown.value = skinIndex;
            }

            // Update the preview image
            previewImage.sprite = skinDropdown.options[skinIndex].image;
        }

        public void SaveSelections()
        {
            // Save the player's color and skin selections
            int colorSelection = colorDropdown.value;
            int skinSelection = skinDropdown.value;

            // Do something with the selections
            Debug.Log("Player color selection: " + colorSelection);
            Debug.Log("Player skin selection: " + skinSelection);
        }
    }

}
