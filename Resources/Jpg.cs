﻿using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Jpg : BaseImage
    {
        private const string FileFormat = ".jpg";
        public Jpg() 
            : base(FileFormat)
        {
        }
        public Jpg(string path) 
            : base(FileFormat, path)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Jpg
                {
                    Id = Id, 
                    Length = Length, 
                    Path = Path
                };
        }
    }
}
