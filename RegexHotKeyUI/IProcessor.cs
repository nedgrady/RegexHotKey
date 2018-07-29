using System.Threading.Tasks;

namespace GlobalKeyListener
{
    interface IProcessor<Tin, Tout>
    {
        bool Add(Tin itemIn, out Tout itemOut);

        void Clear();
    }
}