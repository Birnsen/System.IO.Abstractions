﻿using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    internal sealed class FileStreamWrapper : FileSystemStream, IFileSystemAclSupport
    {
        private readonly FileStream fileStream;

        public FileStreamWrapper(FileStream fileStream)
            : base(fileStream, fileStream.Name, fileStream.IsAsync)

        {
            this.fileStream = fileStream;
        }

        /// <inheritdoc cref="IFileSystemAclSupport.GetAccessControl()" />
        [SupportedOSPlatform("windows")]
        public object GetAccessControl()
        {
            return fileStream.GetAccessControl();
        }

        /// <inheritdoc cref="IFileSystemAclSupport.GetAccessControl(IFileSystemAclSupport.AccessControlSections)" />
        [SupportedOSPlatform("windows")]
        public object GetAccessControl(IFileSystemAclSupport.AccessControlSections includeSections)
        {
            throw new NotSupportedException("GetAccessControl with includeSections is not supported for FileStreams");
        }

        /// <inheritdoc cref="IFileSystemAclSupport.SetAccessControl(object)" />
        [SupportedOSPlatform("windows")]
        public void SetAccessControl(object value)
        {
            if (value is FileSecurity fileSecurity)
            {
                this.fileStream.SetAccessControl(fileSecurity);
            }
            else
            {
                throw new ArgumentException("value must be of type `FileSecurity`");
            }
        }

        /// <summary>
        /// Allows to get the internal FileStream
        /// </summary>
        /// <param name="fsStream">The FileSystemStream to cast</param>
        /// <exception cref="InvalidCastException"></exception>
        public static explicit operator FileStream(FileStreamWrapper fsStream)
        {
            return fsStream.fileStream;
        }
    }
}
