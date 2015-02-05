﻿using UnityEngine;
using System.Collections;
using Magicolo;
using Magicolo.AudioTools;
using System.IO;

// TODO Mute and Solo options on buses
// TODO Gain slider to expand the soundwave on PureDataSetups
// TODO FadeIn and FadeOut settings as sliders under the soundwave
// TODO Obstacle filtering and HRTF (head related transfer function)
// TODO Find a way to retreive the actual settings for settings that can be delayed or ramped (volume, pitch, etc.)
// TODO uaudiosource~
// TODO Sequence containers
// TODO Blend containers
// TODO Give access to spatializer settings
// TODO Find a cleaner way to organize Pure Data patches
// TODO Optimize memory allocation when loading a clip somehow... (pointers?)
// TODO Complete documentation (XML, Tooltips, Help patches)
// TODO Divide sound into multiple parts (play by calling soundName + suffix)
// FIXME Remove the clicks when using PureDataOption.PlayRange or the PureDataOption.Time
// FIXME Solve the "no matching catch" error in Pure Data
// FIXME Unexpected crashes/freezes when Unity compiles (doesn't seem to occur when the scene is played frequently)
// FIXME Error when opening a patch on Android
// FIXME Import setting Force To Mono doesn't work as expected (sound plays only on left speaker)
// TODO Support more than 2 channels
// FIXME Occasionnal glitches when using a Transform as a source asynchronously because spatialization cannot be initialized

// NOTES
// Imported sounds must have the same sample rate than Unity.
// Imported sounds must be Decompressed On Load or Load In Memory as their Load Type.
// Sounds that are to be played asynchronously must be already loaded by using PureData.Load or by using the LoadOnAwake option or by having them played synchronously before.
// PureData.Load and PureData.OpenPatch must be called from the main thread.
// PureData is not compatible with Web applications (Android support incoming).
// The [umixer~] object will throw 'No matching catch' errors when the DSP is on.
// Unexpected crashes/freezes when Unity compiles (doesn't seem to occur when the scene is played frequently).


/// <summary>
/// Gives control over the Pure Data audio engine.
/// </summary>
[ExecuteInEditMode]
public class PureData : MonoBehaviourExtended {
	
