using System.Collections.Generic;
using System.Text;

namespace PrintNodeNet.Util
{
    /// <summary>
    /// The SetBuilder class is a tool that helps mutualising the code for generating sets of any
    /// kind.
    /// To get the set as a string, use the Build method.
    /// </summary>
    public class SetBuilder
    {
        private StringBuilder Set;
        public int Size { get; private set; }

        /// <summary>
        /// This parameterless constructor simply instanciates a new StringBuilder and sets the
        /// Size to 0. Use the Add method to then add content to the set.
        /// </summary>
        public SetBuilder()
        {
            Set = new StringBuilder();
            Size = 0;
        }

        /// <summary>
        /// This constructor creates a new instance of SetBuilder with an Enumerable of longs as
        /// template.
        /// </summary>
        /// <param name="set">The set of ids to add to the new SetBuilder</param>
        public SetBuilder(IEnumerable<long> set)
        {
            Size = 0;
            Set = new StringBuilder();
            foreach (long item in set)
            {
                Set.Append(item);
                Set.Append(',');
                Size++;
            }
        }

        /// <summary>
        /// This method allows the addition of extra elements to the set if needed.
        /// </summary>
        /// <param name="set">The set of ids to add to the new SetBuilder</param>
        public void Add(IEnumerable<long> set)
        {
            foreach (long item in set)
            {
                Set.Append(item);
                Set.Append(',');
                Size++;
            }
        }

        /// <summary>
        /// The Build method returns the string built from the StringBuilder that has been edited
        /// throughout the SetBuilder lifetime.
        /// </summary>
        /// <returns>The set as a string, ready for querying the PrintNode API.</returns>
        public string Build()
        {
            if (Size > 0)
            {
                //We remove the last comma from the set
                Set.Remove(Set.Length -1, 1);
            }

            return Set.ToString();
        }
    }
}
