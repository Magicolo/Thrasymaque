using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public abstract class MonoBehaviourExtended : MonoBehaviour {

		public enum CoroutineStates {
			None,
			Playing,
			Paused,
			Stopped
		}
		
		#region Components
		GameObject _gameObject;
		public new GameObject gameObject { get { return _gameObject ? _gameObject : (_gameObject = base.gameObject); } }
		
		Animation _animation;
		public new Animation animation { get { return _animation ? _animation : (_animation = GetComponent<Animation>()); } }

		AudioSource _audio;
		public new AudioSource audio { get { return _audio ? _audio : (_audio = GetComponent<AudioSource>()); } }

		Camera _camera;
		public new Camera camera { get { return _camera ? _camera : (_camera = GetComponent<Camera>()); } }

		Collider _collider;
		public new Collider collider { get { return _collider ? _collider : (_collider = GetComponent<Collider>()); } }

		Collider2D _collider2D;
		public new Collider2D collider2D { get { return _collider2D ? _collider2D : (_collider2D = GetComponent<Collider2D>()); } }

		ConstantForce _constantForce;
		public new ConstantForce constantForce { get { return _constantForce ? _constantForce : (_constantForce = GetComponent<ConstantForce>()); } }

		GUIElement _guiElement;
		public new GUIElement guiElement { get { return _guiElement ? _guiElement : (_guiElement = GetComponent<GUIElement>()); } }
		
		GUIText _guiText;
		public new GUIText guiText { get { return _guiText ? _guiText : (_guiText = GetComponent<GUIText>()); } }

		GUITexture _guiTexture;
		public new GUITexture guiTexture { get { return _guiTexture ? _guiTexture : (_guiTexture = GetComponent<GUITexture>()); } }

		HingeJoint _hingeJoint;
		public new HingeJoint hingeJoint { get { return _hingeJoint ? _hingeJoint : (_hingeJoint = GetComponent<HingeJoint>()); } }

		Light _light;
		public new Light light { get { return _light ? _light : (_light = GetComponent<Light>()); } }

		NetworkView _networkView;
		public new NetworkView networkView { get { return _networkView ? _networkView : (_networkView = GetComponent<NetworkView>()); } }

		ParticleEmitter _particleEmitter;
		public new ParticleEmitter particleEmitter { get { return _particleEmitter ? _particleEmitter : (_particleEmitter = GetComponent<ParticleEmitter>()); } }

		ParticleSystem _particleSystem;
		public new ParticleSystem particleSystem { get { return _particleSystem ? _particleSystem : (_particleSystem = GetComponent<ParticleSystem>()); } }

		Renderer _renderer;
		public new Renderer renderer { get { return _renderer ? _renderer : (_renderer = GetComponent<Renderer>()); } }

		Rigidbody _rigidbody;
		public new Rigidbody rigidbody { get { return _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>()); } }

		Rigidbody2D _rigidbody2D;
		public new Rigidbody2D rigidbody2D { get { return _rigidbody2D ? _rigidbody2D : (_rigidbody2D = GetComponent<Rigidbody2D>()); } }

		Transform _transform;
		public new Transform transform { get { return _transform ? _transform : (_transform = GetComponent<Transform>()); } }
		
		Joint _joint;
		public Joint joint { get { return _joint ? _joint : (_joint = GetComponent<Joint>()); } }
		
		SpringJoint _springJoint;
		public SpringJoint springJoint { get { return _springJoint ? _springJoint : (_springJoint = GetComponent<SpringJoint>()); } }
		
		FixedJoint _fixedJoint;
		public FixedJoint fixedJoint { get { return _fixedJoint ? _fixedJoint : (_fixedJoint = GetComponent<FixedJoint>()); } }
		
		CharacterJoint _characterJoint;
		public CharacterJoint characterJoint { get { return _characterJoint ? _characterJoint : (_characterJoint = GetComponent<CharacterJoint>()); } }
		
		ConfigurableJoint _configurableJoint;
		public ConfigurableJoint configurableJoint { get { return _configurableJoint ? _configurableJoint : (_configurableJoint = GetComponent<ConfigurableJoint>()); } }
		
		BoxCollider _boxCollider;
		public BoxCollider boxCollider { get { return _boxCollider ? _boxCollider : (_boxCollider = GetComponent<BoxCollider>()); } }
		
		SphereCollider _sphereCollider;
		public SphereCollider sphereCollider { get { return _sphereCollider ? _sphereCollider : (_sphereCollider = GetComponent<SphereCollider>()); } }
		
		MeshCollider _meshCollider;
		public MeshCollider meshCollider { get { return _meshCollider ? _meshCollider : (_meshCollider = GetComponent<MeshCollider>()); } }
		
		CapsuleCollider _capsuleCollider;
		public CapsuleCollider capsuleCollider { get { return _capsuleCollider ? _capsuleCollider : (_capsuleCollider = GetComponent<CapsuleCollider>()); } }
		
		WheelCollider _wheelCollider;
		public WheelCollider wheelCollider { get { return _wheelCollider ? _wheelCollider : (_wheelCollider = GetComponent<WheelCollider>()); } }
		
		CharacterController _characterController;
		public CharacterController characterController { get { return _characterController ? _characterController : (_characterController = GetComponent<CharacterController>()); } }
		
		TerrainCollider _terrainCollider;
		public TerrainCollider terrainCollider { get { return _terrainCollider ? _terrainCollider : (_terrainCollider = GetComponent<TerrainCollider>()); } }
		
		Cloth _cloth;
		public Cloth cloth { get { return _cloth ? _cloth : (_cloth = GetComponent<Cloth>()); } }
		
		InteractiveCloth _interactiveCloth;
		public InteractiveCloth interactiveCloth { get { return _interactiveCloth ? _interactiveCloth : (_interactiveCloth = GetComponent<InteractiveCloth>()); } }
		
		SkinnedCloth _skinnedCloth;
		public SkinnedCloth skinnedCloth { get { return _skinnedCloth ? _skinnedCloth : (_skinnedCloth = GetComponent<SkinnedCloth>()); } }
		
		OffMeshLink _offMeshLink;
		public OffMeshLink offMeshLink { get { return _offMeshLink ? _offMeshLink : (_offMeshLink = GetComponent<OffMeshLink>()); } }
		
		Tree _tree;
		public Tree tree { get { return _tree ? _tree : (_tree = GetComponent<Tree>()); } }
		
		CanvasGroup _canvasGroup;
		public CanvasGroup canvasGroup { get { return _canvasGroup ? _canvasGroup : (_canvasGroup = GetComponent<CanvasGroup>()); } }
		
		CanvasRenderer _canvasRenderer;
		public CanvasRenderer canvasRenderer { get { return _canvasRenderer ? _canvasRenderer : (_canvasRenderer = GetComponent<CanvasRenderer>()); } }
		
		OcclusionArea _occlusionArea;
		public OcclusionArea occlusionArea { get { return _occlusionArea ? _occlusionArea : (_occlusionArea = GetComponent<OcclusionArea>()); } }
		
		OcclusionPortal _occlusionPortal;
		public OcclusionPortal occlusionPortal { get { return _occlusionPortal ? _occlusionPortal : (_occlusionPortal = GetComponent<OcclusionPortal>()); } }
		
		MeshFilter _meshFilter;
		public MeshFilter meshFilter { get { return _meshFilter ? _meshFilter : (_meshFilter = GetComponent<MeshFilter>()); } }
		
		ClothRenderer _clothRenderer;
		public ClothRenderer clothRenderer { get { return _clothRenderer ? _clothRenderer : (_clothRenderer = GetComponent<ClothRenderer>()); } }
		
		SkinnedMeshRenderer _skinnedMeshRenderer;
		public SkinnedMeshRenderer skinnedMeshRenderer { get { return _skinnedMeshRenderer ? _skinnedMeshRenderer : (_skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>()); } }
		
		TrailRenderer _trailRenderer;
		public TrailRenderer trailRenderer { get { return _trailRenderer ? _trailRenderer : (_trailRenderer = GetComponent<TrailRenderer>()); } }
		
		ParticleRenderer _particleRenderer;
		public ParticleRenderer particleRenderer { get { return _particleRenderer ? _particleRenderer : (_particleRenderer = GetComponent<ParticleRenderer>()); } }
		
		LineRenderer _lineRenderer;
		public LineRenderer lineRenderer { get { return _lineRenderer ? _lineRenderer : (_lineRenderer = GetComponent<LineRenderer>()); } }
		
		MeshRenderer _meshRenderer;
		public MeshRenderer meshRenderer { get { return _meshRenderer ? _meshRenderer : (_meshRenderer = GetComponent<MeshRenderer>()); } }
		
		ParticleSystemRenderer _particleSystemRenderer;
		public ParticleSystemRenderer particleSystemRenderer { get { return _particleSystemRenderer ? _particleSystemRenderer : (_particleSystemRenderer = GetComponent<ParticleSystemRenderer>()); } }
		
		SpriteRenderer _spriteRenderer;
		public SpriteRenderer spriteRenderer { get { return _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<SpriteRenderer>()); } }
		
		TextMesh _textMesh;
		public TextMesh textMesh { get { return _textMesh ? _textMesh : (_textMesh = GetComponent<TextMesh>()); } }
		
		ParticleAnimator _particleAnimator;
		public ParticleAnimator particleAnimator { get { return _particleAnimator ? _particleAnimator : (_particleAnimator = GetComponent<ParticleAnimator>()); } }
		
		LODGroup _lODGroup;
		public LODGroup lODGroup { get { return _lODGroup ? _lODGroup : (_lODGroup = GetComponent<LODGroup>()); } }
		
		LightProbeGroup _lightProbeGroup;
		public LightProbeGroup lightProbeGroup { get { return _lightProbeGroup ? _lightProbeGroup : (_lightProbeGroup = GetComponent<LightProbeGroup>()); } }
		
		CircleCollider2D _circleCollider2D;
		public CircleCollider2D circleCollider2D { get { return _circleCollider2D ? _circleCollider2D : (_circleCollider2D = GetComponent<CircleCollider2D>()); } }
		
		BoxCollider2D _boxCollider2D;
		public BoxCollider2D boxCollider2D { get { return _boxCollider2D ? _boxCollider2D : (_boxCollider2D = GetComponent<BoxCollider2D>()); } }
		
		EdgeCollider2D _edgeCollider2D;
		public EdgeCollider2D edgeCollider2D { get { return _edgeCollider2D ? _edgeCollider2D : (_edgeCollider2D = GetComponent<EdgeCollider2D>()); } }
		
		PolygonCollider2D _polygonCollider2D;
		public PolygonCollider2D polygonCollider2D { get { return _polygonCollider2D ? _polygonCollider2D : (_polygonCollider2D = GetComponent<PolygonCollider2D>()); } }
		
		Joint2D _joint2D;
		public Joint2D joint2D { get { return _joint2D ? _joint2D : (_joint2D = GetComponent<Joint2D>()); } }
		
		SpringJoint2D _springJoint2D;
		public SpringJoint2D springJoint2D { get { return _springJoint2D ? _springJoint2D : (_springJoint2D = GetComponent<SpringJoint2D>()); } }
		
		DistanceJoint2D _distanceJoint2D;
		public DistanceJoint2D distanceJoint2D { get { return _distanceJoint2D ? _distanceJoint2D : (_distanceJoint2D = GetComponent<DistanceJoint2D>()); } }
		
		HingeJoint2D _hingeJoint2D;
		public HingeJoint2D hingeJoint2D { get { return _hingeJoint2D ? _hingeJoint2D : (_hingeJoint2D = GetComponent<HingeJoint2D>()); } }
		
		SliderJoint2D _sliderJoint2D;
		public SliderJoint2D sliderJoint2D { get { return _sliderJoint2D ? _sliderJoint2D : (_sliderJoint2D = GetComponent<SliderJoint2D>()); } }
		
		WheelJoint2D _wheelJoint2D;
		public WheelJoint2D wheelJoint2D { get { return _wheelJoint2D ? _wheelJoint2D : (_wheelJoint2D = GetComponent<WheelJoint2D>()); } }
		
		NavMeshAgent _navMeshAgent;
		public NavMeshAgent navMeshAgent { get { return _navMeshAgent ? _navMeshAgent : (_navMeshAgent = GetComponent<NavMeshAgent>()); } }
		
		NavMeshObstacle _navMeshObstacle;
		public NavMeshObstacle navMeshObstacle { get { return _navMeshObstacle ? _navMeshObstacle : (_navMeshObstacle = GetComponent<NavMeshObstacle>()); } }
		
		AudioListener _audioListener;
		public AudioListener audioListener { get { return _audioListener ? _audioListener : (_audioListener = GetComponent<AudioListener>()); } }
		
		AudioReverbZone _audioReverbZone;
		public AudioReverbZone audioReverbZone { get { return _audioReverbZone ? _audioReverbZone : (_audioReverbZone = GetComponent<AudioReverbZone>()); } }
		
		AudioLowPassFilter _audioLowPassFilter;
		public AudioLowPassFilter audioLowPassFilter { get { return _audioLowPassFilter ? _audioLowPassFilter : (_audioLowPassFilter = GetComponent<AudioLowPassFilter>()); } }
		
		AudioHighPassFilter _audioHighPassFilter;
		public AudioHighPassFilter audioHighPassFilter { get { return _audioHighPassFilter ? _audioHighPassFilter : (_audioHighPassFilter = GetComponent<AudioHighPassFilter>()); } }
		
		AudioDistortionFilter _audioDistortionFilter;
		public AudioDistortionFilter audioDistortionFilter { get { return _audioDistortionFilter ? _audioDistortionFilter : (_audioDistortionFilter = GetComponent<AudioDistortionFilter>()); } }
		
		AudioEchoFilter _audioEchoFilter;
		public AudioEchoFilter audioEchoFilter { get { return _audioEchoFilter ? _audioEchoFilter : (_audioEchoFilter = GetComponent<AudioEchoFilter>()); } }
		
		AudioChorusFilter _audioChorusFilter;
		public AudioChorusFilter audioChorusFilter { get { return _audioChorusFilter ? _audioChorusFilter : (_audioChorusFilter = GetComponent<AudioChorusFilter>()); } }
		
		AudioReverbFilter _audioReverbFilter;
		public AudioReverbFilter audioReverbFilter { get { return _audioReverbFilter ? _audioReverbFilter : (_audioReverbFilter = GetComponent<AudioReverbFilter>()); } }
		
		Animator _animator;
		public Animator animator { get { return _animator ? _animator : (_animator = GetComponent<Animator>()); } }
			
		Canvas _canvas;
		public Canvas canvas { get { return _canvas ? _canvas : (_canvas = GetComponent<Canvas>()); } }
		
		LensFlare _lensFlare;
		public LensFlare lensFlare { get { return _lensFlare ? _lensFlare : (_lensFlare = GetComponent<LensFlare>()); } }
		
		Projector _projector;
		public Projector projector { get { return _projector ? _projector : (_projector = GetComponent<Projector>()); } }
		
		Skybox _skybox;
		public Skybox skybox { get { return _skybox ? _skybox : (_skybox = GetComponent<Skybox>()); } }
		
		GUILayer _guiLayer;
		public GUILayer guiLayer { get { return _guiLayer ? _guiLayer : (_guiLayer = GetComponent<GUILayer>()); } }
		
		Terrain _terrain;
		public Terrain terrain { get { return _terrain ? _terrain : (_terrain = GetComponent<Terrain>()); } }
		
		RectTransform _rectTransform;
		public RectTransform rectTransform { get { return _rectTransform ? _rectTransform : (_rectTransform = GetComponent<RectTransform>()); } }
		#endregion
		
		readonly Dictionary<string, List<IEnumerator>> coroutineDict = new Dictionary<string, List<IEnumerator>>();
		readonly Dictionary<IEnumerator, CoroutineStates> coroutineStateDict = new Dictionary<IEnumerator, CoroutineStates>();

		public void StartCoroutine(string coroutineName, IEnumerator coroutine) {
			if (!coroutineDict.ContainsKey(coroutineName)) {
				coroutineDict[coroutineName] = new List<IEnumerator>();
			}
		
			coroutineDict[coroutineName].Add(coroutine);
			SetCoroutineState(coroutine, CoroutineStates.Playing);
			StartCoroutine(coroutine);
			RemoveCompletedCoroutines();
		}
	
		public void StartCoroutines(string coroutineName, params IEnumerator[] coroutines) {
			foreach (IEnumerator coroutine in coroutines) {
				StartCoroutine(coroutineName, coroutine);
			}
		}
		
		public void PauseCoroutine(string coroutineName, int index) {
			IEnumerator coroutine = GetCoroutine(coroutineName, index);
			
			if (GetCoroutineState(coroutine) == CoroutineStates.Playing) {
				SetCoroutineState(coroutine, CoroutineStates.Paused);
				StopCoroutine(coroutine);
			}
			
			RemoveCompletedCoroutines();
		}
	
		public void PauseCoroutines(string coroutineName) {
			foreach (IEnumerator coroutine in GetCoroutines(coroutineName)) {
				SetCoroutineState(coroutine, CoroutineStates.Paused);
				StopCoroutine(coroutine);
			}
			
			RemoveCompletedCoroutines();
		}
	
		public void PauseAllCoroutines() {
			foreach (string key in coroutineDict.Keys) {
				PauseCoroutines(key);
			}
		}
	
		public void PauseAllCoroutinesExcept(params string[] coroutineNames) {
			foreach (string key in coroutineDict.Keys) {
				if (!coroutineNames.Contains(key)) {
					PauseCoroutines(key);
				}
			}
		}
		
		public void ResumeCoroutine(string coroutineName, int index) {
			IEnumerator coroutine = GetCoroutine(coroutineName, index);
			
			if (GetCoroutineState(coroutine) == CoroutineStates.Paused) {
				SetCoroutineState(coroutine, CoroutineStates.Playing);
				StartCoroutine(coroutine);
			}
			
			RemoveCompletedCoroutines();
		}
	
		public void ResumeCoroutines(string coroutineName) {
			foreach (IEnumerator coroutine in GetCoroutines(coroutineName)) {
				SetCoroutineState(coroutine, CoroutineStates.Playing);
				StartCoroutine(coroutine);
			}
			
			RemoveCompletedCoroutines();
		}
	
		public void ResumeAllCoroutines() {
			foreach (string key in coroutineDict.Keys) {
				ResumeCoroutines(key);
			}
		}
	
		public void StopCoroutine(string coroutineName, int index) {
			IEnumerator coroutine = GetCoroutine(coroutineName, index);
			StopCoroutine(coroutine);
			RemoveCoroutine(coroutineName, coroutine);
			RemoveCompletedCoroutines();
		}
	
		public void StopCoroutines(string coroutineName) {
			foreach (IEnumerator coroutine in GetCoroutines(coroutineName)) {
				StopCoroutine(coroutine);
			}
			
			RemoveCoroutines(coroutineName);
			RemoveCompletedCoroutines();
		}
	
		public new void StopAllCoroutines() {
			foreach (string key in coroutineDict.Keys) {
				StopCoroutines(key);
			}
		}
	
		public void StopAllCoroutinesExcept(params string[] coroutineNames) {
			foreach (string key in coroutineDict.Keys) {
				if (!coroutineNames.Contains(key)) {
					StopCoroutines(key);
				}
			}
		}
		
		public IEnumerator GetCoroutine(string coroutineName, int index) {
			IEnumerator coroutine = null;
			
			try {
				coroutine = coroutineDict[coroutineName][index];
			}
			catch {
				Logger.LogError(string.Format("Coroutine named {0} was not found at index {1}.", coroutineName, index));
			}
			
			return coroutine;
		}
		
		public IEnumerator[] GetCoroutines(string coroutineName) {
			List<IEnumerator> coroutines = null;
			
			try {
				coroutines = coroutineDict[coroutineName];
			}
			catch {
				Logger.LogError(string.Format("Coroutines named {0} were not found.", coroutineName));
			}
			
			return coroutines.ToArray();
		}

		public Dictionary<string, IEnumerator[]> GetAllCoroutines() {
			Dictionary<string, IEnumerator[]> coroutines = new Dictionary<string, IEnumerator[]>();
			
			foreach (string key in coroutineDict.Keys) {
				coroutines[key] = coroutineDict[key].ToArray();
			}
			
			return coroutines;
		}
		
		public CoroutineStates GetCoroutineState(string coroutineName, int index) {
			return GetCoroutineState(GetCoroutine(coroutineName, index));
		}
		
		public bool CoroutineExists(string coroutineName, int index) {
			return coroutineDict.ContainsKey(coroutineName) && coroutineDict[coroutineName].Count > index;
		}
		
		public bool CoroutinesExist(string coroutineName) {
			return coroutineDict.ContainsKey(coroutineName);
		}
		
		CoroutineStates GetCoroutineState(IEnumerator coroutine) {
			CoroutineStates state = CoroutineStates.None;
			
			try {
				state = coroutineStateDict[coroutine];
			}
			catch {
				Logger.LogError(string.Format("Coroutine state was not found."));
			}
			
			return state;
		}
		
		void SetCoroutineState(IEnumerator coroutine, CoroutineStates state) {
			coroutineStateDict[coroutine] = state;
		}
		
		void RemoveCoroutine(string coroutineName, IEnumerator coroutine) {
			try {
				coroutineDict[coroutineName].Remove(coroutine);
				coroutineStateDict.Remove(coroutine);
			}
			catch {
				Logger.LogError(string.Format("Coroutine named {0} was not found.", coroutineName));
			}
		}
		
		void RemoveCoroutines(string coroutineName) {
			foreach (IEnumerator coroutine in GetCoroutines(coroutineName)) {
				RemoveCoroutine(coroutineName, coroutine);
			}
			
			coroutineDict.Remove(coroutineName);
		}
		
		void RemoveCompletedCoroutines() {
			foreach (string key in coroutineDict.Keys) {
				foreach (IEnumerator coroutine in GetCoroutines(key)) {
					if (coroutine.Current == null) {
						RemoveCoroutine(key, coroutine);
					}
				}
			}
		}
			
		public void InvokeMethod(string methodName, float delay, params object[] arguments) {
			StartCoroutine(InvokeDelayed(methodName, delay, arguments));
		}
	
		public void InvokeMethod(string methodName, params object[] arguments) {
			StartCoroutine(InvokeDelayed(methodName, 0, arguments));
		}
	
		public void InvokeMethodRepeating(string methodName, float delay, float repeatRate, params object[] arguments) {
			StartCoroutine(InvokeDelayedRepeating(methodName, delay, repeatRate, arguments));
		}
		
		public void InvokeMethodRepeating(string methodName, float repeatRate, params object[] arguments) {
			StartCoroutine(InvokeDelayedRepeating(methodName, 0, repeatRate, arguments));
		}
	
		IEnumerator InvokeDelayed(string methodName, float delay, params object[] arguments) {
			yield return new WaitForSeconds(delay);
			InvokeMethod(methodName, arguments);
		}
	
		IEnumerator InvokeDelayedRepeating(string methodName, float delay, float repeatRate, params object[] arguments) {
			yield return new WaitForSeconds(delay);
		
			while (this != null && enabled) {
				InvokeMethod(methodName, arguments);
				yield return new WaitForSeconds(repeatRate);
			}
		}
	}
}

