using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class DynamicColor {
    private const int MILLISECONDS_PER_SECOND = 1000;
    
    private Color currentColor;

    private DynamicColor(Color color) {
        this.currentColor = color;
    }

    public IEnumerator TransitionTo(Color finalColor, float durationInSeconds) {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        float elapsedSeconds = 0;
        while (elapsedSeconds <= durationInSeconds) {
            elapsedSeconds = stopwatch.ElapsedMilliseconds / MILLISECONDS_PER_SECOND;
            currentColor = Color.Lerp(currentColor, finalColor, ((elapsedSeconds / durationInSeconds) * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
        stopwatch.Stop();
    }

    public static implicit operator Color(DynamicColor dynamicColor) {
        return dynamicColor.currentColor;
    }

    public static implicit operator DynamicColor(Color color) {
        return new DynamicColor(color);
    }
}