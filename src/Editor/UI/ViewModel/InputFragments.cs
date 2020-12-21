using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public class InputFragments : IEnumerable<InputFragment>, IEnumerable
    {
        readonly InputFragment[] m_array;

        public InputFragments(IEnumerable<InputFragment> enumerable)
        {
            m_array = (enumerable ?? throw new ArgumentNullException(nameof(enumerable))).ToArray();
        }

        public InputFragments(params InputFragment[] array)
        {
            m_array = array ?? throw new ArgumentNullException(nameof(array));
        }

        public IEnumerator<InputFragment> GetEnumerator()
        {
            return ((IEnumerable<InputFragment>)m_array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_array.GetEnumerator();
        }
    }
}