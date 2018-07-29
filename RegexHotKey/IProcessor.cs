using System.Threading.Tasks;

namespace RegexHotKey
{
    interface IProcessor<Tin, Tout>
    {
        bool Add(Tin itemIn, out Tout itemOut);

        void Clear();
    }
}