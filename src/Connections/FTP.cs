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
    public enum FtpProtocolType
    {
        /// <summary> File Transfer Protocol </summary>
        FTP,
        /// <summary> SSH File Transfer Protocol </summary>
        SFTP
    }

    /// <summary>
    /// Element type from a remote ftp folder 
    /// </summary>
    public enum FtpElementType
    {
        /// <summary> A remote file </summary>
        FILE,
        /// <summary> A remote folder </summary>
        DIRECTORY
    }

    /// <summary>
    /// A remote Element, can be a file or a folder
    /// </summary>
    public class FtpElement
    {
        /// <summary> Type of the element </summary>
        public FtpElementType type {get; private set;}
        /// <summary> Name and extension (if is a file) of the element </summary>
        public string name;
        /// <summary> Full path of the element </summary>
        public string path;

        /// <summary>
        /// A remote Element, can be a file or a folder
        /// </summary>
        public FtpElement(FtpElementType _type, string _path)
        {
            path = _path;
            name = Path.GetFileName(_path);
            type = _type;

        }  
    }

    /// <summary>
    /// Class to manage a connection to a specific ftp or sftp server
    /// </summary>
    public class FtpConnection: IDisposable
    {
        /// <summary> If true, print additional debug information on console. (Default false) </summary>
        public static bool   printDebug = false;
        /// <summary> Protocol type of this connection </summary>
        public  FtpProtocolType protocol  { get; } = FtpProtocolType.FTP;

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
        public FtpConnection(FtpProtocolType connectionProtocol, string host, string user, string pass, string port)
        {
            try
            { 
                if(connectionProtocol == FtpProtocolType.FTP)
                {
                    ftpClient = new FtpClient(host, user, pass);
                    ftpClient.AutoConnect();

                    protocol = FtpProtocolType.FTP;

                    if(printDebug)
                        Console.WriteLine("FTP connected to host: "+host);
                }
                if(connectionProtocol == FtpProtocolType.SFTP)
                {
                   sftpClient = new SftpClient(host, int.Parse(port), user, pass);
                   sftpClient.Connect();

                    protocol = FtpProtocolType.SFTP;

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
        /// Choose a remote file or folder to download (remotePath) and place the downloaded content inside a local file or folder (localPath)
        /// </summary>
        /// <exception cref="FtpDownloadException"></exception>
        public void Download(string remotePath, string localPath)
        {
            try
            {
                if(protocol == FtpProtocolType.FTP)
                {
                    if(ftpClient == null)
                        throw(new NullReferenceException("FTP client is not connected"));
                    
                    FtpFileSystemObjectType remoteType = ftpClient.GetObjectInfo(remotePath).Type;

                    //remote path is a directory
                    if(remoteType == FtpFileSystemObjectType.Directory)
                    {
                        if(!Directory.Exists(localPath))
                            Directory.CreateDirectory(localPath);

                        foreach (FtpListItem item in ftpClient.GetListing(remotePath))
                        {
                            string finalPath = Path.Combine(localPath, item.Name);

	                        if(item.Type == FtpFileSystemObjectType.File)
                                ftpClient.DownloadFile(finalPath, item.FullName);    

                            if(printDebug)
                                Console.WriteLine("Saving file in: "+finalPath);
                        }  
                    }
                    //remote path is a file
                    if(remoteType == FtpFileSystemObjectType.File)
                    {
                        string finalPath = localPath;

                        //if the destination is a folder create the folder an set the final path as 'folder + file_name'
                        if(Path.GetExtension(localPath) == "" )
                        {
                            if(!Directory.Exists(localPath))
                                Directory.CreateDirectory(localPath);

                            finalPath = Path.Combine(localPath, Path.GetFileName(remotePath));

                            if(printDebug)
                                Console.WriteLine("Saving file in: "+finalPath);
                        }

                        ftpClient.DownloadFile(finalPath, remotePath);    
                    }     
                }

                if(protocol == FtpProtocolType.SFTP)
                {
                    if(sftpClient == null)
                        throw(new NullReferenceException("SFTP client is not connected"));

                    bool isDirectory = sftpClient.Get(remotePath).Attributes.IsDirectory;

                    //remote path is a directory
                    if(isDirectory)
                    {
                        if(!Directory.Exists(localPath))
                            Directory.CreateDirectory(localPath);

                        foreach (SftpFile item in sftpClient.ListDirectory(remotePath))
                        {
	                        if(item.IsRegularFile == true)
                            {
                                string finalPath = Path.Combine(localPath, item.Name);

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
                        string finalPath = localPath;

                        //if the destination is a folder create the folder an set the final path as 'folder + file_name'
                        if(Path.GetExtension(localPath) == "" )
                        {
                            if(!Directory.Exists(localPath))
                                Directory.CreateDirectory(localPath);

                            finalPath = Path.Combine(localPath, Path.GetFileName(remotePath));
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
        /// Choose a local file or folder (localPath) to upload to a remote file or folder (remotePath)
        /// </summary>
        /// <exception cref="FtpUploadException"></exception>
        public void Upload(string localPath, string remotePath)
        {
            try
            {
                List<string> files = ElaboratePath(localPath);
                         
                if(protocol == FtpProtocolType.FTP)
                {
                    if(ftpClient == null)
                        throw(new NullReferenceException("FTP client is not connected"));

                    foreach(string file in files)
                    { 
                        string finalPath = remotePath;
                        
                        if( files.Count > 1 || (files.Count == 1 && Path.GetFileName(file) != Path.GetFileName(remotePath)) )
                            finalPath = Path.Combine(remotePath, Path.GetFileName(file));

                        ftpClient.UploadFile(file, finalPath);     
                    
                        if(printDebug)
                            Console.WriteLine("File uploaded to: "+finalPath);
                    }
                }

                if(protocol == FtpProtocolType.SFTP)
                {
                   
                    if(sftpClient == null)
                        throw(new NullReferenceException("SFTP client is not connected"));

                    foreach(string file in files)
                    { 
                        string finalPath = remotePath;
                        
                        if( files.Count > 1 || (files.Count == 1 && Path.GetFileName(file) != Path.GetFileName(remotePath)) )
                            finalPath = Path.Combine(remotePath, Path.GetFileName(file));

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
        /// Delete a remote file
        /// </summary>
        /// <exception cref="FtpDeleteException"></exception>
        public void DeleteFile(string remotePath)
        {
            try
            {  
                if(protocol == FtpProtocolType.FTP)
                {
                    if(ftpClient == null)
                        throw(new NullReferenceException("FTP client is not connected"));

                    ftpClient.DeleteFile(remotePath);
                }

                if(protocol == FtpProtocolType.SFTP)
                {
                    if(sftpClient == null)
                        throw(new NullReferenceException("SFTP client is not connected"));

                    sftpClient.DeleteFile(remotePath);
                }
                
            }
            catch(Exception ex)
            { 
                throw new FtpDeleteException(ex.Message);
            }
        }

        /// <summary>
        /// Delete a remote folder with every content
        /// </summary>
        /// <exception cref="FtpDeleteException"></exception>
        public void DeleteFolder(string remotePath)
        {
            try
            {  
                if(protocol == FtpProtocolType.FTP)
                {
                    if(ftpClient == null)
                        throw(new NullReferenceException("FTP client is not connected"));

                    ftpClient.DeleteDirectory(remotePath, FtpListOption.Recursive);
                }

                if(protocol == FtpProtocolType.SFTP)
                {
                    if(sftpClient == null)
                        throw(new NullReferenceException("SFTP client is not connected"));

                    sftpClient.DeleteDirectory(remotePath);
                }
                
            }
            catch(Exception ex)
            { 
                throw new FtpDeleteException(ex.Message);
            }
        }

        /// <summary>
        /// Delete all the content of a remote folder
        /// </summary>
        /// <exception cref="FtpDeleteException"></exception>
        public void ClearFolder(string remotePath, bool deleteSubFolder = true)
        {
            try
            {  
                if(protocol == FtpProtocolType.FTP)
                {
                    if(ftpClient == null)
                        throw(new NullReferenceException("FTP client is not connected"));

                    foreach(FtpListItem item in ftpClient.GetListing(remotePath))
                    {
                        if(item.Type == FtpFileSystemObjectType.File)
                            ftpClient.DeleteFile(item.FullName);
                        if(item.Type == FtpFileSystemObjectType.Directory && deleteSubFolder)
                            ftpClient.DeleteDirectory(item.FullName, FtpListOption.Recursive);
                    }
                }

                if(protocol == FtpProtocolType.SFTP)
                {
                    if(sftpClient == null)
                        throw(new NullReferenceException("SFTP client is not connected"));

                    foreach(SftpFile item in sftpClient.ListDirectory(remotePath))
                    {
                        if(item.IsRegularFile)
                            sftpClient.DeleteFile(item.FullName);   
                         if(item.IsDirectory && item.Name != "." && item.Name != ".." && deleteSubFolder)
                            sftpClient.DeleteDirectory(item.FullName); 
                    }
                }
                
            }
            catch(Exception ex)
            { 
                throw new FtpDeleteException(ex.Message);
            }
        }

        /// <summary>
        /// Get an array of <see cref="FtpElement"/> contained in the passed remote path
        /// </summary>
        /// <exception cref="FtpListException"></exception>
        public FtpElement[] ListDirectory(string remotePath)
        {
            try
            {  
                List<FtpElement> list= new List<FtpElement>();

                if(protocol == FtpProtocolType.FTP)
                {
                    if(ftpClient == null)
                        throw(new NullReferenceException("FTP client is not connected"));

                    foreach(FtpListItem item in ftpClient.GetListing(remotePath))
                    {
                        if(item.Type==FtpFileSystemObjectType.File)
                            list.Add(new FtpElement(FtpElementType.FILE, item.FullName));
                        if(item.Type==FtpFileSystemObjectType.Directory)
                            list.Add(new FtpElement(FtpElementType.DIRECTORY, item.FullName));
                    }
                }

                if(protocol == FtpProtocolType.SFTP)
                {
                    if(sftpClient == null)
                        throw(new NullReferenceException("SFTP client is not connected"));

                    foreach(SftpFile item in sftpClient.ListDirectory(remotePath))
                    {
                        if(item.IsRegularFile)
                            list.Add(new FtpElement(FtpElementType.FILE, item.FullName));
                        if(item.IsDirectory)
                            list.Add(new FtpElement(FtpElementType.DIRECTORY, item.FullName));
                    }
                }

                return list.ToArray();
            }
            catch(Exception ex)
            { 
                throw new FtpListException(ex.Message);
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
