﻿using System;
using System.Collections.Generic;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class Filterchain<TOutput>
        where TOutput : IResource
    {
        internal Filterchain(TOutput outputToUse) 
        {
            if (outputToUse == null)
            {
                throw new ArgumentNullException("outputToUse");
            }

            Output = outputToUse;
            ResourceList = new List<CommandResourceReceipt>();
            Filters = new AppliesToCollection<IFilter>(outputToUse.GetType());
        }
        internal Filterchain(TOutput outputToUse, params IFilter[] filters) 
            : this(outputToUse)
        {
            if (filters.Length > 0)
            {
                Filters.AddRange(filters); 
            }
        }

        public static implicit operator Filterchain<IResource>(Filterchain<TOutput> filterchain)
        {
            var filterchainNew = new Filterchain<IResource>(filterchain.Output, filterchain.Filters.List.ToArray());
            if (filterchain.ResourceList.Count > 0)
            {
                filterchainNew.SetResources(filterchain.ResourceList);
            }
            return filterchainNew;
        }

        public TOutput Output { get; protected set; }

        public AppliesToCollection<IFilter> Filters { get; protected set; }

        public IReadOnlyList<CommandResourceReceipt> Resources { get { return ResourceList.AsReadOnly(); } }

        public void SetResources(params CommandResourceReceipt[] resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            if (resources.Length == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one resource.");
            }

            SetResources(new List<CommandResourceReceipt>(resources));
        }

        public void SetResources(List<CommandResourceReceipt> resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            if (resources.Count == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one resource.");
            }

            ResourceList = resources;
        }

        public Filterchain<TResource> Copy<TResource>()
            where TResource : IResource
        {
            return Filterchain.FilterTo(Output.Copy<TResource>(), Filters.List.ToArray());
        }

        public override string ToString()
        {
            if (Filters.Count == 0)
            {
                throw new InvalidOperationException("Filterchain must contain at least one filter.");
            }
            if (Resources.Count == 0)
            {
                throw new InvalidOperationException("Filterchain must contain at least one resource.");
            }

            var filterChain = new StringBuilder(100);
            var firstFilter = true;

            ResourceList.ForEach(resource =>
            {
                filterChain.Append(Formats.Map(resource.Map));
                filterChain.Append(" ");
            });

            Filters.List.ForEach(filter =>
            {
                if (firstFilter)
                {
                    firstFilter = false;
                }
                else
                {
                    filterChain.Append(",");
                }

                filterChain.Append(filter.ToString());
                filterChain.Append(" ");
            });

            filterChain.Append(Formats.Map(Output.Map));

            return filterChain.ToString();
        }

        #region Internals 
        public List<CommandResourceReceipt> ResourceList { get; protected set; } 
        #endregion 
    }
}
