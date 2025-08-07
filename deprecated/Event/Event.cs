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

using System;
using System.Collections.Generic;

namespace NoZ
{
    public class EventBase
    {
        /// <summary>
        /// Delegate used by Broadcast to broadcast the actual event to the target
        /// </summary>
        /// <param name="d">Delegate to call</param>
        /// <param name="source">Source that is broadcsting the event</param>
        /// <param name="target">Target to call delegate on</param>
        protected delegate void BroadcastDelegate(Delegate d, UnityEngine.Object source, UnityEngine.Object target);

        /// <summary>
        /// List of all registered event handlers sorted by event and handler
        /// </summary>
        private static List<EventHandler> _handlers = new List<EventHandler>();

        /// <summary>
        /// True if an event is currently being broadcast
        /// </summary>
        private static int _isBroadcasting = 0;

        /// <summary>
        /// Next unique identifier for identifier generation
        /// </summary>
        private static int _nextId = 1;

        /// <summary>
        /// True if the handlers list is dirty
        /// </summary>
        private static bool _handlersDirty = true;

        /// <summary>
        /// Unique Identifier of the event
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected EventBase()
        {
            Id = _nextId++;
        }

        /// <summary>
        /// Update the handlers list if it is dirty
        /// </summary>
        private void UpdateHandlers ()
        {
            if (!_handlersDirty)
                return;

            // Remove any stale handlers
            for (int i = _handlers.Count - 1; i >= 0; i--)
                if (!_handlers[i].IsValid)
                    RemoveHandler(i);

            // Sort all handlers
            _handlers.Sort(EventHandler.Comparer.Instance);

            _handlersDirty = false;
        }

        /// <summary>
        /// Unsubscribe all observers of teh event
        /// </summary>
        public void UnsubscribeAll ()
        {
            for (var i = _handlers.Count - 1; i >= 0; i--)
                if (_handlers[i]._event.Id == Id)
                    RemoveHandler(i);
        }

        /// <summary>
        /// Unsubscribe from all handlers matching the given target.
        /// </summary>
        /// <param name="target"></param>
        public void UnsubscribeAll(UnityEngine.Object target)
        {
            for (var i = _handlers.Count - 1; i >= 0; i--)
                if (_handlers[i]._target != null && _handlers[i]._event.Id == Id)
                    if (!_handlers[i]._target == target)
                        RemoveHandler(i);
        }

        /// <summary>
        /// Unsubscribe all observers of events on the given source object
        /// </summary>
        /// <param name="source"></param>
        public static void UnsubscribeAllObservers(UnityEngine.Object source)
        {
            for (int i = _handlers.Count - 1; i >= 0; i--)
                if (ReferenceEquals(_handlers[i]._source, source))
                    RemoveHandler(i);
        }

        public void Unsubscribe(Delegate d)
        {
            for (var i = _handlers.Count - 1; i >= 0; i--)
                if (ReferenceEquals(_handlers[i]._delegate.Method, d.Method) && ReferenceEquals(_handlers[i]._target, d.Target))
                    RemoveHandler(i);
        }

        /// <summary>
        /// Add a new event handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="d"></param>
        /// <param name="oneShot"></param>
        protected void AddHandler(UnityEngine.Object source, Delegate d, bool oneShot)
        {
            var target = d.Target as UnityEngine.Object;
            if(null == target)
                throw new ArgumentException("Event handlers must be non static and a member of an Object derived class");

            // Create the event handler.
            _handlers.Add(new EventHandler
            {
                _id = _nextId++,
                _event = this,
                _delegate = EventDelegate.Create(d),
                _target = target,
                _source = source,
                _isOneShot = oneShot,
                _hasSource = source != null
            });

            _handlersDirty = true;
        }

        /// <summary>
        /// Remove a handler at the given index.  Note if an event is currently broadcasting then
        /// the handler will only be flagged for removal and removed at a later time.
        /// </summary>
        /// <param name="index"></param>
        protected static void RemoveHandler (int index)
        {
            _handlersDirty = true;

            if (_isBroadcasting > 0)
            {
                _handlers[index]._target = null;
            }                
            else 
            {
                if (index != _handlers.Count - 1)
                    _handlers[index] = _handlers[_handlers.Count - 1];

                _handlers.RemoveAt(_handlers.Count - 1);
            }
        }

        /// <summary>
        /// Broadcast event from the given source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="broadcastDelegate"></param>
        protected void Broadcast (UnityEngine.Object source, BroadcastDelegate broadcastDelegate)
        {
            var isFirst = _isBroadcasting == 0;
            if (isFirst)
            {
                UpdateHandlers();
                _isBroadcasting = _handlers.Count;
            }

            // Iterate over all of the handlers, any new ones added during call will be ignored
            for (int i = 0, c = _isBroadcasting; i < c; i++)
            {
                var handler = _handlers[i];
                var remove = false;

                if (handler._event.Id != Id)
                    continue;

                // If handler is limited to a specific source then check that now
                if(handler._hasSource)
                {
                    if(handler._source == null)
                        remove = true;
                    else if (handler._source != source)
                        continue;
                }

                // Get the target
                if (!remove && handler._target != null)
                    broadcastDelegate(handler._delegate, source, handler._target);
                else
                    remove = true;

                if (remove || handler._isOneShot)
                    RemoveHandler(i);
            }

            if (isFirst)
                _isBroadcasting = 0;
        }
    }

