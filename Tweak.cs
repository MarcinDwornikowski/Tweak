using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tweak
{
    /// <summary>
    /// A class for easy execution of basic coroutines. First needs to be initialised and given the Owner Monobehaviour.
    /// </summary>
    public class Tweak
    {
        public MonoBehaviour Owner;
        private CoroutineWithData[] _cd;
        public object[] Result;

        /// <summary>
        /// Initialise, define owner, create slots.
        /// </summary>
        /// <param name="owner">MonoBehaviour that will start and stop all the coroutines.</param>
        /// <param name="numberOfSlots">The number of slots given for different coroutines to be held in.</param>
        public Tweak(MonoBehaviour owner, int numberOfSlots = 0)
        {
            Owner = owner;
            _cd = new CoroutineWithData[numberOfSlots + 1];
            Result = new object[numberOfSlots + 1];
        }

        /// <summary>
        /// Checks whether a slot is currently updating.
        /// </summary>
        /// <param name="slot">The slot index you want to check.</param>
        /// <returns>True if the given slot is currently updating.</returns>
        public bool IsUpdating(int slot)
        {
            if (_cd[slot] != null && _cd[slot].Result.ToString() != "finished")
            {
                Result[slot] = _cd[slot].Result;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Stops a coroutine held currently in a slot.
        /// </summary>
        /// <param name="slot">The slot index you want to stop.</param>
        public void StopSlotCoroutine(int slot)
        {
            if (_cd != null)
                if (_cd[slot] != null && _cd[slot].Coroutine != null)
                    Owner.StopCoroutine(_cd[slot].Coroutine);
        }

        #region Floats

        public void LerpFloat(int slot, float duration, float startValue, float endValue, Action<float> ToDo, Action Callback)
        {
            _cd[slot] = new CoroutineWithData(Owner, LerpFloatCo(duration, startValue, endValue, ToDo, Callback));
        }

        public void LerpFloat(float duration, float startValue, float endValue, Action<float> ToDo, Action Callback)
        {
            _cd[0] = new CoroutineWithData(Owner, LerpFloatCo(duration, startValue, endValue, ToDo, Callback));
        }

        public void LerpFloat(float duration, float startValue, float endValue, Action<float> ToDo, Action Callback, out Coroutine coroutine)
        {
            _cd[0] = new CoroutineWithData(Owner, LerpFloatCo(duration, startValue, endValue, ToDo, Callback));

            coroutine = _cd[0].Coroutine;
        }

        private IEnumerator LerpFloatCo(float duration, float startValue, float endValue, Action<float> ToDo, Action Callback)
        {
            float elapsedTime = 0;
            float curValue = 0;
            while (elapsedTime < duration)
            {
                curValue = Mathf.Lerp(startValue, endValue, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                if (ToDo != null)
                    ToDo(curValue);
                yield return new WaitForEndOfFrame();
            }

            curValue = endValue;
            if (ToDo != null)
                ToDo(curValue);

            if (Callback != null)
                Callback();
        }

        public void LerpFloatSmooth(int slot, float duration, float startValue, float endValue, Action<float> ToDo, Action Callback)
        {
            _cd[slot] = new CoroutineWithData(Owner, LerpFloatSmoothCo(duration, startValue, endValue, ToDo, Callback));
        }

        public void LerpFloatSmooth(float duration, float startValue, float endValue, Action<float> ToDo, Action Callback)
        {
            _cd[0] = new CoroutineWithData(Owner, LerpFloatSmoothCo(duration, startValue, endValue, ToDo, Callback));
        }

        public void LerpFloatSmooth(float duration, float startValue, float endValue, Action<float> ToDo, Action Callback, out Coroutine coroutine)
        {
            _cd[0] = new CoroutineWithData(Owner, LerpFloatSmoothCo(duration, startValue, endValue, ToDo, Callback));

            coroutine = _cd[0].Coroutine;
        }

        private IEnumerator LerpFloatSmoothCo(float duration, float startValue, float endValue, Action<float> ToDo, Action Callback)
        {
            float elapsedTime = 0;
            float curValue = 0;
            while (elapsedTime < duration)
            {
                curValue = Mathf.SmoothStep(startValue, endValue, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                if (ToDo != null)
                    ToDo(curValue);
                yield return new WaitForEndOfFrame();
            }

            curValue = endValue;
            if (ToDo != null)
                ToDo(curValue);

            if (Callback != null)
                Callback();
        }
        #endregion

        #region Utility
        public void Delay(int slot, float delay, Action Callback)
        {
            _cd[slot] = new CoroutineWithData(Owner, DelayCo(delay, Callback));
        }

        public void Delay(float delay, Action Callback)
        {
            _cd[0] = new CoroutineWithData(Owner, DelayCo(delay, Callback));
        }

        public void Delay(float delay, Action Callback, out Coroutine coroutine)
        {
            _cd[0] = new CoroutineWithData(Owner, DelayCo(delay, Callback));

            coroutine = _cd[0].Coroutine;
        }

        private IEnumerator DelayCo(float delay, Action Callback)
        {
            yield return new WaitForSeconds(delay);
            if (Callback != null)
                Callback();
        }

        public void WaitForEndOfFrame(int slot, Action Callback)
        {
            _cd[slot] = new CoroutineWithData(Owner, WaitFramesCo(1, Callback));
        }

        public void WaitForEndOfFrame(Action Callback)
        {
            _cd[0] = new CoroutineWithData(Owner, WaitFramesCo(1, Callback));
        }

        public void WaitFrames(int slot, int frames, Action Callback)
        {
            _cd[slot] = new CoroutineWithData(Owner, WaitFramesCo(frames, Callback));
        }

        public void WaitFrames(int frames, Action Callback)
        {
            _cd[0] = new CoroutineWithData(Owner, WaitFramesCo(frames, Callback));
        }

        public void WaitFrames(int frames, Action Callback, out Coroutine coroutine)
        {
            _cd[0] = new CoroutineWithData(Owner, WaitFramesCo(frames, Callback));

            coroutine = _cd[0].Coroutine;
        }

        private IEnumerator WaitFramesCo(int frames, Action Callback)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            if (Callback != null)
                Callback();
        }


        /// <summary>
        /// Will wait amount of seconds specified by delay, then call ToRepeat and loop again.
        /// </summary>
        public void Repeater(int slot, float delay, Action ToRepeat)
        {
            _cd[slot] = new CoroutineWithData(Owner, RepeaterCo(delay, ToRepeat));
        }

        /// <summary>
        /// Will wait amount of seconds specified by delay, then call ToRepeat and loop again.
        /// </summary>
        public void Repeater(float delay, Action ToRepeat)
        {
            _cd[0] = new CoroutineWithData(Owner, RepeaterCo(delay, ToRepeat));
        }

        /// <summary>
        /// Will wait amount of seconds specified by delay, then call ToRepeat and loop again.
        /// </summary>
        public void Repeater(float delay, Action ToRepeat, out Coroutine coroutine)
        {
            _cd[0] = new CoroutineWithData(Owner, RepeaterCo(delay, ToRepeat));

            coroutine = _cd[0].Coroutine;
        }

        private IEnumerator RepeaterCo(float delay, Action ToRepeat)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                if (ToRepeat != null)
                    ToRepeat();
            }
        }
        #endregion
    }

    internal class CoroutineWithData
    {
        internal Coroutine Coroutine { get; private set; }
        internal object Result;
        private IEnumerator _target;

        internal CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            this._target = target;
            this.Coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (_target.MoveNext())
            {
                Result = _target.Current;
                yield return Result;
            }
            Result = "finished";
        }
    }
}