	static PureData instance;
	static PureData Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<PureData>();
			}
			return instance;
		}
	}
	
	#region Components
	public PureDataListener listener;
	public PureDataFilterRead filterRead;
	public PureDataReferences references;
	public PureDataGeneralSettings generalSettings;
	public PureDataBridge bridge;
	public PureDataCommunicator communicator;
	public PureDataPatchManager patchManager;
	public PureDataIdManager idManager;
	public PureDataItemManager itemManager;
	public PureDataSourceManager sourceManager;
	public PureDataInfoManager infoManager;
	public PureDataClipManager clipManager;
	public PureDataBusManager busManager;
	public PureDataSpatializerManager spatializerManager;
	public PureDataContainerManager containerManager;
	public PureDataHierarchyManager hierarchyManager;
	public PureDataEditorHelper editorHelper;
	#endregion
	
	#region Initialize
	public void Initialize() {
		if (SingletonCheck()) {
			return;
		}
		
		InitializeSettings();
		InitializeRuntime();
		InitializeManagers();
		InitializeHelpers();
	}
	
	public void InitializeSettings() {
		bool saveAssets = false;
		string path = HelperFunctions.GetAssetPath("PureDataSettings.asset");
		path = string.IsNullOrEmpty(path) ? "Assets/PureDataSettings.asset" : path;
		
		if (references == null) {
			references = new PureDataReferences(Instance);
		}
		references.Initialize(Instance);
		
		if (generalSettings == null) {
			generalSettings = PureDataGeneralSettings.Create(path);
			saveAssets = true;
		}
		generalSettings.Initialize(Instance);
		
		if (busManager == null) {
			busManager = PureDataBusManager.Create(path);
			saveAssets = true;
		}
		busManager.Initialize(Instance);
		
		if (spatializerManager == null) {
			spatializerManager = PureDataSpatializerManager.Create(path);
			saveAssets = true;
		}
		spatializerManager.Initialize(Instance);
		
		if (containerManager == null) {
			containerManager = PureDataContainerManager.Create(path);
			saveAssets = true;
		}
		containerManager.Initialize(Instance);
		
		if (saveAssets) {
			HelperFunctions.SaveAssets();
		}
	}

	public void InitializeRuntime() {
		if (Application.isPlaying) {
			DontDestroyOnLoad(this);
			
			listener = new PureDataListener(Instance);
			bridge = new PureDataBridge(Instance);
			communicator = new PureDataCommunicator(Instance);
			patchManager = new PureDataPatchManager(Instance);
			idManager = new PureDataIdManager(Instance);
			itemManager = new PureDataItemManager(Instance);
			
			bridge.Start();
			communicator.Start();
			patchManager.Start();
		}
	}
	
	public void InitializeManagers() {
		sourceManager = sourceManager ?? new PureDataSourceManager(Instance); // Must be first for appropriate connection with uaudiosources.
		sourceManager.Initialize(Instance);
		infoManager = infoManager ?? new PureDataInfoManager(Instance);
		infoManager.Initialize(Instance);
		clipManager = clipManager ?? new PureDataClipManager(Instance);
		clipManager.Initialize(Instance);
		hierarchyManager = hierarchyManager ?? new PureDataHierarchyManager(Instance);
		hierarchyManager.Initialize(Instance);
	}
		
	public void InitializeHelpers() {
		if (editorHelper == null) {
			editorHelper = new PureDataEditorHelper(Instance);
		}
		editorHelper.Initialize(Instance);
	}
	#endregion
	
	void Awake() {
		Initialize();
	}
		
	void Start() {
		SingletonCheck();
	}

	void Update() {
		if (Application.isPlaying) {
			listener.Update();
			communicator.Update();
			sourceManager.Update();
			spatializerManager.Update();
		}
		else {
			#if UNITY_WEBPLAYER
			Logger.LogWarning("Pure Data is incompatible with Unity's Web Player.");
			#endif
		}
	}
	
	void FixedUpdate() {
		if (Application.isPlaying) {
			communicator.Update();
		}
	}
	
	void LateUpdate() {
		if (Application.isPlaying) {
			communicator.Update();
		}
	}
	
	void OnLevelWasLoaded(int level) {
		if (SingletonCheck()) {
			return;
		}
		listener.Initialize(Instance);
	}
		
	void OnApplicationQuit() {
		if (Application.isPlaying) {
			sourceManager.StopAllImmediate();
			patchManager.Stop();
			bridge.Stop();
			communicator.Stop();
		}
	}
	
	void OnDrawGizmos() {
		spatializerManager.OnDrawGizmos();
	}

	bool SingletonCheck() {
		if (Instance != null && Instance != this) {
			if (Application.isPlaying) {
				PureDataSwitcher.Switch(Instance, this);
			}
			else {
				Logger.LogError(string.Format("There can only be one {0}.", GetType().Name));
			}
			
			gameObject.Remove();
			return true;
		}
		return false;
	}
	
	#if UNITY_EDITOR
	[UnityEditor.Callbacks.DidReloadScripts]
	static void OnReloadScripts() {
		if (Instance != null) {
			Instance.InitializeHelpers();
		}
	}
	
	[UnityEditor.Callbacks.PostProcessBuild]
	static void OnPostProcessBuild(UnityEditor.BuildTarget buildTarget, string buildPath) {
		if (!Application.HasProLicense()) {
			string libpdcsharpSourcePath = Application.dataPath + HelperFunctions.GetAssetPath("Plugins/libpdcsharp.dll").Substring("Assets".Length);
			string libpdcsharpTargetPath = Path.GetDirectoryName(buildPath) + Path.DirectorySeparatorChar + "libpdcsharp.dll";
			string pthreadGC2SourcePath = Application.dataPath + HelperFunctions.GetAssetPath("Plugins/pthreadGC2.dll").Substring("Assets".Length);
			string pthreadGC2TargetPath = Path.GetDirectoryName(buildPath) + Path.DirectorySeparatorChar + "pthreadGC2.dll";
		
			if (!File.Exists(libpdcsharpTargetPath)) {
				File.Copy(libpdcsharpSourcePath, libpdcsharpTargetPath);
			}
		
			if (!File.Exists(pthreadGC2TargetPath)) {
				File.Copy(pthreadGC2SourcePath, pthreadGC2TargetPath);
			}
		}
	}
	#endif
		
	#region Play
	/// <summary>
	/// Plays a sound spatialized around the listener.
	/// </summary>
	/// <param name="soundName">The name of the sound to be played.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>The PureDataItem that will let you control the sound.</returns>
	public static PureDataItem Play(string soundName, params PureDataOption[] options) {
		return instance.itemManager.Play(soundName, instance.listener, 0, options);
	}
	
	/// <summary>
	/// Plays a sound spatialized around the listener.
	/// </summary>
	/// <param name="soundName">The name of the sound to be played.</param>
	/// <param name = "delay">The delay in seconds before playing the sound.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>The PureDataItem that will let you control the sound.</returns>
	public static PureDataItem Play(string soundName, float delay, params PureDataOption[] options) {
		return instance.itemManager.Play(soundName, instance.listener, delay, options);
	}

	/// <summary>
	/// Plays a sound spatialized around the <paramref name="source"/>.
	/// </summary>
	/// <param name="soundName">The name of the sound to be played.</param>
	/// <param name="source">The source around which the sound will be spatialized.</param>
	/// <param name = "delay">The delay in seconds before playing the sound.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>The PureDataItem that will let you control the sound.</returns>
	/// <remarks>Using a Transform asynchronously as a source might introduce glitches.</remarks>
	public static PureDataItem Play(string soundName, Transform source, float delay, params PureDataOption[] options) {
		return instance.itemManager.Play(soundName, source, delay, options);
	}
		
	/// <summary>
	/// Plays a sound spatialized around the <paramref name="source"/>.
	/// </summary>
	/// <param name="soundName">The name of the sound to be played.</param>
	/// <param name="source">The source around which the sound will be spatialized.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>The PureDataItem that will let you control the sound.</returns>	
	/// <remarks>Using a Transform asynchronously as a source might introduce glitches.</remarks>
	public static PureDataItem Play(string soundName, Transform source, params PureDataOption[] options) {
		return instance.itemManager.Play(soundName, source, 0, options);
	}
		
	/// <summary>
	/// Plays a sound spatialized around the <paramref name="source"/>.
	/// </summary>
	/// <param name="soundName">The name of the sound to be played.</param>
	/// <param name="source">The source around which the sound will be spatialized.</param>
	/// <param name = "delay">The delay in seconds before playing the sound.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>The PureDataItem that will let you control the sound.</returns>	
	public static PureDataItem Play(string soundName, Vector3 source, float delay, params PureDataOption[] options) {
		return instance.itemManager.Play(soundName, source, delay, options);
	}
		
	/// <summary>
	/// Plays a sound spatialized around the <paramref name="source"/>.
	/// </summary>
	/// <param name="soundName">The name of the sound to be played.</param>
	/// <param name="source">The source around which the sound will be spatialized.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>The PureDataItem that will let you control the sound.</returns>	
	public static PureDataItem Play(string soundName, Vector3 source, params PureDataOption[] options) {
		return instance.itemManager.Play(soundName, source, 0, options);
	}
		
	/// <summary>
	/// Plays a container spatialized around the listener.
	/// </summary>
	/// <param name="containerName">The name of the container to be played.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>An item that will let you control the container.</returns>	
	public static PureDataItem PlayContainer(string containerName, params PureDataOption[] options) {
		return instance.itemManager.PlayContainer(containerName, instance.listener, 0, options);
	}

	/// <summary>
	/// Plays a container spatialized around the listener.
	/// </summary>
	/// <param name="containerName">The name of the container to be played.</param>
	/// <param name = "delay">The delay in seconds before playing the container.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>An item that will let you control the container.</returns>	
	public static PureDataItem PlayContainer(string containerName, float delay, params PureDataOption[] options) {
		return instance.itemManager.PlayContainer(containerName, instance.listener, delay, options);
	}

	/// <summary>
	/// Plays a container spatialized around the <paramref name="source"/>.
	/// </summary>
	/// <param name="containerName">The name of the container to be played.</param>
	/// <param name="source">The source around which the container will be spatialized.</param>
	/// <param name = "delay">The delay in seconds before playing the container.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>An item that will let you control the container.</returns>	
	/// <remarks>Using a Transform asynchronously as a source might introduce glitches.</remarks>
	public static PureDataItem PlayContainer(string containerName, Transform source, float delay, params PureDataOption[] options) {
		return instance.itemManager.PlayContainer(containerName, source, delay, options);
	}
	
	/// <summary>
	/// Plays a container spatialized around the <paramref name="source"/>.
	/// </summary>
	/// <param name="containerName">The name of the container to be played.</param>
	/// <param name="source">The source around which the container will be spatialized.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>An item that will let you control the container.</returns>	
	/// <remarks>Using a Transform asynchronously as a source might introduce glitches.</remarks>
	public static PureDataItem PlayContainer(string containerName, Transform source, params PureDataOption[] options) {
		return instance.itemManager.PlayContainer(containerName, source, 0, options);
	}
		
	/// <summary>
	/// Plays a container spatialized around the <paramref name="source"/>.
	/// </summary>
	/// <param name="containerName">The name of the container to be played.</param>
	/// <param name="source">The source around which the container will be spatialized.</param>
	/// <param name = "delay">The delay in seconds before playing the container.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>An item that will let you control the container.</returns>	
	public static PureDataItem PlayContainer(string containerName, Vector3 source, float delay, params PureDataOption[] options) {
		return instance.itemManager.PlayContainer(containerName, source, delay, options);
	}
		
	/// <summary>
	/// Plays a container spatialized around the <paramref name="source"/>.
	/// </summary>
	/// <param name="containerName">The name of the container to be played.</param>
	/// <param name="source">The source around which the container will be spatialized.</param>
	/// <param name = "options">The options that will override the default options set in the inspector.</param>
	/// <returns>An item that will let you control the container.</returns>	
	public static PureDataItem PlayContainer(string containerName, Vector3 source, params PureDataOption[] options) {
		return instance.itemManager.PlayContainer(containerName, source, 0, options);
	}
	#endregion
	
	#region Stop
	/// <summary>
	/// Stops all currently active items with fade out.
	/// </summary>
	public static void StopAll() {
		instance.sourceManager.StopAll(0);
	}
	
	/// <summary>
	/// Stops all currently active items with fade out.
	/// </summary>
	/// <param name = "delay">The delay in seconds before stopping the item.</param>
	public static void StopAll(float delay) {
		instance.sourceManager.StopAll(delay);
	}
	
	/// <summary>
	/// Stops all currently active items immediatly. Stopping an item this way might generate clicks.
	/// </summary>
	public static void StopAllImmediate() {
		instance.sourceManager.StopAllImmediate();
	}
	#endregion
	
	#region Volume
	/// <summary>
	/// Ramps the master volume.
	/// </summary>
	/// <param name="targetVolume">The target to which the volume will be ramped.</param>
	/// <param name="time">The time in seconds it will take for the volume to reach the <paramref name="targetVolume"/></param>
	/// <param name="delay">The delay in seconds before the volume will be ramped.</param>
	public static void SetMasterVolume(float targetVolume, float time, float delay) {
		instance.generalSettings.SetMasterVolume(targetVolume, time, delay);
	}
	
	/// <summary>
	/// Ramps the master volume.
	/// </summary>
	/// <param name="targetVolume">The target to which the volume will be ramped.</param>
	/// <param name="time">The time in seconds it will take for the volume to reach the <paramref name="targetVolume"/></param>
	public static void SetMasterVolume(float targetVolume, float time) {
		instance.generalSettings.SetMasterVolume(targetVolume, time, 0);
	}
	
	/// <summary>
	/// Sets the master volume.
	/// </summary>
	/// <param name="targetVolume">The target to which the volume will be set.</param>
	public static void SetMasterVolume(float targetVolume) {
		instance.generalSettings.SetMasterVolume(targetVolume, 0, 0);
	}
	
	/// <summary>
	/// Ramps the volume of a bus.
	/// </summary>
	/// <param name = "busName">The name of the bus that will have its volume ramped.</param>
	/// <param name="targetVolume">The target to which the volume will be ramped.</param>
	/// <param name="time">The time in seconds it will take for the volume to reach the <paramref name="targetVolume"/></param>
	/// <param name="delay">The delay in seconds before the volume will be ramped.</param>	
	public static void SetBusVolume(string busName, float targetVolume, float time, float delay) {
		instance.busManager.SetBusVolume(busName, targetVolume, time, delay);
	}
		
	/// <summary>
	/// Ramps the volume of a bus.
	/// </summary>
	/// <param name = "busName">The name of the bus that will have its volume ramped.</param>
	/// <param name="targetVolume">The target to which the volume will be ramped.</param>
	/// <param name="time">The time in seconds it will take for the volume to reach the <paramref name="targetVolume"/></param>
	public static void SetBusVolume(string busName, float targetVolume, float time) {
		instance.busManager.SetBusVolume(busName, targetVolume, time, 0);
	}
		
	/// <summary>
	/// Sets the volume of a bus.
	/// </summary>
	/// <param name = "busName">The name of the bus that will have its volume set.</param>
	/// <param name="targetVolume">The target to which the volume will be set.</param>
	public static void SetBusVolume(string busName, float targetVolume) {
		instance.busManager.SetBusVolume(busName, targetVolume, 0, 0);
	}
	#endregion
	
	#region Memory
	/// <summary>
	/// Loads a sound in memory.
	/// </summary>
	/// <param name="soundName">The name of the sound to be loaded.</param>
	public static void Load(string soundName) {
		instance.clipManager.Load(soundName);
	}
		
	/// <summary>
	/// Unloads a sound from memory.
	/// </summary>
	/// <param name="soundName"></param>
	/// <remarks>Unloading a sound that is playing will stop it and might generate clicks.</remarks>
	public static void Unload(string soundName) {
		instance.clipManager.Unload(soundName);
	}
		
	/// <summary>
	/// Unloads all unused sounds.
	/// </summary>
	public static void UnloadUnused() {
		instance.clipManager.UnloadUnused();
	}
	#endregion
	
	#region Send
	/// <summary>
	/// Converts and sends a value to Pure Data. In Pure Data, you can receive the value with a <c>[receive <paramref name="receiverName"/>]</c>.
	/// </summary>
	/// <param name="receiverName">The name of to be used in Pure Data to receive the value.</param>
	/// <param name="toSend">The value to be sent. Valid types include int, int[] float, float[], double, double[], bool, bool[], char, char[], string, string[], Enum, Enum[], Vector2, Vector3, Vector4, Quaternion, Rect, Bounds and Color.</param>
	/// <returns>True if the value has been successfully sent and received.</returns>
	public static bool Send(string receiverName, object toSend) {
		return instance.communicator.Send(receiverName, toSend);
	}
	
	/// <summary>
	/// Converts and sends a value to Pure Data. In Pure Data, you can receive the value with a <c>[receive <paramref name="receiverName"/>]</c>.
	/// </summary>
	/// <param name="receiverName">The name of to be used in Pure Data to receive the value.</param>
	/// <param name="toSend">The value to be sent. Valid types include int, int[] float, float[], double, double[], bool, bool[], char, char[], string, string[], Enum, Enum[], Vector2, Vector3, Vector4, Quaternion, Rect, Bounds and Color.</param>
	/// <returns>True if the value has been successfully sent and received.</returns>
	public static bool Send<T>(string receiverName, params T[] toSend) {
		return instance.communicator.Send(receiverName, toSend);
	}
		
	/// <summary>
	/// Converts and sends a value to Pure Data. In Pure Data, you can receive the value with a <c>[receive <paramref name="receiverName"/>]</c>.
	/// </summary>
	/// <param name="receiverName">The name of to be used in Pure Data to receive the value.</param>
	/// <param name="toSend">The value to be sent. Valid types include int, int[] float, float[], double, double[], bool, bool[], char, char[], string, string[], Enum, Enum[], Vector2, Vector3, Vector4, Quaternion, Rect, Bounds and Color.</param>
	/// <returns>True if the value has been successfully sent and received.</returns>
	public static bool Send(string receiverName, params object[] toSend) {
		return instance.communicator.Send(receiverName, toSend);
	}
				
	/// <summary>
	/// Sends a bang to Pure Data. In Pure Data, you can receive the bang with a <c>[receive <paramref name="receiverName"/>]</c>.
	/// </summary>
	/// <param name="receiverName">The name of to be used in Pure Data to receive the value.</param>
	/// <returns>True if the bang has been successfully sent and received.</returns>
	public static bool Send(string receiverName) {
		return instance.communicator.SendBang(receiverName);
	}

	/// <summary>
	/// Sends a message to Pure Data. In Pure Data, you can receive the message with a <c>[receive <paramref name="receiverName"/>]</c>.
	/// </summary>
	/// <param name="receiverName">The name of to be used in Pure Data to receive the value.</param>
	/// <param name="message">The message to be sent.</param>
	/// <param name="arguments">Additional arguments can be added to the message. Valid types include int, float, string.</param>
	/// <returns>True if the message has been successfully sent and received.</returns>
	public static bool SendMessage<T>(string receiverName, string message, params T[] arguments) {
		return instance.communicator.SendMessage(receiverName, message, arguments);
	}
	
	/// <summary>
	/// Sends a aftertouch event to Pure Data. In Pure Data, you can receive the aftertouch event with a <c>[touchin]</c>.
	/// </summary>
	/// <param name="channel">The channel to be sent.</param>
	/// <param name="value">The value to be sent.</param>
	/// <returns>True if the aftertouch event has been successfully sent and received.</returns>
	public static bool SendAftertouch(int channel, int value) {
		return instance.communicator.SendAftertouch(channel, value);
	}
	
	/// <summary>
	/// Sends a control change event to Pure Data. In Pure Data, you can receive the control change event with a <c>[ctlin]</c>.
	/// </summary>
	/// <param name="channel">The channel to be sent.</param>
	/// <param name="controller">The controller to be sent.</param>
	/// <param name="value">The value to be sent.</param>
	/// <returns>True if the control change event has been successfully sent and received.</returns>
	public static bool SendControlChange(int channel, int controller, int value) {
		return instance.communicator.SendControlChange(channel, controller, value);
	}
	
	/// <summary>
	/// Sends a raw midi byte to Pure Data. In Pure Data, you can receive the raw midi byte with a <c>[midiin]</c>.
	/// </summary>
	/// <param name="port">The port to be sent.</param>
	/// <param name="value">The value to be sent.</param>
	/// <returns>True if the raw midi byte has been successfully sent and received.</returns>
	public static bool SendMidiByte(int port, int value) {
		return instance.communicator.SendMidiByte(port, value);
	}
	
	/// <summary>
	/// Sends a note on event to Pure Data. In Pure Data, you can receive the note on event with a <c>[notein]</c>.
	/// </summary>
	/// <param name="channel">The channel to be sent.</param>
	/// <param name="pitch">The pitch to be sent.</param>
	/// <param name="velocity">The velocity to be sent.</param>
	/// <returns>True if the note on event has been successfully sent and received.</returns>
	public static bool SendNoteOn(int channel, int pitch, int velocity) {
		return instance.communicator.SendNoteOn(channel, pitch, velocity);
	}
	
	/// <summary>
	/// Sends a pitch bend event to Pure Data. In Pure Data, you can receive the pitch bend event with a <c>[bendin]</c>.
	/// </summary>
	/// <param name="channel">The channel to be sent.</param>
	/// <param name="value">The value to be sent.</param>
	/// <returns>True if the pitch bend event has been successfully sent and received.</returns>
	public static bool SendPitchbend(int channel, int value) {
		return instance.communicator.SendPitchbend(channel, value);
	}
	
	/// <summary>
	/// Sends a polyphonic aftertouch event to Pure Data. In Pure Data, you can receive the polyphonic aftertouch event with a <c>[polytouchin]</c>.
	/// </summary>
	/// <param name="channel">The channel to be sent.</param>
	/// <param name="pitch">The pitch to be sent.</param>
	/// <param name="value">The value to be sent.</param>
	/// <returns>True if the polyphonic aftertouch event has been successfully sent and received.</returns>
	public static bool SendPolyAftertouch(int channel, int pitch, int value) {
		return instance.communicator.SendPolyAftertouch(channel, pitch, value);
	}
	
	/// <summary>
	/// Sends a program change event to Pure Data. In Pure Data, you can receive the program change event with a <c>[pgmin]</c>.
	/// </summary>
	/// <param name="channel">The channel to be sent.</param>
	/// <param name="value">The value to be sent.</param>
	/// <returns>True if the program change event has been successfully sent and received.</returns>
	public static bool SendProgramChange(int channel, int value) {
		return instance.communicator.SendProgramChange(channel, value);
	}
	
	/// <summary>
	/// Sends a byte of a sysex message to Pure Data. In Pure Data, you can receive the byte of the sysex message with a <c>[sysexin]</c>.
	/// </summary>
	/// <param name="port">The port to be sent.</param>
	/// <param name="value">The value to be sent.</param>
	/// <returns>True if the byte of the sysex message has been successfully sent and received.</returns>
	public static bool SendSysex(int port, int value) {
		return instance.communicator.SendSysex(port, value);
	}
	
	/// <summary>
	/// Sends a byte to Pure Data. In Pure Data, you can receive the byte with a <c>[realtimein]</c>.
	/// </summary>
	/// <param name="port">The port to be sent.</param>
	/// <param name="value">The value to be sent.</param>
	/// <returns>True if the byte has been successfully sent and received.</returns>
	public static bool SendSysRealtime(int port, int value) {
		return instance.communicator.SendSysRealtime(port, value);
	}
	#endregion
	
	#region Receive
	/// <summary>
	/// Subscribes a delegate so that it receives bangs from all <c>[send <paramref name="sendName"/>]</c> in Pure Data.
	/// </summary>
	/// <param name="sendName">The name of the sender from which the bangs will be received.</param>
	/// <param name="bangReceiver">The delegate that will receive the bangs.</param>
	/// <param name="asynchronous">If true, the bangs will be received as soon as they are sent by Pure Data, otherwise they will be received on the next Unity Update, FixedUpdate or LateUpdate.</param>
	public static void Receive(string sendName, BangReceiveCallback bangReceiver, bool asynchronous = false) {
		instance.communicator.Receive(sendName, bangReceiver, asynchronous);
	}
	
	/// <summary>
	/// Subscribes a delegate so that it receives floats from all <c>[send <paramref name="sendName"/>]</c> in Pure Data.
	/// </summary>
	/// <param name="sendName">The name of the sender from which the floats will be received.</param>
	/// <param name="floatReceiver">The delegate that will receive the floats.</param>
	/// <param name="asynchronous">If true, the floats will be received as soon as they are sent by Pure Data, otherwise they will be received on the next Unity Update, FixedUpdate or LateUpdate.</param>
	public static void Receive(string sendName, FloatReceiveCallback floatReceiver, bool asynchronous = false) {
		instance.communicator.Receive(sendName, floatReceiver, asynchronous);
	}
		
	/// <summary>
	/// Subscribes a delegate so that it receives symbols from all <c>[send <paramref name="sendName"/>]</c> in Pure Data.
	/// </summary>
	/// <param name="sendName">The name of the sender from which the symbols will be received.</param>
	/// <param name="symbolReceiver">The delegate that will receive the symbols.</param>
	/// <param name="asynchronous">If true, the symbols will be received as soon as they are sent by Pure Data, otherwise they will be received on the next Unity Update, FixedUpdate or LateUpdate.</param>
	public static void Receive(string sendName, SymbolReceiveCallback symbolReceiver, bool asynchronous = false) {
		instance.communicator.Receive(sendName, symbolReceiver, asynchronous);
	}
		
	/// <summary>
	/// Subscribes a delegate so that it receives lists from all <c>[send <paramref name="sendName"/>]</c> in Pure Data.
	/// </summary>
	/// <param name="sendName">The name of the sender from which the lists will be received.</param>
	/// <param name="listReceiver">The delegate that will receive the lists.</param>
	/// <param name="asynchronous">If true, the lists will be received as soon as they are sent by Pure Data, otherwise they will be received on the next Unity Update, FixedUpdate or LateUpdate.</param>
	public static void Receive(string sendName, ListReceiveCallback listReceiver, bool asynchronous = false) {
		instance.communicator.Receive(sendName, listReceiver, asynchronous);
	}
		
	/// <summary>
	/// Subscribes a delegate so that it receives messages from all <c>[send <paramref name="sendName"/>]</c> in Pure Data.
	/// </summary>
	/// <param name="sendName">The name of the sender from which the messages will be received.</param>
	/// <param name="messageReceiver">The delegate that will receive the messages.</param>
	/// <param name="asynchronous">If true, the messages will be received as soon as they are sent by Pure Data, otherwise they will be received on the next Unity Update, FixedUpdate or LateUpdate.</param>
	public static void Receive(string sendName, MessageReceiveCallback messageReceiver, bool asynchronous = false) {
		instance.communicator.Receive(sendName, messageReceiver, asynchronous);
	}
	#endregion
	
	#region Release
	/// <summary>
	/// Unsubscribes a delegate so that it stops receiving bangs from Pure Data.
	/// </summary>
	/// <param name="bangReceiver">The subscribed delegate that will stop receiving bangs.</param>
	public static void Release(BangReceiveCallback bangReceiver) {
		instance.communicator.Release(bangReceiver);
	}
	
	/// <summary>
	/// Unsubscribes a delegate so that it stops receiving floats from Pure Data.
	/// </summary>
	/// <param name="floatReceiver">The subscribed delegate that will stop receiving floats.</param>
	public static void Release(FloatReceiveCallback floatReceiver) {
		instance.communicator.Release(floatReceiver);
	}
	
	/// <summary>
	/// Unsubscribes a delegate so that it stops receiving symbols from Pure Data.
	/// </summary>
	/// <param name="symbolReceiver">The subscribed delegate that will stop receiving symbols.</param>
	public static void Release(SymbolReceiveCallback symbolReceiver) {
		instance.communicator.Release(symbolReceiver);
	}
	
	/// <summary>
	/// Unsubscribes a delegate so that it stops receiving lists from Pure Data.
	/// </summary>
	/// <param name="listReceiver">The subscribed delegate that will stop receiving lists.</param>
	public static void Release(ListReceiveCallback listReceiver) {
		instance.communicator.Release(listReceiver);
	}
	
	/// <summary>
	/// Unsubscribes a delegate so that it stops receiving messages from Pure Data.
	/// </summary>
	/// <param name="messageReceiver">The subscribed delegate that will stop receiving messages.</param>
	public static void Release(MessageReceiveCallback messageReceiver) {
		instance.communicator.Release(messageReceiver);
	}
	
	/// <summary>
	/// Unsubscribes all delegates so that they stop receiving data from Pure Data.
	/// </summary>
	public static void ReleaseAll() {
		instance.communicator.ReleaseAll();
	}
	#endregion
	
	#region Array
	/// <summary>
	/// Reads the array of <paramref name="arrayName"/> from Pure Data and stores it in <paramref name="data"/>.
	/// </summary>
	/// <param name="arrayName">The name of the array that will be read.</param>
	/// <param name="data">The allocated array that will receive the data.</param>
	/// <returns>True if the data has been successfully retrieved.</returns>
	public static bool ReadArray(string arrayName, float[] data) {
		return instance.communicator.ReadArray(arrayName, data);
	}
	
	/// <summary>
	/// Writes <paramref name="data"/> to a Pure Data array (the array will be resized if needed). In Pure Data, you can receive the data with a <c>[table <paramref name="arrayName"/>]</c>.
	/// </summary>
	/// <param name="arrayName">The name of the array that will receive the data.</param>
	/// <param name="data">The data to be written to the array.</param>
	/// <returns>True if the data has been successfully sent and received.</returns>
	public static bool WriteArray(string arrayName, float[] data) {
		return instance.communicator.WriteArray(arrayName, data);
	}
	
	/// <summary>
	/// Resizes a Pure Data array to a new size. Can be used to free memory.
	/// </summary>
	/// <param name="arrayName">The name of the array to be resized.</param>
	/// <param name="size">The target size of the array.</param>
	/// <returns>True if the array has been successfully resized.</returns>
	public static bool ResizeArray(string arrayName, int size) {
		return instance.communicator.ResizeArray(arrayName, size);
	}
	#endregion
	
	#region Patch
	/// <summary>
	/// Opens a patch and starts the DSP.
	/// </summary>
	/// <param name="patchName">The name of the patch (without the extension) to be opened relative to <c>Assets/StreamingAssets/<paramref name="patchesPath"/>/</c></param>.
	public static void OpenPatch(string patchName) {
		instance.patchManager.Open(patchName);
	}
	
	/// <summary>
	/// Opens patches and starts the DSP.
	/// </summary>
	/// <param name="patchesName">The name of the patches (without the extension) to be opened relative to <c>Assets/StreamingAssets/<paramref name="patchesPath"/>/</c></param>
	public static void OpenPatches(params string[] patchesName) {
		instance.patchManager.Open(patchesName);
	}
	
	/// <summary>
	/// Closes a patch.
	/// </summary>
	/// <param name="patchName">The name of the patch (without the extension and directory) to be closed.</param>
	public static void ClosePatch(string patchName) {
		instance.patchManager.Close(patchName);
	}
	
	/// <summary>
	/// Closes patches.
	/// </summary>
	/// <param name="patchesName">The name of the patches (without the extension and directory) to be closed.</param>
	public static void ClosePatches(params string[] patchesName) {
		instance.patchManager.Close(patchesName);
	}
	
	/// <summary>
	/// Closes all opened patches.
	/// </summary>
	public static void CloseAllPatches() {
		instance.patchManager.CloseAll();
	}
	#endregion
}
