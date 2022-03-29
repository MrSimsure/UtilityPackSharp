using FluentFTP;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace UtilityPack.Connections.Ftp
{
    /// <summary>
    /// Type of FTP protocols
    /// </summary>
    public enum ProtocolType
    {
        /// <summary> File Transfer Protocol </summary>
        FTP,
        /// <summary> SSH File Transfer Protocol </summary>
        SFTP
    }

    /// <summary>
    /// Class to manage a connection to a specific ftp or sftp server
    /// </summary>
    public class FtpConnection: IDisposable
    {
        /// <summary> If true, print additional debug information on console. (Default false) </summary>
        public static bool   printDebug = false;
        /// <summary> Protocol type of this connection </summary>
        public  ProtocolType protocol  { get; } = ProtocolType.FTP;

        private FtpClient    ftpClient  = null;
        private SftpClient   sftpClient = null;

        /// <summary>
        /// Create an instance of an FTP connection passing the connection type and acces data
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="SshConnectionException"></exception>
        /// <exception cref="SshAuthenticationException"></exception>
        /// <exception cref="ProxyException"></exception>
        public FtpConnection(ProtocolType connectionProtocol, string host, string user, string pass, string port)
        {
            try
            { 
                if(connectionProtocol == ProtocolType.FTP)
                {
                    ftpClient = new FtpClient(host, user, pass);
                    ftpClient.AutoConnect();

                    protocol = ProtocolType.FTP;

                    if(printDebug)
                        Console.WriteLine("FTP connected to host: "+host);
                }
                if(connectionProtocol == ProtocolType.SFTP)
                {
                   sftpClient = new SftpClient(host, int.Parse(port), user, pass);
                   sftpClient.Connect();

                    protocol = ProtocolType.SFTP;

                    if(printDebug)
                        Console.WriteLine("SFTP connected to host: "+host);
                }
            }
            catch(Exception ex)
            {
                throw new FtpConnectionException(ex.Message);
            }
        }

        /// <summary>
        /// Choose a remote file or folder to download (remotePaht) and place the downloaded content inside a local file or folder (destinationPath)
        /// </summary>
        public void DownloadFile(string remotePath, string destinationPath)
        {
            try
            {
                if(protocol == ProtocolType.FTP)
                {
                    if(ftpClient == null)
                        throw(new NullReferenceException("FTP client is not connected"));
                    
                    FtpFileSystemObjectType remoteType = ftpClient.GetObjectInfo(remotePath).Type;

                    //remote path is a directory
                    if(remoteType == FtpFileSystemObjectType.Directory)
                    {
                        if(!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);

                        foreach (FtpListItem item in ftpClient.GetListing(remotePath))
                        {
                            string finalPath = Path.Combine(destinationPath, item.Name);

	                        if(item.Type == FtpFileSystemObjectType.File)
                                ftpClient.DownloadFile(finalPath, item.FullName);    

                            if(printDebug)
                                Console.WriteLine("Saving file in: "+finalPath);
                        }  
                    }
                    //remote path is a file
                    if(remoteType == FtpFileSystemObjectType.File)
                    {
                        string finalPath = destinationPath;

                        //if the destination is a folder create the folder an set the final path as 'folder + file_name'
                        if(Path.GetExtension(destinationPath) == "" )
                        {
                            if(!Directory.Exists(destinationPath))
                                Directory.CreateDirectory(destinationPath);

                            finalPath = Path.Combine(destinationPath, Path.GetFileName(remotePath));

                            if(printDebug)
                                Console.WriteLine("Saving file in: "+finalPath);
                        }

                        ftpClient.DownloadFile(finalPath, remotePath);    
                    }     
                }

                if(protocol == ProtocolType.SFTP)
                {
                    if(sftpClient == null)
                        throw(new NullReferenceException("SFTP client is not connected"));

                    bool isDirectory = sftpClient.Get(remotePath).Attributes.IsDirectory;

                    //remote path is a directory
                    if(isDirectory)
                    {
                        if(!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);

                        foreach (SftpFile item in sftpClient.ListDirectory(remotePath))
                        {
	                        if(item.IsRegularFile == true)
                            {
                                string finalPath = Path.Combine(destinationPath, item.Name);

                                using(FileStream fs = new FileStream( finalPath, FileMode.Create))
                                {
                                    sftpClient.DownloadFile(item.FullName, fs);
                                }

                                if(printDebug)
                                    Console.WriteLine("Saving file in: "+finalPath);
                            }
                        }  
                    }

                    //remote path is a file
                    if(isDirectory == false)
                    {
                        string finalPath = destinationPath;

                        //if the destination is a folder create the folder an set the final path as 'folder + file_name'
                        if(Path.GetExtension(destinationPath) == "" )
                        {
                            if(!Directory.Exists(destinationPath))
                                Directory.CreateDirectory(destinationPath);

                            finalPath = Path.Combine(destinationPath, Path.GetFileName(remotePath));
                        }

                        if(printDebug)
                            Console.WriteLine("Saving file in: "+finalPath);

                        using(FileStream fs = new FileStream( finalPath, FileMode.Create))
                        {
                            sftpClient.DownloadFile(remotePath, fs);
                        } 
                    } 
                }
            }
            catch(Exception ex)
            { 
                throw new FtpDownloadException(ex.Message);
            }
        }

        /// <summary>
        /// Choose a local file or folder (filePath) to upload to a remote file or folder (destionationPath)
        /// </summary>
        public void UploadFile(string filePath, string destinationPath)
        {
            try
            {
                List<string> files = ElaboratePath(filePath);
                             
                if(protocol == ProtocolType.FTP)
                {
                    if(ftpClient == null)
                        throw(new NullReferenceException("FTP client is not connected"));

                    foreach(string file in files)
                    { 
                        string finalPath = Path.Combine(destinationPath, Path.GetFileName(file));
                        ftpClient.UploadFile(file, finalPath);     
                    
                        if(printDebug)
                            Console.WriteLine("File uploaded to: "+finalPath);
                    }
                }

                if(protocol == ProtocolType.SFTP)
                {
                   
                    if(sftpClient == null)
                        throw(new NullReferenceException("SFTP client is not connected"));

                    foreach(string file in files)
                    { 
                        string finalPath = Path.Combine(destinationPath, Path.GetFileName(file));

                        using(FileStream fs = File.OpenRead(file))
                        { 
                            sftpClient.BufferSize=4*1024;
                            sftpClient.UploadFile(fs, finalPath, null);  
                        }

                        if(printDebug)
                            Console.WriteLine("File uploaded to: "+finalPath);
                    }
                }
                
            }
            catch(Exception ex)
            { 
                throw new FtpUploadException(ex.Message);
            }
        }


        /// <summary>
        /// Giving a local path elaborate it further and return a list.      <br/>
        /// If the passed path is just a file the list will contains only it <br/>
        /// If the passed path is a folder the list will contains every file inside that folder
        /// </summary>
        private List<string> ElaboratePath(string path)
        {
            List<string> files = new List<string>();

            if(Directory.Exists(path))
                files.AddRange(Directory.GetFiles(path));
            else
                files.Add(path);

            return files;
        }

        /// <summary>
        /// Dispose every connection
        /// </summary>
        public void Dispose()
        {
            if(sftpClient != null)
                sftpClient.Dispose();

            if(ftpClient != null)
                ftpClient.Dispose();
        }
    }   
}