    /// <summary>
    /// Represents and event with no parameters.
    /// </summary>
    public class Event<Source> : EventBase where Source : UnityEngine.Object
    {
        public delegate void EventDelegate(Source source);

        private BroadcastDelegate _broadcastDelegate;

        public Event()
        {
            _broadcastDelegate = (Delegate d, UnityEngine.Object source, UnityEngine.Object target) 
                => ((Action<UnityEngine.Object,Source>)d)(target,source as Source);
        }

        public void Subscribe(EventDelegate d, bool oneShot = false) => AddHandler(null, d, oneShot);
        public void Subscribe(Source source, EventDelegate d, bool oneShot = false) => AddHandler(source, d, oneShot);

        public void Broadcast(Source source) => Broadcast(source, _broadcastDelegate);
    }

    /// <summary>
    /// Represents an event with a single parameter
    /// </summary>
    /// <typeparam name="Arg1">Type of first event parameter</typeparam>
    public class Event<Source,Arg1> : EventBase where Source : UnityEngine.Object
    {
        public delegate void EventDelegate(Source source, Arg1 arg1);
        private Stack<Arg1> paramStack = new Stack<Arg1>(16);
        private BroadcastDelegate _broadcastDelegate;

        public Event ()
        {
            _broadcastDelegate = (Delegate d, UnityEngine.Object source, UnityEngine.Object target) => 
                ((Action<UnityEngine.Object, Source, Arg1>)d)(target, source as Source, paramStack.Peek());
        }

        public void Subscribe(EventDelegate d, bool oneShot = false) => AddHandler(null, d, oneShot);
        public void Subscribe(Source source, EventDelegate d, bool oneShot = false) => AddHandler(source, d, oneShot);

        public void Broadcast(Source source, Arg1 arg1)
        {
            paramStack.Push(arg1);
            Broadcast(source, _broadcastDelegate);
            paramStack.Pop();
        }

        public void Broadcast(Arg1 arg1) => Broadcast(null, arg1);
    }

    /// <summary>
    /// Represents an event with a two parameters
    /// </summary>
    /// <typeparam name="Arg1">Type of first event parameter</typeparam>
    /// <typeparam name="Arg2">Type of second event parameter</typeparam>
    public class Event<Source, Arg1, Arg2> : EventBase where Source : UnityEngine.Object
    {
        public delegate void EventDelegate(Source source, Arg1 arg1, Arg2 arg2);
        public struct Params { public Arg1 arg1; public Arg2 arg2; }
        private Stack<Params> paramStack = new Stack<Params>(16);
        private BroadcastDelegate _broadcastDelegate;

        public Event()
        {
            _broadcastDelegate = (Delegate d, UnityEngine.Object source, UnityEngine.Object target) =>
            {
                var p = paramStack.Peek();
                ((Action<UnityEngine.Object, Source, Arg1, Arg2>)d)(target, source as Source, p.arg1, p.arg2);
            };
        }

        public void Subscribe(EventDelegate d, bool oneShot = false) => AddHandler(null, d, oneShot);
        public void Subscribe(Source source, EventDelegate d, bool oneShot = false) => AddHandler(source, d, oneShot);

        public void Broadcast(Source source, Arg1 arg1, Arg2 arg2)
        {
            paramStack.Push(new Params { arg1 = arg1, arg2 = arg2 });
            Broadcast(source, _broadcastDelegate);
            paramStack.Pop();
        }

        public void Broadcast(Arg1 arg1, Arg2 arg2) => Broadcast(null, arg1, arg2);
    }

    /// <summary>
    /// Represents an event with a three parameters
    /// </summary>
    /// <typeparam name="Arg1">Type of first event parameter</typeparam>
    /// <typeparam name="Arg2">Type of second event parameter</typeparam>
    /// <typeparam name="Arg3">Type of third event parameter</typeparam>
    public class Event<Source, Arg1, Arg2, Arg3> : EventBase where Source : UnityEngine.Object
    {
        public delegate void EventDelegate(Source source, Arg1 arg1, Arg2 arg2, Arg3 arg3);
        public struct Params { public Arg1 arg1; public Arg2 arg2; public Arg3 arg3; }
        private Stack<Params> paramStack = new Stack<Params>(16);
        private BroadcastDelegate _broadcastDelegate;

        public Event()
        {
            _broadcastDelegate = (Delegate d, UnityEngine.Object source, UnityEngine.Object target) =>
            {
                var p = paramStack.Peek();
                ((Action<UnityEngine.Object, Source, Arg1, Arg2, Arg3>)d)(target, source as Source, p.arg1, p.arg2, p.arg3);
            };
        }

        public void Subscribe(EventDelegate d, bool oneShot = false) => AddHandler(null, d, oneShot);
        public void Subscribe(Source source, EventDelegate d, bool oneShot = false) => AddHandler(source, d, oneShot);

        public void Broadcast(Source source, Arg1 arg1, Arg2 arg2, Arg3 arg3)
        {
            paramStack.Push(new Params { arg1 = arg1, arg2 = arg2, arg3 = arg3 });
            Broadcast(source, _broadcastDelegate);
            paramStack.Pop();
        }

        public void Broadcast(Arg1 arg1, Arg2 arg2, Arg3 arg3) => Broadcast(null, arg1, arg2, arg3);
    }
}
