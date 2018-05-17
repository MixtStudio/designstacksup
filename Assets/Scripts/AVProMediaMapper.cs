using System.Collections;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

[RequireComponent(typeof(MediaPlayer))]
public class AVProMediaMapper : MonoBehaviour {
	
	private MediaPlayer player;

	[SerializeField]
	private RenderTexture targetTexture;

	private void Awake() {
		// get the player
		player = GetComponent<MediaPlayer>();

		// neutralize the playback texture
		RenderTexture.active = targetTexture;
		GL.Clear(true, true, Color.black);
		RenderTexture.active = null;
	}

	private void FixedUpdate() {
		if (!player.Control.IsPlaying() || player.TextureProducer.GetTexture() == null) {
			return;
		}
		
		if (player.TextureProducer.RequiresVerticalFlip()) {
			// The above Blit can't flip unless using a material, so we use Graphics.DrawTexture instead
			GL.PushMatrix();
			RenderTexture.active = targetTexture;
			GL.LoadPixelMatrix(0f, targetTexture.width, 0f, targetTexture.height);
			Rect sourceRect = new Rect(0f, 0f, 1f, 1f);
			Rect destRect = new Rect(0f, 0f, targetTexture.width, targetTexture.height);

			Graphics.DrawTexture(destRect, player.TextureProducer.GetTexture(), sourceRect, 0, 0, 0, 0);
			GL.PopMatrix();
			GL.InvalidateState();
		} else {
			Graphics.Blit(player.TextureProducer.GetTexture(), targetTexture);
		}

	}

}
