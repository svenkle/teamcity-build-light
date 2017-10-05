using System;
using log4net;

namespace Svenkle.TeamCityBuildLight.Infrastructure.Logger
{
    public class Logger : ILogger
    {
        private readonly ILog _log;

        public Logger(string name)
        {
            _log = LogManager.GetLogger(name);
        }

        public Logger(Type type)
        {
            _log = LogManager.GetLogger(type);
        }

        public void Debug(Exception exception)
        {
            _log.Debug(exception);
        }

        public void Debug(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }

        public void Debug(string message, Exception exception)
        {
            _log.Debug(message, exception);
        }

        public void Error(Exception exception)
        {
            _log.Error(exception);
        }

        public void Error(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }

        public void Error(string message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Fatal(Exception exception)
        {
            _log.Fatal(exception);
        }

        public void Fatal(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }

        public void Fatal(string message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void Info(Exception exception)
        {
            _log.Info(exception);
        }

        public void Info(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }

        public void Info(string message, Exception exception)
        {
            _log.Info(message, exception);
        }

        public void Warn(Exception exception)
        {
            _log.Warn(exception);
        }

        public void Warn(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }

        public void Warn(string message, Exception exception)
        {
            _log.Warn(message, exception);
        }
    }
}