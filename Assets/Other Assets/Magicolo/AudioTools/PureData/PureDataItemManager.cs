using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using LibPDBinding;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataItemManager {
		
		public PureData pureData;
		
		public PureDataItemManager(PureData pureData) {
			this.pureData = pureData;
		}

		public PureDataItem Play(string soundName, object source, float delay, params PureDataOption[] options) {
			PureDataSingleItem item = GetPureDataSingleItem(soundName, source);
			item.ApplyOptions(options);
			item.Play(delay);
			return item;
		}

		public PureDataItem PlayContainer(string containerName, object source, float delay, params PureDataOption[] options) {
			PureDataMultipleItem item = GetPureDataMultipleItem(containerName, source);
			item.ApplyOptions(options);
			item.Play(delay);
			return item;
		}
	
		public PureDataSingleItem GetPureDataSingleItem(string soundName, object source) {
			return new PureDataSingleItem(pureData.sourceManager.GetSource(soundName, source), pureData);
		}
				
		public PureDataMultipleItem GetPureDataMultipleItem(string containerName, object source) {
			return GetPureDataMultipleItem(pureData.containerManager.GetContainer(containerName), source);
		}
		
		public PureDataMultipleItem GetPureDataMultipleItem(PureDataContainer container, object source) {
			PureDataMultipleItem multipleItem;
			
			switch (container.type) {
				case PureDataContainer.Types.RandomContainer:
					multipleItem = GetPureDataRandomItem(container, container.childrenIds, source);
					break;
				case PureDataContainer.Types.SwitchContainer:
					multipleItem = GetPureDataSwitchItem(container, container.childrenIds, source);
					break;
				default:
					multipleItem = GetPureDataMixItem(container, container.childrenIds, source);
					break;
			}
			return multipleItem;
		}
		
		public PureDataItem GetSubContainerAudioItem(PureDataContainer container, PureDataSubContainer subContainer, object source) {
			PureDataItem item = null;
			
			if (subContainer.IsSource) {
				item = GetPureDataSourceItem(subContainer, source);
			}
			else {
				item = GetPureDataContainerItem(container, subContainer, source);
			}
			return item;
		}

		public PureDataSingleItem GetPureDataSourceItem(PureDataSubContainer subContainer, object source) {
			PureDataSingleItem sourceAudioItem = null;
			
			switch (subContainer.type) {
				default:
					sourceAudioItem = GetPureDataSingleItem(subContainer.infoName, source);
					sourceAudioItem.ApplyOptions(subContainer.options);
					break;
			}
			return sourceAudioItem;
		}
		
		public PureDataMultipleItem GetPureDataContainerItem(PureDataContainer container, PureDataSubContainer subContainer, object source) {
			PureDataMultipleItem multipleAudioItem = null;
			
			switch (subContainer.type) {
				case PureDataSubContainer.Types.RandomContainer:
					multipleAudioItem = GetPureDataRandomItem(container, subContainer.childrenIds, source);
					break;
				case PureDataSubContainer.Types.SwitchContainer:
					multipleAudioItem = GetPureDataSwitchItem(container, subContainer.childrenIds, source);
					break;
				default:
					multipleAudioItem = GetPureDataMixItem(container, subContainer.childrenIds, source);
					break;
			}
			return multipleAudioItem;
		}
		
		public PureDataMultipleItem GetPureDataMixItem(PureDataContainer container, List<int> childrenIds, object source) {
			PureDataMultipleItem mixAudioItem = new PureDataMultipleItem(container.Name, pureData);
			
			foreach (int childrenId in childrenIds) {
				PureDataItem childItem = GetSubContainerAudioItem(container, container.GetSubContainerWithID(childrenId), source);
				if (childItem != null) {
					mixAudioItem.AddItem(childItem);
				}
			}
			
			return mixAudioItem;
		}
		
		public PureDataMultipleItem GetPureDataRandomItem(PureDataContainer container, List<int> childrenIds, object source) {
			PureDataMultipleItem randomAudioItem = new PureDataMultipleItem(container.Name, pureData);
			List<PureDataSubContainer> childcontainers = new List<PureDataSubContainer>();
			List<float> weights = new List<float>();
			
			for (int i = 0; i < childrenIds.Count; i++) {
				PureDataSubContainer childContainer = container.GetSubContainerWithID(childrenIds[i]);
				if (childContainer != null) {
					childcontainers.Add(childContainer);
					weights.Add(childContainer.weight);
				}
			}
			
			PureDataSubContainer randomChildContainer = HelperFunctions.WeightedRandom(childcontainers, weights);
			if (randomAudioItem != null) {
				PureDataItem childAudioItem = GetSubContainerAudioItem(container, randomChildContainer, source);
				if (childAudioItem != null) {
					randomAudioItem.AddItem(childAudioItem);
				}
			}
			
			return randomAudioItem;
		}
		
		public PureDataMultipleItem GetPureDataSwitchItem(PureDataContainer container, List<int> childrenIds, object source) {
			PureDataMultipleItem switchAudioItem = new PureDataMultipleItem(container.Name, pureData);
			int stateIndex = int.MinValue;
			PureDataSubContainer[] childrenSubContainers = container.IdsToSubContainers(childrenIds);
			
			if (childrenSubContainers[0].parentId == 0) {
				stateIndex = container.switchSettings.GetCurrentStateIndex();
			}
			else {
				PureDataSubContainer parentSubContainer = container.GetSubContainerWithID(childrenSubContainers[0].parentId);
				stateIndex = parentSubContainer.switchSettings.GetCurrentStateIndex();
			}
			
			if (stateIndex != int.MinValue) {
				foreach (PureDataSubContainer childSubContainer in childrenSubContainers) {
					if (childSubContainer.switchSettings.stateIndex == stateIndex) {
						PureDataItem childAudioItem = GetSubContainerAudioItem(container, childSubContainer, source);
						
						if (childAudioItem != null) {
							switchAudioItem.AddItem(childAudioItem);
						}
					}
				}
			}
			
			return switchAudioItem;
		}
	}
}
