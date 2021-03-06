﻿/* Created by Str4tos. 2017 */

//#define CopyVectorProperty

using UnityEngine;
using System.Collections;

[System.Serializable]
public struct ConstraintCoord2D
{
    public ConstraintCoord xAxis;
    public ConstraintCoord yAxis;

    public ConstraintCoord2D(float xMin, float xMax, float yMin, float yMax)
    {
        xAxis = new ConstraintCoord(xMin, xMax);
        yAxis = new ConstraintCoord(yMin, yMax);
    }
    public ConstraintCoord2D(ConstraintCoord xAxis, ConstraintCoord yAxis)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
    }

    /// <summary>
    /// Clamp position in range constraint.
    /// </summary>
    public Vector2 Clamp(Vector2 position)
    {
        xAxis.Clamp(position.x);
        yAxis.Clamp(position.y);
        return position;
    }
    /// <summary>
    /// Clamp position in range constraint. Skip clamp if limit equal zero.
    /// </summary>
    public Vector2 ClampVerific(Vector2 position)
    {
        xAxis.ClampVerific(ref position.x);
        yAxis.ClampVerific(ref position.y);
        return position;
    }
    /// <summary>
    /// Clamp position in range constraint. Skip clamp if limit equal zero.
    /// </summary>
    public void ClampVerific(ref Vector2 position)
    {
        xAxis.ClampVerific(ref position.x);
        yAxis.ClampVerific(ref position.y);
    }
    /// <summary>
    /// Return random position in range constraint.
    /// </summary>
    public Vector2 GetRandom()
    {
        return new Vector2(xAxis.GetRandom(), yAxis.GetRandom());
    }
}

[System.Serializable]
public struct ConstraintCoord
{
    public float min;
    public float max;

    public ConstraintCoord(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// Return true if empty or min & max equals zero.
    /// </summary>
    public bool IsZero()
    {
        return min == default(float) && max == default(float);
    }
    /// <summary>
    /// Return clamped value in range between min and max.
    /// </summary>
    public float Clamp(float value)
    {
        return Mathf.Clamp(value, min, max);
    }
    /// <summary>
    /// Clamp value in range between min and max. Skip clamp if limit equal zero.
    /// </summary>
    public float ClampVerific(float value)
    {
        if (min != 0.0 && value < min)
        {
            return min;
        }
        if (max != 0.0f && value > max)
        {
            return max;
        }
        return value;
    }
    /// <summary>
    /// Clamp value in range between min and max. Skip clamp if limit equal zero.
    /// </summary>
    public void ClampVerific(ref float value)
    {
        if (min != 0.0 && value < min)
        {
            value = min;
        }
        if (max != 0.0f && value > max)
        {
            value = max;
        }
    }
    /// <summary>
    /// Return random value in range between min and max.
    /// </summary>
    public float GetRandom()
    {
        return Random.Range(min, max);
    }
    /// <summary>
    /// Return true if value out of limit coordinates.
    /// </summary>
    public bool IsOutOfLimits(float value)
    {
        return value > max || value < min;
    }
    /// <summary>
    /// Return true if value out of limit coordinates considering the direction.
    /// </summary>
    public bool IsOutOfLimits(float value, bool directionToMax)
    {
#if UNITY_EDITOR
        if (max < min)
            Debug.LogError("ConstraintCoord error max < min");
#endif
        if (directionToMax)
        {
            if (value > max)
                return true;
        }
        else
        {
            if (value < min)
                return true;
        }
        return false;
    }
    /// <summary>
    /// Return true if value closer maximum coordinate.
    /// </summary>
    /// <param name="offset">Offset of maximum</param>
    public bool IsCloserMaxBound(float value, float offset = 0.0f)
    {
        return (value - max + offset) < (min - value);
    }
    /// <summary>
    /// Return true if value closer minimum coordinate.
    /// </summary>
    /// <param name="offset">Offset of minimum</param>
    public bool IsCloserMinBound(float value, float offset = 0.0f)
    {
        return (value - max) > (min + offset - value);
    }
    /// <summary>
    /// Set minimum and maximum by center 'Value' +- offset.
    /// </summary>
    public void SetNearValue(float value, float offset)
    {
        min = value - offset;
        max = value + offset;
    }

    /// <summary>
    /// Transform minimum and maximum coordinates to world space from target.
    /// Error if coordinates already in world space.
    /// </summary>
    public void TransformToWorld(Transform target)
    {
        float offset = max - min;
        min = target.TransformPoint(min, 0.0f, 0.0f).x;
        max = min + offset;
    }
    /// <summary>
    /// Transform minimum and maximum coordinates to local space from target.
    /// Error if coordinates already in local space.
    /// </summary>
    public void TransformToLocal(Transform target)
    {
        float offset = max - min;
        min = target.InverseTransformPoint(min, 0.0f, 0.0f).x;
        max = min + offset;
    }

    public static implicit operator Vector2(ConstraintCoord l)
    {
        return new Vector2(l.min, l.max);
    }
    public static implicit operator ConstraintCoord(Vector2 v)
    {
        return new ConstraintCoord(v.x, v.y);
    }
    public static implicit operator ConstraintCoord(Vector3 v)
    {
        return new ConstraintCoord(v.x, v.y);
    }
    public static implicit operator bool(ConstraintCoord limit)
    {
        return !limit.IsZero();
    }
}

#if UNITY_EDITOR && CopyVectorProperty
public class CoordsFromVector : PropertyAttribute
{
    public readonly string originPropertyName;
    public CoordsFromVector(string originPropertyName)
    {
        this.originPropertyName = originPropertyName;
    }
}
#endif
