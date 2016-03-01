﻿#region Copyright (c) 2012 by SharpCrafters s.r.o.

// Copyright (c), SharpCrafters s.r.o. All rights reserved.

#endregion

#define  SLIM_SYNCHRONIZATION_PRIMITIVES
using System;

namespace PostSharp.Samples.WeakEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            EventClient eventClient = new EventClient();
            MyEvent += eventClient.EventHandler;

            // Forcing GC here to prove that we are not collecting the handler when the client is alive.
            GC.Collect();

            // Raise the event when the client is alive.
            MyEvent(null, EventArgs.Empty);
            Console.WriteLine("EventHandlerCount: {0}", EventClient.EventHandlerCount);


            // Cause the client to be collected.
            WeakReference weakReference = new WeakReference(eventClient);
            eventClient = null;
            GC.Collect();

            Console.WriteLine("Client is alive: {0}", weakReference.IsAlive);

            
            // Raise the event when the client is dead.
            MyEvent(null, EventArgs.Empty);
            Console.WriteLine("EventHandlerCount: {0}", EventClient.EventHandlerCount);

        }

        [WeakEvent]
        static event EventHandler MyEvent;
    }

    [WeakEventClient]
    class EventClient
    {
        static public int EventHandlerCount;

        public void EventHandler(object sender, EventArgs e)
        {
            Console.WriteLine("Oops!");
            EventHandlerCount++;
        }
    }
}