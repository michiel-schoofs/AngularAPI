using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers {
    public class EggWorker : IWorker {
        private readonly IEggRepository _eggrepo;
        private readonly IAnimalRepository _animalrepo;

        public EggWorker(IEggRepository eggrepo,IAnimalRepository animalrepo) {
            _eggrepo = eggrepo;
            _animalrepo = animalrepo;
        }

        public override void PreformDatabaseActions() {
            var eggs = _eggrepo.GetEggsInNeedOfHatching();

            foreach (var egg in eggs) {
                Animal al = egg.Hatch();
                _eggrepo.Delete(egg);
                _animalrepo.AddAnimal(al,true);
            }

            System.Diagnostics.Debug.WriteLine("Preformed Egg operation");
        }
    }
}
