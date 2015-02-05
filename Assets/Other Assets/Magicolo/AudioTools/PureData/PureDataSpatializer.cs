using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSpatializer : INamable, IShowable {

		[SerializeField]
		string name;
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}

		[SerializeField]
		bool showing;
		public bool Showing {
			get {
				return showing;
			}
			set {
				showing = value;
			}
		}
		
		[SerializeField, PropertyField]
		PureDataVolumeRolloffModes volumeRolloffMode;
		public PureDataVolumeRolloffModes VolumeRolloffMode {
			get {
				return volumeRolloffMode;
			}
			set {
				volumeRolloffMode = value;
				spatialize = true;
			}
		}

		[SerializeField, PropertyField(typeof(MinAttribute))]
		float minDistance = 1;
		public float MinDistance {
			get {
				return minDistance;
			}
			set {
				minDistance = value;
				spatialize = true;
			}
		}

		[SerializeField, PropertyField(typeof(MinAttribute))]
		float maxDistance = 500;
		public float MaxDistance {
			get {
				return maxDistance;
			}
			set {
				maxDistance = value;
				spatialize = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 1)]
		float panLevel = 0.75F;
		public float PanLevel {
			get {
				return panLevel;
			}
			set {
				panLevel = value;
				spatialize = true;
			}
		}

		GameObject source;
		public GameObject Source {
			get {
				if (source == null && sourceId != 0 || !pureData.generalSettings.applicationPlaying) {
					source = pureData.references.GetObjectWithId<GameObject>(sourceId);
					sourceId = source == null ? 0 : sourceId;
				}
				return source;
			}
			set {
				if (source != value) {
					source = value;
					pureData.references.RemoveReference(sourceId);
				
					if (source == null) {
						sourceId = 0;
					}
					else {
						sourceId = pureData.references.AddReference(source);
					}
				}
			}
		}
		
		[SerializeField]
		int sourceId;
		
		string panLeftSendName;
		string panRightSendName;
		bool spatialize = true;
		PureData pureData;
		
		public PureDataSpatializer(string name, PureData pureData) {
			this.name = name;
			this.pureData = pureData;
		}
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			if (Application.isPlaying) {
				panLeftSendName = "uspatializer_pan_left" + name;
				panRightSendName = "uspatializer_pan_right" + name;
			}
		}
		
		public void Update() {
			if (SourceHasChanged()) {
				Spatialize();
			}
		}

		public void OnDrawGizmos() {
			#if UNITY_EDITOR
			if (Source == null){
				return;
			}
			
			Gizmos.DrawIcon(Source.transform.position, "pd.png", true);
			
			if ((UnityEditor.Selection.gameObjects.Contains(pureData.gameObject) || UnityEditor.Selection.gameObjects.Contains(Source)) && Showing) {
				Gizmos.color = new Color(0.25F, 0.5F, 0.75F, 1);
				Gizmos.DrawWireSphere(Source.transform.position, MinDistance);
				Gizmos.color = new Color(0.25F, 0.75F, 0.5F, 0.35F);
				Gizmos.DrawWireSphere(Source.transform.position, MaxDistance);
			}
			#endif
		}
		
		public void Spatialize() {
			if (Source == null) {
				pureData.communicator.Send(panLeftSendName, 1, 0);
				pureData.communicator.Send(panRightSendName, 1, 0);
				return;
			}
			
			const float curveDepth = 3.5F;
			
			Vector3 sourcePosition = Source.transform.position;
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
			
			pureData.communicator.Send(panLeftSendName, panLeft, 10);
			pureData.communicator.Send(panRightSendName, panRight, 10);
		}
		
		public bool SourceHasChanged() {
			bool hasChanged = false;
			
			if (spatialize) {
				hasChanged = true;
				spatialize = false;
			}
			else if (Source != null) {
				if (pureData.listener.transform.hasChanged) {
					pureData.SetTransformHasChanged(pureData.listener.transform, false);
					spatialize = true;
					hasChanged = true;
				}
			
				if (Source.transform.hasChanged) {
					pureData.SetTransformHasChanged(Source.transform, false);
					spatialize = true;
					hasChanged = true;
				}
			}
			
			return hasChanged;
		}
	}
}
