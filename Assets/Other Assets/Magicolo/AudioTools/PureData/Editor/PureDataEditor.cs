using System;
using System.Collections;
using System.Reflection;
using Magicolo.EditorTools;
using Magicolo.GeneralTools;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Magicolo.AudioTools {
	[CustomEditor(typeof(PureData))]
	public class PureDataEditor : CustomEditorBase {
		
		PureData pureData;
		
		PureDataGeneralSettings generalSettings;
		SerializedObject generalSettingsSerialized;
		
		PureDataBusManager busManager;
		SerializedObject busManagerSerialized;
		SerializedProperty busesProperty;
		PureDataBus currentBus;
		SerializedProperty currentBusProperty;
		
		PureDataSpatializerManager spatializerManager;
		SerializedObject spatializerManagerSerialized;
		SerializedProperty spatializersProperty;
		PureDataSpatializer currentSpatializer;
		SerializedProperty currentSpatializerProperty;
		
		PureDataContainerManager containerManager;
		SerializedObject containerManagerSerialized;
		SerializedProperty containersProperty;
		PureDataContainer currentContainer;
		SerializedProperty currentContainerProperty;
		SerializedProperty subContainersProperty;
		PureDataSubContainer currentSubContainer;
		int currentSubContainerIndex;
		SerializedProperty currentSubContainerProperty;
		PureDataOption currentOption;
		SerializedProperty currentOptionProperty;
		
		string[] sampleRates = { "44 100 Hz", "48 000 Hz", "88 200 Hz", "96 000 Hz" };
		
		public override void OnEnable() {
			base.OnEnable();
			
			pureData = (PureData)target;
			pureData.SetExecutionOrder(-15);
		}
		
		public override void OnInspectorGUI() {
			pureData.InitializeSettings();
			
			generalSettings = pureData.generalSettings;
			generalSettingsSerialized = new SerializedObject(generalSettings);
			busManager = pureData.busManager;
			busManagerSerialized = new SerializedObject(busManager);
			spatializerManager = pureData.spatializerManager;
			spatializerManagerSerialized = new SerializedObject(spatializerManager);
			containerManager = pureData.containerManager;
			containerManagerSerialized = new SerializedObject(containerManager);
				
			Begin();
			
			ShowGeneralSettings();
			Separator();
			ShowBuses();
			ShowSpatializers();
			ShowContainers();
			Separator();
			
			End();
			
			generalSettingsSerialized.ApplyModifiedProperties();
			busManagerSerialized.ApplyModifiedProperties();
			spatializerManagerSerialized.ApplyModifiedProperties();
			containerManagerSerialized.ApplyModifiedProperties();
		}

		void ShowGeneralSettings() {
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			
			FolderPathButton(generalSettingsSerialized.FindProperty("patchesPath"), Application.streamingAssetsPath + "/", new GUIContent("Patches Path", "The path where the Pure Data patches are relative to Assets/StreamingAssets/."));
			
			EditorGUI.BeginChangeCheck();
			string sampleRateFormatted = generalSettings.SampleRate.ToString().Substring(0, 2) + " " + generalSettings.SampleRate.ToString().Substring(2, 3) + " Hz";
			int currentSampleRate = int.Parse(Popup(sampleRateFormatted, sampleRates, new GUIContent("Sample Rate", "The output sample rate. Be sure that the sample rate of all imported sounds correspond to this value.")).Replace(" ", "").Substring(0, 5));
			if (EditorGUI.EndChangeCheck()) {
				generalSettings.SampleRate = currentSampleRate;
				generalSettingsSerialized.Update();
			}
			
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(generalSettingsSerialized.FindProperty("maxVoices"), new GUIContent("Max Voices", "Sets the maximum simultaneous sources that can be played. The higher this value is, the longer it will take to initialize Pure Data."));
			if (EditorGUI.EndChangeCheck()) {
				pureData.sourceManager.UpdateSourceContainer();
			}
			EditorGUI.EndDisabledGroup();
			
			EditorGUILayout.PropertyField(generalSettingsSerialized.FindProperty("speedOfSound"), new GUIContent("Speed Of Sound", "Sets the speed of sound for doppler effects calculations."));
			EditorGUILayout.PropertyField(generalSettingsSerialized.FindProperty("masterVolume"), new GUIContent("Master Volume", "Controls the volume of all Pure Data sources."));
		}
		
		#region Buses
		void ShowBuses() {
			busesProperty = busManagerSerialized.FindProperty("buses");
			
			if (AddFoldOut(busesProperty, "Buses".ToGUIContent())) {
				busManager.buses[busManager.buses.Length - 1] = new PureDataBus(pureData);
				busManager.buses[busManager.buses.Length - 1].SetUniqueName("default", "", busManager.buses);
				busManager.UpdateMixer();
			}
			
			if (busesProperty.isExpanded) {
				if (busesProperty.arraySize > 0) {
					EditorGUILayout.HelpBox("Be sure to include exactly one [umixer~] object in your main Pure Data patch and to reload it each time you make modifications to the buses.", MessageType.Info);
				}
				
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < busesProperty.arraySize; i++) {
					currentBus = busManager.buses[i];
					currentBusProperty = busesProperty.GetArrayElementAtIndex(i);
					
					BeginBox();
					
					if (BusDeleteFoldout(i)) {
						busManager.UpdateMixer();
						break;
					}
					
					ShowBus();
					
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		bool BusDeleteFoldout(int index) {
			EditorGUILayout.BeginHorizontal();
			
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;
			
			Foldout(currentBusProperty, currentBus.Name.ToGUIContent(), style);
			if (!currentBusProperty.isExpanded) {
				EditorGUILayout.PropertyField(currentBusProperty.FindPropertyRelative("volume"), GUIContent.none);
			}
			bool pressed = DeleteButton(busesProperty, index);
			
			EditorGUILayout.EndHorizontal();
			
			if (!pressed) {
				Reorderable(busesProperty, index, true, OnBusReorder);
			}
			
			return pressed;
		}
		
		void ShowBus() {
			if (currentBusProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
				EditorGUI.BeginChangeCheck();
				string busName = EditorGUILayout.TextField("Name", currentBus.Name);
				if (EditorGUI.EndChangeCheck()) {
					currentBus.SetUniqueName(busName, currentBus.Name, busManager.buses);
					busManager.UpdateMixer();
				}
				EditorGUI.EndDisabledGroup();
				
				EditorGUILayout.PropertyField(currentBusProperty.FindPropertyRelative("volume"));
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void OnBusReorder(SerializedProperty arrayProperty, int sourceIndex, int targetIndex) {
			ReorderArray(arrayProperty, sourceIndex, targetIndex);
			busManager.UpdateMixer();
		}
		#endregion
		
		#region Spatializers
		void ShowSpatializers() {
			spatializersProperty = spatializerManagerSerialized.FindProperty("spatializers");
			
			if (AddFoldOut(spatializersProperty, "Spatializers".ToGUIContent())) {
				spatializerManager.spatializers[spatializerManager.spatializers.Length - 1] = new PureDataSpatializer("", pureData);
				spatializerManager.spatializers[spatializerManager.spatializers.Length - 1].SetUniqueName("default", "", spatializerManager.spatializers);
			}
			
			if (spatializersProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < spatializersProperty.arraySize; i++) {
					currentSpatializer = spatializerManager.spatializers[i];
					currentSpatializerProperty = spatializersProperty.GetArrayElementAtIndex(i);
					
					BeginBox();
					
					GUIStyle style = new GUIStyle("foldout");
					style.fontStyle = FontStyle.Bold;
			
					if (DeleteFoldOut(spatializersProperty, i, currentSpatializer.Name.ToGUIContent(), style)) {
						break;
					}
					currentSpatializer.Showing = currentSpatializerProperty.isExpanded;
					
					ShowSpatializer();
					
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void OnSpatializersDropped(PureDataSpatializerManager droppedObject) {
			if (droppedObject != null) {
				PureDataSpatializerManager.Switch(spatializerManager, droppedObject);
			}
		}
		
		void ShowSpatializer() {
			if (currentSpatializerProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
				EditorGUI.BeginChangeCheck();
				string spatializerName = EditorGUILayout.TextField("Name", currentSpatializer.Name);
				if (EditorGUI.EndChangeCheck()) {
					currentSpatializer.SetUniqueName(spatializerName, currentSpatializer.Name, spatializerManager.spatializers);
				}
				EditorGUI.EndDisabledGroup();
				
				currentSpatializer.Source = EditorGUILayout.ObjectField("Source".ToGUIContent(), currentSpatializer.Source, typeof(GameObject), true) as GameObject;
				EditorGUILayout.PropertyField(currentSpatializerProperty.FindPropertyRelative("volumeRolloffMode"));
				EditorGUILayout.PropertyField(currentSpatializerProperty.FindPropertyRelative("minDistance"));
				EditorGUILayout.PropertyField(currentSpatializerProperty.FindPropertyRelative("maxDistance"));
				EditorGUILayout.PropertyField(currentSpatializerProperty.FindPropertyRelative("panLevel"));
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		#endregion
		
		#region Containers
		void ShowContainers() {
			containersProperty = containerManagerSerialized.FindProperty("containers");
		
			if (AddFoldOut(containersProperty, "Containers".ToGUIContent())) {
				containerManager.containers[containerManager.containers.Length - 1] = new PureDataContainer("", pureData);
				containerManager.containers[containerManager.containers.Length - 1].SetUniqueName("default", containerManager.containers);
			}
		
			if (containersProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				for (int i = 0; i < containerManager.containers.Length; i++) {
					currentContainer = containerManager.containers[i];
					currentContainerProperty = containersProperty.GetArrayElementAtIndex(i);
		
					BeginBox();
		
					if (DeleteFoldOut(containersProperty, i, currentContainer.Name.ToGUIContent(), GetContainerStyle(currentContainer.type.ConvertByName<PureDataSubContainer.Types>()))) {
						break;
					}
		
					ShowContainer();
		
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowContainer() {
			subContainersProperty = currentContainerProperty.FindPropertyRelative("subContainers");
		
			if (currentContainerProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
				EditorGUI.BeginChangeCheck();
				string containerName = EditorGUILayout.TextField(currentContainer.Name);
				if (EditorGUI.EndChangeCheck()) {
					currentContainer.SetUniqueName(containerName, currentContainer.Name, "default", containerManager.containers);
				}
				EditorGUI.EndDisabledGroup();
				
				currentContainer.type = (PureDataContainer.Types)EditorGUILayout.EnumPopup(currentContainer.type);
		
				BeginBox();
		
				if (currentContainer.type == PureDataContainer.Types.SwitchContainer) {
					ShowSwitchContainerEnums(currentContainerProperty, currentContainer.switchSettings);
				}
		
				ShowSources();
		
				if (subContainersProperty.isExpanded) {
					EditorGUI.indentLevel += 1;
		
					ShowSubContainers(currentContainer.childrenIds);
		
					EditorGUI.indentLevel -= 1;
				}
		
				EndBox();
		
				Separator();
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowSources() {
			if (AddFoldOut<PureDataSetup>(subContainersProperty, currentContainer.childrenIds.Count, "Sources".ToGUIContent(), OnContainerSourceDropped)) {
				if (currentContainer.childrenIds.Count > 0) {
					currentContainer.subContainers[currentContainer.subContainers.Count - 1] = new PureDataSubContainer(currentContainer, 0, currentContainer.GetSubContainerWithID(currentContainer.childrenIds.Last()), pureData);
				}
				else {
					currentContainer.subContainers[currentContainer.subContainers.Count - 1] = new PureDataSubContainer(currentContainer, 0, pureData);
				}
				containerManagerSerialized.Update();
			}
		}
		
		void OnContainerSourceDropped(PureDataSetup droppedObject) {
			AddToArray(subContainersProperty);
			currentContainer.subContainers[currentContainer.subContainers.Count - 1] = new PureDataSubContainer(currentContainer, 0, droppedObject, pureData);
			containerManagerSerialized.Update();
		}
		
		void ShowSubContainers(List<int> ids) {
			for (int i = 0; i < ids.Count; i++) {
				currentSubContainer = currentContainer.GetSubContainerWithID(ids[i]);
				currentSubContainerIndex = currentContainer.subContainers.IndexOf(currentSubContainer);
				currentSubContainerProperty = subContainersProperty.GetArrayElementAtIndex(currentSubContainerIndex);
		
				if (DeleteFoldOut<PureDataSetup>(subContainersProperty, currentSubContainerIndex, currentSubContainer.Name.ToGUIContent(), GetContainerStyle(currentSubContainer.type), OnSubContainerDropped, OnSubContainerReorder)) {
					currentSubContainer.Remove(currentContainer);
					break;
				}
		
				ShowSubContainer();
			}
		}
		
		void OnSubContainerDropped(PureDataSetup droppedObject) {
			if (currentSubContainer.type == PureDataSubContainer.Types.AudioSource) {
				currentSubContainer.Setup = droppedObject;
				containerManagerSerialized.Update();
			}
			else {
				OnSubContainerSourceDropped(droppedObject);
			}
		}
		
		void OnSubContainerReorder(SerializedProperty arrayProperty, int sourceIndex, int targetIndex) {
			PureDataSubContainer sourceSubContainer = currentContainer.subContainers[sourceIndex];
			PureDataSubContainer targetSubContainer = currentContainer.subContainers[targetIndex];
			List<int> sourceParentIds = sourceSubContainer.parentId == 0 ? currentContainer.childrenIds : currentContainer.GetSubContainerWithID(sourceSubContainer.parentId).childrenIds;
			List<int> targetParentIds = targetSubContainer.parentId == 0 ? currentContainer.childrenIds : currentContainer.GetSubContainerWithID(targetSubContainer.parentId).childrenIds;
		
			if (sourceParentIds == targetParentIds) {
				int sourceSubContainerIndex = sourceParentIds.IndexOf(sourceSubContainer.id);
				int targetSubContainerIndex = targetParentIds.IndexOf(targetSubContainer.id);
				sourceParentIds.Move(sourceSubContainerIndex, targetSubContainerIndex);
			}
			else {
				int targetSubContainerIndex = targetParentIds.IndexOf(targetSubContainer.id);
				targetParentIds.Insert(targetSubContainerIndex, sourceSubContainer.id);
				sourceParentIds.Remove(sourceSubContainer.id);
				sourceSubContainer.parentId = targetSubContainer.parentId;
			}
			containerManagerSerialized.Update();
		}
		
		void ShowSubContainer() {
			AdjustContainerName();
		
			if (currentSubContainerProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				ShowGeneralContainerSettings();
		
				if (currentSubContainer.IsSource) {
					switch (currentSubContainer.type) {
						case PureDataSubContainer.Types.AudioSource:
							ShowAudioSource();
							break;
					}
				}
				else {
					BeginBox();
		
					switch (currentSubContainer.type) {
						case PureDataSubContainer.Types.MixContainer:
							ShowMixContainer();
							break;
						case PureDataSubContainer.Types.RandomContainer:
							ShowRandomContainer();
							break;
						case PureDataSubContainer.Types.SwitchContainer:
							ShowSwitchContainer();
							break;
					}
		
					EndBox();
		
					Separator();
				}
		
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowAudioSource() {
			currentSubContainer.Setup = EditorGUILayout.ObjectField("Source".ToGUIContent(), currentSubContainer.Setup, typeof(PureDataSetup), true) as PureDataSetup;
			ContextMenu(new []{ "Clear".ToGUIContent() }, new GenericMenu.MenuFunction2[]{ OnAudioSourceCleared }, new object[]{ currentSubContainer });
		
			if (currentSubContainer.Setup != null && currentSubContainer.Setup.Info != null) {
				ShowOptions();
			}
		}
		
		void OnAudioSourceCleared(object data) {
			PureDataSubContainer subContainer = data as PureDataSubContainer;
			subContainer.Setup = null;
			subContainer.infoName = "";
			containerManagerSerialized.Update();
		}
		
		void ShowMixContainer() {
			ShowSubSourcesAddFoldout();
		
			if (currentSubContainer.Showing && currentContainer.childrenIds.Count > 0) {
				EditorGUI.indentLevel += 1;
		
				ShowSubContainers(currentSubContainer.childrenIds);
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowRandomContainer() {
			ShowSubSourcesAddFoldout();
		
			if (currentSubContainer.Showing && currentContainer.childrenIds.Count > 0) {
				EditorGUI.indentLevel += 1;
		
				ShowSubContainers(currentSubContainer.childrenIds);
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowSwitchContainer() {
			ShowSwitchContainerEnums(currentSubContainerProperty, currentSubContainer.switchSettings);
		
			ShowSubSourcesAddFoldout();
		
			if (currentSubContainer.Showing && currentContainer.childrenIds.Count > 0) {
				EditorGUI.indentLevel += 1;
		
				ShowSubContainers(currentSubContainer.childrenIds);
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowSwitchContainerEnums(SerializedProperty property, PureDataSwitchSettings switchSettings) {
			Rect rect = EditorGUILayout.BeginHorizontal();
			
			// State holder
			EditorGUI.BeginChangeCheck();
			
			switchSettings.StateHolder = EditorGUILayout.ObjectField("State Holder".ToGUIContent(), switchSettings.StateHolderObject, typeof(UnityEngine.Object), true, GUILayout.MaxWidth(switchSettings.StateHolderObject == null ? Screen.width : Screen.width / 1.6F));
			
			// Component list
			if (switchSettings.StateHolderObject != null) {
				List<string> componentNames = new List<string>{ "GameObject" };
				Component[] components = switchSettings.StateHolderObject.GetComponents<Component>();
				
				foreach (Component component in components) {
					componentNames.Add(component.GetType().Name);
				}
		
				float width = Mathf.Min(92 + EditorGUI.indentLevel * 16, EditorGUIUtility.currentViewWidth / 5 + EditorGUI.indentLevel * 16);
				int index = EditorGUI.Popup(new Rect(rect.x + rect.width - width, rect.y, width, rect.height), Array.IndexOf(components, switchSettings.StateHolderComponent) + 1, componentNames.ToArray());
				
				switchSettings.StateHolderComponent = index > 0 ? components[index - 1] : null;
			}
			
			EditorGUILayout.EndHorizontal();
		
			// State field
			if (switchSettings.StateHolder != null) {
				string[] enumNames = switchSettings.StateHolder.GetFieldsPropertiesNames(ObjectExtensions.AllPublicFlags, typeof(Enum));
		
				if (enumNames.Length > 0) {
					int index = Mathf.Max(Array.IndexOf(enumNames, switchSettings.statePath), 0);
					index = EditorGUILayout.Popup("State Field", index, enumNames);
					switchSettings.statePath = enumNames[Mathf.Clamp(index, 0, Mathf.Max(enumNames.Length - 1, 0))];
				}
				else {
					EditorGUILayout.Popup("State Field", 0, new string[0]);
				}
			}
			
			if (EditorGUI.EndChangeCheck()) {
				property.serializedObject.Update();
			}
		}
		
		void OnSubContainerSourceDropped(PureDataSetup droppedObject) {
			currentContainer.subContainers.Add(new PureDataSubContainer(currentContainer, currentSubContainer.id, droppedObject, pureData));
			containerManagerSerialized.Update();
		}
		
		void OnSubContainerSourceAdded(SerializedProperty arrayProperty) {
			if (currentSubContainer.childrenIds.Count > 0) {
				currentContainer.subContainers.Add(new PureDataSubContainer(currentContainer, currentSubContainer.id, currentContainer.GetSubContainerWithID(currentSubContainer.childrenIds.Last()), pureData));
			}
			else {
				currentContainer.subContainers.Add(new PureDataSubContainer(currentContainer, currentSubContainer.id, pureData));
			}
			containerManagerSerialized.Update();
		}
		
		void ShowGeneralContainerSettings() {
			currentSubContainer.type = (PureDataSubContainer.Types)EditorGUILayout.EnumPopup(currentSubContainer.type);
		
			if (GetParentContainerType(currentSubContainer, currentContainer) == PureDataSubContainer.Types.RandomContainer) {
				EditorGUILayout.PropertyField(currentSubContainerProperty.FindPropertyRelative("weight"));
			}
			else if (GetParentContainerType(currentSubContainer, currentContainer) == PureDataSubContainer.Types.SwitchContainer) {
				PureDataSwitchSettings parentSwitchSettings = currentSubContainer.parentId == 0 ? currentContainer.switchSettings : currentContainer.GetSubContainerWithID(currentSubContainer.parentId).switchSettings;
				PureDataSwitchSettings switchSettings = currentSubContainer.switchSettings;
		
				if (parentSwitchSettings.StateHolder != null && !string.IsNullOrEmpty(parentSwitchSettings.statePath)) {
					FieldInfo enumField = parentSwitchSettings.StateHolder.GetType().GetField(parentSwitchSettings.statePath, ObjectExtensions.AllPublicFlags);
					PropertyInfo enumProperty = parentSwitchSettings.StateHolder.GetType().GetProperty(parentSwitchSettings.statePath, ObjectExtensions.AllPublicFlags);
					Type enumType = enumField == null ? enumProperty == null ? null : enumProperty.PropertyType : enumField.FieldType;
		
					if (enumType != null) {
						string[] enumNames = Enum.GetNames(enumType);
						Enum defaultState = (Enum)Enum.Parse(enumType, enumNames[0]);
						Enum selectedState = Enum.GetNames(enumType).Contains(switchSettings.stateName) ? (Enum)Enum.Parse(enumType, switchSettings.stateName) : null;
						Enum selectedEnum = selectedState == null ? EditorGUILayout.EnumPopup("State", defaultState) : EditorGUILayout.EnumPopup("State", selectedState);
						
						switchSettings.stateName = selectedEnum.ToString();
						switchSettings.stateIndex = selectedEnum.GetHashCode();
						
						return;
					}
				}
		
				EditorGUILayout.Popup("State", 0, new string[0]);
			}
		}
		
		void ShowOptions() {
			SerializedProperty optionsProperty = currentSubContainerProperty.FindPropertyRelative("options");
		
			if (AddFoldOut(optionsProperty, "Options".ToGUIContent())) {
				optionsProperty.GetLastArrayElement().isExpanded = true;
				containerManagerSerialized.ApplyModifiedProperties();
				currentSubContainer.options[currentSubContainer.options.Length - 1].SetDefaultValue();
			}
		
			if (currentSubContainer.options != null && optionsProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				for (int i = 0; i < currentSubContainer.options.Length; i++) {
					currentOption = currentSubContainer.options[i];
					currentOptionProperty = optionsProperty.GetArrayElementAtIndex(i);
		
					BeginBox();
					
					GUIStyle style = new GUIStyle("foldout");
					style.fontStyle = FontStyle.Bold;
					if (DeleteFoldOut(optionsProperty, i, string.Format("{0} | {1}", currentOption.type, currentOption.GetValueDisplayName()).ToGUIContent(), style)) {
						break;
					}
					
					ShowOption();
					EndBox();
				}
		
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowOption() {
			if (currentOptionProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(currentOptionProperty.FindPropertyRelative("type"));
				if (EditorGUI.EndChangeCheck()) {
					containerManagerSerialized.ApplyModifiedProperties();
					currentOption.SetDefaultValue();
				}
		
				// Float fields
				if (currentOption.type == PureDataOption.OptionTypes.FadeIn || currentOption.type == PureDataOption.OptionTypes.FadeOut || currentOption.type == PureDataOption.OptionTypes.Volume || currentOption.type == PureDataOption.OptionTypes.MinDistance || currentOption.type == PureDataOption.OptionTypes.MaxDistance) {
					currentOption.SetValue(Mathf.Max(EditorGUILayout.FloatField("Value".ToGUIContent(), currentOption.GetValue<float>()), 0));
				}
				else if (currentOption.type == PureDataOption.OptionTypes.Pitch) {
					currentOption.SetValue(EditorGUILayout.FloatField("Value".ToGUIContent(), currentOption.GetValue<float>()));
				}
				// Slider fields
				else if (currentOption.type == PureDataOption.OptionTypes.RandomVolume || currentOption.type == PureDataOption.OptionTypes.RandomPitch || currentOption.type == PureDataOption.OptionTypes.PanLevel || currentOption.type == PureDataOption.OptionTypes.Time) {
					currentOption.SetValue(EditorGUILayout.Slider("Value".ToGUIContent(), currentOption.GetValue<float>(), 0, 1));
				}
				else if (currentOption.type == PureDataOption.OptionTypes.DopplerLevel) {
					currentOption.SetValue(EditorGUILayout.Slider("Value".ToGUIContent(), currentOption.GetValue<float>(), 0, 10));
				}
				// Min max slider fields
				else if (currentOption.type == PureDataOption.OptionTypes.PlayRange) {
					Vector2 playRange = currentOption.GetValue<Vector2>();
					EditorGUILayout.MinMaxSlider("Value".ToGUIContent(), ref playRange.x, ref playRange.y, 0, 1);
					playRange.x = float.IsNaN(playRange.x) ? 0 : Mathf.Clamp(playRange.x, 0, playRange.y);
					playRange.y = float.IsNaN(playRange.y) ? 1 : Mathf.Clamp(playRange.y, playRange.x, 1);
					currentOption.SetValue(playRange);
				}
				// Bool fields
				else if (currentOption.IsBool) {
					currentOption.SetValue(EditorGUILayout.Toggle("Value".ToGUIContent(), currentOption.GetValue<bool>()));
				}
				// Enum fields
				else if (currentOption.type == PureDataOption.OptionTypes.Output) {
					ShowOutput();
				}
				else if (currentOption.IsVolumeRolloffMode) {
					currentOption.SetValue((PureDataVolumeRolloffModes)EditorGUILayout.EnumPopup("Value".ToGUIContent(), currentOption.GetValue<PureDataVolumeRolloffModes>()));
				}
				// Audio clip fields
				else if (currentOption.IsClip) {
					currentOption.SetValue((AudioClip)EditorGUILayout.ObjectField("Value".ToGUIContent(), currentOption.GetValue<AudioClip>(), typeof(AudioClip), true));
				}
		
				EditorGUI.indentLevel -= 1;
			}
		}
			
		void ShowOutput() {
			List<string> options = new List<string>();
			options.Add("Master");
			options.AddRange(pureData.busManager.buses.GetNames());
			currentOption.SetValue(Popup(currentOption.GetValue<string>(), options.ToArray(), "Output".ToGUIContent()));
		}
		
		void ShowSubSourcesAddFoldout() {
			AddFoldOut<PureDataSetup>(subContainersProperty, currentSubContainer, currentSubContainer.childrenIds.Count, "Sources".ToGUIContent(), OnSubContainerSourceDropped, OnSubContainerSourceAdded);
		}
		
		void AdjustContainerName() {
			switch (currentSubContainer.type) {
				case PureDataSubContainer.Types.AudioSource:
					if (currentSubContainer.Setup == null) {
						AdjustName("Audio Source: null", currentSubContainer, currentContainer);
					}
					else {
						AdjustName("Audio Source: " + currentSubContainer.Setup.name, currentSubContainer, currentContainer);
					}
					break;
				case PureDataSubContainer.Types.MixContainer:
					AdjustName("Mix Container", currentSubContainer, currentContainer);
					break;
				case PureDataSubContainer.Types.RandomContainer:
					AdjustName("Random Container", currentSubContainer, currentContainer);
					break;
				case PureDataSubContainer.Types.SwitchContainer:
					AdjustName("Switch Container", currentSubContainer, currentContainer);
					break;
			}
		}
		
		void AdjustName(string prefix, PureDataSubContainer subContainer, PureDataContainer container) {
			subContainer.Name = prefix;
		
			if (subContainer.IsContainer) {
				subContainer.Name += string.Format(" ({0})", subContainer.childrenIds.Count);
			}
		
			if (GetParentContainerType(subContainer, container) == PureDataSubContainer.Types.RandomContainer) {
				subContainer.Name += " | Weight: " + subContainer.weight;
			}
			else if (GetParentContainerType(subContainer, container) == PureDataSubContainer.Types.SwitchContainer) {
				subContainer.Name += " | State: " + subContainer.switchSettings.stateName;
			}
		}
		
		GUIStyle GetContainerStyle(PureDataSubContainer.Types type) {
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;
			Color textColor = style.normal.textColor;
		
			switch (type) {
				case PureDataSubContainer.Types.AudioSource:
					textColor = new Color(1, 0.5F, 0.3F, 10);
					break;
				case PureDataSubContainer.Types.MixContainer:
					textColor = new Color(0, 1, 1, 10);
					break;
				case PureDataSubContainer.Types.RandomContainer:
					textColor = new Color(1, 1, 0, 10);
					break;
				case PureDataSubContainer.Types.SwitchContainer:
					textColor = new Color(0.5F, 1, 0.3F, 10);
					break;
			}
		
			style.normal.textColor = textColor * 0.7F;
			style.onNormal.textColor = textColor * 0.7F;
			style.focused.textColor = textColor * 0.85F;
			style.onFocused.textColor = textColor * 0.85F;
			style.active.textColor = textColor * 0.85F;
			style.onActive.textColor = textColor * 0.85F;
		
			return style;
		}
		
		PureDataSubContainer.Types GetParentContainerType(PureDataSubContainer subContainer, PureDataContainer container) {
			PureDataSubContainer.Types type = PureDataSubContainer.Types.AudioSource;
		
			if (subContainer.parentId != 0) {
				type = container.GetSubContainerWithID(subContainer.parentId).type;
			}
			else {
				type = container.type.ConvertByName<PureDataSubContainer.Types>();
			}
		
			return type;
		}
		#endregion
	}
}