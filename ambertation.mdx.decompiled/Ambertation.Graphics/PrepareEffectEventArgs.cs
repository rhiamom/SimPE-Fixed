using System;

namespace Ambertation.Graphics;

public class PrepareEffectEventArgs : EventArgs
{
	private MeshBox mb;

	public MeshBox MeshBox => mb;

	internal PrepareEffectEventArgs(MeshBox mb)
	{
		this.mb = mb;
	}
}
