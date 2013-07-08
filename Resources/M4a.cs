﻿using System;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class M4A : BaseAudio
    {
        private const string FileFormat = ".m4a";
        public M4A() 
            : base(FileFormat)
        {
        }
        public M4A(string path) 
            : base(FileFormat, path)
        {
        }
        public M4A(string path, TimeSpan length) 
            : base(FileFormat, path, length)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new M4A
            {
                Id = Id,
                Length = Length,
                Path = Path
            };
        }
    }
}
