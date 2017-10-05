using System;
using JetBrains.Annotations;

namespace Svenkle.TeamCityBuildLight.Infrastructure.Logger
{
    public interface ILogger
    {
        void Debug(Exception exception);
        void Debug(string message, Exception exception);

        [StringFormatMethod("format")]
        void Debug(string format, params object[] args);

        void Error(Exception exception);
        void Error(string message, Exception exception);

        [StringFormatMethod("format")]
        void Error(string format, params object[] args);

        void Fatal(Exception exception);
        void Fatal(string message, Exception exception);

        [StringFormatMethod("format")]
        void Fatal(string format, params object[] args);

        void Info(Exception exception);
        void Info(string message, Exception exception);

        [StringFormatMethod("format")]
        void Info(string format, params object[] args);

        void Warn(Exception exception);
        void Warn(string message, Exception exception);

        [StringFormatMethod("format")]
        void Warn(string format, params object[] args);
    }
}