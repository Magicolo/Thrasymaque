using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class StateMachineMonoBehaviour : MonoBehaviourExtended {

	public StateMachine machine;
	
	public new string name { get { return machine.name; } }
	public new bool enabled { get { return machine.enabled; } }
	public new string tag { get { return machine.tag; } }
	public new HideFlags hideFlags { get { return machine.hideFlags; } }
	
	public new GameObject gameObject { get { return machine.gameObject; } }
	public new Animation animation { get { return machine.animation; } }
	public new AudioSource audio { get { return machine.audio; } }
	public new Camera camera { get { return machine.camera; } }
	public new Collider collider { get { return machine.collider; } }
	public new Collider2D collider2D { get { return machine.collider2D; } }
	public new ConstantForce constantForce { get { return machine.constantForce; } }
	public new GUIElement guiElement { get { return machine.guiElement; } }
	public new GUIText guiText { get { return machine.guiText; } }
	public new GUITexture guiTexture { get { return machine.guiTexture; } }
	public new HingeJoint hingeJoint { get { return machine.hingeJoint; } }
	public new Light light { get { return machine.light; } }
	public new NetworkView networkView { get { return machine.networkView; } }
	public new ParticleEmitter particleEmitter { get { return machine.particleEmitter; } }
	public new ParticleSystem particleSystem { get { return machine.particleSystem; } }
	public new Renderer renderer { get { return machine.renderer; } }
	public new Rigidbody rigidbody { get { return machine.rigidbody; } }
	public new Rigidbody2D rigidbody2D { get { return machine.rigidbody2D; } }
	public new Transform transform { get { return machine.transform; } }
	public new Joint joint { get { return machine.joint; } }
	public new SpringJoint springJoint { get { return machine.springJoint; } }
	public new FixedJoint fixedJoint { get { return machine.fixedJoint; } }
	public new CharacterJoint characterJoint { get { return machine.characterJoint; } }
	public new ConfigurableJoint configurableJoint { get { return machine.configurableJoint; } }
	public new BoxCollider boxCollider { get { return machine.boxCollider; } }
	public new SphereCollider sphereCollider { get { return machine.sphereCollider; } }
	public new MeshCollider meshCollider { get { return machine.meshCollider; } }
	public new CapsuleCollider capsuleCollider { get { return machine.capsuleCollider; } }
	public new WheelCollider wheelCollider { get { return machine.wheelCollider; } }
	public new CharacterController characterController { get { return machine.characterController; } }
	public new TerrainCollider terrainCollider { get { return machine.terrainCollider; } }
	public new Cloth cloth { get { return machine.cloth; } }
	public new InteractiveCloth interactiveCloth { get { return machine.interactiveCloth; } }
	public new SkinnedCloth skinnedCloth { get { return machine.skinnedCloth; } }
	public new OffMeshLink offMeshLink { get { return machine.offMeshLink; } }
	public new Tree tree { get { return machine.tree; } }
	public new CanvasGroup canvasGroup { get { return machine.canvasGroup; } }
	public new CanvasRenderer canvasRenderer { get { return machine.canvasRenderer; } }
	public new OcclusionArea occlusionArea { get { return machine.occlusionArea; } }
	public new OcclusionPortal occlusionPortal { get { return machine.occlusionPortal; } }
	public new MeshFilter meshFilter { get { return machine.meshFilter; } }
	public new ClothRenderer clothRenderer { get { return machine.clothRenderer; } }
	public new SkinnedMeshRenderer skinnedMeshRenderer { get { return machine.skinnedMeshRenderer; } }
	public new TrailRenderer trailRenderer { get { return machine.trailRenderer; } }
	public new ParticleRenderer particleRenderer { get { return machine.particleRenderer; } }
	public new LineRenderer lineRenderer { get { return machine.lineRenderer; } }
	public new MeshRenderer meshRenderer { get { return machine.meshRenderer; } }
	public new ParticleSystemRenderer particleSystemRenderer { get { return machine.particleSystemRenderer; } }
	public new SpriteRenderer spriteRenderer { get { return machine.spriteRenderer; } }
	public new TextMesh textMesh { get { return machine.textMesh; } }
	public new ParticleAnimator particleAnimator { get { return machine.particleAnimator; } }
	public new LODGroup lODGroup { get { return machine.lODGroup; } }
	public new LightProbeGroup lightProbeGroup { get { return machine.lightProbeGroup; } }
	public new CircleCollider2D circleCollider2D { get { return machine.circleCollider2D; } }
	public new BoxCollider2D boxCollider2D { get { return machine.boxCollider2D; } }
	public new EdgeCollider2D edgeCollider2D { get { return machine.edgeCollider2D; } }
	public new PolygonCollider2D polygonCollider2D { get { return machine.polygonCollider2D; } }
	public new Joint2D joint2D { get { return machine.joint2D; } }
	public new SpringJoint2D springJoint2D { get { return machine.springJoint2D; } }
	public new DistanceJoint2D distanceJoint2D { get { return machine.distanceJoint2D; } }
	public new HingeJoint2D hingeJoint2D { get { return machine.hingeJoint2D; } }
	public new SliderJoint2D sliderJoint2D { get { return machine.sliderJoint2D; } }
	public new WheelJoint2D wheelJoint2D { get { return machine.wheelJoint2D; } }
	public new NavMeshAgent navMeshAgent { get { return machine.navMeshAgent; } }
	public new NavMeshObstacle navMeshObstacle { get { return machine.navMeshObstacle; } }
	public new AudioListener audioListener { get { return machine.audioListener; } }
	public new AudioReverbZone audioReverbZone { get { return machine.audioReverbZone; } }
	public new AudioLowPassFilter audioLowPassFilter { get { return machine.audioLowPassFilter; } }
	public new AudioHighPassFilter audioHighPassFilter { get { return machine.audioHighPassFilter; } }
	public new AudioDistortionFilter audioDistortionFilter { get { return machine.audioDistortionFilter; } }
	public new AudioEchoFilter audioEchoFilter { get { return machine.audioEchoFilter; } }
	public new AudioChorusFilter audioChorusFilter { get { return machine.audioChorusFilter; } }
	public new AudioReverbFilter audioReverbFilter { get { return machine.audioReverbFilter; } }
	public new Animator animator { get { return machine.animator; } }
	public new Canvas canvas { get { return machine.canvas; } }
	public new LensFlare lensFlare { get { return machine.lensFlare; } }
	public new Projector projector { get { return machine.projector; } }
	public new Skybox skybox { get { return machine.skybox; } }
	public new GUILayer guiLayer { get { return machine.guiLayer; } }
	public new Terrain terrain { get { return machine.terrain; } }
	public new RectTransform rectTransform { get { return machine.rectTransform; } }
	
	public new void BroadcastMessage(string methodName, object parameter, SendMessageOptions options) {
		machine.BroadcastMessage(methodName, parameter, options);
	}
	
	public new void BroadcastMessage(string methodName, object parameter) {
		machine.BroadcastMessage(methodName, parameter);
	}
	
	public new void BroadcastMessage(string methodName) {
		machine.BroadcastMessage(methodName);
	}
	
	public new void BroadcastMessage(string methodName, SendMessageOptions options) {
		machine.BroadcastMessage(methodName, options);
	}

	public new bool CompareTag(string tag) {
		return machine.CompareTag(tag);
	}

	public new Component GetComponent(System.Type type) {
		return machine.GetComponent(type);
	}
	
	public new T GetComponent<T>() where T : Component {
		return machine.GetComponent<T>();
	}
	
	public new Component GetComponent(string type) {
		return machine.GetComponent(type);
	}

	public new Component GetComponentInChildren(System.Type type) {
		return machine.GetComponentInChildren(type);
	}
	
	public new T GetComponentInChildren<T>() where T : Component {
		return machine.GetComponentInChildren<T>();
	}
	
	public new Component GetComponentInParent(System.Type type) {
		return machine.GetComponentInParent(type);
	}
	
	public new T GetComponentInParent<T>() where T : Component {
		return machine.GetComponentInParent<T>();
	}
	
	public new Component[] GetComponents(System.Type type) {
		return machine.GetComponents(type);
	}
	
	public new T[] GetComponents<T>() where T : Component {
		return machine.GetComponents<T>();
	}
	
	public new void GetComponents<T>(List<T> result) where T : Component {
		machine.GetComponents<T>(result);
	}
	
	public new void GetComponents(System.Type type, List<Component> result) {
		machine.GetComponents(type, result);
	}

	public new Component[] GetComponentsInChildren(System.Type type) {
		return machine.GetComponentsInChildren(type);
	}
	
	public new Component[] GetComponentsInChildren(System.Type type, bool includeInactive) {
		return machine.GetComponentsInChildren(type, includeInactive);
	}
		
	public new T[] GetComponentsInChildren<T>() where T : Component {
		return machine.GetComponentsInChildren<T>();
	}
	
	public new T[] GetComponentsInChildren<T>(bool includeInactive) where T : Component {
		return machine.GetComponentsInChildren<T>(includeInactive);
	}
	
	public new void GetComponentsInChildren<T>(List<T> result) where T : Component {
		machine.GetComponentsInChildren<T>(result);
	}

	public new void GetComponentsInChildren<T>(bool includeInactive, List<T> result) where T : Component {
		machine.GetComponentsInChildren<T>(includeInactive, result);
	}
	
	public new Component[] GetComponentsInParent(System.Type type) {
		return machine.GetComponentsInParent(type);
	}
	
	public new Component[] GetComponentsInParent(System.Type type, bool includeInactive) {
		return machine.GetComponentsInParent(type, includeInactive);
	}
		
	public new T[] GetComponentsInParent<T>() where T : Component {
		return machine.GetComponentsInParent<T>();
	}
	
	public new T[] GetComponentsInParent<T>(bool includeInactive) where T : Component {
		return machine.GetComponentsInParent<T>(includeInactive);
	}
}

