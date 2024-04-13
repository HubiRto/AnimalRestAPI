package pl.pomoku.xd;

import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping(value = "/api/v1/animal")
@RequiredArgsConstructor
public class AnimalController {
    private final AnimalRepository animalRepository;
    private final VisitRepository visitRepository;
    @GetMapping("/animal/all")
    public List<Animal> getAllAnimals() {
        return animalRepository.findAll();
    }

    @GetMapping("/animal/{id}")
    public Animal getAnimalById(@PathVariable int id) {
        return animalRepository.findById(id).orElse(null);
    }

    @PostMapping("/animal/add")
    public Animal addAnimal(@RequestBody Animal animal) {
        return animalRepository.save(animal);
    }

    @PutMapping("/animal/{id}")
    public void updateAnimal(@PathVariable int id, @RequestBody Animal animal) {
        if (animalRepository.existsById(id)) {
            animal.setId(id);
            animalRepository.save(animal);
        }
    }

    @DeleteMapping("/animal/{id}")
    public void deleteAnimal(@PathVariable int id) {
        animalRepository.deleteById(id);
    }

    @PostMapping("/animal/{animalId}/visit/add")
    public Visit addVisit(@PathVariable int animalId, @RequestBody Visit visit) {
        Animal animal = animalRepository.findById(animalId).orElse(null);
        if (animal != null) {
            visit.setAnimal(animal);
            return visitRepository.save(visit);
        }
        return null;
    }

    @GetMapping("/animal/{animalId}/visits")
    public List<Visit> getVisitsByAnimalId(@PathVariable int animalId) {
        Animal animal = animalRepository.findById(animalId).orElse(null);
        if (animal != null) {
            return visitRepository.findAllByAnimal(animal);
        }
        return null;
    }
}
