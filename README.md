Projekt AnimalRestAPI umożliwia zarządzanie bazą danych zwierząt oraz ich wizyt u weterynarza za pomocą API. API umożliwia dodawanie, edycję, usuwanie zwierząt, dodawanie nowych wizyt oraz pobieranie informacji o zwierzętach i ich wizytach.

Projekt wykorzystuje MySQL Connector do komunikacji z bazą danych MySQL.

#### Funkcje

- Pobieranie listy wszystkich zwierząt
- Pobieranie informacji o konkretnym zwierzęciu
- Dodawanie nowego zwierzęcia
- Edycja istniejącego zwierzęcia
- Usuwanie istniejącego zwierzęcia
- Dodawanie nowej wizyty dla zwierzęcia
- Pobieranie listy wizyt dla danego zwierzęcia

#### Instrukcja użycia

1. Uruchomienie projektu AnimalRestAPI.
2. Wywołanie odpowiednich endpointów API zgodnie z potrzebami aplikacji klienckiej.

#### Docker Compose

Projekt zawiera plik Docker Compose, który umożliwia łatwe uruchomienie aplikacji w kontenerze Docker.

#### Testy

Projekt zawiera również testy API w formacie .http, które mogą być użyte w narzędziach do testowania API, takich jak Postman lub Insomnia.


#### Uwagi

Upewnij się, że posiadasz dostęp do bazy danych MySQL oraz skonfigurowałeś odpowiednio połączenie w pliku appsettings.json.

PS: Jak ktoś chce zoabczyć jak w normlanym Freeamworku się robi takie rzeczy to niech znajdzie ukryty moduł ;)