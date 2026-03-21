/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/

using System;
using System.Threading;

namespace Ambertation.Threading
{
	/// <summary>
	/// Implements a Thread that can be stopped
	/// </summary>
	public abstract class StoppableThread : System.IDisposable
	{
		bool async;

		public StoppableThread() : this(SimPe.Helper.XmlRegistry.AsynchronLoad) {}
		public StoppableThread(bool async)
		{
			this.async = async;
			stop = new ManualResetEvent(false);
			ended = new ManualResetEvent(true);
		}

		protected ManualResetEvent stop;
		protected ManualResetEvent ended;

        /// <summary>
		/// block until the loader thread is cancled
		/// </summary>
        protected void WaitForEnd()
        {
            WaitForEnd(SimPe.Wait.TIMEOUT / 100);
        }

		/// <summary>
		/// block until the loader thread is cancled
		/// </summary>
		protected bool WaitForEnd(int timeout)
		{
			if (!async) return true;
            if (stop == null) return true;
            if (worker == null) return true;

			stop.Set();
			int ct=0;
            while (worker.IsAlive && (ct <= timeout || timeout<0)) 
			{
                ct++;
				stop.Set();
				
                Thread.Sleep(100);
			}
            
			ended.Set();
            return !worker.IsAlive;
		}

		public virtual void Dispose()
		{
			WaitForEnd();
		}

		protected abstract void StartThread();

		void ThreadEntry()
		{
			stop.Reset();
            ended.Reset();

			try 
			{
				StartThread();
			} 
			finally 
			{
				ended.Set();
			}
		}

		protected bool HaveToStop
		{
			get 
			{
				if (async)
					if (stop.WaitOne(0, true)) return true;
				return false;
			}
		}

		protected void ExecuteThread(ThreadPriority tp, string name)
		{
			ExecuteThread(tp, name, false, true, 500);
		}

		protected void ExecuteThread(ThreadPriority tp, string name, bool sync)
		{
			ExecuteThread(tp, name, sync, true, 500);
		}

		protected void ExecuteThread(ThreadPriority tp, string name, bool sync, bool events)
		{
			ExecuteThread(tp, name, sync, events, 500);
		}

        protected Thread worker;
		protected void ExecuteThread(ThreadPriority tp, string name, bool sync, bool events, int synctime)
		{
			WaitForEnd();
			if (!async) 
			{
				ThreadEntry();
			} 
			else 
			{
                worker = new Thread(new ThreadStart(ThreadEntry));
                worker.Priority = tp;
                worker.Name = name;
                //worker.SetApartmentState(ApartmentState.STA);
                worker.Start();

				if (sync)
                    while (worker.IsAlive) 
					{
                        worker.Join(synctime);
					}
			}
		}
	}
}
