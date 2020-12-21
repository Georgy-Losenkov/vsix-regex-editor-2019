using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public class LineReplacements : IEnumerable<LineReplacement>, IEnumerable
    {
        readonly LineReplacement[] m_array;

        public LineReplacements(IEnumerable<LineReplacement> enumerable)
        {
            m_array = (enumerable ?? throw new ArgumentNullException(nameof(enumerable))).ToArray();
        }

        public LineReplacements(params LineReplacement[] array)
        {
            m_array = array ?? throw new ArgumentNullException(nameof(array));
        }

        public IEnumerator<LineReplacement> GetEnumerator()
        {
            return ((IEnumerable<LineReplacement>)m_array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_array.GetEnumerator();
        }
    }
}