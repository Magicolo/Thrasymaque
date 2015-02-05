using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSource : INamable, IIdentifiable {

		public string Name {
			get {
				return Info.Name;
			}
			set {
				Info.Name = value;
			}
		}
		
		int id;
		public int Id {
			get {
				return id;
			}
			set {
				id = value;
				dopplerSendName = "uaudiosource_doppler" + id;
				panLeftSendName = "uaudiosource_pan_left" + id;
				panRightSendName = "uaudiosource_pan_right" + id;
			}
		}
		
		PureDataStates state;
		public PureDataStates State {
			get {
				return state;
			}
			set {
				state = value;
			}
		}
		
		PureDataClip clip;
		public PureDataClip Clip {
			get {
				return clip;
			}
			set {
				clip = value;
				pureData.communicator.Send("uaudiosource_clipsamples" + Id, clip.Samples);
				pureData.communicator.Send("uaudiosource_clipname" + Id, clip.Name);
			}
		}

		PureDataInfo info;
		public PureDataInfo Info {
			get {
				return info;
			}
			set {
				info = value;
			}
		}

		#region Spatial settings
		public float DopplerLevel {
			get {
				return Info.dopplerLevel;
			}
			set {
				Info.dopplerLevel = value;
				spatialize = true;
			}
		}
		
		public PureDataVolumeRolloffModes VolumeRolloffMode {
			get {
				return Info.volumeRolloffMode;
			}
			set {
				Info.volumeRolloffMode = value;
				spatialize = true;
			}
		}

		public float MinDistance {
			get {
				return Info.minDistance;
			}
			set {
				Info.minDistance = value;
				spatialize = true;
			}
		}

		public float MaxDistance {
			get {
				return Info.maxDistance;
			}
			set {
				Info.maxDistance = value;
				spatialize = true;
			}
		}

		public float PanLevel {
			get {
				return Info.panLevel;
			}
			set {
				Info.panLevel = value;
				spatialize = true;
			}
		}
		#endregion
		
		#region Internal settings
		public float adjustedLength {
			get {
				return Info.length / Mathf.Abs(Info.pitch);
			}
		}
		public float adjustedClippedLength {
			get {
				return (Info.playRangeEnd - Info.playRangeStart) * adjustedLength;
			}
		}
		public float adjustedThreshold {
			get {
				if (Info.pitch >= 0) {
					if (Info.isFixed) {
						return (adjustedClippedLength - adjustedFadeOut) / adjustedClippedLength;
					}
				
					return (adjustedClippedLength - adjustedFadeOut) / adjustedLength + Info.playRangeStart;
				}
				
				if (Info.isFixed) {
					return (adjustedClippedLength - adjustedFadeOut) / adjustedClippedLength - 1;
				}
				
				return (adjustedClippedLength - adjustedFadeOut) / adjustedLength - Info.playRangeEnd;

			}
		}
		public float adjustedFadeOut {
			get {
				if (Info.isFixed) {
					return Mathf.Clamp(Info.fadeOut, thresholdEpsilon * adjustedClippedLength, adjustedClippedLength - (thresholdEpsilon * adjustedClippedLength));
				}
				
				return Mathf.Clamp(Info.fadeOut, thresholdEpsilon * adjustedLength, adjustedClippedLength - (thresholdEpsilon * adjustedLength));
			}
		}
		public float adjustedPhaseStart {
			get {
				if (Info.pitch >= 0) {
					return Info.isFixed ? 0 : Info.playRangeStart;
				}
				
				return Info.isFixed ? 1 - thresholdEpsilon : Info.playRangeEnd;
			}
		}
		public object source;
		public float readSpeed;
		public float threshold;
		public const float thresholdEpsilon = 0.001F;
		public float phase;
		public bool sourceSwitch;
		public bool memoryReaderSwitch;
		public bool stopperSwitch;
		public bool monoSwitch;
		public bool stereoSwitch;
		#endregion
		
		public PureData pureData;
		
		bool spatialize;
		float lastDistance;
		string dopplerSendName;
		string panLeftSendName;
		string panRightSendName;

		public PureDataSource(PureData pureData) {
			this.pureData = pureData;
		}

		public void Update() {
			if (SourceHasChanged()) {
				Spatialize();
			}
		}
		
		public void Play(float delay = 0) {
			delay = Mathf.Max(delay, 0);
			
			if (State == PureDataStates.Waiting) {
				State = PureDataStates.Playing;
				
				pureData.sourceManager.Activate(this);
				
				SwitchOn(delay);
				FadeIn(delay);
			}
			else if (State == PureDataStates.Paused) {
				State = PureDataStates.Playing;
				
				SetPaused(false, 0.01F, delay);
			}
		}

		public void Pause(float delay = 0) {
			delay = Mathf.Max(delay, 0);
			
			if (State == PureDataStates.Playing) {
				State = PureDataStates.Paused;
				
				SetPaused(true, 0.01F, delay);
			}
		}
		
		public void Stop(float delay = 0) {
			delay = Mathf.Max(delay, 0);
			
			if ((State == PureDataStates.Playing || State == PureDataStates.Paused) && State != PureDataStates.Stopping && State != PureDataStates.Stopped) {
				State = PureDataStates.Stopping;
				
				FadeOut(delay);
				SetStopperSwitch(false, delay);
			}
		}
		
		public void StopImmediate() {
			if (State != PureDataStates.Stopped) {
				State = PureDataStates.Stopped;
				
				SwitchOff();
				SetSourceSwitch(false);
				pureData.sourceManager.Deactivate(this);
			}
		}

		public void FadeIn(float delay = 0) {
			SendMessage("uaudiosource_fade", delay, 0, 0);
			SendMessage("uaudiosource_fade", delay, 1, Mathf.Max(Info.fadeIn * 1000, 10));
		}
		
		public void FadeOut(float delay = 0) {
			SendMessage("uaudiosource_fade", delay, 0, Mathf.Max(adjustedFadeOut * 1000, 10));
			SendMessage("uaudiosource_stopdelay", delay, Mathf.Max(adjustedFadeOut * 1000, 10));
		}
		
		public void SwitchOn(float delay = 0) {
			SetMemoryReaderSwitch(true, delay);
			
			if (Clip.Channels == 1) {
				SetMonoSwitch(true, delay);
				SetStereoSwitch(false, delay);
			}
			else {
				SetMonoSwitch(false, delay);
				SetStereoSwitch(true, delay);
			}
		}
		
		public void SwitchOff(float delay = 0) {
			SetMemoryReaderSwitch(false, delay);
			SetStopperSwitch(false, delay);
			SetMonoSwitch(false, delay);
			SetStereoSwitch(false, delay);
		}

		public void SetClip(PureDataClip clip) {
			Clip = clip;
			Load();
			SwitchOff();
			SetSourceSwitch(true);
			
			Info = new PureDataInfo(pureData.infoManager.GetInfo(clip.Name), pureData);
			State = PureDataStates.Waiting;
			SetPaused(false);
			SetOutput(Info.output);
			SetVolume(Info.volume + Info.volume * HelperFunctions.RandomRange(-Info.randomVolume, Info.randomVolume));
			SetPitch(Info.pitch + Info.pitch * HelperFunctions.RandomRange(-Info.randomPitch, Info.randomPitch));
			SetPlayRange(Info.playRangeStart, Info.playRangeEnd);
			SetFadeIn(Info.fadeIn);
			SetFadeOut(Info.fadeOut);
			SetLoop(Info.loop);
			spatialize = true;
		}
		
		public void SetSource(object targetSource) {
			source = targetSource;
			lastDistance = Vector3.Distance(GetSourcePosition(), pureData.listener.position);
			
			Spatialize(true);
		}
		
		public void SetPaused(bool paused, float time = 0, float delay = 0) {
			int pauseState = (!paused).GetHashCode();
			
			if (paused) {
				SendMessage("uaudiosource_pause", delay + time, pauseState);
			}
			else {
				SendMessage("uaudiosource_pause", 0, pauseState);
			}
			
			SendMessage("uaudiosource_pausevolume", delay, pauseState, time * 1000);
			
		}
		
		public void SetOutput(string targetOutput, float delay = 0) {
			Info.output = targetOutput;
			
			SendMessage("uaudiosource_output", delay, Info.output);
		}
		
		public void SetPlayRange(float start, float end, float delay = 0, bool checkFixed = false) {
			if (checkFixed && Info.isFixed) {
				Logger.LogError("Can not set the play range of a fixed clip.");
				return;
			}
			
			Info.playRangeStart = start;
			Info.playRangeEnd = end;
			
			SetPhase(adjustedPhaseStart, delay);
			SetThreshold(adjustedThreshold, 0, delay);
		}
		
		public void SetVolume(float targetVolume, float time = 0, float delay = 0) {
			Info.volume = Mathf.Max(targetVolume, 0);
			time = Mathf.Max(time, 0);
			
			SendMessage("uaudiosource_volume", delay, Info.volume, time * 1000);
		}
		
		public void SetPitch(float targetPitch, float time = 0, float delay = 0) {
			Info.pitch = targetPitch;
			time = Mathf.Max(time, 0);
			
			SetReadSpeed(((float)Clip.Frequency / (float)Clip.Samples) * Info.pitch, time, delay);
			SetThreshold(adjustedThreshold, time, delay);
		}
		
		public void SetReadSpeed(float targetReadSpeed, float time = 0, float delay = 0) {
			readSpeed = targetReadSpeed;
			
			SendMessage("uaudiosource_readspeed", delay, readSpeed, time * 1000);
		}
		
		public void SetThreshold(float targetThreshold, float time = 0, float delay = 0) {
			threshold = targetThreshold;

			SendMessage("uaudiosource_threshold", delay, threshold, time * 1000);
		}
		
		public void SetPhase(float targetPhase, float delay = 0) {
			phase = targetPhase;
			
			SendMessage("uaudiosource_phase", delay, phase);
		}
		
		public void SetFadeIn(float targetFadeIn) {
			Info.fadeIn = Mathf.Clamp(targetFadeIn, 0, adjustedLength);
		}
		
		public void SetFadeOut(float targetFadeOut) {
			Info.fadeOut = targetFadeOut;
			
			SetThreshold(adjustedThreshold);
		}
		
		public void SetLoop(bool targetLoop, float delay = 0) {
			Info.loop = targetLoop;
			
			SetStopperSwitch(!Info.loop, delay);
		}
		
		public void SetSourceSwitch(bool targetSwitch, float delay = 0) {
			sourceSwitch = targetSwitch;
			
			SendMessage("uaudiosource_switch", delay, sourceSwitch);
		}
		
		public void SetMemoryReaderSwitch(bool targetSwitch, float delay = 0) {
			memoryReaderSwitch = targetSwitch;
			
			SendMessage("uaudiosource_memoryreaderswitch", delay, memoryReaderSwitch);
		}
		
		public void SetStopperSwitch(bool targetSwitch, float delay = 0) {
			stopperSwitch = targetSwitch;
			
			SendMessage("uaudiosource_stopperswitch", delay, stopperSwitch);
		}
		
		public void SetMonoSwitch(bool targetSwitch, float delay = 0) {
			monoSwitch = targetSwitch;
			
			SendMessage("uaudiosource_monoswitch", delay, monoSwitch);
		}
		
		public void SetStereoSwitch(bool targetSwitch, float delay = 0) {
			stereoSwitch = targetSwitch;
			
			SendMessage("uaudiosource_stereoswitch", delay, stereoSwitch);
		}
		
		public void Load() {
			Clip.Load();
		}
		
		public void Unload() {
			Clip.Unload();
		}
		
		public void ApplyOptions(params PureDataOption[] options) {
			foreach (PureDataOption option in options) {
				ApplyOption(option);
			}
		}
		
		public void ApplyOption(PureDataOption option, float delay = 0) {
			option.Apply(this, delay);
		}
		
		public void Spatialize(bool initialize = false) {
			if (source == null) {
				pureData.communicator.Send(panLeftSendName, 1, 0);
				pureData.communicator.Send(panRightSendName, 1, 0);
				return;
			}
			
			const float curveDepth = 3.5F;
			
			Vector3 sourcePosition = GetSourcePosition();
			Vector3 listenerToSource = sourcePosition - pureData.listener.position;
			
			// Attenuation
			float distance = Vector3.Distance(sourcePosition, pureData.listener.position);
			float adjustedDistance = Mathf.Clamp01(Mathf.Max(distance - MinDistance, 0) / Mathf.Max(MaxDistance - MinDistance, 0.001F));
			float attenuation;
			
			if (VolumeRolloffMode == PureDataVolumeRolloffModes.Logarithmic) {
				attenuation = Mathf.Pow((1F - Mathf.Pow(adjustedDistance, 1F / curveDepth)), curveDepth);
			}
			else {
				attenuation = 1F - adjustedDistance;
			}
			
			// Pan
			float angle = Vector3.Angle(pureData.listener.right, listenerToSource);
			float panLeft = ((1 - PanLevel) + PanLevel * Mathf.Sin(Mathf.Max(180 - angle, 90) * Mathf.Deg2Rad)) * attenuation;
			float panRight = ((1 - PanLevel) + PanLevel * Mathf.Sin(Mathf.Max(angle, 90) * Mathf.Deg2Rad)) * attenuation;
			
			pureData.communicator.Send(panLeftSendName, panLeft, initialize ? 0 : 10);
			pureData.communicator.Send(panRightSendName, panRight, initialize ? 0 : 10);
			
			// Doppler
			if (initialize) {
				pureData.communicator.Send(dopplerSendName, 1, 0);
			}
			else {
				float doppler = (pureData.generalSettings.speedOfSound + (lastDistance - distance) * DopplerLevel / Time.deltaTime) / pureData.generalSettings.speedOfSound;
				lastDistance = distance;
				pureData.communicator.Send(dopplerSendName, doppler, 100);
			}
		}

		public Vector3 GetSourcePosition() {
			Vector3 sourcePosition = pureData.listener.position;
			
			if (source as PureDataListener != null) {
				sourcePosition = ((PureDataListener)source).position;
			}
			else if (pureData.generalSettings.IsMainThread() && source as Transform != null) {
				sourcePosition = ((Transform)source).position;
			}
			else if (source is Vector3) {
				sourcePosition = ((Vector3)source);
			}
			return sourcePosition;
		}
		
		public bool SourceHasChanged() {
			bool hasChanged = false;
			
			if (spatialize) {
				hasChanged = true;
				spatialize = false;
			}
			
			if (source != null && source != pureData.listener && pureData.listener.transform.hasChanged) {
				pureData.SetTransformHasChanged(pureData.listener.transform, false);
				spatialize = true;
				hasChanged = true;
			}
			
			if (source as Transform != null && ((Transform)source).hasChanged) {
				pureData.SetTransformHasChanged(((Transform)source), false);
				spatialize = true;
				hasChanged = true;
			}
			
			return hasChanged;
		}
		
		public void SendMessage(string sendName, float delay, float arg1) {
			if (delay > 0) {
				pureData.communicator.Send("uresources_messagedelayer_f", arg1, sendName + Id, delay * 1000);
			}
			else {
				pureData.communicator.Send(sendName + Id, arg1);
			}
		}
		
		public void SendMessage(string sendName, float delay, float arg1, float arg2) {
			if (delay > 0) {
				pureData.communicator.Send("uresources_messagedelayer_ff", arg1, arg2, sendName + Id, delay * 1000);
			}
			else {
				pureData.communicator.Send(sendName + Id, arg1, arg2);
			}
		}
		
		public void SendMessage(string sendName, float delay, bool arg1) {
			SendMessage(sendName, delay, arg1.GetHashCode());
		}
		
		public void SendMessage(string sendName, float delay, string arg1) {
			if (delay > 0) {
				pureData.communicator.Send("uresources_messagedelayer_s", arg1, sendName + Id, delay * 1000);
			}
			else {
				pureData.communicator.Send(sendName + Id, arg1);
			}
		}
	}
}
