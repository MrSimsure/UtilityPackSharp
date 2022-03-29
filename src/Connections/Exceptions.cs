using System;

namespace UtilityPack.Connections.Ftp
{
    /// <summary> Exception thrown when an ftp connection fails</summary>
    [Serializable]
    public class FtpConnectionException : Exception
    {
        /// <summary> Exception thrown when an ftp connection fails</summary>
        public FtpConnectionException()
        {
        }

        /// <summary> Exception thrown when an ftp connection fails</summary>
        public FtpConnectionException(string message) : base(message)
        {
        }

        /// <summary> Exception thrown when an ftp connection fails</summary>
        public FtpConnectionException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    /// <summary> Exception thrown when an ftp download fails</summary>
    [Serializable]
    public class FtpDownloadException : Exception
    {
        /// <summary> Exception thrown when an ftp download fails</summary>
        public FtpDownloadException()
        {
        }

        /// <summary> Exception thrown when an ftp download fails</summary>
        public FtpDownloadException(string message) : base(message)
        {
        }

        /// <summary> Exception thrown when an ftp download fails</summary>
        public FtpDownloadException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    /// <summary> Exception thrown when an ftp upload fails</summary>
    [Serializable]
    public class FtpUploadException : Exception
    {
        /// <summary> Exception thrown when an ftp upload fails</summary>
        public FtpUploadException()
        {
        }

        /// <summary> Exception thrown when an ftp upload fails</summary>
        public FtpUploadException(string message) : base(message)
        {
        }

        /// <summary> Exception thrown when an ftp upload fails</summary>
        public FtpUploadException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
