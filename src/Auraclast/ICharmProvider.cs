using System.Collections;
using System.Threading.Tasks;

namespace Auraclast;

// sort of a generic placeholder until I get a handle on what the api should look like
public interface ICharmProvider {
    Task<IEnumerable<Charm>> GetAllCharmsAsync();
};