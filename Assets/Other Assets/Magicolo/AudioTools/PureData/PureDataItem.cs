using UnityEngine;
using System.Collections;

/// <summary>
/// Gives control over a sound or container.
/// </summary>
[System.Serializable]
public abstract class PureDataItem {

	public abstract string Name {
		get;
	}
	
	public abstract PureDataStates State {
		get;
	}
	
	public abstract bool IsContainer {
		get;
	}
	
	protected PureData pureData;
	
	protected PureDataItem(PureData pureData) {
		this.pureData = pureData;
	}
	
	/// <summary>
	/// Resumes an item that has been paused.
	/// </summary>
	/// <param name = "delay">The delay in seconds before playing the item.</param>
	public abstract void Play(float delay);
	
	/// <summary>
	/// Resumes an item that has been paused.
	/// </summary>
	public virtual void Play() {
		Play(0);
	}
	
	/// <summary>
	/// Pauses an item.
	/// </summary>
	/// <param name = "delay">The delay in seconds before pausing the item.</param>	
	public abstract void Pause(float delay);
	
	/// <summary>
	/// Pauses an item.
	/// </summary>
	public virtual void Pause() {
		Pause(0);
	}
	
	/// <summary>
	/// Stops an item with fade out.
	/// </summary>
	/// <param name = "delay">The delay in seconds before stopping the item.</param>
	/// <remarks>After an item has been stopped, it is obsolete.</remarks>
	public abstract void Stop(float delay);
	
	/// <summary>
	/// Stops an item with fade out.
	/// </summary>
	/// <remarks>After an item has been stopped, it is obsolete.</remarks>
	public virtual void Stop() {
		Stop(0);
	}
	
	/// <summary>
	/// Stops an item immediatly. Stopping an item this way might generate clicks.
	/// </summary>
	/// <remarks>After an item has been stopped, it is obsolete.</remarks>
	public abstract void StopImmediate();
	
	/// <summary>
	/// Ramps the volume of an item.
	/// </summary>
	/// <param name="targetVolume">The target to which the volume will be ramped.</param>
	/// <param name="time">The time in seconds it will take for the volume to reach the <paramref name="targetVolume"/></param>
	/// <param name="delay">The delay in seconds before the volume will be ramped.</param>
	public abstract void SetVolume(float targetVolume, float time, float delay);
	
	/// <summary>
	/// Ramps the volume of an item.
	/// </summary>
	/// <param name="targetVolume">The target to which the volume will be ramped.</param>
	/// <param name="time">The time in seconds it will take for the volume to reach the <paramref name="targetVolume"/></param>
	public virtual void SetVolume(float targetVolume, float time) {
		SetVolume(targetVolume, time, 0);
	}
	
	/// <summary>
	/// Sets the volume of an item.
	/// </summary>
	/// <param name="targetVolume">The target to which the volume will be set.</param>
	public virtual void SetVolume(float targetVolume) {
		SetVolume(targetVolume, 0, 0);
	}
	
	/// <summary>
	/// Ramps the pitch of an item.
	/// </summary>
	/// <param name="targetPitch">The target to which the pitch will be ramped.</param>
	/// <param name="time">The time in seconds it will take for the pitch to reach the <paramref name="targetPitch"/></param>
	/// <param name="delay">The delay in seconds before the pitch will be ramped.</param>
	public abstract void SetPitch(float targetPitch, float time, float delay);
	
	/// <summary>
	/// Ramps the pitch of an item.
	/// </summary>
	/// <param name="targetPitch">The target to which the pitch will be ramped.</param>
	/// <param name="time">The time in seconds it will take for the pitch to reach the <paramref name="targetPitch"/></param>
	public virtual void SetPitch(float targetPitch, float time) {
		SetPitch(targetPitch, time, 0);
	}
	
