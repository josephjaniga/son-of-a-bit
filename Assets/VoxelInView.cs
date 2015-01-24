using UnityEngine;
using System.Collections;

public class VoxelInView : MonoBehaviour {

	void OnBecameVisible() {
        enabled = true;
    }

    void OnBecameInvisible() {
        enabled = false;
    }

}
