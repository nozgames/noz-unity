/*
  NoZ Game Engine

  Copyright(c) 2019 NoZ Games, LLC

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files(the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions :

  The above copyright notice and this permission notice shall be included in all
  copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  SOFTWARE.
*/

using UnityEngine;

namespace NoZ
{
    public static partial class Extensions
    {
        public static Vector2 DirectionTo (this Component go, in Component to) =>
            ((Vector2)to.transform.position - (Vector2)go.transform.position).normalized;

        public static Vector2 DirectionTo(this Component go, in Vector2 to) =>
            (to - (Vector2)go.transform.position).normalized;

        public static float DistanceTo(this Component go, GameObject to) =>
            ((Vector2)go.transform.position - (Vector2)to.transform.position).magnitude;

        public static float DistanceTo(this Component go, Component to) =>
            ((Vector2)go.transform.position - (Vector2)to.transform.position).magnitude;

        public static float DistanceTo(this Component go, Transform to) =>
            ((Vector2)go.transform.position - (Vector2)to.position).magnitude;

        public static float DistanceTo(this Component go, in Vector2 pos) =>
            ((Vector2)go.transform.position - pos).magnitude;

        public static float DistanceTo(this Component go, in Vector3 pos) =>
            ((Vector2)go.transform.position - (Vector2)pos).magnitude;

        public static float SqrDistanceTo(this Component go, GameObject to) =>
            ((Vector2)go.transform.position - (Vector2)to.transform.position).sqrMagnitude;

        public static float SqrDistanceTo(this Component go, Component to) =>
            ((Vector2)go.transform.position - (Vector2)to.transform.position).sqrMagnitude;

        public static float SqrDistanceTo(this Component go, Transform to) =>
            ((Vector2)go.transform.position - (Vector2)to.position).sqrMagnitude;

        public static float SqrDistanceTo(this Component go, in Vector2 pos) =>
            ((Vector2)go.transform.position - pos).sqrMagnitude;

        public static float SqrDistanceTo(this Component go, in Vector3 pos) =>
            ((Vector2)go.transform.position - (Vector2)pos).sqrMagnitude;

        public static void Subscribe<Source>(this Component c, Event<Source> e, Event<Source>.EventDelegate d, bool oneShot = false) where Source : UnityEngine.Object => e.Subscribe((Source)(UnityEngine.Object)c, d, oneShot);
        public static void Subscribe<Source,Arg1>(this Component c, Event<Source,Arg1> e, Event<Source,Arg1>.EventDelegate d, bool oneShot = false) where Source : UnityEngine.Object => e.Subscribe(c as Source, d, oneShot);
        public static void Subscribe<Source,Arg1,Arg2>(this Component c, Event<Source,Arg1,Arg2> e, Event<Source,Arg1,Arg2>.EventDelegate d, bool oneShot = false) where Source : UnityEngine.Object => e.Subscribe(c as Source, d, oneShot);
        public static void Subscribe<Source,Arg1,Arg2,Arg3>(this Component c, Event<Source,Arg1,Arg2,Arg3> e, Event<Source,Arg1,Arg2,Arg3>.EventDelegate d, bool oneShot = false) where Source : UnityEngine.Object => e.Subscribe(c as Source, d, oneShot);

        public static void Unsubscribe<Source>(this Component c, Event<Source> e, Object target) where Source : UnityEngine.Object => e.UnsubscribeAll(target);
        public static void Unsubscribe<Source,Arg1>(this Component c, Event<Source,Arg1> e, Object target) where Source : UnityEngine.Object => e.UnsubscribeAll(target);
        public static void Unsubscribe<Source,Arg1,Arg2>(this Component c, Event<Source,Arg1, Arg2> e, Object target) where Source : UnityEngine.Object => e.UnsubscribeAll(target);
        public static void Unsubscribe<Source,Arg1,Arg2,Arg3>(this Component c, Event<Source,Arg1, Arg2, Arg3> e, Object target) where Source : UnityEngine.Object => e.UnsubscribeAll(target);

        public static void UnsubscribeAll(this Component c) => EventBase.UnsubscribeAllObservers(c);

        public static void Broadcast<Source>(this Component c, Event<Source> e) where Source : UnityEngine.Object => e.Broadcast(c as Source);
        public static void Broadcast<Source,Arg1>(this Component c, Event<Source, Arg1> e, Arg1 arg1) where Source : UnityEngine.Object => e.Broadcast(c as Source, arg1);
        public static void Broadcast<Source,Arg1,Arg2>(this Component c, Event<Source,Arg1, Arg2> e, Arg1 arg1, Arg2 arg2) where Source : UnityEngine.Object => e.Broadcast(c as Source, arg1, arg2);
        public static void Broadcast<Source,Arg1,Arg2,Arg3>(this Component c, Event<Source, Arg1, Arg2, Arg3> e, Arg1 arg1, Arg2 arg2, Arg3 arg3) where Source : UnityEngine.Object => e.Broadcast(c as Source, arg1, arg2, arg3);
    }
}
