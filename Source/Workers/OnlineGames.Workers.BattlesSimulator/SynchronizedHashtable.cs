// <copyright file="SynchronizedHashtable.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator
{
    using System;
    using System.Collections;

    public class SynchronizedHashtable
    {
        private readonly Hashtable hashtable;

        public SynchronizedHashtable()
        {
            var unsynchronizedHashtable = new Hashtable();
            this.hashtable = Hashtable.Synchronized(unsynchronizedHashtable);
        }

        public bool Contains(object value)
        {
            return this.hashtable.ContainsKey(value);
        }

        public bool Add(object value)
        {
            if (this.hashtable.ContainsKey(value))
            {
                return false;
            }

            try
            {
                this.hashtable.Add(value, true);
                return true;
            }
            catch (ArgumentException)
            {
                // The item is already in the hashtable.
                return false;
            }
        }

        public void Remove(object value)
        {
            this.hashtable.Remove(value);
        }
    }
}
