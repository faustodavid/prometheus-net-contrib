﻿using System.Collections.Generic;
using Prometheus.Contrib.Core;
using Prometheus.Contrib.EventListeners.Counters;

namespace Prometheus.Contrib.EventListeners.Adapters
{
    internal class PrometheusSignalRCounterAdapter : ICounterAdapter
    {
        public const string EventSourceName = "Microsoft.AspNetCore.Http.Connections";
        
        internal readonly MeanCounter ConnectionsStarted = new MeanCounter("connections-started", "signalr_connections_started_total", "Total Connections Started");
        internal readonly MeanCounter ConnectionsStopped = new MeanCounter("connections-stopped", "aspnetcore_requests_current_total", "Current Requests");
        internal readonly MeanCounter ConnectionsTimedOut = new MeanCounter("connections-timed-out", "signalr_connections_timed_out_total", "Total Connections Timed Out");
        internal readonly MeanCounter CurrentConnections = new MeanCounter("current-connections", "signalr_connections_current_total", "Current Connections");
        internal readonly MeanCounter ConnectionsDuration = new MeanCounter("connections-duration", "signalr_connections_duration_seconds", "Average Connection Duration");

        private readonly Dictionary<string, BaseCounter> _counters;

        public PrometheusSignalRCounterAdapter()
        {
            _counters = BaseCounter.GenerateDictionary(this);
        }

        public void OnCounterEvent(IDictionary<string, object> eventPayload)
        {
            if (!eventPayload.TryGetValue("Name", out var counterName))
            {
                return;
            }
            
            if (!_counters.TryGetValue((string) counterName, out var counter))
                return;

            counter.TryReadEventCounterData(eventPayload);
        }
    }
}
