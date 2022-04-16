using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCurveUtils
{
   public static AnimationCurve InverseIncreasingCurve(AnimationCurve curve)
    {
        var reverse = new AnimationCurve();
        for (int i = 0; i < curve.keys.Length; i++)
        {
            var keyframe = curve.keys[i];
            var reverseKeyframe = new Keyframe();
            reverseKeyframe.weightedMode = WeightedMode.Both;
            
            reverseKeyframe.value = keyframe.time;
            reverseKeyframe.time = keyframe.value;
            
            reverseKeyframe.inTangent = 1 / keyframe.inTangent;
            reverseKeyframe.outTangent = 1 / keyframe.outTangent;
            
            reverse.AddKey(reverseKeyframe);
        }
        for (int i = 0; i < curve.keys.Length; i++)
        {
            Keyframe[] reverseKeyframes = reverse.keys;
            
            if (i != 0)
            {
                float distToPrevious = curve.keys[i].time - curve.keys[i - 1].time;
                float inHandleLengthX = curve.keys[i].inWeight * distToPrevious;
                float inHandleLengthY = -curve.keys[i].inTangent * inHandleLengthX;

                float reverseInHandleLengthX = inHandleLengthY;

                float reverseDistToPrevious = reverseKeyframes[i].time - reverseKeyframes[i - 1].time;
                reverseKeyframes[i].inWeight = -reverseInHandleLengthX / reverseDistToPrevious;
            }

            if (i != curve.keys.Length - 1)
            {
                float distToNext = curve.keys[i + 1].time - curve.keys[i].time;
                float outHandleLengthX = curve.keys[i].outWeight * distToNext;
                float outHandleLengthY = curve.keys[i].outTangent * outHandleLengthX;

                float reverseOutHandleLengthX = outHandleLengthY;

                float reverseDistToNext = reverseKeyframes[i + 1].time - reverseKeyframes[i].time;
                reverseKeyframes[i].outWeight = reverseOutHandleLengthX / reverseDistToNext;
            }

            reverse.keys = reverseKeyframes;
        }

        return reverse;
    }

    public static AnimationCurve InverseDecreasingCurve(AnimationCurve curve)
    {
        var reverse = new AnimationCurve();
        for (int i = 0; i < curve.keys.Length; i++)
        {
            var keyframe = curve.keys[i];
            var reverseKeyframe = new Keyframe();
            reverseKeyframe.weightedMode = WeightedMode.Both;
            
            reverseKeyframe.value = -keyframe.time;
            reverseKeyframe.time = -keyframe.value;
            
            reverseKeyframe.inTangent = 1 / keyframe.inTangent;
            reverseKeyframe.outTangent = 1 / keyframe.outTangent;
            
            reverse.AddKey(reverseKeyframe);
        }
        for (int i = 0; i < curve.keys.Length; i++)
        {
            Keyframe[] reverseKeyframes = reverse.keys;
            
            if (i != 0)
            {
                float distToPrevious = curve.keys[i].time - curve.keys[i - 1].time;
                float inHandleLengthX = curve.keys[i].inWeight * distToPrevious;
                float inHandleLengthY = curve.keys[i].inTangent * inHandleLengthX;

                float reverseInHandleLengthX = -inHandleLengthY;

                float reverseDistToPrevious = reverseKeyframes[i].time - reverseKeyframes[i - 1].time;
                reverseKeyframes[i].inWeight = reverseInHandleLengthX / reverseDistToPrevious;
            }

            if (i != curve.keys.Length - 1)
            {
                float distToNext = curve.keys[i + 1].time - curve.keys[i].time;
                float outHandleLengthX = curve.keys[i].outWeight * distToNext;
                float outHandleLengthY = curve.keys[i].outTangent * outHandleLengthX;

                float reverseOutHandleLengthX = -outHandleLengthY;

                float reverseDistToNext = reverseKeyframes[i + 1].time - reverseKeyframes[i].time;
                reverseKeyframes[i].outWeight = reverseOutHandleLengthX / reverseDistToNext;
            }

            reverse.keys = reverseKeyframes;
        }

        return reverse;
    }
}
