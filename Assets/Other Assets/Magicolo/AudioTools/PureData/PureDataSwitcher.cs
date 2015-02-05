using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public static class PureDataSwitcher {

		public static void Switch(PureData source, PureData target) {
			PureDataSpatializerManager.Switch(source.spatializerManager, target.spatializerManager);
			PureDataContainerManager.Switch(source.containerManager, target.containerManager);
			PureDataInfoManager.Switch(source.infoManager, target.infoManager);
			source.clipManager.InitializeClips();
		}
	}
}

