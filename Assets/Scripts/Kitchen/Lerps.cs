using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lerps {

    public static IEnumerator MoveLerp(Transform movable, Vector2 finPos, float t, System.Func<float, float> lerpNormalizator = null) {
        var startPos = movable.position;
        float deltaLerp = Time.fixedDeltaTime / t;
        for (float lerp = 0; lerp < 1; lerp += deltaLerp) {
            movable.position = Vector2.Lerp(startPos, finPos, lerpNormalizator?.Invoke(lerp) ?? lerp);
            yield return new WaitForFixedUpdate();
        }
        movable.position = finPos;
    }

    public static IEnumerator RotateLerp(Transform movable, float finAngle, float t, System.Func<float, float> lerpNormalizator = null) {
        float startAngle = movable.rotation.eulerAngles.z;
        float deltaLerp = Time.fixedDeltaTime / t;
        for (float lerp = 0; lerp < 1; lerp += deltaLerp) {
            movable.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(startAngle, finAngle, lerpNormalizator?.Invoke(lerp) ?? lerp));
            yield return new WaitForFixedUpdate();
        }
        movable.rotation = Quaternion.Euler(0, 0, finAngle);
    }

    public static IEnumerator ColorLerp(Transform movable, Color finCol, float t, System.Func<float, float> lerpNormalizator = null) {
        var sRenderer = movable.gameObject.GetComponent<SpriteRenderer>();
        if(sRenderer == null) { throw new System.NullReferenceException("Movable object is null or it has no SpriteRenderer component on it"); }

        var startCol = sRenderer.color;
        float deltaLerp = Time.fixedDeltaTime / t;
        for (float lerp = 0; lerp < 1; lerp += deltaLerp) {
            sRenderer.color = Color.Lerp(startCol, finCol, lerpNormalizator?.Invoke(lerp) ?? lerp);
            yield return new WaitForFixedUpdate();
        }
        sRenderer.color = finCol;
    }

    public static class Normalizators {
        public static float Default(float l) => l;
        public static float Squared(float l) {
            float x = (-1 + Mathf.Sqrt(5)) / 2;
            return -1 / (l + x) + x + 1;
            // return Mathf.Pow(l, 1/3f);
        }
    }
}
