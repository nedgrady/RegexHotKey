﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RegexHotKey
{
    public abstract class StreamProcessor<Tin, Tout>
        : IProcessor<Tin, Tout>
    {
        public bool ClearOnMatch { get; set; }

        protected List<Tin> _stream = new List<Tin>();

        public IEnumerable<Tin> ClearItems => _clearItems;
        protected IEnumerable<Tin> _clearItems;

        private object processingLock = new object();

        protected abstract Tout Transform(IEnumerable<Tin> stream);
        protected abstract bool Test(IEnumerable<Tin> stream);

        public StreamProcessor(bool clearStreamOnMatch = true, IEnumerable<Tin> clearItems = null)
        {
            //if (testFunc == null)
            //    throw new ArgumentNullException("processFunc");
            //
            //if (transformFunc == null)
            //    throw new ArgumentNullException("transform");

            ClearOnMatch = clearStreamOnMatch;

            _clearItems = clearItems ?? Enumerable.Empty<Tin>();

            //_testFunc = testFunc;
            //_transformFunc = transformFunc;
        }

        public bool Add(Tin itemIn, out Tout itemOut)
        {
            _stream.Add(itemIn);

            lock(processingLock)
            {
                if (Test(_stream))
                {
                    itemOut = Transform(_stream);

                    if (ClearOnMatch)
                        Clear();

                    return true;
                }
                else
                {
                     if (_clearItems.Contains(itemIn))
                        Clear();
                }
            }

            itemOut = default(Tout);
            return false;
        }

        public void Clear()
        {
            _stream.Clear();
        }
    }
}
