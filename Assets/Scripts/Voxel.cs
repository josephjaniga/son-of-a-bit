﻿using UnityEngine;
using System;

[Serializable]
public class Voxel {

	public bool state;

	public Vector2 position, xEdgePosition, yEdgePosition;

	public Voxel (int x, int y, float size) {
		position.x = (x + 0.5f) * size;
		position.y = (y + 0.5f) * size;

		xEdgePosition = position;
		xEdgePosition.x += size * 0.5f;
		yEdgePosition = position;
		yEdgePosition.y += size * 0.5f;
	}

	public Voxel () {}

	public void BecomeXDummyOf (Voxel voxel, float offset) {
		state = voxel.state;
		position = voxel.position;
		xEdgePosition = voxel.xEdgePosition;
		yEdgePosition = voxel.yEdgePosition;
		position.x += offset;
		xEdgePosition.x += offset;
		yEdgePosition.x += offset;
	}

	public void BecomeYDummyOf (Voxel voxel, float offset) {
		state = voxel.state;
		position = voxel.position;
		xEdgePosition = voxel.xEdgePosition;
		yEdgePosition = voxel.yEdgePosition;
		position.y += offset;
		xEdgePosition.y += offset;
		yEdgePosition.y += offset;
	}

	public void BecomeXYDummyOf (Voxel voxel, float offset) {
		state = voxel.state;
		position = voxel.position;
		xEdgePosition = voxel.xEdgePosition;
		yEdgePosition = voxel.yEdgePosition;
		position.x += offset;
		position.y += offset;
		xEdgePosition.x += offset;
		xEdgePosition.y += offset;
		yEdgePosition.x += offset;
		yEdgePosition.y += offset;
	}
}