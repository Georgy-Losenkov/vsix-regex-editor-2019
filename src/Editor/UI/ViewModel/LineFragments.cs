using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public class LineFragments : IEnumerable<LineFragment>, IEnumerable
    {
        readonly LineFragment[] m_array;

        public LineFragments(IEnumerable<LineFragment> enumerable)
        {
            m_array = (enumerable ?? throw new ArgumentNullException(nameof(enumerable))).ToArray();
        }

        public LineFragments(params LineFragment[] array)
        {
            m_array = array ?? throw new ArgumentNullException(nameof(array));
        }

        public IEnumerator<LineFragment> GetEnumerator()
        {
            return ((IEnumerable<LineFragment>)m_array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_array.GetEnumerator();
        }
    }
}