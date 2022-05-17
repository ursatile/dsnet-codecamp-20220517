﻿using EasyNetQ;

namespace Autobarn.Website.Tests {
	public class TestBus : IBus {
		private readonly TestPubSub pubSub;

		public TestBus() {
			this.pubSub = new TestPubSub();
		}
		public void Dispose() { }
		public IPubSub PubSub { get => this.pubSub; }
		public IRpc Rpc { get; }
		public ISendReceive SendReceive { get; }
		public IScheduler Scheduler { get; }
		public IAdvancedBus Advanced { get; }
	}
}