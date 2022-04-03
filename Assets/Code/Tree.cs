using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    private int hits = 0;

    public void Interact(Controller controller) {
        hits++;
        SpawnParticles(controller.GetPointerPosition());
        if (hits > 2) CutDown();
    }

    private void SpawnParticles(Vector3 pos) {
        var thing = Resources.Load<GameObject>("Tree Break Particles (0)");
        Instantiate(thing, transform.parent).transform.position = pos;
    }

    private void CutDown() {
        var thing = Resources.Load<GameObject>("Stick (0)");
        var offsets = new Vector3[] {
            new Vector3(-0.5f, -0.3f, 0),
            new Vector3(0.5f, -0.3f, 0),
            Vector3.up,
        };

        for (int i = 0; i < 3; i++) {
            var stick = Instantiate(thing, transform.parent);
            stick.transform.position = transform.position + offsets[i];
            stick.transform.localEulerAngles = Vector3.forward * Random.value * 180;
        }

        Destroy(gameObject);
    }
}
