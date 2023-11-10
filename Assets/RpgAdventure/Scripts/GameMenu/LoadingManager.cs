using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RpgAdventure.Scripts.GameMenu
{
    public class LoadingManager : MonoBehaviour
    {
        public Slider loadingScreenSlider;

        public void SetLoadingFullTime(float maxLoading)
        {
            loadingScreenSlider.maxValue = maxLoading;
        }
        public void SetLoadTime(float counter)
        {
            loadingScreenSlider.value = counter / loadingScreenSlider.maxValue;
        }
    }
}