	/// <summary>
	/// Sets the pitch of an item.
	/// </summary>
	/// <param name="targetPitch">The target to which the pitch will be set.</param>
	public virtual void SetPitch(float targetPitch) {
		SetPitch(targetPitch, 0, 0);
	}
	
	/// <summary>
	/// Loads the sounds attached to the item in memory.
	/// </summary>
	public abstract void Load();
	
	/// <summary>
	/// Unloads the sounds attached to the item in memory.
	/// </summary>
	/// <remarks>Unloading a sound that is playing will stop it and might generate clicks.</remarks>
	public abstract void Unload();
	
	/// <summary>
	/// Retreives an item within a container.
	/// </summary>
	/// <param name="index">The index at which the item will be retreived.</param>
	/// <param name="ignoreContainers">Wether or not to ignore containers.</param>
	/// <returns>The PureDataItem.</returns>
	/// <remarks>If this item is not a container, it will be returned.</remarks>
	public abstract PureDataItem GetItem(int index, bool ignoreContainers);
		
	/// <summary>
	/// Retreives an item within a container.
	/// </summary>
	/// <param name="index">The index at which the item will be retreived.</param>
	/// <returns>The PureDataItem.</returns>
	/// <remarks>If this item is not a container, it will be returned.</remarks>
	public virtual PureDataItem GetItem(int index) {
		return GetItem(index, false);
	}
	
	/// <summary>
	/// Retreives an item within a container.
	/// </summary>
	/// <param name="name">The name of the item to retreive.</param>
	/// <param name="ignoreContainers">Wether or not to ignore containers.</param>
	/// <returns>The PureDataItem.</returns>
	/// <remarks>If this item is not a container, it will be returned.</remarks>
	public abstract PureDataItem GetItem(string name, bool ignoreContainers);
		
	/// <summary>
	/// Retreives an item within a container.
	/// </summary>
	/// <param name="name">The name of the item to retreive.</param>
	/// <returns>The PureDataItem.</returns>
	/// <remarks>If this item is not a container, it will be returned.</remarks>
	public virtual PureDataItem GetItem(string name) {
		return GetItem(name, false);
	}
	 
	/// <summary>
	/// Retreives all items within a container.
	/// </summary>
	/// <param name="ignoreContainers">Wether or not to include containers.</param>
	/// <returns>The PureDataItem array.</returns>
	/// <remarks>If this item is not a container, a single element array with it inside will be returned.</remarks>
	public abstract PureDataItem[] GetItems(bool ignoreContainers);
		
	/// <summary>
	/// Retreives all items within a container.
	/// </summary>
	/// <returns>The PureDataItem array.</returns>
	/// <remarks>If this item is not a container, a single element array with it inside will be returned.</remarks>
	public virtual PureDataItem[] GetItems() {
		return GetItems(false);
	}
	
	/// <summary>
	/// Gives access to all the current settings of the item.
	/// </summary>
	/// <returns>A container for all the settings.</returns>
	/// <remarks>If this item is a container, <c>null</c> will be returned.</remarks>
	public abstract PureDataItemInfo GetInfo();
	
	/// <summary>
	/// Overrides previously set options of an item.
	/// </summary>
	/// <param name = "options">The overriding options.</param>
	/// <remarks>Some options can only be applied when an item is initialized.</remarks>
	public abstract void ApplyOptions(params PureDataOption[] options);
	
	/// <summary>
	/// Overrides a previously set option of an item.
	/// </summary>
	/// <param name = "option">The overriding option.</param>
	/// <param name = "delay">The delay in seconds before applying the option.</param>
	/// <remarks>Some options can only be applied when an item is initialized.</remarks>
	public abstract void ApplyOption(PureDataOption option, float delay);
	
	/// <summary>
	/// Overrides a previously set option of an item.
	/// </summary>
	/// <param name = "option">The overriding option.</param>
	/// <remarks>Some options can only be applied when an item is initialized.</remarks>
	public virtual void ApplyOption(PureDataOption option) {
		ApplyOption(option, 0);
	}
}
