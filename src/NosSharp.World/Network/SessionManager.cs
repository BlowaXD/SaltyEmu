using System;
using System.Collections.Concurrent;

namespace NosSharp.World.Network
{
    public class SessionManager
    {
        private readonly ConcurrentDictionary<string, int> _sessions = new ConcurrentDictionary<string, int>();

        public void RegisterSession(string key, int sessionId)
        {
            _sessions.TryAdd(key, sessionId);
        }

        public bool GetSession(string key, out int sessionId) => _sessions.TryGetValue(key, out sessionId);

        public void UnregisterSession(string key)
        {
            _sessions.TryRemove(key, out int _);
        }

        #region Singleton

        private static readonly Lazy<SessionManager> LazyInstance = new Lazy<SessionManager>(() => new SessionManager());

        public static SessionManager Instance => LazyInstance.Value;

        #endregion
    }
}