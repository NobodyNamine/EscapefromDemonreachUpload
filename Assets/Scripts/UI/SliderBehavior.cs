using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehavior : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

	// Reference: https://docs.unity3d.com/540/Documentation/ScriptReference/UI.Slider-onValueChanged.html
	public void Start()
	{
		//Adds a listener to the main slider and invokes a method when the value changes.
		masterSlider.onValueChanged.AddListener(delegate { MasterSlider(); });
		musicSlider.onValueChanged.AddListener(delegate { MusicSlider(); });
		sfxSlider.onValueChanged.AddListener(delegate { SFXSlider(); });
	}

	// Invoked when the value of the slider changes.
	public void MasterSlider()
	{
		FMOD.Studio.Bus masterBus;

		masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
		masterBus.setVolume(masterSlider.value);
	}

	public void MusicSlider()
	{
		FMOD.Studio.Bus musicBus;

		musicBus = FMODUnity.RuntimeManager.GetBus("bus:/MusicBus");
		musicBus.setVolume(musicSlider.value);
	}

	public void SFXSlider()
	{
		FMOD.Studio.Bus sfxBus;

		sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFXBus");
		sfxBus.setVolume(sfxSlider.value);
	}

}
